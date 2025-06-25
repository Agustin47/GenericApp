namespace Framework.Specification;

public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    private OrSpecification(Specification<T> left, Specification<T> right) : base(null)
    {
        _left = left;
        _right = right;
    }
    
    public static Specification<T> Create(Specification<T> left, Specification<T> right)
        => new OrSpecification<T>(left, right);
    
    public override bool IsSatisfiedBy(T value)
        => _left.IsSatisfiedBy(value) || _right.IsSatisfiedBy(value);
    
}