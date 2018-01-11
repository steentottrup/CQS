using CreativeMinds.CQS.Commands;
using System;
using System.Security.Principal;
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

		protected abstract Task<Boolean> CheckPermissionsAsync(TCommand command, IIdentity user);

		public virtual async Task<IPermissionCheckResult> CheckAsync(TCommand command, IIdentity user) {
			Boolean hasPermissions = await this.CheckPermissionsAsync(command, user);
			return new PermissionCheckResult {
				HasPermissions = hasPermissions,
				ErrorCode = hasPermissions ? 0 : this.ErrorCode,
				ErrorMessage = hasPermissions ? string.Empty : this.ErrorMessage
			};
		}
	}
}
