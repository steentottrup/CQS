using System;
using System.Reflection;

namespace CreativeMinds.CQS {

	public class NoCheckPermissionsHandlerImplementedException : ApplicationException {
		public Type CommandType { get; private set; }
		public Type QueryType { get; private set; }
		public Type ResultType { get; private set; }

		public NoCheckPermissionsHandlerImplementedException(Type commandType) : base($"A permission check decorator was found for the \"{commandType.GetTypeInfo().Name}\" command, but no handler was located.") {
			this.CommandType = commandType;
		}

		public NoCheckPermissionsHandlerImplementedException(Type queryType, Type resultType) : base($"A permission check decorator was found for the \"{queryType.GetTypeInfo().Name}\" query and \"{resultType.GetTypeInfo().Name}\" result, but no handler was located.") {
			this.QueryType = queryType;
			this.ResultType = resultType;
		}
	}
}
