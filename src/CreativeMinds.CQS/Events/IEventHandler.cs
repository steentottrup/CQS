using System;

namespace CreativeMinds.CQS.Events {

	public interface IEventHandler<in TEvent> where TEvent : IEvent {
		void Handle(TEvent @event);
		//Task HandleAsync(TEvent @event);
	}
}
