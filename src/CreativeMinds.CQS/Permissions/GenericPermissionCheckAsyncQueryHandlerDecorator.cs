using CreativeMinds.CQS.Queries;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Permissions {

	public class GenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult> : IGenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IAsyncQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IIdentity currentUser;
		private readonly IAsyncPermissionCheck<TQuery> check;

		public GenericPermissionCheckAsyncQueryHandlerDecorator(IAsyncQueryHandler<TQuery, TResult> wrappedHandler, IIdentity currentUser, IAsyncPermissionCheck<TQuery> check) {
			this.wrappedHandler = wrappedHandler;
			this.currentUser = currentUser;
			this.check = check;
		}

		protected async Task PerformCheckAsync(TQuery query) {
			IPermissionCheckResult result = await this.check.CheckAsync(query, this.currentUser);
			if (!result.HasPermissions) {
				throw new PermissionException(result.ErrorCode, result.ErrorMessage);
			}
		}

		public async Task<TResult> HandleAsync(TQuery query) {
			await this.PerformCheckAsync(query);
			return await this.wrappedHandler.HandleAsync(query);
		}
	}
}
