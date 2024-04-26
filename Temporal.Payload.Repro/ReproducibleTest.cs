using Temporalio.Client;
using Temporalio.Converters;
using Temporalio.Testing;
using Temporalio.Worker;

namespace Temporal.Payload.Repro
{
	[TestFixture]
	public class ReproducibleTest
	{
		public const string TaskQueue = "my-task-queue";

		internal static WorkflowEnvironment TestEnvironment { get; set; } = null!;

		internal static ITemporalClient Client { get; set; } = null!;

		[OneTimeSetUp]
		public async Task SetUpEnvironmentAsync()
		{
			TestEnvironment = await Temporalio.Testing.WorkflowEnvironment.StartLocalAsync(new()
			{
				DevServerOptions = new()
				{
					ExtraArgs = new List<string>
					{
						// Disable search attribute cache
						"--dynamic-config-value",
						"system.forceSearchAttributesCacheRefreshOnRead=true",
					},
				},
				UI = true,
				DataConverter = DataConverter.Default with
				{
					PayloadCodec = new MetadataExaminingPayloadCodec()
				}
			}).ConfigureAwait(false);

			Client = TestEnvironment.Client;
		}

		[OneTimeTearDown]
		public async Task CleanUpEnvironmentAsync()
		{
			await TestEnvironment.DisposeAsync().ConfigureAwait(false);
		}

		[Test]
		public async Task Reproduce_Problem()
		{
			TemporalWorkerOptions workerOptions = new TemporalWorkerOptions()
			{
				TaskQueue = TaskQueue,
				DebugMode = true
			};

			workerOptions.AddActivity(Activities.FailureActivityWithDetails);
			workerOptions.AddWorkflow<WorkflowWithActivityFailureDetails>();

			using TemporalWorker worker = new TemporalWorker(Client, workerOptions);

			async Task RunWorkflow()
			{
				WorkflowInput input = new();

				WorkflowHandle<WorkflowWithActivityFailureDetails, WorkflowOutput> handle = await Client.StartWorkflowAsync<WorkflowWithActivityFailureDetails, WorkflowOutput>(
					wf => wf.RunAsync(input),
					new WorkflowOptions(Guid.NewGuid().ToString(), TaskQueue)).ConfigureAwait(false);

				await handle.GetResultAsync().ConfigureAwait(false);
			}

			await worker.ExecuteAsync(RunWorkflow, CancellationToken.None).ConfigureAwait(false);
		}
	}
}
