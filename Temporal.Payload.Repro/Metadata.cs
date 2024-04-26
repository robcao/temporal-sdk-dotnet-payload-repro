using Google.Protobuf;
using System.Text;

namespace Temporal.Payload.Repro
{
	public static class Metadata
	{
		public const string Header = "X-My-Metadata";

		public static bool TryAddMetadata(this Temporalio.Api.Common.V1.Payload payload)
		{
			if (payload.TryReadMetadata(out _))
			{
				return false;
			}

			payload.Metadata.Add(Header, ByteString.CopyFrom(Guid.NewGuid().ToString(), Encoding.UTF8));

			return true;
		}

		public static bool TryReadMetadata(this Temporalio.Api.Common.V1.Payload payload, out string? metadata)
		{
			if (!payload.Metadata.TryGetValue(Header, out ByteString value))
			{
				metadata = null;
				return false;
			}

			metadata = value.ToString(Encoding.UTF8);
			return true;
		}
	}
}
