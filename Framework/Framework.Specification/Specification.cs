namespace Framework.Specification;

public class Specification<T> : ISpecification<T>
{
    private Func<T, bool> _action;

    protected Specification(Func<T, bool> action)
    {
        _action = action;
    }

    public static Specification<T> Create(Func<T, bool> action) => new(action); 

    public virtual bool IsSatisfiedBy(T value)
        => _action(value);
    
    public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        => AndSpecification<T>.Create(left, right);
    
    public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        => OrSpecification<T>.Create(left, right);
}