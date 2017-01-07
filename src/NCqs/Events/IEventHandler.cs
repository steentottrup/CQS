using System;

namespace NCqs.Events {

	public interface IEventHandler<in TEvent> where TEvent : IEvent {
		void Handle(TEvent @event);
		//Task HandleAsync(TEvent @event);
	}
}
