using Framework.Common.Result;

namespace Framework.CQRS.Queries;

public interface IQueryPermissionValidator<in TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : class
{
    Result ValidatePermission(TQuery query);
}