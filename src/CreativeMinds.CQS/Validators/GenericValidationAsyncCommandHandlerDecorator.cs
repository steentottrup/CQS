using CreativeMinds.CQS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Validators {

	public class GenericValidationAsyncCommandHandlerDecorator<TCommand> : IGenericValidationAsyncCommandHandlerDecorator<TCommand> where TCommand : ICommand {
		private readonly IAsyncCommandHandler<TCommand> wrappedHandler;
		private readonly IEnumerable<IAsyncValidator<TCommand>> validators;

		public GenericValidationAsyncCommandHandlerDecorator(IAsyncCommandHandler<TCommand> wrappedHandler, IEnumerable<IAsyncValidator<TCommand>> validators) {
			this.wrappedHandler = wrappedHandler;
			this.validators = validators;
		}

		public async Task ExecuteAsync(TCommand command) {
			if (this.validators.Any()) {
				List<ValidationResult> results = new List<ValidationResult>();
				foreach (var validator in this.validators) {
					ValidationResult result = await validator.ValidateAsync(command);
					results.Add(result);
				}

				if (results.Any(r => r.Errors.Any())) {
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}
			await this.wrappedHandler.ExecuteAsync(command);
		}
	}
}
