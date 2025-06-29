namespace Framework.Security.Dto;

public class UserLogin
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public DateTime Login { get; set; }
}