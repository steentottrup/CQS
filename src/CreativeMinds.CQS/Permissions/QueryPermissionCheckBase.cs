using CreativeMinds.CQS.Queries;
using System;
using System.Security.Principal;

namespace CreativeMinds.CQS.Permissions {

	public abstract class QueryPermissionCheckBase<TQuery, TResult> : IPermissionCheck<TQuery> where TQuery : IQuery<TResult> {
		protected virtual String ErrorMessage { get; set; }
		protected virtual Int32 ErrorCode { get; set; }

		protected QueryPermissionCheckBase() { }

		protected QueryPermissionCheckBase(Int32 errorCode, String errorMessage) {
			this.ErrorCode = errorCode;
			this.ErrorMessage = errorMessage;
		}

		protected abstract Boolean CheckPermissions(TQuery query, IIdentity user);

		public virtual IPermissionCheckResult Check(TQuery query, IIdentity user) {
			Boolean hasPermissions = this.CheckPermissions(query, user);
			return new PermissionCheckResult {
				HasPermissions = hasPermissions,
				ErrorCode = hasPermissions ? 0 : this.ErrorCode,
				ErrorMessage = hasPermissions ? string.Empty : this.ErrorMessage
			};
		}
	}
}
