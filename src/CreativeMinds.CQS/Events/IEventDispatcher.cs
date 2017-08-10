using System;

namespace CreativeMinds.CQS.Events {

	public interface IEventDispatcher {
		void Publish<TEvent>(TEvent @eventMessage) where TEvent: IEvent;
		//Task PublishAsync<TEvent>(TEvent @eventMessage) where TEvent : IEvent;
	}
}
