#pragma warning disable SA1402 // File may only contain a single class
using ProtoBuf;

namespace Havit.Blazor.Grpc.TestContracts;

[ProtoContract]
public class Dto<TValue>
{
	[ProtoMember(1)]
	public TValue Value { get; set; }

	public Dto()
	{
		// NOOP				
	}

	public Dto(TValue value)
	{
		Value = value;
	}
}

public static class Dto
{
	public static Dto<TValue> FromValue<TValue>(TValue value)
	{
		return new Dto<TValue>(value);
	}
}
