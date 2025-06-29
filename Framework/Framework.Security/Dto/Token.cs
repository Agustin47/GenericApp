namespace Framework.Security.Dto;

public class Token
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string Value { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Create { get; set; }
    public DateTime Expire { get; set; }
}