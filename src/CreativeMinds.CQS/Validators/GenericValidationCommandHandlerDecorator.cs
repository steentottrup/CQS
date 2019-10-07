using CreativeMinds.CQS.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQS.Validators {

	public class GenericValidationCommandHandlerDecorator<TCommand> : IGenericValidationCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly ICommandHandler<TCommand> wrappedHandler;
		private readonly IEnumerable<IValidator<TCommand>> validators;
		protected readonly ILogger logger;

		public GenericValidationCommandHandlerDecorator(ICommandHandler<TCommand> wrappedHandler, IEnumerable<IValidator<TCommand>> validators, ILogger<GenericValidationCommandHandlerDecorator<TCommand>> logger) {
			this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
			this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void Execute(TCommand command) {
			if (this.validators.Any()) {
				this.logger.LogInformation($"Command handler validation(s) found, count {this.validators.Count()}", this.validators);

				List<ValidationResult> results = new List<ValidationResult>();
				this.validators.ToList().ForEach(validator => {
					results.Add(validator.Validate(command));
				});

				if (results.Any(r => r.Errors.Any())) {
					this.logger.LogCritical("Command handler validations returned errors", results);
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}
			else {
				this.logger.LogWarning($"A validation decorator was found, but no validations for \"{typeof(TCommand).Name}\" command");
			}
			this.wrappedHandler.Execute(command);
		}
	}
}
