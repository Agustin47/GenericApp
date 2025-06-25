namespace Domain;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    
    public User(Guid id)
    {
        Id = id;
    }
    
    public void AsingName(string name,
        string userContext, DateTime? actionTime = null)
    {
        Name = name;
    }
}