using CreativeMinds.CQS.Resolvers;

namespace CreativeMinds.CQS {
	public abstract class DispatcherBase : ResolverBase {

		public DispatcherBase() : base() { }
		//protected readonly Dictionary<Type, Type> handlers;

		//protected void ProbeAssembly(Assembly assembly, Type forType) {
		//	foreach (Type type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(x => x.IsConstructedGenericType == true && x.GetGenericTypeDefinition() == forType))) {
		//		foreach (Type handler in type.GetInterfaces().Where(x => x.IsConstructedGenericType == true && x.GetGenericTypeDefinition() == forType)) {
		//			this.handlers.Add(handler.GenericTypeArguments.First(), handler);
		//		}
		//	}
		//}

		//protected Type GetGenericType(Type genericParameterType) {
		//	if (!this.handlers.ContainsKey(genericParameterType)) {
		//		throw new ProbeException(genericParameterType);
		//	}

		//	return this.handlers[genericParameterType];
		//}
	}
}