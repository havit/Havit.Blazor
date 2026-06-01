using Havit.Blazor.Grpc.TestContracts;
using ProtoBuf.Meta;

namespace Havit.Blazor.Grpc.Core.Tests;

public class ProtoBufModelConfigurationTests
{
	[Fact]
	public void ProtoBufModelConfiguration_RuntimeTypeModel_CanSerializeGenericDtoWithAttributes()
	{
		// act
		var model = RuntimeTypeModel.Create();

		// assert
		Assert.True(model.CanSerialize(typeof(Dto<int>)));
	}

	[Fact]
	public void ProtoBufModelConfiguration_RegisterApplicationContracts_RegistersDtoWithNestedClassIncludingNestedClass()
	{
		// arrange
		var model = RuntimeTypeModel.Create();

		// act
		model.RegisterApplicationContracts(typeof(Dto).Assembly);

		// assert
		Assert.True(model.CanSerialize(typeof(DtoWithNestedClass)));
		Assert.True(model.CanSerialize(typeof(DtoWithNestedClass.NestedClass)));
	}
}
