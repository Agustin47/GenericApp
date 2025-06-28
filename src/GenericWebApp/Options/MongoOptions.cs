using Framework.Database.MongoDB;

namespace GenericWebApp.Options;

public class MongoOptions : IMongoOptions
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
}