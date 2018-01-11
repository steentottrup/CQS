using CreativeMinds.CQS.Commands;
using System;
using System.Security.Principal;

namespace CreativeMinds.CQS.Permissions {

	public class GenericPermissionCheckCommandHandlerDecorator<TCommand> : IGenericPermissionCheckCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly ICommandHandler<TCommand> wrappedHandler;
		private readonly IIdentity currentUser;
		private readonly IPermissionCheck<TCommand> check;

		public GenericPermissionCheckCommandHandlerDecorator(ICommandHandler<TCommand> wrappedHandler, IIdentity currentUser, IPermissionCheck<TCommand> check) {
			this.wrappedHandler = wrappedHandler;
			this.currentUser = currentUser;
			this.check = check;
		}

		protected void PerformCheck(TCommand command) {
			IPermissionCheckResult result = this.check.Check(command, this.currentUser);
			if (!result.HasPermissions) {
				throw new PermissionException(result.ErrorCode, result.ErrorMessage);
			}
		}

		public void Execute(TCommand command) {
			this.PerformCheck(command);
			this.wrappedHandler.Execute(command);
		}
	}
}