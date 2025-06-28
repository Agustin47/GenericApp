using Framework.Specification;

namespace Framework.Database;

public record QueryRepository<T>(
    List<IFilter>? Filter,
    List<ISpecification<T>>? Specifications,
    IPaging? Paging,
    ISorting? Sorting);