using System;

namespace NCqs{

	public class ProbeException : Exception {
		public readonly Type GenericParameterType;

		public ProbeException(Type type) : base($"Could not locate anything with the generic type parameter {type.Name}") {
			this.GenericParameterType = type;
		}
	}
}