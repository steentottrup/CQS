using NCqs.Events;
using System;
using System.Collections.Generic;

namespace NCqs.Resolvers {

	public interface IEventHandlerResolver {
		IEnumerable<IEventHandler<TEvent>> Resolve<TEvent>() where TEvent : IEvent;
	}
}
