using NCqs.Commands;
using System;
using System.Security.Principal;

namespace NCqs.Permissions {

	public class GenericPermissionCheckCommandHandlerDecorator<TCommand> : IGenericPermissionCheckCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly ICommandHandler<TCommand> wrappedHandler;
		private readonly IPrincipal currentUser;
		private readonly IPermissionCheck<TCommand> check;

		public GenericPermissionCheckCommandHandlerDecorator(ICommandHandler<TCommand> wrappedHandler, IPrincipal currentUser, IPermissionCheck<TCommand> check) {
			this.wrappedHandler = wrappedHandler;
			this.currentUser = currentUser;
			this.check = check;
		}

		protected void PerformCheck(TCommand command) {
			Tuple<Boolean, String, Int32> tuple = this.check.Check(command, this.currentUser);
			if (!tuple.Item1) {
				throw new PermissionException(tuple.Item2, tuple.Item3);
			}
		}

		public void Execute(TCommand command) {
			this.PerformCheck(command);
			this.wrappedHandler.Execute(command);
		}

		//public Task ExecuteAsync(TCommand command) {
		//	this.PerformCheck(command);
		//	return this.wrappedHandler.ExecuteAsync(command);
		//}
	}
}