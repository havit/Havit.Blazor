using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

	[TestMethod]
	public void HxInputText_TypeText_UpdatesBoundValue()
	{
		// Arrange
		string currentValue = "";

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputText>(0);
			builder.AddAttribute(1, "Value", currentValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { currentValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => currentValue));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		cut.Find("input").Change("Hello World");

		// Assert
		Assert.AreEqual("Hello World", currentValue);
	}

	[TestMethod]
	public void HxInputText_RequiredEmpty_ShowsValidationError()
	{
		// Arrange
		var model = new RequiredFormData { Name = "initial" };

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, "Model", model);
			builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)((context) => (builder2) =>
			{
				builder2.OpenComponent<DataAnnotationsValidator>(0);
				builder2.CloseComponent();

				builder2.OpenComponent<HxInputText>(1);
				builder2.AddAttribute(2, "Value", model.Name);
				builder2.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { model.Name = value; }));
				builder2.AddAttribute(4, "ValueExpression", (Expression<Func<string>>)(() => model.Name));
				builder2.CloseComponent();
			}));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		cut.Find("input").Change("");

		// Assert
		Assert.AreEqual("", model.Name);
		Assert.Contains("invalid-feedback", cut.Markup);
		Assert.Contains("Name is required.", cut.Markup);
	}

	[TestMethod]
	public void HxInputText_ClearInput_ResetsValue()
	{
		// Arrange
		string currentValue = "initial text";

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputText>(0);
			builder.AddAttribute(1, "Value", currentValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { currentValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => currentValue));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		cut.Find("input").Change("");

		// Assert
		Assert.AreEqual("", currentValue);
	}

	[TestMethod]
	public void HxInputText_Placeholder_DisplayedWhenEmpty()
	{
		// Arrange
		string currentValue = "";

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputText>(0);
			builder.AddAttribute(1, "Value", currentValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { currentValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => currentValue));
			builder.AddAttribute(4, "Placeholder", "Enter your name");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		Assert.AreEqual("Enter your name", cut.Find("input").GetAttribute("placeholder"));
	}

	private record FormData
	{
		[MaxLength(100)]
		public string StringValue { get; set; }
	}

	private class RequiredFormData
	{
		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }
	}
}
