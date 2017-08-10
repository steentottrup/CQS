using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using Ninject;
using Ninject.Parameters;
using System;

namespace CreativeMinds.CQS.Ninject {

	public class CommandDispatcher : CommandDispatcherBase {
		private readonly IReadOnlyKernel kernel;

		public CommandDispatcher(ILogger logger, IReadOnlyKernel kernel) : base(logger) {
			this.kernel = kernel;
		}

		protected override IGenericPermissionCheckCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
			return this.kernel.Get<IGenericPermissionCheckCommandHandlerDecorator<TCommand>>(new IParameter[] { new ConstructorArgument("wrappedHandler", wrappedHandler) });
		}

		protected override IGenericValidationCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
			return this.kernel.Get<IGenericValidationCommandHandlerDecorator<TCommand>>(new IParameter[] { new ConstructorArgument("wrappedHandler", wrappedHandler) });
		}

		protected override ICommandHandler<TCommand> GetCommandHandler<TCommand>() {
			return this.kernel.Get<ICommandHandler<TCommand>>();
		}
	}
}
