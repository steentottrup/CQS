using CreativeMinds.CQS.Commands;
using System;

namespace CreativeMinds.CQS.Validators {

	public interface IGenericValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand { }
}
