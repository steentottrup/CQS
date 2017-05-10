using System;

namespace CreativeMinds.CQS.Permissions {

	public interface IPermissionCheckResult {
		Boolean HasPermissions { get; }
		String ErrorMessage { get; }
		Int32 ErrorCode { get; }
	}
}
