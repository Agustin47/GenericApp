using Framework.Common.Result;
using Framework.CQRS.Queries;

namespace Framework.CQRS.Implementation;

public abstract class QueryBasePermissionValidator<TQuery, TResult> : IQueryPermissionValidator<TQuery, TResult>
    where TQuery : QueryBase<TResult>
    where TResult : class
{
    protected abstract string Permission { get; }

    public virtual Result ValidatePermission(TQuery query)
        => query.UserContext.Permissions.Contains(Permission)
            ? Result.Success()
            : Result.Failed(ExpectedErrors.UnAuthorized);
}