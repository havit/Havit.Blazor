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
	public void HxButton_Disabled_RendersDisabledAttribute()
	{
		// Arrange & Act
		var cut = RenderComponent<HxButton>(parameters => parameters
			.Add(p => p.Text, "Disabled")
			.Add(p => p.Enabled, false)
		);

		// Assert - button should be rendered with the disabled attribute
		var button = cut.Find("button");
		Assert.IsTrue(button.HasAttribute("disabled"));
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
			Assert.IsNotEmpty(spinners, "Spinner should be rendered during async OnClick");
		});

		// Complete the async operation
		tcs.SetResult();
		await clickTask;

		// Assert - spinner should be gone after completion
		cut.WaitForAssertion(() =>
		{
			var spinners = cut.FindAll(".spinner-border");
			Assert.IsEmpty(spinners, "Spinner should not be rendered after async OnClick completes");
		});
	}
}
