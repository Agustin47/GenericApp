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
        var queryHandler = _serviceProvider.GetService(queryType);
        
        if (queryHandler == null)
            throw new Exception("QueryHandler not found");

        return (queryHandler as dynamic).Handle(query);
    }
}