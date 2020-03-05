using CreativeMinds.CQS.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Permissions {

	public class GenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult> : IGenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult> where TQuery : IQuery<TResult> {
		private readonly IAsyncQueryHandler<TQuery, TResult> wrappedHandler;
		private readonly IIdentity currentUser;
		private readonly IAsyncPermissionCheck<TQuery> check;
		protected readonly ILogger logger;

		public GenericPermissionCheckAsyncQueryHandlerDecorator(IAsyncQueryHandler<TQuery, TResult> wrappedHandler, IIdentity currentUser, IAsyncPermissionCheck<TQuery> check, ILogger<GenericPermissionCheckAsyncQueryHandlerDecorator<TQuery, TResult>> logger) {
			this.wrappedHandler = wrappedHandler ?? throw new ArgumentNullException(nameof(wrappedHandler));
			this.currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
			this.check = check ?? throw new ArgumentNullException(nameof(check));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		protected async Task PerformCheckAsync(TQuery query, CancellationToken cancellationToken) {
			this.logger.LogDebug($"Doing a permission check for the query \"{typeof(TQuery).GetTypeInfo().Name}\" using the class \"{this.check.GetType().Name}\"");
			IPermissionCheckResult result = await this.check.CheckAsync(query, this.currentUser, cancellationToken);
			if (!result.HasPermissions) {
				this.logger.LogCritical("Query handler permission check returned NO!", result.ErrorMessage);
				throw new PermissionException(result.ErrorCode, result.ErrorMessage);
			}
		}

		public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken) {
			await this.PerformCheckAsync(query, cancellationToken);
			return await this.wrappedHandler.HandleAsync(query, cancellationToken);
		}
	}
}
