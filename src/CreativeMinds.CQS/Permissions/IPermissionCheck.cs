using System;
using System.Security.Principal;

namespace CreativeMinds.CQS.Permissions {

	public interface IPermissionCheck<TMessage> where TMessage : IMessage {
		Tuple<Boolean, String, Int32> Check(TMessage message, IPrincipal user);
	}
}
