using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Queries;
using CreativeMinds.CQS.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class QueryDispatcherBase : IQueryDispatcher {

		protected abstract IGenericValidationQueryHandlerDecorator<TQuery, TResult> GetValidationHandler<TQuery, TResult>() where TQuery : IQuery<TResult>;
		protected abstract IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> GetPermissionCheckHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> validationHandler) where TQuery : IQuery<TResult>;
		protected abstract IQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>() where TQuery : IQuery<TResult>;

		protected virtual IQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>() where TQuery : IQuery<TResult> {
			IQueryHandler<TQuery, TResult> handler = null;
			try {
				IEnumerable<Attribute> attrs = typeof(TQuery).GetTypeInfo().GetCustomAttributes();
				if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.ValidateAttribute))) {
					handler = this.GetValidationHandler<TQuery, TResult>();
				}

				if (attrs.Any(a => a.GetType() == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute)) ||
					attrs.Any(a => a.GetType().GetTypeInfo().BaseType == typeof(CreativeMinds.CQS.Decorators.CheckPermissionsAttribute))) {

					handler = this.GetPermissionCheckHandler<TQuery, TResult>(handler);
				}

				if (handler == null) {
					handler = this.GetQueryHandler<TQuery, TResult>();
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
