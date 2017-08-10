using CreativeMinds.CQS.Dispatchers;
using System;
using CreativeMinds.CQS.Events;
using System.Collections.Generic;
using Ninject;
using Microsoft.Extensions.Logging;

namespace CreativeMinds.CQS.Ninject {

	public class EventDispatcher : EventDispatcherBase {
		private readonly IReadOnlyKernel kernel;

		public EventDispatcher(ILogger logger, IReadOnlyKernel kernel) : base(logger) {
			this.kernel = kernel;
		}

		protected override IEnumerable<IEventHandler<TEvent>> GetEventHandlers<TEvent>() {
			return this.kernel.GetAll<IEventHandler<TEvent>>();
		}
	}
}
