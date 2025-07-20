namespace Framework.EventManager;

public interface IEventIndexDb
{
    Task<int> GetNextIndexAsync(string id, string entityName);
    Task<int> GetCurrentIndexAsync(string id, string entityName);
    Task SaveProjectionIndexAsync(string id, string entityName, int index);
}