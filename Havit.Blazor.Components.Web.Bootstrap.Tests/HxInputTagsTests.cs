using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxInputTagsTests : BunitTestBase
{
	[Fact]
	public void HxInputTags_WithTags_RendersChips()
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

		// Assert — tags render as chip elements
		var tagElements = cut.FindAll(".chip.hx-tag");
		Assert.Equal(3, tagElements.Count());
	}

	[Fact]
	public void HxInputTags_EmptyTags_NoChips()
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

		// Assert — no tag chips rendered
		var tagElements = cut.FindAll(".chip.hx-tag");
		Assert.Empty(tagElements);
	}

	[Fact]
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
		Assert.Equal("Add tags...", input.GetAttribute("placeholder"));
	}

	[Fact]
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
		Assert.True(control.ClassList.Contains("hx-input-tags-control-disabled"), "Control should have disabled CSS class when Enabled=false.");
	}

	[Fact]
	public void HxInputTags_ChipInputWrapper_Rendered()
	{
		// Arrange
		var tags = new List<string> { "test" };

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

		// Assert — the chip-input wrapper carries the visual chrome, the inner input is a form-ghost
		var wrapper = cut.Find(".chip-input");
		Assert.NotNull(wrapper.QuerySelector("input.form-ghost"));
	}

	[Fact]
	public void HxInputTags_LabelTypeFloating_ThrowsInvalidOperationException()
	{
		// LabelType.Floating is not supported in Bootstrap 6 — the chip-input wrapper owns the visual chrome and cannot host a floating label.

		// Arrange
		var tags = new List<string>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputTags>(0);
			builder.AddAttribute(1, "Value", tags);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<List<string>>(this, v => { }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<List<string>>>)(() => tags));
			builder.AddAttribute(4, "Label", "Tags");
			builder.AddAttribute(5, "LabelType", (LabelType?)LabelType.Floating);
			builder.CloseComponent();
		};

		// Act + Assert
		Assert.Throws<InvalidOperationException>(() => Render(componentRenderer));
	}

	[Fact]
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
