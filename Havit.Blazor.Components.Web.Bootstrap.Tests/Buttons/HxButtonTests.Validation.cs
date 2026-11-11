using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxButtonTests
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void HxButton_OnValidClickOnInvalidClick_InvokesCallbackByValidationResult(bool isValid)
	{
		// Arrange
		var model = new object();
		var editContext = new EditContext(model);
		var messageStore = new ValidationMessageStore(editContext);
		editContext.OnValidationRequested += (_, _) =>
		{
			messageStore.Clear();
			if (!isValid)
			{
				messageStore.Add(new FieldIdentifier(model, "Field"), "Invalid");
			}
		};

		var validClickCount = 0;
		var invalidClickCount = 0;
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Click me")
			.Add(p => p.EditContext, editContext)
			.Add(p => p.OnValidClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => validClickCount++))
			.Add(p => p.OnInvalidClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => invalidClickCount++))
		);

		// Act
		cut.Find("button").Click();

		// Assert
		Assert.Equal(isValid ? 1 : 0, validClickCount);
		Assert.Equal(isValid ? 0 : 1, invalidClickCount);
	}

#if NET11_0_OR_GREATER
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task HxButton_OnValidClickOnInvalidClick_WaitsForAsyncValidation_WithSingleClickProtection(bool isValid)
	{
		// Arrange
		var model = new object();
		var editContext = new EditContext(model);
		var messageStore = new ValidationMessageStore(editContext);
		var validationCompletion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		var validationRequestedCount = 0;
		editContext.OnValidationRequested += (_, args) =>
		{
			validationRequestedCount++;
			messageStore.Clear();
			args.AddAsyncValidator(async cancellationToken =>
			{
				await validationCompletion.Task.WaitAsync(cancellationToken);
				if (!isValid)
				{
					messageStore.Add(new FieldIdentifier(model, "Field"), "Invalid");
				}
			});
		};

		var validClickCount = 0;
		var invalidClickCount = 0;
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Click me")
			.Add(p => p.EditContext, editContext)
			.Add(p => p.OnValidClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => validClickCount++))
			.Add(p => p.OnInvalidClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => invalidClickCount++))
		);

		// Act - click while the async validation is pending
		var firstClickTask = cut.Find("button").ClickAsync(new MouseEventArgs());

		// Assert - no callback before validation completes, button protected against another click
		Assert.False(firstClickTask.IsCompleted);
		Assert.Equal(0, validClickCount);
		Assert.Equal(0, invalidClickCount);
		Assert.True(cut.Find("button").HasAttribute("disabled"));

		await cut.Find("button").ClickAsync(new MouseEventArgs()); // second click is ignored while the first one is in progress
		Assert.Equal(1, validationRequestedCount);

		// Act - complete the validation
		validationCompletion.SetResult();
		await firstClickTask;

		// Assert
		Assert.Equal(isValid ? 1 : 0, validClickCount);
		Assert.Equal(isValid ? 0 : 1, invalidClickCount);
		Assert.Equal(1, validationRequestedCount);
		Assert.False(cut.Find("button").HasAttribute("disabled"));
	}
#endif
}
