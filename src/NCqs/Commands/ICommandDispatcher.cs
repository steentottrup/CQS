using System;

namespace NCqs.Commands {

	public interface ICommandDispatcher {
		void Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
		//Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
	}
}
