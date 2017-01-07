using System;
using System.Collections.Generic;

namespace NCqs.Validators {

	public class ValidationException : Exception {
		public readonly IEnumerable<ValidationFailure> Errors;

		public ValidationException(IEnumerable<ValidationFailure> errors) : base("todo") {
			this.Errors = errors;
		}
	}
}