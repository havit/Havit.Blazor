using System.Linq.Expressions;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms.Internal;

public class ChipRemoveActionBuilderTests
{
	private class SimpleModel
	{
		public string Name { get; set; } = "Initial";
	}

	private class NestedModel
	{
		public InnerModel Inner { get; set; } = new InnerModel();
	}

	private class InnerModel
	{
		public int Age { get; set; } = 42;
	}

	[Fact]
	public void ChipRemoveActionBuilder_SimplePropertyToDefault()
	{
		var model = new SimpleModel { Name = "Set" };
		var builder = new ChipRemoveActionBuilder(() => model.Name);
		var action = builder.Build();

		action(model);

		Assert.Null(model.Name);
	}

	[Fact]
	public void ChipRemoveActionBuilder_SimplePropertyToExplicit_Value()
	{
		var model = new SimpleModel { Name = "Set" };
		var builder = new ChipRemoveActionBuilder(() => model.Name, "Reset");
		var action = builder.Build();

		action(model);

		Assert.Equal("Reset", model.Name);
	}

	[Fact]
	public void ChipRemoveActionBuilder_NestedProperty()
	{
		var model = new NestedModel();
		var builder = new ChipRemoveActionBuilder(() => model.Inner.Age);
		var action = builder.Build();

		action(model);

		Assert.Equal(0, model.Inner.Age);
	}

	[Fact]
	public void ChipRemoveActionBuilder_WorksWithCapturedContext()
	{
		var context = new NestedModel();
		var builder = new ChipRemoveActionBuilder(() => context.Inner.Age);
		var action = builder.Build();

		var model = new NestedModel { Inner = new InnerModel { Age = 99 } };
		action(model);

		Assert.Equal(0, model.Inner.Age);
	}

	[Fact]
	public void ChipRemoveActionBuilder_ThrowsIfModelInstanceIsNull()
	{
		var model = new SimpleModel();
		var builder = new ChipRemoveActionBuilder(() => model.Name);
		var action = builder.Build();

		Assert.Throws<ArgumentNullException>(() => action(null));
	}

	[Fact]
	public void ChipRemoveActionBuilder_ThrowsIfNoMemberMatchesModelType()
	{
		var model = new SimpleModel();
		var unrelated = new Unrelated();

		var builder = new ChipRemoveActionBuilder(() => model.Name);
		var action = builder.Build();

		Assert.Throws<InvalidOperationException>(() => action(unrelated));
	}

	[Fact]
	public void ChipRemoveActionBuilder_ThrowsIfIntermediateIsNull()
	{
		var model = new NestedModel { Inner = null };
		var builder = new ChipRemoveActionBuilder(() => model.Inner.Age);
		var action = builder.Build();

		Assert.Throws<NullReferenceException>(() => action(model));
	}

	[Fact]
	public void ChipRemoveActionBuilder_ThrowsIfFinalMemberIsNotProperty()
	{
		var model = new WithField();
		var builder = new ChipRemoveActionBuilder(() => model.Field);
		var action = builder.Build();

		Assert.Throws<NotSupportedException>(() => action(model));
	}

	private class WithField
	{
		public int Field = 42;
	}

	private class Unrelated
	{
		public string SomethingElse { get; set; }
	}
}
