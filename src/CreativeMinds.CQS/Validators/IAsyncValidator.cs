using System;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeMinds.CQS.Validators {

	public interface IAsyncValidator<in TMessage> where TMessage : IMessage {
		Task<ValidationResult> ValidateAsync(TMessage msg, CancellationToken cancellationToken);
	}
}