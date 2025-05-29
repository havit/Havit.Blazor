using System.Linq.Expressions;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms.Internal;

[TestClass]
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

	[TestMethod]
	public void ChipRemoveActionBuilder_SimplePropertyToDefault()
	{
		var model = new SimpleModel { Name = "Set" };
		var builder = new ChipRemoveActionBuilder(() => model.Name);
		var action = builder.Build();

		action(model);

		Assert.IsNull(model.Name);
	}

	[TestMethod]
	public void ChipRemoveActionBuilder_SimplePropertyToExplicit_Value()
	{
		var model = new SimpleModel { Name = "Set" };
		var builder = new ChipRemoveActionBuilder(() => model.Name, "Reset");
		var action = builder.Build();

		action(model);

		Assert.AreEqual("Reset", model.Name);
	}

	[TestMethod]
	public void ChipRemoveActionBuilder_NestedProperty()
	{
		var model = new NestedModel();
		var builder = new ChipRemoveActionBuilder(() => model.Inner.Age);
		var action = builder.Build();

		action(model);

		Assert.AreEqual(0, model.Inner.Age);
	}

	[TestMethod]
	public void ChipRemoveActionBuilder_WorksWithCapturedContext()
	{
		var context = new NestedModel();
		var builder = new ChipRemoveActionBuilder(() => context.Inner.Age);
		var action = builder.Build();

		var model = new NestedModel { Inner = new InnerModel { Age = 99 } };
		action(model);

		Assert.AreEqual(0, model.Inner.Age);
	}

	[TestMethod]
	public void ChipRemoveActionBuilder_ThrowsIfModelInstanceIsNull()
	{
		var model = new SimpleModel();
		var builder = new ChipRemoveActionBuilder(() => model.Name);
		var action = builder.Build();

		Assert.ThrowsExactly<ArgumentNullException>(() => action(null));
	}

	[TestMethod]
	public void ChipRemoveActionBuilder_ThrowsIfNoMemberMatchesModelType()
	{
		var model = new SimpleModel();
		var unrelated = new Unrelated();

		var builder = new ChipRemoveActionBuilder(() => model.Name);
		var action = builder.Build();

		Assert.ThrowsExactly<InvalidOperationException>(() => action(unrelated));
	}

	[TestMethod]
	public void ChipRemoveActionBuilder_ThrowsIfIntermediateIsNull()
	{
		var model = new NestedModel { Inner = null };
		var builder = new ChipRemoveActionBuilder(() => model.Inner.Age);
		var action = builder.Build();

		Assert.ThrowsExactly<NullReferenceException>(() => action(model));
	}

	[TestMethod]
	public void ChipRemoveActionBuilder_ThrowsIfFinalMemberIsNotProperty()
	{
		var model = new WithField();
		var builder = new ChipRemoveActionBuilder(() => model.Field);
		var action = builder.Build();

		Assert.ThrowsExactly<NotSupportedException>(() => action(model));
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
