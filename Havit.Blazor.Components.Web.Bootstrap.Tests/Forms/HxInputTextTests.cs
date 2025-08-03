using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputTextTests : BunitTestBase
{
	[TestMethod]
	public void HxInputText_BindingToArrayOfString_Issue874()
	{
		// Arrange
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
		var cut = Render(componentRenderer);

		// Assert			
		Assert.DoesNotContain("maxlength", cut.Markup);
	}

	[TestMethod]
	public void HxInputText_BindingToListOfString_Issue874()
	{
		// Arrange
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
		var cut = Render(componentRenderer);

		// Assert			
		Assert.DoesNotContain("maxlength", cut.Markup);
	}

	[TestMethod]
	public void HxInputText_BindingToArrayOfModel_Issue874()
	{
		// Arrange
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
		var cut = Render(componentRenderer);

		// Assert			
		Assert.Contains("maxlength=\"100\"", cut.Markup);
	}

	[TestMethod]
	public void HxInputText_BindingToListOfModel_Issue874()
	{
		// Arrange
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
		var cut = Render(componentRenderer);

		// Assert			
		Assert.Contains("maxlength=\"100\"", cut.Markup);
	}

	private record FormData
	{
		[MaxLength(100)]
		public string StringValue { get; set; }
	}
}
