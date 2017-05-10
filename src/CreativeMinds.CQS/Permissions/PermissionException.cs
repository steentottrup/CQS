using System;

namespace CreativeMinds.CQS.Permissions {

	public class PermissionException : Exception {
		public readonly string FailureMessage;
		public readonly int Code;

		public PermissionException(Int32 code, String message) : base($"{code} - {message}") {
			this.FailureMessage = message;
			this.Code = code;
		}
	}
}