using CreativeMinds.CQS.Dispatchers;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Queries;
using CreativeMinds.CQS.Validators;
using Ninject;
using Ninject.Parameters;
using System;

namespace CreativeMinds.CQS.Ninject {

	public class QueryDispatcher : QueryDispatcherBase {
		private readonly IReadOnlyKernel kernel;

		public QueryDispatcher(IReadOnlyKernel kernel) {
			this.kernel = kernel;
		}

		protected override IGenericValidationQueryHandlerDecorator<TQuery, TResult> GetValidationHandler<TQuery, TResult>() {
			return this.kernel.Get<IGenericValidationQueryHandlerDecorator<TQuery, TResult>>();
		}

		protected override IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> GetPermissionCheckHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> validationHandler) {
			if (validationHandler != null) {
				return this.kernel.Get<IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult>>(new IParameter[] { new ConstructorArgument("wrappedHandler", validationHandler) });
			}
			return this.kernel.Get<IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult>>();
		}

		protected override IQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>() {
			return this.kernel.Get<IQueryHandler<TQuery, TResult>>();
		}
	}
}
