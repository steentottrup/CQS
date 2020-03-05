using CreativeMinds.CQS.Queries;
using System;

namespace CreativeMinds.CQS.Permissions {

	public interface IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }
}
