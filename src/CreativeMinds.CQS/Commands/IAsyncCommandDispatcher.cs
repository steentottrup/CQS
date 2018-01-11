using System;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Commands {

	public interface IAsyncCommandDispatcher {
		Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
	}
}
