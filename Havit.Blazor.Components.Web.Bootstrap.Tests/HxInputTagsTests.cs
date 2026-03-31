using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxInputTagsTests : BunitTestBase
{
	[TestMethod]
	public void HxInputTags_WithTags_RendersBadges()
	{
		// Arrange
		var tags = new List<string> { "red", "blue", "green" };

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputTags>(0);
			builder.AddAttribute(1, "Value", tags);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<List<string>>>)(() => tags));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — tags render as badge elements
		var tagElements = cut.FindAll(".hx-tag");
		Assert.HasCount(3, tagElements);
	}

	[TestMethod]
	public void HxInputTags_EmptyTags_NoBadges()
	{
		// Arrange
		var tags = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputTags>(0);
			builder.AddAttribute(1, "Value", tags);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<List<string>>>)(() => tags));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — no tag badges rendered
		var tagElements = cut.FindAll(".hx-tag");
		Assert.IsEmpty(tagElements);
	}

	[TestMethod]
	public void HxInputTags_Render_InputFieldPresent()
	{
		// Arrange
		var tags = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputTags>(0);
			builder.AddAttribute(1, "Value", tags);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<List<string>>>)(() => tags));
			builder.AddAttribute(4, "Placeholder", "Add tags...");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — input field is present with placeholder
		var input = cut.Find("input[type='text']");
		Assert.AreEqual("Add tags...", input.GetAttribute("placeholder"));
	}

	[TestMethod]
	public void HxInputTags_Enabled_False_DisablesControl()
	{
		// Arrange
		var tags = new List<string> { "test" };

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputTags>(0);
			builder.AddAttribute(1, "Value", tags);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<List<string>>>)(() => tags));
			builder.AddAttribute(4, "Enabled", false);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — control shows disabled state
		var control = cut.Find(".hx-input-tags-control");
		Assert.IsTrue(control.ClassList.Contains("hx-input-tags-control-disabled"),
			"Control should have disabled CSS class when Enabled=false.");
	}

	[TestMethod]
	public void HxInputTags_Label_RendersLabel()
	{
		// Arrange
		var tags = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputTags>(0);
			builder.AddAttribute(1, "Value", tags);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<List<string>>>)(() => tags));
			builder.AddAttribute(4, "Label", "Tags");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert — label is rendered
		var label = cut.Find("label");
		Assert.Contains("Tags", label.TextContent);
	}
}
