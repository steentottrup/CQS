using Autofac;
using Autofac.Core;
using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using System;

namespace CreativeMinds.CQS.Autofac {

	public class CommandDispatcher : CommandDispatcherBase {
		private readonly IContainer container;

		public CommandDispatcher(ILogger logger, IContainer container) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.container = container;
		}

		protected override IGenericPermissionCheckCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
			return this.container.Resolve<IGenericPermissionCheckCommandHandlerDecorator<TCommand>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}

		protected override IGenericValidationCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
			return this.container.Resolve<IGenericValidationCommandHandlerDecorator<TCommand>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}

		protected override ICommandHandler<TCommand> GetCommandHandler<TCommand>() {
			return this.container.Resolve<ICommandHandler<TCommand>>();
		}
	}
}
