using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputTextTests
{
#if NET8_0_OR_GREATER
	[TestMethod]
	public void HxInputBase_BindingToArrayOfString_Issue874()
	{
		// Arrange
		var ctx = new Bunit.TestContext();
		string[] values = ["test"];

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputText>(0);
			builder.AddAttribute(1, "Value", values[0]);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { values[0] = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => values[0]));
			builder.CloseComponent();
		};

		// Act
		var cut = ctx.Render(componentRenderer);

		// Assert			
		Assert.IsFalse(cut.Markup.Contains("maxlength"));
	}

	[TestMethod]
	public void HxInputBase_BindingToListOfString_Issue874()
	{
		// Arrange
		var ctx = new Bunit.TestContext();
		List<string> values = ["test"];

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputText>(0);
			builder.AddAttribute(1, "Value", values[0]);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { values[0] = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => values[0]));
			builder.CloseComponent();
		};

		// Act
		var cut = ctx.Render(componentRenderer);

		// Assert			
		Assert.IsFalse(cut.Markup.Contains("maxlength"));
	}
#endif


	[TestMethod]
	public void HxInputBase_BindingToArrayOfModel_Issue874()
	{
		// Arrange
		var ctx = new Bunit.TestContext();
		FormData[] values = [new FormData()];

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputText>(0);
			builder.AddAttribute(1, "Value", values[0].StringValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { values[0].StringValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => values[0].StringValue));
			builder.CloseComponent();
		};

		// Act
		var cut = ctx.Render(componentRenderer);

		// Assert			
		Assert.IsTrue(cut.Markup.Contains("maxlength=\"100\""));
	}

	[TestMethod]
	public void HxInputBase_BindingToListOfModel_Issue874()
	{
		// Arrange
		var ctx = new Bunit.TestContext();
		FormData[] values = [new FormData()];

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputText>(0);
			builder.AddAttribute(1, "Value", values[0].StringValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { values[0].StringValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => values[0].StringValue));
			builder.CloseComponent();
		};

		// Act
		var cut = ctx.Render(componentRenderer);

		// Assert			
		Assert.IsTrue(cut.Markup.Contains("maxlength=\"100\""));
	}

	private record FormData
	{
		[MaxLength(100)]
		public string StringValue { get; set; }
	}
}
