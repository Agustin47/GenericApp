using Framework.Domain;

namespace Domain;

public record PersonId(Guid Value) : IEntityId;