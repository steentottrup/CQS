using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class CommandDispatcherBase : ICommandDispatcher {

		protected abstract IGenericValidationCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>() where TCommand : ICommand;
		protected abstract IGenericPermissionCheckCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(ICommandHandler<TCommand> validationHandler) where TCommand : ICommand;
		protected abstract ICommandHandler<TCommand> GetCommandHandler<TCommand>() where TCommand : ICommand;

		protected virtual ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand {
			ICommandHandler<TCommand> handler = null;
			try {
				IEnumerable<Attribute> attrs = typeof(TCommand).GetTypeInfo().GetCustomAttributes();
				if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.ValidateAttribute))) {
					handler = this.GetValidationHandler<TCommand>();
				}

				if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute)) ||
					attrs.Any(a => a.GetType().GetTypeInfo().BaseType == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute))) {

					handler = this.GetPermissionCheckHandler<TCommand>(handler);
				}

				if (handler == null) {
					handler = this.GetCommandHandler<TCommand>();
				}
			}
			catch (Exception ex) {
				// TODO: log
				throw ex;
			}

			if (handler == null) {
				throw new RequiredHandlerNotFoundException();
			}

			return handler;
		}

		public virtual void Dispatch<TCommand>(TCommand command) where TCommand : ICommand {
			ICommandHandler<TCommand> handler = this.Resolve<TCommand>();
			handler.Execute(command);
		}

		//public virtual Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand {
		//	ICommandHandler<TCommand> handler = this.Resolve<TCommand>();
		//	return handler.ExecuteAsync(command);
		//}
	}
}
