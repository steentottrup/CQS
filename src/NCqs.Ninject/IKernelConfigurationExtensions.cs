using NCqs.Commands;
using NCqs.Permissions;
using NCqs.Queries;
using NCqs.Validators;
using Ninject;
using System;
using System.Linq;
using System.Reflection;

namespace NCqs.Ninject {

	public static class IKernelConfigurationExtensions {

		public static void BindCommandHandlers(this IKernelConfiguration config, Assembly assembly) {
			foreach (var type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))) {
				foreach (Type commandHandler in type.GetInterfaces().Where(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))) {
					config
						.Bind(commandHandler)
						.To(type);
				}
			}
		}

		public static void BindQueryHandlers(this IKernelConfiguration config, Assembly assembly) {
			foreach (var type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))) {
				foreach (Type queryHandler in type.GetInterfaces().Where(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))) {
					config
						.Bind(queryHandler)
						.To(type);
				}
			}
		}

		public static void BindPermissionChecks(this IKernelConfiguration config, Assembly assembly) {
			foreach (var type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(IPermissionCheck<>)))) {
				foreach (Type permissionCheck in type.GetInterfaces().Where(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(IPermissionCheck<>))) {
					config
						.Bind(permissionCheck)
						.To(type);
				}
			}
		}

		public static void BindValidators(this IKernelConfiguration config, Assembly assembly) {
			foreach (var type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(IValidator<>)))) {
				foreach (Type validator in type.GetInterfaces().Where(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == typeof(IValidator<>))) {
					config
						.Bind(validator)
						.To(type);
				}
			}
		}
	}
}
