using CreativeMinds.CQS.Queries;
using System;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class QueryDispatcherBase : IQueryDispatcher {

		protected abstract IQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>() where TQuery : IQuery<TResult>;

		public virtual TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> {
			IQueryHandler<TQuery, TResult> handler = this.Resolve<TQuery, TResult>();
			return handler.Handle(query);
		}

		//public virtual Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> {
		//	IQueryHandler<TQuery, TResult> handler = this.Resolve<TQuery, TResult>();
		//	return handler.HandleAsync(query);
		//}
	}
}
