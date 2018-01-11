using CreativeMinds.CQS.Commands;
using System;

namespace CreativeMinds.CQS.Validators {

	public interface IGenericValidationAsyncCommandHandlerDecorator<TCommand> : IAsyncCommandHandler<TCommand> where TCommand : ICommand { }
}
