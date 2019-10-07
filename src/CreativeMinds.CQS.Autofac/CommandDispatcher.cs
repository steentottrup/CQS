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
		private readonly ILifetimeScope scope;

		public CommandDispatcher(ILogger<CommandDispatcher> logger, ILifetimeScope scope) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.scope = scope;
		}

		protected override IGenericPermissionCheckCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
			return this.scope.Resolve<IGenericPermissionCheckCommandHandlerDecorator<TCommand>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}

		protected override IGenericValidationCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
			return this.scope.Resolve<IGenericValidationCommandHandlerDecorator<TCommand>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}

		protected override ICommandHandler<TCommand> GetCommandHandler<TCommand>() {
			return this.scope.Resolve<ICommandHandler<TCommand>>();
		}
	}
}
