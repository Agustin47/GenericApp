using Framework.Common.Result;

namespace Framework.CQRS.Queries;

public interface IQuery<TResult> where TResult : class;