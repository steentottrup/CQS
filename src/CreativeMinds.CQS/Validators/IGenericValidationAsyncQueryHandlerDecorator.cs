using CreativeMinds.CQS.Queries;
using System;

namespace CreativeMinds.CQS.Validators {

	public interface IGenericValidationAsyncQueryHandlerDecorator<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }
}
