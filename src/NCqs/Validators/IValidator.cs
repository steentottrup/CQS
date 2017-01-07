using System;

namespace NCqs.Validators {

	public interface IValidator<in TMessage> where TMessage : IMessage {
		ValidationResult Validate(TMessage msg);
		//Task<ValidationResult> ValidateAsync(TMessage msg);
	}
}