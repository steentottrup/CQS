using NCqs.Queries;
using System;

namespace NCqs.Resolvers {

	public interface IQueryHandlerResolver {
		IQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>() where TQuery : IQuery<TResult>;
	}
}
