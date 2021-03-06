﻿using System;
using System.Collections.Generic;

namespace CreativeMinds.CQS.Validators {

	public class ValidationException : Exception {
		public readonly IEnumerable<ValidationFailure> Errors;

		public ValidationException(IEnumerable<ValidationFailure> errors) : base($"{String.Join(", ", errors)}") {
			this.Errors = errors;
		}
	}
}