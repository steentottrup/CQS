# CreativeMinds CQS

A .NET library for simple command query separation, build for .NET Core (.netstandard 2.0).

According to CQS, every method should either be a command that performs an action, or a query that returns data to the caller, but not both.

If you need to validate your commands or queries or check for permissions, CreativeMinds CQS will handle that too, automatically. As per CQS, CreativeMinds CQS does not allow commands to return any data, so if any validation or permission check fail, an exception will be thrown.

Commands and queries will have their permission checks executed first, if any are present, then the validations, again if any are present, and finally the actual command or query will be executed.

CreativeMinds CQS also includes classes and interfaces for using events. You can have any number of event handlers for a given event.

### Breaking change in v1.1

Version 1.1 has, what can be, a breaking change. Before v1.1 you could decorate commands/queries with the ValidateAttribute and/or CheckPermissionsAttribute without actually implementing the needed classes to actually validate and/or check permissions. Before v1.1 this would just mean a warning logged. From v1.1 an exception is thrown.

###### Validation
```
[Validate]
public class CreateForumCommand : ICommand
{
	public string Name {get;set;}
}
```
By adding the ```ValidateAttribute``` to your command classes, they will be validated before being executed, if you remember to add one or more validator classes for the command.

```
public class CreateForumValidator : IValidator<CreateForumCommand>
{
	public ValidationResult Validate(CreateForumCommand command) 
	{
		// ......
	}
}
```
You can add any number of validators for the same command.

###### Permission Check

```
[CheckPermissions]
public class CreateForumCommand : ICommand
{
	public string Name {get;set;}
}
```
By adding the ```PermissionChecksAttribute``` to your command classes, they will be checked before being executed, if you remember to add a permission check class for the command.

```
public class CreateForumPermissionCheck : IPermissionCheck<CreateForumCommand>
{
		protected override Boolean CheckPermissions(CreateForumCommand command, IPrincipal user) {
			// .....
		}
}
```
You can add one permission check for each command.

# CreativeMinds CQS with Autofac

An implementation using Autofac for dependency injection is included.

Included is implementations for a ```CommandDispatcher```, a ```QueryDispatcher``` and an ```EventDispatcher```, as well as async dispatchers.

# CreativeMinds CQS with Ninject

An implementation using Ninject for dependency injection has been discontinued because Ninject has not yet moved to .Net Core.
