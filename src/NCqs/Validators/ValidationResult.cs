using System;
using System.Collections.Generic;
using System.Linq;

namespace NCqs.Validators {

	public class ValidationResult {
		private readonly List<ValidationFailure> errors;

		public IEnumerable<ValidationFailure> Errors {
			get {
				return this.errors;
			}
		}

		public bool IsValid {
			get {
				return !this.Errors.Any<ValidationFailure>();
			}
		}

		public ValidationResult() {
			this.errors = new List<ValidationFailure>();
		}

		public void AddError(string message, int code) {
			this.errors.Add(new ValidationFailure() {
				Message = message,
				Code = code
			});
		}
	}
}
