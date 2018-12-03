using CreativeMinds.CQS.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class AsyncEventDispatcherBase {
		protected ILogger logger;
		protected abstract IEnumerable<IAsyncEventHandler<TEvent>> GetEventHandlers<TEvent>() where TEvent : IEvent;

		protected AsyncEventDispatcherBase(ILogger logger) {
			this.logger = logger;
		}

		public void PublishAsync<TEvent>(TEvent eventMessage) where TEvent : IEvent {
			this.logger.LogDebug($"Publishing the {typeof(TEvent).GetType().Name} event.", eventMessage);
			IEnumerable<IAsyncEventHandler<TEvent>> handlers = this.GetEventHandlers<TEvent>();
			this.logger.LogDebug($"Found {handlers.Count()} event handlers.", eventMessage);
			foreach (IAsyncEventHandler<TEvent> handler in handlers) {
				try {
					handler.HandleAsync(eventMessage);
				}
				catch (Exception ex) {
					this.logger.LogCritical($"An exception was throw when trying to publish a {eventMessage.GetType().Name} event using the {handler.GetType().Name} handler", ex, eventMessage);
				}
			}
		}
	}
}
