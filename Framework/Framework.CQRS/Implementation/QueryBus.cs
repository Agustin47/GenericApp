using Framework.Common.Result;
using Framework.CQRS.Queries;

namespace Framework.CQRS.Implementation;

public class QueryBus : IQueryBus
{
    private readonly IServiceProvider _serviceProvider;


    public QueryBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<Result<TResult?>> Handle<TResult>(dynamic query)
        where TResult : class
    {
        Type queryType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var queryHandler = _serviceProvider.GetService(queryType) as dynamic;

        if (queryHandler == null)
            throw new Exception("QueryHandler not found");
        
        Type queryValidatorType = typeof(IQueryValidator<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var queryValidator = _serviceProvider.GetService(queryValidatorType) as dynamic;
        if (queryValidator != null)
        {
            var validationResult = queryValidator.ValidateQuery(query);
            if (validationResult.IsFailed)
                return validationResult;
        }
        
        Type queryPermissionValidatorType = typeof(IQueryPermissionValidator<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var queryPermissionValidator = _serviceProvider.GetService(queryPermissionValidatorType) as dynamic;
        if (queryPermissionValidator != null)
        {
            var permissionValidationResult = queryPermissionValidator.ValidatePermission(query);
            if (permissionValidationResult.IsFailed)
                return permissionValidationResult;
        }
        
        return queryHandler.Handle(query);
    }
}