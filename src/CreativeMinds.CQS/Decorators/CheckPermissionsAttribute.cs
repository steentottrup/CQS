using System;

namespace CreativeMinds.CQS.Decorators {

	[AttributeUsage(AttributeTargets.Class)]
	public class CheckPermissionsAttribute : Attribute {
		protected readonly Boolean FailWhenMissing;

		public CheckPermissionsAttribute(Boolean failWhenMissing = true) {
			this.FailWhenMissing = failWhenMissing;
		}
	}
}
