using Temporalio.Converters;

namespace Temporal.Payload.Repro
{
	public class MetadataExaminingPayloadCodec : IPayloadCodec
	{
		/// <summary>
		/// Attempts to read the X-My-Metadata header on the metadata of each payload.
		/// </summary>
		public Task<IReadOnlyCollection<Temporalio.Api.Common.V1.Payload>> DecodeAsync(IReadOnlyCollection<Temporalio.Api.Common.V1.Payload> payloads)
		{
			List<Temporalio.Api.Common.V1.Payload> clonedPayloads = new List<Temporalio.Api.Common.V1.Payload>();

			foreach (Temporalio.Api.Common.V1.Payload payload in payloads)
			{
				if (payload.TryReadMetadata(out string? metadata))
				{
					TestContext.Out.WriteLine($"Now reading metadata {metadata}...");
				}

				clonedPayloads.Add(payload.Clone());
			}

			return Task.FromResult<IReadOnlyCollection<Temporalio.Api.Common.V1.Payload>>(clonedPayloads);
		}

		/// <summary>
		/// Attempts to add a new uuid to the X-My-Metadata header on the metadata of each payload, if not already exists.
		/// </summary>
		public Task<IReadOnlyCollection<Temporalio.Api.Common.V1.Payload>> EncodeAsync(IReadOnlyCollection<Temporalio.Api.Common.V1.Payload> payloads)
		{
			List<Temporalio.Api.Common.V1.Payload> clonedPayloads = new List<Temporalio.Api.Common.V1.Payload>();

			foreach (Temporalio.Api.Common.V1.Payload payload in payloads)
			{
				Temporalio.Api.Common.V1.Payload clone = payload;

				if (clone.TryAddMetadata())
				{
					clone.TryReadMetadata(out string? metadata);
					TestContext.Out.WriteLine($"Successfully added metadata {metadata}...");
				}

				else
				{
					clone.TryReadMetadata(out string? metadata);
					TestContext.Out.WriteLine($"WARN: tried to add metadata, but metadata already exists with a value of {metadata}...");
				}


				clonedPayloads.Add(clone);
			}

			return Task.FromResult<IReadOnlyCollection<Temporalio.Api.Common.V1.Payload>>(clonedPayloads);
		}
	}
}
