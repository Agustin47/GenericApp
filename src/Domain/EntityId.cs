using Framework.Domain;

namespace Domain;

public record EntityId(Guid Value) : IEntityId;