using Autofac;
using CreativeMinds.CQS.Commands;
using CreativeMinds.CQS.Events;
using CreativeMinds.CQS.Permissions;
using CreativeMinds.CQS.Queries;
using CreativeMinds.CQS.Validators;
using System;
using System.Linq;
using System.Reflection;

namespace CreativeMinds.CQS.Autofac {

	public static class ContainerBuilderExtensions {

		public static void RegisterAll(this ContainerBuilder builder, Assembly assembly) {
			builder.RegisterEventHandlers(assembly);
			builder.RegisterCommandHandlers(assembly);
			builder.RegisterQueryHandlers(assembly);
			builder.RegisterPermissionChecks(assembly);
			builder.RegisterValidators(assembly);

			builder.RegisterGeneric(typeof(GenericValidationAsyncCommandHandlerDecorator<>))
				.As(typeof(IGenericValidationAsyncCommandHandlerDecorator<>))
				.InstancePerLifetimeScope();

			builder.RegisterGeneric(typeof(GenericValidationCommandHandlerDecorator<>))
				.As(typeof(IGenericValidationCommandHandlerDecorator<>))
				.InstancePerLifetimeScope();

			builder.RegisterGeneric(typeof(GenericValidationQueryHandlerDecorator<,>))
				.As(typeof(IGenericValidationQueryHandlerDecorator<,>))
				.InstancePerLifetimeScope();

			builder.RegisterGeneric(typeof(GenericValidationAsyncQueryHandlerDecorator<,>))
				.As(typeof(IGenericValidationAsyncQueryHandlerDecorator<,>))
				.InstancePerLifetimeScope();

			builder
				.RegisterType<AsyncCommandDispatcher>()
				.As<IAsyncCommandDispatcher>()
				.InstancePerLifetimeScope();
			builder
				.RegisterType<AsyncQueryDispatcher>()
				.As<IAsyncQueryDispatcher>()
				.InstancePerLifetimeScope();

			// TODO: Permissions
		}

		private static void RegisterHandlers(ContainerBuilder builder, Assembly assembly, Type handlerType) {
			foreach (var type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == handlerType))) {
				builder
					.RegisterType(type)
					.As(type.GetInterfaces().First(i => i.IsConstructedGenericType == true && i.GetGenericTypeDefinition() == handlerType))
					.InstancePerLifetimeScope();
			}
		}

		public static void RegisterEventHandlers(this ContainerBuilder builder, Assembly assembly) {
			RegisterHandlers(builder, assembly, typeof(IEventHandler<>));
			//RegisterHandlers(builder, assembly, typeof(IAsyncEventHandlers<>));
		}

		public static void RegisterCommandHandlers(this ContainerBuilder builder, Assembly assembly) {
			RegisterHandlers(builder, assembly, typeof(ICommandHandler<>));
			RegisterHandlers(builder, assembly, typeof(IAsyncCommandHandler<>));
		}

		public static void RegisterQueryHandlers(this ContainerBuilder builder, Assembly assembly) {
			RegisterHandlers(builder, assembly, typeof(IQueryHandler<,>));
			RegisterHandlers(builder, assembly, typeof(IAsyncQueryHandler<,>));
		}

		public static void RegisterPermissionChecks(this ContainerBuilder builder, Assembly assembly) {
			RegisterHandlers(builder, assembly, typeof(IPermissionCheck<>));
			RegisterHandlers(builder, assembly, typeof(IAsyncPermissionCheck<>));
		}

		public static void RegisterValidators(this ContainerBuilder builder, Assembly assembly) {
			RegisterHandlers(builder, assembly, typeof(IValidator<>));
			RegisterHandlers(builder, assembly, typeof(IAsyncValidator<>));
		}
	}
}
