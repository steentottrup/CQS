﻿using System;

namespace CreativeMinds.CQS.Decorators {

	[AttributeUsage(AttributeTargets.Class)]
	public class ValidateAttribute : Attribute {
		protected readonly Boolean FailWhenMissing;

		public ValidateAttribute(Boolean failWhenMissing = true) {
			this.FailWhenMissing = failWhenMissing;
		}
	}
}
