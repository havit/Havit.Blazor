using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxValidationMessageTests : BunitTestBase
{
	[Fact]
	public void HxValidationMessage_NoErrors_RendersEmpty()
	{
		// Arrange
		var model = new TestModel();
		var editContext = new EditContext(model);

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<CascadingValue<EditContext>>(0);
			builder.AddAttribute(1, "Value", editContext);
			builder.AddAttribute(2, "ChildContent", (RenderFragment)((builder2) =>
			{
				builder2.OpenComponent<HxValidationMessage<string>>(0);
				builder2.AddAttribute(1, "For", (Expression<Func<string>>)(() => model.Name));
				builder2.CloseComponent();
			}));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert - no validation errors means no error messages rendered
		Assert.DoesNotContain("invalid-feedback", cut.Markup);
		Assert.DoesNotContain("is-invalid", cut.Markup);
	}

	[Fact]
	public void HxValidationMessage_WithError_DisplaysErrorText()
	{
		// Arrange
		var model = new TestModel();
		var editContext = new EditContext(model);
		var messageStore = new ValidationMessageStore(editContext);
		var fieldIdentifier = editContext.Field(nameof(TestModel.Name));
		messageStore.Add(fieldIdentifier, "Name is required.");

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<CascadingValue<EditContext>>(0);
			builder.AddAttribute(1, "Value", editContext);
			builder.AddAttribute(2, "ChildContent", (RenderFragment)((builder2) =>
			{
				builder2.OpenComponent<HxValidationMessage<string>>(0);
				builder2.AddAttribute(1, "For", (Expression<Func<string>>)(() => model.Name));
				builder2.CloseComponent();
			}));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert - validation error should be displayed
		Assert.Contains("invalid-feedback", cut.Markup);
		Assert.Contains("is-invalid", cut.Markup);
		Assert.Contains("Name is required.", cut.Markup);
	}

	[Fact]
	public void HxValidationMessage_MultipleErrors_DisplaysAll()
	{
		// Arrange
		var model = new TestModel();
		var editContext = new EditContext(model);
		var messageStore = new ValidationMessageStore(editContext);
		var fieldIdentifier = editContext.Field(nameof(TestModel.Name));
		messageStore.Add(fieldIdentifier, "Name is required.");
		messageStore.Add(fieldIdentifier, "Name must be at least 3 characters.");

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<CascadingValue<EditContext>>(0);
			builder.AddAttribute(1, "Value", editContext);
			builder.AddAttribute(2, "ChildContent", (RenderFragment)((builder2) =>
			{
				builder2.OpenComponent<HxValidationMessage<string>>(0);
				builder2.AddAttribute(1, "For", (Expression<Func<string>>)(() => model.Name));
				builder2.CloseComponent();
			}));
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert - all validation errors should be displayed
		Assert.Contains("Name is required.", cut.Markup);
		Assert.Contains("Name must be at least 3 characters.", cut.Markup);

		// Verify there are exactly 2 span elements with error messages
		var spans = cut.FindAll(".invalid-feedback span");
		Assert.Equal(2, spans.Count());
	}

	private class TestModel
	{
		public string Name { get; set; }
	}
}
