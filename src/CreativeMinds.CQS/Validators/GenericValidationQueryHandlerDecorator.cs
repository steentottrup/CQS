using CreativeMinds.CQS.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeMinds.CQS.Validators {

	public class GenericValidationQueryHandlerDecorator<TQuery, TResult> : IGenericValidationQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IEnumerable<IValidator<TQuery>> validators;

		public GenericValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> wrappedHandler, IEnumerable<IValidator<TQuery>> validators) {
			this.wrappedHandler = wrappedHandler;
			this.validators = validators;
		}

		public TResult Handle(TQuery query) {
			if (this.validators.Any()) {
				List<ValidationResult> results = new List<ValidationResult>();
				this.validators.ToList().ForEach(validator => {
					results.Add(validator.Validate(query));
				});

				if (results.Any(r => r.Errors.Any())) {
					throw new ValidationException(results.SelectMany(r => r.Errors));
				}
			}

			return this.wrappedHandler.Handle(query);
		}
	}
}
