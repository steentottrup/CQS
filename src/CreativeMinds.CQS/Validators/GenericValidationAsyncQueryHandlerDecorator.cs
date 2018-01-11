using CreativeMinds.CQS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Validators {

	public class GenericValidationAsyncQueryHandlerDecorator<TQuery, TResult> : IGenericValidationAsyncQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IAsyncQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IEnumerable<IAsyncValidator<TQuery>> validators;

		public GenericValidationAsyncQueryHandlerDecorator(IAsyncQueryHandler<TQuery, TResult> wrappedHandler, IEnumerable<IAsyncValidator<TQuery>> validators) {
			this.wrappedHandler = wrappedHandler;
			this.validators = validators;
		}

		public async Task<TResult> HandleAsync(TQuery query) {
			if (this.validators.Any()) {
				List<ValidationResult> results = new List<ValidationResult>();
				this.validators.ToList().ForEach(async validator => {
					results.Add(await validator.ValidateAsync(query));
				});

				if (results.Any(r => r.Errors.Any())) {
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}
			return await this.wrappedHandler.HandleAsync(query);
		}
	}
}
