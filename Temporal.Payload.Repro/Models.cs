namespace Temporal.Payload.Repro
{
	public record WorkflowInput
	{
		public string Input { get; set; } = "input";
	}

	public record WorkflowOutput
	{

	}

	public record ActivityInput
	{
		public string Input { get; set; } = "input";
	}

	public record ActivityOutput
	{
	}
}
