using Autofac;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CreativeMinds.CQS.Autofac {

	public class EventDispatcher : EventDispatcherBase {
		private readonly ILifetimeScope scope;

		public EventDispatcher(ILogger<EventDispatcher> logger, ILifetimeScope scope) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.scope = scope;
		}

		protected override IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() {
			return this.scope.Resolve<IEnumerable<IEventHandler<TEvent>>>();
		}
	}
}
