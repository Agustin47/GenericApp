using Framework.Domain;

namespace Domain;

public class Person : DomainEntity<PersonId>
{
    public string Identification { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Age { get; private set; }
    public Person(PersonId id) : base(id){}
    
    public void Register(string id, string name, string lastName, int age,
        string userContext, DateTime? actionTime = null)
    {
        Identification = id;
        Name = name;
        LastName = lastName;
        Age = age;
    }
}