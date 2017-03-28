using System;

namespace CreativeMinds.CQS.Commands {

	public class CommandWithStatus : ICommand {

		public CommandWithStatus() {
			this.TaskId = Guid.NewGuid();
		}

		public Guid TaskId { get; private set; }
	}
}
