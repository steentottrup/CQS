using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Permissions {

	public interface IAsyncPermissionCheck<TMessage> where TMessage : IMessage {
		Task<IPermissionCheckResult> CheckAsync(TMessage message, IIdentity user, CancellationToken cancellationToken);
	}
}
