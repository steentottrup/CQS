using CreativeMinds.CQS.Queries;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Permissions {

	public abstract class AsyncQueryPermissionCheckBase<TQuery, TResult> : IAsyncPermissionCheck<TQuery> where TQuery : IQuery<TResult> where TResult : class {
		protected virtual String ErrorMessage { get; set; }
		protected virtual Int32 ErrorCode { get; set; }

		protected AsyncQueryPermissionCheckBase() { }

		protected AsyncQueryPermissionCheckBase(Int32 errorCode, String errorMessage) {
			this.ErrorCode = errorCode;
			this.ErrorMessage = errorMessage;
		}

		protected abstract Task<Boolean> CheckPermissionsAsync(TQuery query, IIdentity user, CancellationToken cancellationToken);

		public async Task<IPermissionCheckResult> CheckAsync(TQuery message, IIdentity user, CancellationToken cancellationToken) {
			Boolean hasPermissions = await this.CheckPermissionsAsync(message, user, cancellationToken);
			return new PermissionCheckResult {
				HasPermissions = hasPermissions,
				ErrorCode = hasPermissions ? 0 : this.ErrorCode,
				ErrorMessage = hasPermissions ? String.Empty : this.ErrorMessage
			};
		}
	}
}
