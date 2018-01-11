using CreativeMinds.CQS.Commands;
using System;

namespace CreativeMinds.CQS.Permissions {

	public interface IGenericPermissionCheckAsyncCommandHandlerDecorator<TCommand> : IAsyncCommandHandler<TCommand> where TCommand : ICommand { }
}
