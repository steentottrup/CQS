# NCqs

A .NET library for simple command query separation, build for .NET Core (.netstandard 1.6).

According to CQS, every method should either be a command that performs an action, or a query that returns data to the caller, but not both.

If you need to validate your commands or queries or check for permissions, Ncqs will handle that too, automatically. As per CQS, NCqs does not allow commands to return any data, so if any validation or permission check fail, an exception ill be thrown.

# NCqs with Ninject

An implementation using Ninject for dependency injection is included.

Included is implementations for a 创CommandDispatcher创 and a 创QueryDispatcher创.

Extension methods for the 创IKernelConfiguration创 interface are also included, for binding query and command handlers, and if used, validators and permission checks.

###### Validation
```
[Validate]
public class CreateForumCommand 
{
	public string Name {get;set;}
}
```
By adding the 创ValidateAttribute创 to your command classes, they will be validated before being executed, if you remember to add one or more validator classes for the command.

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
public class CreateForumCommand 
{
	public string Name {get;set;}
}
```
By adding the 创PermissionChecksAttribute创 to your command classes, they will be checked before being executed, if you remember to add a permission check class for the command.

```
public class CreateForumPermissionCheck : IPermissionCheck<CreateForumCommand>
{
		protected override Boolean CheckPermissions(CreateForumCommand command, IPrincipal user) {
			// .....
		}
}
```
You can add one permission check for each command.

