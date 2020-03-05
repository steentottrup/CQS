using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Dispatchers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Linq;
using System.Reflection;

namespace BaseTests {

	[TestClass]
	public class CommandHandlerTests {

		[TestMethod]
		public void TestExecuteOnHandler() {
			SimpleCommand cmd = new SimpleCommand { TestString = "Did it work?" };

			ICommandHandler<SimpleCommand> handler = Substitute.For<ICommandHandler<SimpleCommand>>();

			ICommandDispatcher dispatcher = Substitute.For<CommandDispatcherBase>(new Object[] { null });
			ConfiguredCall call = dispatcher.Protected("GetCommandHandler", typeof(SimpleCommand)).ReturnsForAnyArgs(handler, null);

			dispatcher.Dispatch<SimpleCommand>(cmd);
			handler.Received(1).Execute(cmd);
		}
	}

	public static class Reflect {
		public static object Protected(this object target, string name, params object[] args) {
			var type = target.GetType();
			var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
							 .Where(x => x.Name == name).Single();
			return method.Invoke(target, args);
		}

		public static object Protected(this object target, string name) {
			var type = target.GetType();
			var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
							 .Where(x => x.Name == name).Single();
			return method.Invoke(target, null);
		}

		public static object Protected(this object target, string name, Type genericType) {
			var type = target.GetType();
			var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
							 .Where(x => x.Name == name).Single();
			var generic = method.MakeGenericMethod(genericType);
			return generic.Invoke(target, null);
		}
	}
}
