using CreativeMinds.CQS.Validators;
using CreativeMinds.CQS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQS.Validators {

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
	}
}
