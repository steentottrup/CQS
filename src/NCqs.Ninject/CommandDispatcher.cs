using NCqs.Commands;
using NCqs.Dispatchers;
using NCqs.Permissions;
using NCqs.Validators;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NCqs.Ninject {

	public class CommandDispatcher : CommandDispatcherBase {
		private readonly IReadOnlyKernel kernel;

		public CommandDispatcher(IReadOnlyKernel kernel) {
			this.kernel = kernel;
		}

		protected override ICommandHandler<TCommand> Resolve<TCommand>() {
			ICommandHandler<TCommand> handler = null;
			//try {
			List<IParameter> parameters = new List<IParameter>();
			IEnumerable<Attribute> attrs = typeof(TCommand).GetTypeInfo().GetCustomAttributes();
			if (attrs.Any(a => a.GetType() == typeof(NCqs.Decorators.ValidateAttribute))) {
				handler = this.kernel.Get<IGenericValidationCommandHandlerDecorator<TCommand>>();
				parameters.Add(new ConstructorArgument("wrappedHandler", handler));
			}

			if (attrs.Any(a => a.GetType() == typeof(NCqs.Decorators.CheckPermissionsAttribute)) ||
				attrs.Any(a => a.GetType().GetTypeInfo().BaseType == typeof(NCqs.Decorators.CheckPermissionsAttribute))) {

				handler = this.kernel.Get<IGenericPermissionCheckCommandHandlerDecorator<TCommand>>(parameters.ToArray());
			}

			if (handler == null) {
				handler = this.kernel.Get<ICommandHandler<TCommand>>();
			}
			//}
			//catch (Exception ex) {
			//	// TODO: log
			//	throw ex;
			//}

			if (handler == null) {
				throw new RequiredHandlerNotFoundException();
			}

			return handler;
		}
	}
}
