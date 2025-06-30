using System.Security.Cryptography;
using Framework.Common.Result;
using Framework.Security.Dto;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MongoDB.Driver;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Framework.Security;

public interface ISecurityService
{
    Result<Token> Login(string username, string password);
    Result IntrospectToken(string token, string username);
    Result<Token> RefreshToken(string token, string refreshToken);
    Result Logout(string username);
    Result RegisterUser(string username, string password, string email, string name, string lastName, string role, string[] permissions);
    Result ChangePassword(string username, string password);
    Result<UserContext> GetUserContext(string token);
}

public class SecurityService(IMongoDatabase mongoDatabase, ISecurityOptions options) : ISecurityService
{
    private const string _prefix = "Security";
    private readonly IMongoCollection<Token> _token = mongoDatabase.GetCollection<Token>($"{_prefix}-{nameof(Token)}");
    private readonly IMongoCollection<UserLogin> _userLogin = mongoDatabase.GetCollection<UserLogin>($"{_prefix}-{nameof(UserLogin)}");
    private readonly IMongoCollection<User> _users = mongoDatabase.GetCollection<User>($"{_prefix}-{nameof(User)}");
    
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public Result<Token> Login(string username, string password)
    {
        var user = _users.Find(u => u.Username == username).FirstOrDefault();
        if (user == null)
            return Result.Failed(SecurityErrors.LoginFailed);
        
        var hashedPassword = HashPassword(password, user.Salt);
        if(user.Password != hashedPassword)
            return Result.Failed(SecurityErrors.LoginFailed);

        var newTokenValue = GenerateToken(user);
        Token token = new()
        {
            Username = username,
            RefreshToken = GenerateRefreshToken(),
            Value = newTokenValue,
            Create = DateTime.UtcNow,
            Expire = DateTime.UtcNow.AddMinutes(10)
        };

        UserLogin userLogin = new()
        {
            Username = username,
            Login = DateTime.UtcNow,
        };
        
        _token.DeleteMany(t => t.Username == username);
        _token.InsertOne(token);
        _userLogin.InsertOne(userLogin);
        
        return Result.Success(token);
    }

    public Result IntrospectToken(string token, string username)
    {
        var tokenClaims = ValidateToken(token);
        if(tokenClaims == null)
            return Result.Failed();
        
        var tokenEntity = _token
            .Find(t => t.Value == token && t.Username == username && t.Expire > DateTime.UtcNow)
            .FirstOrDefault();
        if(tokenEntity == null)
            return Result.Failed();
        
        return Result.Success();
    }

    public Result<Token> RefreshToken(string token, string refreshToken)
    {
        var tokenEntity = _token.Find(t => t.Value == token && t.RefreshToken == refreshToken).FirstOrDefault();
        if (tokenEntity == null)
            return Result.Failed(SecurityErrors.RefreshTokenFailed);

        var user = _users.Find(u => u.Username == tokenEntity.Username).FirstOrDefault();
        var newTokenValue = GenerateToken(user);
        
        Token newToken = new()
        {
            Username = tokenEntity.Username,
            RefreshToken = GenerateRefreshToken(),
            Value = newTokenValue,
            Create = DateTime.UtcNow,
            Expire = DateTime.UtcNow.AddMinutes(10)
        };
        
        _token.DeleteMany(t => t.Username == tokenEntity.Username);
        _token.InsertOne(newToken);
        
        return Result.Success(newToken);
    }
    
    public Result Logout(string username)
    {
        _token.DeleteMany(t => t.Username == username);
        return Result.Success();
    }

    public Result RegisterUser(string username, string password, string email, string name, string lastName, string role, string[] permissions)
    {
        var salt = RandomNumberGenerator.GetBytes(128 / 8);
        
        User user = new()
        {
            Username = username,
            Password = HashPassword(password, salt),
            Salt = salt,
            Email = email,
            Name = name,
            LastName = lastName,
            Role = role,
            Permissions = permissions.ToList()
        };
        
        _users.InsertOne(user);
        
        return Result.Success();
    }
    
    public Result ChangePassword(string username, string password)
    {
        var user = _users.Find(u => u.Username == username).FirstOrDefault();
        if (user == null)
            return Result.Failed(SecurityErrors.LoginFailed);
        
        var salt = RandomNumberGenerator.GetBytes(128 / 8);
        user.Salt = salt;
        user.Password = HashPassword(password, salt);
        
        _users.ReplaceOne(u => u.Username == username, user);
        return Result.Success();
    }

    public Result<UserContext> GetUserContext(string token)
    {
        var claim = ValidateToken(token);
        if (claim == null) return Result.Failed();
        
        UserContext userContext = new()
        {
            Username = claim.Claims.First(c => c.Type == JwtRegisteredClaimNames.PreferredUsername).Value,
            Permissions = claim.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
        };
        
        return Result.Success(userContext);
    }
    private string HashPassword(string password, byte[] salt)
        => Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

    private string GenerateToken(User user)
    {
        List<Claim> claims = new()
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.PreferredUsername, user.Username),
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (JwtRegisteredClaimNames.Name, user.Name),
            new (JwtRegisteredClaimNames.FamilyName, user.LastName),
            new (JwtRegisteredClaimNames.Profile, user.LastName),
        };
        
        foreach (var userPermission in user.Permissions)
            claims.Add(new(ClaimTypes.Role, userPermission));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(options.ExpireInMinutes),
            signingCredentials: creds
        );
        
        return _jwtSecurityTokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),

                RequireExpirationTime = true,
            };
            
            return _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken _);
        }
        catch (Exception ex)
        {
            // add logins if we want
            return null;
        }
    }
    

    
    private string GenerateRefreshToken()
    {
        var salt = RandomNumberGenerator.GetBytes(128 / 8);
        return HashPassword(Guid.NewGuid().ToString(), salt);
    }
}