using Microsoft.AspNetCore.Components;
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

		// Act - clicking a disabled button should not invoke OnClick
		var button = cut.Find("button");
		button.Click();

		// Assert - button should be rendered as disabled and not trigger OnClick
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
}
