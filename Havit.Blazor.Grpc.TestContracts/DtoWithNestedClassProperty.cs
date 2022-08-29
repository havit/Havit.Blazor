namespace Havit.Blazor.Grpc.TestContracts;

public class DtoWithNestedClass
{
	public NestedClass MyProperty { get; set; }

	public class NestedClass
	{
		public int Value { get; set; }
	}
}
