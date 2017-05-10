using System;

namespace CreativeMinds.CQS.Permissions {

	public class PermissionCheckResult : IPermissionCheckResult {
		public Boolean HasPermissions { get; set; }
		public String ErrorMessage { get; set; }
		public Int32 ErrorCode { get; set; }
	}
}
