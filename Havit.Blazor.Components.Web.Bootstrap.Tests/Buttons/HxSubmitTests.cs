using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxSubmitTests : BunitTestBase
{
	[Fact]
	public void HxSubmit_Click_SubmitsForm()
	{
		// Arrange
		var formSubmitted = false;

		var cut = Render(builder =>
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, nameof(EditForm.Model), new TestFormModel());
			builder.AddAttribute(2, nameof(EditForm.OnValidSubmit), EventCallback.Factory.Create<EditContext>(this, () => formSubmitted = true));
			builder.AddAttribute(3, nameof(EditForm.ChildContent), (RenderFragment<EditContext>)((EditContext _) => (RenderTreeBuilder innerBuilder) =>
			{
				innerBuilder.OpenComponent<HxSubmit>(0);
				innerBuilder.AddAttribute(1, nameof(HxSubmit.Text), "Submit");
				innerBuilder.CloseComponent();
			}));
			builder.CloseComponent();
		});

		// Act
		var submitButton = cut.Find("button");
		Assert.Equal("submit", submitButton.GetAttribute("type"));
		cut.Find("form").Submit();

		// Assert
		Assert.True(formSubmitted, "Form should be submitted when the form is submitted");
	}

	private class TestFormModel
	{
		public string Value { get; set; } = "test";
	}
}
