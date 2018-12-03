using Autofac;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CreativeMinds.CQS.Autofac {

	public class EventDispatcher : EventDispatcherBase {
		private readonly IContainer container;

		public EventDispatcher(ILogger<EventDispatcher> logger, IContainer container) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.container = container;
		}

		protected override IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() {
			return this.container.Resolve<IEnumerable<IEventHandler<TEvent>>>();
		}
	}
}
