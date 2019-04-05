using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Queries;
using CreativeMinds.CQS.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class AsyncQueryDispatcherBase : IAsyncQueryDispatcher {
		protected ILogger logger;
		protected abstract IGenericValidationAsyncQueryHandlerDecorator<TQuery, TResult> GetValidationHandler<TQuery, TResult>(IAsyncQueryHandler<TQuery, TResult> wrappedHandler) where TQuery : IQuery<TResult>;
		protected abstract IGenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult> GetPermissionCheckHandler<TQuery, TResult>(IAsyncQueryHandler<TQuery, TResult> wrappedHandler) where TQuery : IQuery<TResult>;
		protected abstract IAsyncQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>() where TQuery : IQuery<TResult>;

		protected AsyncQueryDispatcherBase(ILogger logger) {
			this.logger = logger;
		}

		protected virtual IAsyncQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>(CancellationToken cancellationToken) where TQuery : IQuery<TResult> {
			cancellationToken.ThrowIfCancellationRequested();

			this.logger.LogDebug($"Resolving the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result.");
			IAsyncQueryHandler<TQuery, TResult> handler = this.GetQueryHandler<TQuery, TResult>();
			if (handler == null) {
				this.logger.LogCritical($"Trying to resolve a handler for the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result failed. No handler found.");
				throw new RequiredHandlerNotFoundException();
			}

			//try {
			IEnumerable<Attribute> attrs = typeof(TQuery).GetTypeInfo().GetCustomAttributes();
			// Any permission check decorator found on the command?
			if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute)) ||
				attrs.Any(a => a.GetType().GetTypeInfo().BaseType == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute))) {

				this.logger.LogDebug($"Found a permission check decorator for the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result.");
				// Let's get the permission check handler, we need it.
				IAsyncQueryHandler<TQuery, TResult> permissionCheckHandler = this.GetPermissionCheckHandler<TQuery, TResult>(handler);
				if (permissionCheckHandler == null) {
					this.logger.LogWarning($"A permission check decorator was found for the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result, but no handler was located.");
				}
				else {
					handler = permissionCheckHandler;
				}
			}
			else {
				this.logger.LogWarning($"No permission decorator found for the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result.");
			}

			// Any validation decorator found on the query?
			if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.ValidateAttribute))) {
				this.logger.LogDebug($"Found a validation decorator for the \"{typeof(TQuery).Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result.");
				// Let's get the validation handler, we need it.
				IAsyncQueryHandler<TQuery, TResult> validationHandler = this.GetValidationHandler<TQuery, TResult>(handler);
				if (validationHandler == null) {
					this.logger.LogWarning($"A validation decorator was found for the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result, but no handler was located.");
				}
				else {
					handler = validationHandler;
				}
			}
			else {
				this.logger.LogWarning($"No validation decorator found for the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result.");
			}

			//}
			//catch (Exception ex) {
			//	// TODO: log
			//	throw ex;
			//}

			this.logger.LogInformation($"Found a QueryHandler for the \"{typeof(TQuery).GetTypeInfo().Name}\" query and \"{typeof(TResult).GetTypeInfo().Name}\" result");
			return handler;
		}

		public virtual Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResult> {
			cancellationToken.ThrowIfCancellationRequested();

			IAsyncQueryHandler<TQuery, TResult> handler = this.Resolve<TQuery, TResult>(cancellationToken);
			return handler.HandleAsync(query, cancellationToken);
		}
	}
}
