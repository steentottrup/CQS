using CreativeMinds.CQS.Commands;
using System;

namespace CreativeMinds.CQS.Permissions {

	public interface IGenericPermissionCheckCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand { }
}
