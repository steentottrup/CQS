using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Commands {

	public interface IAsyncCommandDispatcher {
		Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;
	}
}
