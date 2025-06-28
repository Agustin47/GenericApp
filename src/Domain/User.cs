using Framework.Domain;

namespace Domain;

public class User : DomainEntity<UserId>
{
    public string Name { get; private set; }
    public User(UserId id) : base(id){}
    
    public void AsingName(string name,
        string userContext, DateTime? actionTime = null)
    {
        Name = name;
    }
}