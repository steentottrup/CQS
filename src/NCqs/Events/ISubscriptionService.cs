using System;
using System.Collections.Generic;

namespace NCqs.Events {

	public interface ISubscriptionService {
		IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>() where TEvent : IEvent;
	}
}