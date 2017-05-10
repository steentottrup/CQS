using System;
using System.Security.Principal;

namespace CreativeMinds.CQS.Permissions {

	public interface IPermissionCheck<TMessage> where TMessage : IMessage {
		IPermissionCheckResult Check(TMessage message, IIdentity user);
	}
}
