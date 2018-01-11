using System;

namespace CreativeMinds.CQS.Validators {

	public interface IValidator<in TMessage> where TMessage : IMessage {
		ValidationResult Validate(TMessage msg);
	}
}