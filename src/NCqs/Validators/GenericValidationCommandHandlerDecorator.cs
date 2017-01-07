using NCqs.Validators;
using NCqs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCqs.Validators {

	public class GenericValidationCommandHandlerDecorator<TCommand> : IGenericValidationCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly ICommandHandler<TCommand> wrappedHandler;
		private readonly IEnumerable<IValidator<TCommand>> validators;

		public GenericValidationCommandHandlerDecorator(ICommandHandler<TCommand> wrappedHandler, IEnumerable<IValidator<TCommand>> validators) {
			this.wrappedHandler = wrappedHandler;
			this.validators = validators;
		}

		public void Execute(TCommand command) {
			if (this.validators.Any()) {
				List<ValidationResult> results = new List<ValidationResult>();
				this.validators.ToList().ForEach(validator => {
					results.Add(validator.Validate(command));
				});

				if (results.Any(r => r.Errors.Any())) {
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}
			this.wrappedHandler.Execute(command);
		}

		//public async Task ExecuteAsync(TCommand command) {
		//	if (this.validators.Any()) {
		//		List<ValidationResult> results = new List<ValidationResult>();
		//		foreach (var validator in this.validators) {
		//			ValidationResult result = await validator.ValidateAsync(command);
		//			results.Add(result);
		//		}

		//		if (results.Any(r => r.Errors.Any())) {
		//			throw new ValidationException(results.SelectMany(r => r.Errors));
		//		}
		//	}
		//	await this.wrappedHandler.ExecuteAsync(command);
		//}
	}
}
