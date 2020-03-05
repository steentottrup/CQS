//using CreativeMinds.CQS.Commands;
//using CreativeMinds.CQS.Dispatchers;
//using CreativeMinds.CQS.Permissions;
//using CreativeMinds.CQS.Validators;
//using System;

//namespace BaseTests {

//	public class MockedDispatcher : CommandDispatcherBase {

//		protected override ICommandHandler<TCommand> GetCommandHandler<TCommand>() {
//			throw new NotImplementedException();
//		}

//		protected override IGenericPermissionCheckCommandHandlerDecorator<TCommand> GetPermissionCheckHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
//			throw new NotImplementedException();
//		}

//		protected override IGenericValidationCommandHandlerDecorator<TCommand> GetValidationHandler<TCommand>(ICommandHandler<TCommand> wrappedHandler) {
//			throw new NotImplementedException();
//		}
//	}
//}
