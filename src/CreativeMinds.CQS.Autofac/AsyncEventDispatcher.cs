using Autofac;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CreativeMinds.CQS.Autofac {

	public class AsyncEventDispatcher : AsyncEventDispatcherBase {
		private readonly ILifetimeScope scope;

		public AsyncEventDispatcher(ILogger<AsyncEventDispatcher> logger, ILifetimeScope scope) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.scope = scope;
		}

		protected override IEnumerable<IAsyncEventHandler<TEvent>> GetEventHandlers<TEvent>() {
			return this.scope.Resolve<IEnumerable<IAsyncEventHandler<TEvent>>>();
		}
	}
}
