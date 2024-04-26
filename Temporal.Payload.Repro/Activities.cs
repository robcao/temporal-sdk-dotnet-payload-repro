using Temporalio.Activities;

namespace Temporal.Payload.Repro
{
	public static class Activities
	{
		/// <summary>
		/// Activity that always throws a <see cref="NonRetryableWithDetailsException"/>.
		/// </summary>
		[Activity]
		public static Task<ActivityOutput> FailureActivityWithDetails(ActivityInput input)
		{
			return Task.FromException<ActivityOutput>(new NonRetryableWithDetailsException(new FailureDetails()));
		}
	}
}
