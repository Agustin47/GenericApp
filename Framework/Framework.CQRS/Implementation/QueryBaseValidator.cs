using FluentValidation;
using Framework.Common.Result;
using Framework.CQRS.Queries;

namespace Framework.CQRS.Implementation;

public class QueryBaseValidator<TQuery, TResult> : AbstractValidator<TQuery>, IQueryValidator<TQuery, TResult>
    where TQuery : QueryBase<TResult>
    where TResult : class
{
    public Result ValidateQuery(TQuery query)
    {
        var fluentResult = Validate(query);
        if (fluentResult.IsValid)
            return Result.Success();

        var causes = fluentResult.Errors.Select(x => new ErrorCause(x.PropertyName, x.ErrorMessage)).ToArray();
        return Result.Failed(ExpectedErrors.Validation(causes));
    }
}