using CreativeMinds.CQS.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Permissions {

	public class GenericPermissionCheckAsyncCommandHandlerDecorator<TCommand> : IGenericPermissionCheckAsyncCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly IAsyncCommandHandler<TCommand> wrappedHandler;
		private readonly IIdentity currentUser;
		private readonly IAsyncPermissionCheck<TCommand> check;
		protected readonly ILogger logger;

		public GenericPermissionCheckAsyncCommandHandlerDecorator(IAsyncCommandHandler<TCommand> wrappedHandler, IIdentity currentUser, IAsyncPermissionCheck<TCommand> check, ILogger<GenericPermissionCheckAsyncCommandHandlerDecorator<TCommand>> logger) {
			this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
			this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
			this.check = check ?? throw new ArgumentNullException(nameof(check));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected async Task PerformCheckAsync(TCommand command, CancellationToken cancellationToken) {
			IPermissionCheckResult result = await this.check.CheckAsync(command, this.currentUser, cancellationToken);
			if (!result.HasPermissions) {
				this.logger.LogCritical("Command handler permission check returned NO!", result.ErrorMessage);
				throw new PermissionException(result.ErrorCode, result.ErrorMessage);
			}
		}

		public async Task ExecuteAsync(TCommand command, CancellationToken cancellationToken) {
			await this.PerformCheckAsync(command, cancellationToken);
			await this.wrappedHandler.ExecuteAsync(command, cancellationToken);
		}
	}
}
