namespace Framework.Specification;

internal class AndSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    private AndSpecification(ISpecification<T> left, ISpecification<T> right) : base(null)
    {
        _left = left;
        _right = right;
    }
    
    public static Specification<T> Create(ISpecification<T> left, ISpecification<T> right)
        => new AndSpecification<T>(left, right);
    
    public override bool IsSatisfiedBy(T value)
        => _left.IsSatisfiedBy(value) && _right.IsSatisfiedBy(value);
    
}