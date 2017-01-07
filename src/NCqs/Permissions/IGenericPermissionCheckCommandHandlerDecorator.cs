using NCqs.Commands;
using System;

namespace NCqs.Permissions {

	public interface IGenericPermissionCheckCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand { }
}
