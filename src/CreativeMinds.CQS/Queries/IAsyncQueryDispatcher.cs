using System;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Queries {

	public interface IAsyncQueryDispatcher {
		Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
	}
}
