using Autofac;
using Autofac.Core;
using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Queries;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using System;

namespace CreativeMinds.CQS.Autofac {

	public class QueryDispatcher : QueryDispatcherBase {
		private readonly IContainer container;

		public QueryDispatcher(ILogger logger, IContainer container) : base(logger) {
			// Needing the container itself is a bit of an anti-pattern, but unfortunately the building DI container in Core
			// can not do what we need it to do! So no way around this, at least not at the moment.
			this.container = container;
		}

		protected override IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> GetPermissionCheckHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> wrappedHandler) {
			return this.container.Resolve<IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}

		protected override IQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>() {
			return this.container.Resolve<IQueryHandler<TQuery, TResult>>();
		}

		protected override IGenericValidationQueryHandlerDecorator<TQuery, TResult> GetValidationHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> wrappedHandler) {
			return this.container.Resolve<IGenericValidationQueryHandlerDecorator<TQuery, TResult>>(new Parameter[] { new NamedParameter("wrappedHandler", wrappedHandler) });
		}
	}
}
