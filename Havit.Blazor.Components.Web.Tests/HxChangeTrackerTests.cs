using System.ComponentModel;
using Havit.Blazor.Components.Web.Bootstrap.Tests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Tests;

public class HxChangeTrackerTests : BunitTestBase
{
	[Fact]
	public async Task HxChangeTracker_ValueNotSet_DisposeShouldNotThrow()
	{
		// Arrange
		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxChangeTracker>(0);
			builder.CloseComponent();
		};
		Assert.Throws<ArgumentException>(() => Render(componentRenderer));

		// Act + Assert
		await DisposeComponentsAsync();
	}

	[Fact]
	public async Task HxChangeTracker_Disposed_ShouldUnsubscribeFromValue()
	{
		// Arrange
		var value = new TrackedModel();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxChangeTracker>(0);
			builder.AddAttribute(1, nameof(HxChangeTracker.Value), value);
			builder.CloseComponent();
		};
		Render(componentRenderer);

		// Act
		await DisposeComponentsAsync();

		// Assert
		Assert.False(value.HasSubscribers);
	}

	private class TrackedModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public bool HasSubscribers => PropertyChanged is not null;
	}
}
