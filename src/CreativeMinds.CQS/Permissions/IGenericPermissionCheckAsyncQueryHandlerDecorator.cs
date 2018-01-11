using CreativeMinds.CQS.Queries;
using System;

namespace CreativeMinds.CQS.Permissions {

	public interface IGenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }
}
