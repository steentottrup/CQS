using CreativeMinds.CQS.Queries;
using System;

namespace CreativeMinds.CQS.Validators {

	public interface IGenericValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }
}
