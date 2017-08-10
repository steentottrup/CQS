using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Queries;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class QueryDispatcherBase : IQueryDispatcher {

		protected ILogger logger;
		protected abstract IGenericValidationQueryHandlerDecorator<TQuery, TResult> GetValidationHandler<TQuery, TResult>() where TQuery : IQuery<TResult>;
		protected abstract IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> GetPermissionCheckHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> validationHandler) where TQuery : IQuery<TResult>;
		protected abstract IQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>() where TQuery : IQuery<TResult>;

		protected QueryDispatcherBase(ILogger logger) {
			this.logger = logger;
		}

		protected virtual IQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>() where TQuery : IQuery<TResult> {
			this.logger.LogDebug($"Resolving the {typeof(TQuery).GetTypeInfo().Name} query and {typeof(TResult).GetTypeInfo().Name} result.");
			IQueryHandler<TQuery, TResult> handler = this.GetQueryHandler<TQuery, TResult>();
			if (handler == null) {
				this.logger.LogCritical($"Trying to resolve a handler for the {typeof(TQuery).GetTypeInfo().Name} query and {typeof(TResult).GetTypeInfo().Name} result failed. No handler found.");
				throw new RequiredHandlerNotFoundException();
			}

			//try {
			IEnumerable<Attribute> attrs = typeof(TQuery).GetTypeInfo().GetCustomAttributes();
			// Any validation decorator found on the query?
			if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.ValidateAttribute))) {
				this.logger.LogDebug($"Found a validation decorator for the {typeof(TQuery).Name} query and {typeof(TResult).GetTypeInfo().Name} result.");
				// Let's get the validation handler, we need it.
				IQueryHandler<TQuery, TResult> validationHandler = this.GetValidationHandler<TQuery, TResult>();
				if (validationHandler == null) {
					this.logger.LogWarning($"A validation decorator was found for the {typeof(TQuery).GetTypeInfo().Name} query and {typeof(TResult).GetTypeInfo().Name} result, but no handler was located.");
				}
				else {
					handler = validationHandler;
				}
			}

			// Any permission check decorator found on the command?
			if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute)) ||
				attrs.Any(a => a.GetType().GetTypeInfo().BaseType == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute))) {

				this.logger.LogDebug($"Found a permission check decorator for the {typeof(TQuery).GetTypeInfo().Name} query and {typeof(TResult).GetTypeInfo().Name} result.");
				// Let's get the permission check handler, we need it.
				IQueryHandler<TQuery, TResult> permissionCheckHandler = this.GetPermissionCheckHandler<TQuery, TResult>(handler);
				if (permissionCheckHandler == null) {
					this.logger.LogWarning($"A permission check decorator was found for the {typeof(TQuery).GetTypeInfo().Name} query and {typeof(TResult).GetTypeInfo().Name} result, but no handler was located.");
				}
				else {
					handler = permissionCheckHandler;
				}
			}

			//}
			//catch (Exception ex) {
			//	// TODO: log
			//	throw ex;
			//}

			return handler;
		}

		public virtual TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> {
			IQueryHandler<TQuery, TResult> handler = this.Resolve<TQuery, TResult>();
			return handler.Handle(query);
		}

		//public virtual Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> {
		//	IQueryHandler<TQuery, TResult> handler = this.Resolve<TQuery, TResult>();
		//	return handler.HandleAsync(query);
		//}
	}
}
