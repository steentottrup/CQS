using CreativeMinds.CQS.Commands;
using System;

namespace CreativeMinds.CQS.Dispatchers {

	public abstract class CommandDispatcherBase : ICommandDispatcher {

		protected abstract ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand;

		public virtual void Dispatch<TCommand>(TCommand command) where TCommand : ICommand {
			ICommandHandler<TCommand> handler = this.Resolve<TCommand>();
			handler.Execute(command);
		}

		//public virtual Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand {
		//	ICommandHandler<TCommand> handler = this.Resolve<TCommand>();
		//	return handler.ExecuteAsync(command);
		//}
	}
}
