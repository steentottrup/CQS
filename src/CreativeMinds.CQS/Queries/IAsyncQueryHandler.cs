using System;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Queries {

	public interface IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> {
		Task<TResult> HandleAsync(TQuery query);
	}
}
