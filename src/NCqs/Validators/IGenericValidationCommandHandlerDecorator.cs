using NCqs.Commands;
using System;

namespace NCqs.Validators {

	public interface IGenericValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand { }
}
