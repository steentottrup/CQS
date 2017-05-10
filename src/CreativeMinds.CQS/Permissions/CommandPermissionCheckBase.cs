using CreativeMinds.CQS.Commands;
using System;
using System.Security.Principal;

namespace CreativeMinds.CQS.Permissions {

	public abstract class CommandPermissionCheckBase<TCommand> : IPermissionCheck<TCommand> where TCommand : ICommand {
		protected virtual String ErrorMessage { get; set; }
		protected virtual Int32 ErrorCode { get; set; }

		protected CommandPermissionCheckBase() { }

		protected CommandPermissionCheckBase(Int32 errorCode, String errorMessage) {
			this.ErrorCode = errorCode;
			this.ErrorMessage = errorMessage;
		}

		protected abstract Boolean CheckPermissions(TCommand command, IIdentity user);

		public virtual IPermissionCheckResult Check(TCommand command, IIdentity user) {
			Boolean hasPermissions = this.CheckPermissions(command, user);
			return new PermissionCheckResult {
				HasPermissions = hasPermissions,
				ErrorCode = hasPermissions ? 0 : this.ErrorCode,
				ErrorMessage = hasPermissions ? string.Empty : this.ErrorMessage
			};
		}
	}
}
