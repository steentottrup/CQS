using NCqs.Queries;
using System;

namespace NCqs.Validators {

	public interface IGenericValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }
}
