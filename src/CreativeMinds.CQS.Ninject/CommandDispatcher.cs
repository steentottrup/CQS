using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Validators;
using Ninject;
using Ninject.Parameters;
using System;

namespace CreativeMinds.CQS.Ninject {

	public class CommandDispatcher : CommandDispatcherBase {
		private readonly IReadOnlyKernel kernel;

		public CommandDispatcher(IReadOnlyKernel kernel) {
			this.kernel = kernel;
		}

		protected override IGenericPermissionCheckCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(ICommandHandler<TCommand> validationHandler) {
			if (validationHandler != null) {
				return this.kernel.Get<IGenericPermissionCheckCommandHandlerDecorator<TCommand>>(new IParameter[] { new ConstructorArgument("wrappedHandler", validationHandler) });
			}
			return this.kernel.Get<IGenericPermissionCheckCommandHandlerDecorator<TCommand>>();
		}

		protected override IGenericValidationCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>() {
			return this.kernel.Get<IGenericValidationCommandHandlerDecorator<TCommand>>();
		}

		protected override ICommandHandler<TCommand> GetCommandHandler<TCommand>() {
			return this.kernel.Get<ICommandHandler<TCommand>>();
		}
	}
}
