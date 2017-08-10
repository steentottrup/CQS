using CreativeMinds.CQS.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class EventDispatcherBase : IEventDispatcher {

		protected ILogger logger;
		protected abstract IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : IEvent;

		protected EventDispatcherBase(ILogger logger) {
			this.logger = logger;
		}

		public void Publish<TEvent>(TEvent eventMessage) where TEvent : IEvent {
			this.logger.LogDebug($"Publishing the {typeof(TEvent).GetType().Name} event.", eventMessage);
			IEnumerable<IEventHandler<TEvent>> handlers = this.GetEventHandlers<TEvent>();
			this.logger.LogDebug($"Found {handlers.Count()} event handlers.", eventMessage);
			foreach (IEventHandler<TEvent> handler in handlers) {
				try {
					handler.Handle(eventMessage);
				}
				catch (Exception ex) {
					this.logger.LogCritical($"An exception was throw when trying to publish a {eventMessage.GetType().Name} event using the {handler.GetType().Name} handler", ex, eventMessage);
				}
			}
		}
	}
}
