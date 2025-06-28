namespace Framework.Database.MongoDB;

public interface IMongoOptions : IRepositoryOptions
{
    string? DatabaseName { get; set; }
}