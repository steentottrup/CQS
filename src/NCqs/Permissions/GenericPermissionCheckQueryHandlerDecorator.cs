using NCqs.Queries;
using System;
using System.Security.Principal;

namespace NCqs.Permissions {

	public class GenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> : IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IPrincipal currentUser;
		private readonly IPermissionCheck<TQuery> check;

		public GenericPermissionCheckQueryHandlerDecorator(IQueryHandler<TQuery, TResult> wrappedHandler, IPrincipal currentUser, IPermissionCheck<TQuery> check) {
			this.wrappedHandler = wrappedHandler;
			this.currentUser = currentUser;
			this.check = check;
		}

		protected void PerformCheck(TQuery query) {
			Tuple<Boolean, String, Int32> tuple = this.check.Check(query, this.currentUser);
			if (!tuple.Item1) {
				throw new PermissionException(tuple.Item2, tuple.Item3);
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
