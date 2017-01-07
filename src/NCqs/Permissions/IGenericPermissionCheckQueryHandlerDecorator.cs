using NCqs.Queries;
using System;

namespace NCqs.Permissions {

	public interface IGenericPermissionCheckQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> { }
}
