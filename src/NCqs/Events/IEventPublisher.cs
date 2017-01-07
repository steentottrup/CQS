using System;

namespace NCqs.Events {

	public interface IEventPublisher {
		void Publish<TEvent>(TEvent @eventMessage) where TEvent: IEvent;
		//Task PublishAsync<TEvent>(TEvent @eventMessage) where TEvent : IEvent;
	}
}
