using System.Security.Cryptography;
using Framework.Database;
using Framework.Common.Result;
using Framework.Security.Dto;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Framework.Security;

public interface ISecurityService
{
    Task<Result<Token>> Login(string username, string password);
    Task<Result> IntrospectToken(string token, string username);
    Task<Result<Token>> RefreshToken(string token, string refreshToken);
    Task<Result> Logout(string username);
    Task<Result> RegisterUser(string username, string password, string email, string name, string lastName, string role, string[] permissions);
    Task<Result> ChangePassword(string username, string password);
    Task<Result<UserContext>> GetUserContext(string token);
}

public class SecurityService(IRepositoryFactory repositoryFactory, ISecurityOptions options) : ISecurityService
{
    private const string _prefix = "Security";
    private readonly IRepository<Token> _token = repositoryFactory.GetRepository<Token>(_prefix);
    private readonly IRepository<UserLogin> _userLogin = repositoryFactory.GetRepository<UserLogin>(_prefix);
    private readonly IRepository<User> _users = repositoryFactory.GetRepository<User>(_prefix);
    
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public async Task<Result<Token>> Login(string username, string password)
    {
        var spec1 = Specification.Specification<User>.Create(u => u.Username == username);
        var userQuery = QueryRepositoryBuilder<User>.Create()
            .AddSpecs(spec1)
            .Build();
        var userResult = await _users.FirstOrDefault(userQuery);
        if (userResult.IsFailed || userResult.Value == null)
            return Result.Failed(SecurityErrors.LoginFailed);
        
        var user = userResult.Value;
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
        
        await CleanAndSaveToken(token);
        await _userLogin.CreateAsync(userLogin);
        
        return Result.Success(token);
    }

    public async Task<Result> IntrospectToken(string token, string username)
    {
        var tokenClaims = ValidateToken(token);
        if(tokenClaims == null)
            return Result.Failed();
        
        var spec1 = Specification.Specification<Token>.Create(t => t.Value == token && t.Username == username && t.Expire > DateTime.UtcNow);
        var tokenQuery = QueryRepositoryBuilder<Token>.Create()
            .AddSpecs(spec1)
            .Build();
        
        var tokenResult = await _token.FirstOrDefault(tokenQuery);
        if(tokenResult.IsFailed || tokenResult.Value == null)
            return Result.Failed();
        
        return Result.Success();
    }

    public async Task<Result<Token>> RefreshToken(string token, string refreshToken)
    {
        var spec1 = Specification.Specification<Token>.Create(t => t.Value == token && t.RefreshToken == refreshToken);
        var tokenQuery = QueryRepositoryBuilder<Token>.Create()
            .AddSpecs(spec1)
            .Build();
        var tokenResult = await _token.FirstOrDefault(tokenQuery);
        if (tokenResult.IsFailed || tokenResult.Value == null)
            return Result.Failed(SecurityErrors.RefreshTokenFailed);

        var tokenEntity = tokenResult.Value;
        var spec2 = Specification.Specification<User>.Create(u => u.Username == tokenEntity.Username);
        var userQuery = QueryRepositoryBuilder<User>.Create()
            .AddSpecs(spec2)
            .Build();
        var userResult = await _users.FirstOrDefault(userQuery);
        if (userResult.IsFailed || userResult.Value == null)
            return Result.Failed(SecurityErrors.RefreshTokenFailed);
        
        var user = userResult.Value;
        var newTokenValue = GenerateToken(user);
        
        Token newToken = new()
        {
            Username = tokenEntity.Username,
            RefreshToken = GenerateRefreshToken(),
            Value = newTokenValue,
            Create = DateTime.UtcNow,
            Expire = DateTime.UtcNow.AddMinutes(10)
        };
        
        await CleanAndSaveToken(newToken);
        return Result.Success(newToken);
    }
    
    public async Task<Result> Logout(string username)
    {
        await CleanAndSaveToken(new(){Username = username});
        return Result.Success();
    }

    public async Task<Result> RegisterUser(string username, string password, string email, string name, string lastName, string role, string[] permissions)
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
        
        await _users.CreateAsync(user);
        
        return Result.Success();
    }
    
    public async Task<Result> ChangePassword(string username, string password)
    {
        var spec1 = Specification.Specification<User>.Create(u => u.Username == username);
        var userQuery = QueryRepositoryBuilder<User>.Create()
            .AddSpecs(spec1)
            .Build();
        var userResult = await _users.FirstOrDefault(userQuery);
        
        if (userResult.IsFailed || userResult.Value == null)
            return Result.Failed(SecurityErrors.LoginFailed);
        var user = userResult.Value;
        
        var salt = RandomNumberGenerator.GetBytes(128 / 8);
        user.Salt = salt;
        user.Password = HashPassword(password, salt);
        
        //_users.ReplaceOne(u => u.Username == username, user);
        return Result.Success();
    }

    public async Task<Result<UserContext>> GetUserContext(string token)
    {
        var claim = ValidateToken(token);
        if (claim == null) return Result.Failed();

        var username = claim.Claims.First(c => c.Type == JwtRegisteredClaimNames.PreferredUsername).Value;
        var permissions = claim.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        
        UserContext userContext = new(username, permissions);
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

    private async Task CleanAndSaveToken(Token token)
    {
        var spec1 = Specification.Specification<Token>.Create(t => t.Username == token.Username);
        var tokenQuery = QueryRepositoryBuilder<Token>.Create()
            .AddSpecs(spec1)
            .Build();
        
        var tokenListResult = await _token.Filter(tokenQuery);
        var tokenList = tokenListResult.Value ?? [];
        foreach (var t in tokenList)
            await _token.DeleteAsync(t.Id);
        
        if(!string.IsNullOrWhiteSpace(token.Value))
            await _token.CreateAsync(token);
    }
    
    private ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;
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