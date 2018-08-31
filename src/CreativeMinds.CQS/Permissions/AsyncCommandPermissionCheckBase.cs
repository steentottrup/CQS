using CreativeMinds.CQS.Commands;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Permissions {

	public abstract class AsyncCommandPermissionCheckBase<TCommand> : IAsyncPermissionCheck<TCommand> where TCommand : ICommand {
		protected virtual String ErrorMessage { get; set; }
		protected virtual Int32 ErrorCode { get; set; }

		protected AsyncCommandPermissionCheckBase() { }

		protected AsyncCommandPermissionCheckBase(Int32 errorCode, String errorMessage) {
			this.ErrorCode = errorCode;
			this.ErrorMessage = errorMessage;
		}

		protected abstract Task<Boolean> CheckPermissionsAsync(TCommand command, IIdentity user, CancellationToken cancellationToken);

		public virtual async Task<IPermissionCheckResult> CheckAsync(TCommand command, IIdentity user, CancellationToken cancellationToken) {
			Boolean hasPermissions = await this.CheckPermissionsAsync(command, user, cancellationToken);
			return new PermissionCheckResult {
				HasPermissions = hasPermissions,
				ErrorCode = hasPermissions ? 0 : this.ErrorCode,
				ErrorMessage = hasPermissions ? String.Empty : this.ErrorMessage
			};
		}
	}
}
