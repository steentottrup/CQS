using Autofac;
using Autofac.Core;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Queries;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using System;

namespace CreativeMinds.CQS.Autofac {

	public class AsyncQueryDispatcher : AsyncQueryDispatcherBase {
		private readonly IContainer container;

		public AsyncQueryDispatcher(ILogger<AsyncQueryDispatcher> logger, IContainer container) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.container = container;
		}

		protected override IGenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult> GetPermissionCheckHandler<TQuery, TResult>(IAsyncQueryHandler<TQuery, TResult> wrappedHandler) {
			return this.container.Resolve<IGenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}

		protected override IAsyncQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>() {
			return this.container.Resolve<IAsyncQueryHandler<TQuery, TResult>>();
		}

		protected override IGenericValidationAsyncQueryHandlerDecorator<TQuery, TResult> GetValidationHandler<TQuery, TResult>(IAsyncQueryHandler<TQuery, TResult> wrappedHandler) {
			return this.container.Resolve<IGenericValidationAsyncQueryHandlerDecorator<TQuery, TResult>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}
	}
}
