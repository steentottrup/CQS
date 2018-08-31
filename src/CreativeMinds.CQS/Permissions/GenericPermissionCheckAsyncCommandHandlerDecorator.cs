using CreativeMinds.CQS.Commands;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Permissions {

	public class GenericPermissionCheckAsyncCommandHandlerDecorator<TCommand> : IGenericPermissionCheckAsyncCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly IAsyncCommandHandler<TCommand> wrappedHandler;
		private readonly IIdentity currentUser;
		private readonly IAsyncPermissionCheck<TCommand> check;

		public GenericPermissionCheckAsyncCommandHandlerDecorator(IAsyncCommandHandler<TCommand> wrappedHandler, IIdentity currentUser, IAsyncPermissionCheck<TCommand> check) {
			this.wrappedHandler = wrappedHandler;
			this.currentUser = currentUser;
			this.check = check;
		}

		protected async Task PerformCheckAsync(TCommand command, CancellationToken cancellationToken) {
			IPermissionCheckResult result = await this.check.CheckAsync(command, this.currentUser, cancellationToken);
			if (!result.HasPermissions) {
				throw new PermissionException(result.ErrorCode, result.ErrorMessage);
			}
		}

		public async Task ExecuteAsync(TCommand command, CancellationToken cancellationToken) {
			await this.PerformCheckAsync(command, cancellationToken);
			await this.wrappedHandler.ExecuteAsync(command, cancellationToken);
		}
	}
}
