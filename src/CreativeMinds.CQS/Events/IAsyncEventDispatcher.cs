using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Events {

	public interface IAsyncEventDispatcher {
		Task PublishAsync<TEvent>(TEvent @eventMessage, CancellationToken cancellationToken) where TEvent : IEvent;
	}
}
