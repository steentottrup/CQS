using System;

namespace NCqs.Queries {

	public interface IQueryDispatcher {
		TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
		//Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
	}
}
