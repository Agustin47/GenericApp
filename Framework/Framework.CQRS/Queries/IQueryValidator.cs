namespace Framework.CQRS.Queries;

public interface IQueryValidator<in TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : class
{
    bool Validate(TQuery command);
}