﻿using System;

namespace CreativeMinds.CQS.Commands {

	public interface ICommandHandler<in TCommand> where TCommand : ICommand {
		void Execute(TCommand command);
	}
}
