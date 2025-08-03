using Framework.Common.Result;
using Framework.CQRS.Queries;
using Microsoft.Extensions.Logging;

namespace Framework.CQRS.Implementation;

public class QueryBus : IQueryBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QueryBus> _logger;


    public QueryBus(IServiceProvider serviceProvider, ILogger<QueryBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task<Result<TResult?>> Handle<TResult>(dynamic query)
        where TResult : class
    {
        _logger.LogInformation($"Handling Query {query.GetType().Name}");
        _logger.LogInformation($"Query: {System.Text.Json.JsonSerializer.Serialize(query)}");
        
        Type queryType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var queryHandler = _serviceProvider.GetService(queryType) as dynamic;

        if (queryHandler == null)
            throw new Exception("QueryHandler not found");
        
        Type queryValidatorType = typeof(IQueryValidator<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var queryValidator = _serviceProvider.GetService(queryValidatorType) as dynamic;
        if (queryValidator != null)
        {
            _logger.LogInformation("Executing query validator");
            var validationResult = queryValidator.ValidateQuery(query);
            if (validationResult.IsFailed)
                return validationResult;
        }
        
        Type queryPermissionValidatorType = typeof(IQueryPermissionValidator<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var queryPermissionValidator = _serviceProvider.GetService(queryPermissionValidatorType) as dynamic;
        if (queryPermissionValidator != null)
        {
            _logger.LogInformation("Executing query permission validator");
            var permissionValidationResult = queryPermissionValidator.ValidatePermission(query);
            if (permissionValidationResult.IsFailed)
                return permissionValidationResult;
        }
        
        return queryHandler.Handle(query);
    }
}