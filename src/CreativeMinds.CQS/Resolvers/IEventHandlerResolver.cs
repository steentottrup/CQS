using CreativeMinds.CQS.Events;
using System;
using System.Collections.Generic;

namespace CreativeMinds.CQS.Resolvers {

	public interface IEventHandlerResolver {
		IEnumerable<IEventHandler<TEvent>> Resolve<TEvent>() where TEvent : IEvent;
	}
}
