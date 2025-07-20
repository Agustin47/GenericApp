namespace Framework.Common;

public interface IEntityState : IEntity
{
    int Version { get; set; }
};