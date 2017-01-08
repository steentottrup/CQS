using System;

namespace CreativeMinds.CQS.Permissions {

	public class PermissionException : Exception {
		public readonly string FailureMessage;
		public readonly int Code;

		public PermissionException(String message, Int32 code) : base("todo") {
			this.FailureMessage = message;
			this.Code = code;
		}
	}
}