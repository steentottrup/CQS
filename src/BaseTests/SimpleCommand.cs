using CreativeMinds.CQS.Commands;
using System;

namespace BaseTests {

	public class SimpleCommand : ICommand {
		public String TestString { get; set; }
	}
}
