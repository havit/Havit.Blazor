using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxButtonTests
{
	[Fact]
	public void HxButton_Click_TriggersOnClick()
	{
		// Arrange
		var clicked = false;
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Click me")
			.Add(p => p.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true))
		);

		// Act
		cut.Find("button").Click();

		// Assert
		Assert.True(clicked);
	}

	[Fact]
	public void HxButton_Disabled_RendersDisabledAttribute()
	{
		// Arrange & Act
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Disabled")
			.Add(p => p.Enabled, false)
		);

		// Assert - button should be rendered with the disabled attribute
		var button = cut.Find("button");
		Assert.True(button.HasAttribute("disabled"));
	}

	[Fact]
	public async Task HxButton_Spinner_ShowsDuringAsyncOnClick()
	{
		// Arrange
		var tcs = new TaskCompletionSource();
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Async")
			.Add(p => p.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => tcs.Task))
		);

		// Act - initiate the click (async handler will be "in progress")
		var clickTask = cut.InvokeAsync(() => cut.Find("button").Click());

		// Assert - spinner should be shown during async operation
		cut.WaitForAssertion(() =>
		{
			var spinners = cut.FindAll(".spinner-border");
			Assert.NotEmpty(spinners);
		});

		// Complete the async operation
		tcs.SetResult();
		await clickTask;

		// Assert - spinner should be gone after completion
		cut.WaitForAssertion(() =>
		{
			var spinners = cut.FindAll(".spinner-border");
			Assert.Empty(spinners);
		});
	}

	[Fact]
	public async Task HxButton_Spinner_IsAriaHiddenWithoutStatusRole()
	{
		// Arrange
		var tcs = new TaskCompletionSource();
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Save")
			.Add(p => p.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => tcs.Task))
		);

		// Act
		var clickTask = cut.InvokeAsync(() => cut.Find("button").Click());

		// Assert - the spinner text must not become part of the button's accessible name
		cut.WaitForAssertion(() =>
		{
			var spinner = cut.Find("button .spinner-border");
			Assert.Equal("true", spinner.GetAttribute("aria-hidden"));
			Assert.Empty(cut.FindAll("button [role=status]"));
			Assert.Empty(cut.FindAll("button .visually-hidden"));
		});

		tcs.SetResult();
		await clickTask;
	}

	[Theory]
	[InlineData(null, false, null)]
	[InlineData(false, false, "false")]
	[InlineData(true, true, "true")]
	public void HxButton_Active_RendersActiveClassAndAriaPressed(bool? active, bool expectedActiveClass, string expectedAriaPressed)
	{
		// Arrange & Act
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Toggle")
			.Add(p => p.Active, active));

		// Assert
		var button = cut.Find("button");
		Assert.Equal(expectedActiveClass, button.ClassList.Contains("active"));
		Assert.Equal(expectedAriaPressed, button.GetAttribute("aria-pressed"));
	}
}
