using Temporalio.Api.Failure.V1;
using Temporalio.Converters;
using Temporalio.Exceptions;

namespace Temporal.Payload.Repro
{
	/// <summary>
	/// An exception that automatically fails the workflow, with an added failure details property to be encoded.
	/// </summary>
	public class NonRetryableWithDetailsException : ApplicationFailureException
	{
		public NonRetryableWithDetailsException(FailureDetails details) : base("activity failed", nameof(NonRetryableWithDetailsException), true, new List<object?>() { details })
		{
		}

		protected internal NonRetryableWithDetailsException(Failure failure, Exception? inner, IPayloadConverter converter) : base(failure, inner, converter)
		{
		}
	}
}
