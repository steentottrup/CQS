using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Queries {

	public interface IAsyncQueryDispatcher {
		Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult>;
	}
}
