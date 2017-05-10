using CreativeMinds.CQS.Queries;
using System;
using System.Security.Principal;

namespace CreativeMinds.CQS.Permissions {

	public class GenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> : IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IIdentity currentUser;
		private readonly IPermissionCheck<TQuery> check;

		public GenericPermissionCheckQueryHandlerDecorator(IQueryHandler<TQuery, TResult> wrappedHandler, IIdentity currentUser, IPermissionCheck<TQuery> check) {
			this.wrappedHandler = wrappedHandler;
			this.currentUser = currentUser;
			this.check = check;
		}

		protected void PerformCheck(TQuery query) {
			IPermissionCheckResult result = this.check.Check(query, this.currentUser);
			if (!result.HasPermissions) {
				throw new PermissionException(result.ErrorCode, result.ErrorMessage);
			}
		}

		public TResult Handle(TQuery query) {
			this.PerformCheck(query);
			return this.wrappedHandler.Handle(query);
		}

		//public Task<TResult> HandleAsync(TQuery query) {
		//	this.PerformCheck(query);
		//	return this.wrappedHandler.HandleAsync(query);
		//}
	}
}
