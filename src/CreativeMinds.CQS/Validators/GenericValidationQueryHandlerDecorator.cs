using CreativeMinds.CQS.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQS.Validators {

	public class GenericValidationQueryHandlerDecorator<TQuery, TResult> : IGenericValidationQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IEnumerable<IValidator<TQuery>> validators;
		protected readonly ILogger logger;

		public GenericValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> wrappedHandler, IEnumerable<IValidator<TQuery>> validators, ILogger<GenericValidationQueryHandlerDecorator<TQuery, TResult>> logger) {
			this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
			this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public TResult Handle(TQuery query) {
			if (this.validators.Any()) {
				this.logger.LogInformation("Query handler validations found", this.validators);

				List<ValidationResult> results = new List<ValidationResult>();
				this.validators.ToList().ForEach(validator => {
					results.Add(validator.Validate(query));
				});

				if (results.Any(r => r.Errors.Any())) {
					this.logger.LogCritical("Query handler validations returned errors", results);
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}
			else {
				this.logger.LogWarning($"A validation decorator was found, but no validations for \"{typeof(TQuery).Name}\" query and \"{typeof(TResult).Name}\" result");
			}

			return this.wrappedHandler.Handle(query);
		}
	}
}
