using Temporalio.Workflows;

namespace Temporal.Payload.Repro
{
	[Workflow]
	public class WorkflowWithActivityFailureDetails
	{
		/// <summary>
		/// A workflow that calls a single activity that raises a non retryable exception with failure details.
		/// </summary>
		[WorkflowRun]
		public async Task<WorkflowOutput> RunAsync(WorkflowInput input)
		{
			await Temporalio.Workflows.Workflow.ExecuteActivityAsync(
				() => Activities.FailureActivityWithDetails(new ActivityInput()),
				new()
				{
					StartToCloseTimeout = TimeSpan.FromMinutes(5),
				}).ConfigureAwait(true);

			return new WorkflowOutput();
		}
	}
}
