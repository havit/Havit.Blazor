using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxButtonTests
{
	[TestMethod]
	public void HxButton_Click_TriggersOnClick()
	{
		// Arrange
		var clicked = false;
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Text, "Click me")
			.Add(p => p.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true))
		);

		// Act
		cut.Find("button").Click();

		// Assert
		Assert.IsTrue(clicked);
	}

	[TestMethod]
	public void HxButton_Disabled_DoesNotTriggerOnClick()
	{
		// Arrange
		var clicked = false;
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Text, "Disabled")
			.Add(p => p.Enabled, false)
			.Add(p => p.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true))
		);

		// Assert - button should be rendered as disabled
		var button = cut.Find("button");
		Assert.IsTrue(button.HasAttribute("disabled"));
		Assert.IsFalse(clicked);
	}

	[TestMethod]
	public async Task HxButton_Spinner_ShowsDuringAsyncOnClick()
	{
		// Arrange
		var tcs = new TaskCompletionSource();
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Text, "Async")
			.Add(p => p.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => tcs.Task))
		);

		// Act - initiate the click (async handler will be "in progress")
		var clickTask = cut.InvokeAsync(() => cut.Find("button").Click());

		// Assert - spinner should be shown during async operation
		cut.WaitForAssertion(() =>
		{
			var spinners = cut.FindAll(".spinner-border");
			Assert.IsTrue(spinners.Count > 0, "Spinner should be rendered during async OnClick");
		});

		// Complete the async operation
		tcs.SetResult();
		await clickTask;

		// Assert - spinner should be gone after completion
		cut.WaitForAssertion(() =>
		{
			var spinners = cut.FindAll(".spinner-border");
			Assert.AreEqual(0, spinners.Count, "Spinner should not be rendered after async OnClick completes");
		});
	}

	[TestMethod]
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
		cut.Find("button[type='submit']").Click();

		// Assert
		Assert.IsTrue(formSubmitted, "Form should be submitted when HxSubmit button is clicked");
	}

	[TestMethod]
	public void HxButtonGroup_Render_GroupsButtonsHorizontally()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButtonGroup>(parameters => parameters
			.Add(p => p.AriaLabel, "Test group")
			.AddChildContent<HxButton>(buttonParams => buttonParams.Add(p => p.Text, "First"))
			.AddChildContent<HxButton>(buttonParams => buttonParams.Add(p => p.Text, "Second"))
		);

		// Assert - the group should have the correct CSS class and role
		var group = cut.Find("div");
		Assert.IsTrue(group.ClassList.Contains("btn-group"), "Button group should have 'btn-group' CSS class");
		Assert.AreEqual("group", group.GetAttribute("role"));

		// Assert - both buttons should be rendered inside the group
		var buttons = cut.FindAll("button");
		Assert.AreEqual(2, buttons.Count, "Both buttons should be rendered inside the group");
	}

	private class TestFormModel
	{
		public string Value { get; set; } = "test";
	}
}
