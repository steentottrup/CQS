using CreativeMinds.CQS.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Validators {

	public class GenericValidationAsyncCommandHandlerDecorator<TCommand> : IGenericValidationAsyncCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly IAsyncCommandHandler<TCommand> wrappedHandler;
		private readonly IEnumerable<IAsyncValidator<TCommand>> validators;
		protected readonly ILogger logger;

		public GenericValidationAsyncCommandHandlerDecorator(IAsyncCommandHandler<TCommand> wrappedHandler, IEnumerable<IAsyncValidator<TCommand>> validators, ILogger<GenericValidationAsyncCommandHandlerDecorator<TCommand>> logger) {
			this.wrappedHandler = wrappedHandler;
			this.validators = validators;
			this.logger = logger;
		}

		public async Task ExecuteAsync(TCommand command) {
			if (this.validators.Any()) {
				this.logger.LogInformation("Command handler validations found", this.validators);
				List<ValidationResult> results = new List<ValidationResult>();
				foreach (var validator in this.validators) {
					ValidationResult result = await validator.ValidateAsync(command);
					results.Add(result);
				}

				if (results.Any(r => r.Errors.Any())) {
					this.logger.LogCritical("Command handler validations returned errors", results);
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}
			await this.wrappedHandler.ExecuteAsync(command);
		}
	}
}
