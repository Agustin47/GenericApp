using Framework.Domain;

namespace Domain;

public record UserId(Guid Value) : IEntityId;