using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class AsyncCommandDispatcherBase : IAsyncCommandDispatcher {

		protected ILogger logger;
		protected abstract IGenericValidationAsyncCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>(IAsyncCommandHandler<TCommand> wrappedHandler) where TCommand : ICommand;
		protected abstract IGenericPermissionCheckAsyncCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(IAsyncCommandHandler<TCommand> wrappedHandler) where TCommand : ICommand;
		protected abstract IAsyncCommandHandler<TCommand> GetCommandHandler<TCommand>() where TCommand : ICommand;

		protected AsyncCommandDispatcherBase(ILogger<AsyncCommandDispatcherBase> logger) {
			this.logger = logger;
		}

		protected virtual IAsyncCommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand {
			this.logger.LogDebug($"Resolving the \"{typeof(TCommand).Name}\" command.");
			//try {
			// Let's get the command handler, we're going to need this, no matter what!!
			IAsyncCommandHandler<TCommand> handler = this.GetCommandHandler<TCommand>();
			if (handler == null) {
				this.logger.LogCritical($"Trying to resolve a handler for the \"{typeof(TCommand).Name}\" command failed. No handler found.");
				throw new RequiredHandlerNotFoundException();
			}

			IEnumerable<Attribute> attrs = typeof(TCommand).GetTypeInfo().GetCustomAttributes();
			// Any permission check decorator found on the command?
			if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute)) ||
				attrs.Any(a => a.GetType().GetTypeInfo().BaseType == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute))) {

				this.logger.LogDebug($"Found a permission check decorator for the \"{typeof(TCommand).Name}\" command.");
				// Let's get the permission check handler, we need it.
				IAsyncCommandHandler<TCommand> permissionCheckHandler = this.GetPermissionCheckHandler<TCommand>(handler);
				if (permissionCheckHandler == null) {
					this.logger.LogWarning($"A permission check decorator was found for the \"{typeof(TCommand).GetTypeInfo().Name}\" command, but no handler was located.");
				}
				else {
					handler = permissionCheckHandler;
				}
			}
			else {
				this.logger.LogWarning($"No permission decorator found for the \"{typeof(TCommand).Name}\" command.");
			}

			// Any validation decorator found on the command?
			if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.ValidateAttribute))) {
				this.logger.LogDebug($"Found a validation decorator for the \"{typeof(TCommand).Name}\" command.");
				// Let's get the validation handler, we need it.
				IAsyncCommandHandler<TCommand> validationHandler = this.GetValidationHandler<TCommand>(handler);
				if (validationHandler == null) {
					this.logger.LogWarning($"A validation decorator was found for the \"{typeof(TCommand).GetTypeInfo().Name}\" command, but no handler was located.");
				}
				else {
					handler = validationHandler;
				}
			}
			else {
				this.logger.LogWarning($"No validation decorator found for the \"{typeof(TCommand).Name}\" command.");
			}

			//}
			//catch (Exception ex) {
			//	logger.LogError("An exception occured trying to ")
			//	throw ex;
			//}

			this.logger.LogInformation($"Found a CommandHandler for the \"{typeof(TCommand).GetTypeInfo().Name}\" command");
			return handler;
		}

		public virtual Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand {
			IAsyncCommandHandler<TCommand> handler = this.Resolve<TCommand>();
			return handler.ExecuteAsync(command, cancellationToken);
		}
	}
}
