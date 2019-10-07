using Autofac;
using Autofac.Core;
using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using System;

namespace CreativeMinds.CQS.Autofac {

	public class AsyncCommandDispatcher : AsyncCommandDispatcherBase {
		private readonly ILifetimeScope scope;

		public AsyncCommandDispatcher(ILogger<AsyncCommandDispatcher> logger, ILifetimeScope scope) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.scope = scope;
		}

		protected override IAsyncCommandHandler<TCommand> GetCommandHandler<TCommand>() {
			return this.scope.Resolve<IAsyncCommandHandler<TCommand>>();
		}

		protected override IGenericPermissionCheckAsyncCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(IAsyncCommandHandler<TCommand> wrappedHandler) {
			return this.scope.Resolve<IGenericPermissionCheckAsyncCommandHandlerDecorator<TCommand>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}

		protected override IGenericValidationAsyncCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>(IAsyncCommandHandler<TCommand> wrappedHandler) {
			return this.scope.Resolve<IGenericValidationAsyncCommandHandlerDecorator<TCommand>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}
	}
}
