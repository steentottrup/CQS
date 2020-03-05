using CreativeMinds.CQS.Queries;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Validators {

	public class GenericValidationAsyncQueryHandlerDecorator<TQuery, TResult> : IGenericValidationAsyncQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IAsyncQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IEnumerable<IAsyncValidator<TQuery>> validators;
		protected readonly ILogger logger;

		public GenericValidationAsyncQueryHandlerDecorator(IAsyncQueryHandler<TQuery, TResult> wrappedHandler, IEnumerable<IAsyncValidator<TQuery>> validators, ILogger<GenericValidationAsyncQueryHandlerDecorator<TQuery, TResult>> logger) {
			this.wrappedHandler = wrappedHandler ?? throw new System.ArgumentNullException(nameof(wrappedHandler));
			this.validators = validators ?? throw new System.ArgumentNullException(nameof(validators));
			this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
		}

		public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken) {
			if (this.validators.Any()) {
				this.logger.LogInformation($"Query handler validation(s) found, count {this.validators.Count()}", this.validators);
				List<ValidationResult> results = new List<ValidationResult>();
				this.validators.ToList().ForEach(async validator => {
					results.Add(await validator.ValidateAsync(query, cancellationToken));
				});

				if (results.Any(r => r.Errors.Any())) {
					this.logger.LogCritical("Query handler validations returned errors", results);
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}
			else {
				this.logger.LogWarning($"A validation decorator was found, but no validations for \"{typeof(TQuery).Name}\" query and \"{typeof(TResult).Name}\" result");
			}
			return await this.wrappedHandler.HandleAsync(query, cancellationToken);
		}
	}
}
