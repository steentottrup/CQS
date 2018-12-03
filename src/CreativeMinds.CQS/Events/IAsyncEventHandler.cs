using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Events {

	public interface IAsyncEventHandler<in TEvent> where TEvent : IEvent {
		Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
	}
}
