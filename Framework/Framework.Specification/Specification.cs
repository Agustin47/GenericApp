namespace Framework.Specification;

public class Specification<T> : ISpecification<T>
{
    private Func<T, bool> _action;

    protected Specification(Func<T, bool> action)
    {
        _action = action;
    }

    public static Specification<T> Create(Func<T, bool> action) => new(action);
    public static Specification<T> True => new(x => true);

    public virtual bool IsSatisfiedBy(T value)
        => _action(value);
    
    public static Specification<T> operator &(Specification<T> left, ISpecification<T> right)
        => AndSpecification<T>.Create(left, right);

    public static Specification<T> operator |(Specification<T> left, ISpecification<T> right)
        => OrSpecification<T>.Create(left, right);

    public static Specification<T> And(params ISpecification<T>[] specifications)
    {
        var spec =  Specification<T>.True;
        foreach (var specification in specifications)
            spec = spec & specification;
        return spec;
    }
    
    public static Specification<T> Or(params ISpecification<T>[] specifications)
    {
        var spec =  Specification<T>.True;
        foreach (var specification in specifications)
            spec = spec | specification;
        return spec;
    }
}