namespace Framework.Security.Dto;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string Password { get; set; }
    public byte[] Salt { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}