using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Commands {

	public interface IAsyncCommandHandler<in TCommand> where TCommand : ICommand {
		Task ExecuteAsync(TCommand command, CancellationToken cancellationToken);
	}
}
