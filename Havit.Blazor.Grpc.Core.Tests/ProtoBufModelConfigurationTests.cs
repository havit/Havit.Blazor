using Havit.Blazor.Grpc.TestContracts;
using ProtoBuf.Meta;

namespace Havit.Blazor.Grpc.Core.Tests
{
	[TestClass]
	public class ProtoBufModelConfigurationTests
	{
		[TestMethod]
		public void ProtoBufModelConfiguration_RuntimeTypeModel_CanSerializeGenericDtoWithAttributes()
		{
			// act
			var model = RuntimeTypeModel.Create();

			// assert
			Assert.IsTrue(model.CanSerialize(typeof(Dto<int>)));
		}

		[TestMethod]
		public void ProtoBufModelConfiguration_RegisterApplicationContracts_RegistersDtoWithNestedClassIncludingNestedClass()
		{
			// arrange
			var model = RuntimeTypeModel.Create();

			// act
			model.RegisterApplicationContracts(typeof(Dto).Assembly);

			// assert
			Assert.IsTrue(model.CanSerialize(typeof(DtoWithNestedClass)));
			Assert.IsTrue(model.CanSerialize(typeof(DtoWithNestedClass.NestedClass)));
		}
	}
}
