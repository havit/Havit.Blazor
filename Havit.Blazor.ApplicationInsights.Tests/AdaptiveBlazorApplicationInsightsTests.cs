using Havit.Blazor.ApplicationInsights.Telemetry;
using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights.Tests;

public class AdaptiveBlazorApplicationInsightsTests
{
	[Fact]
	public async Task AdaptiveBlazorApplicationInsights_WhenJsNotAvailable_FirstCallDoesNotThrow()
	{
		// Arrange
		var jsRuntime = new FakeJSRuntime { ShouldThrow = true };
		var sut = CreateSut(jsRuntime);

		// Act & Assert — exception must not propagate
		await sut.TrackEventAsync(new EventTelemetry { Name = "test" });
	}

	[Fact]
	public async Task AdaptiveBlazorApplicationInsights_WhenJsNotAvailable_SubsequentCallsAreSkippedWithoutCallingJs()
	{
		// Arrange
		var jsRuntime = new FakeJSRuntime { ShouldThrow = true };
		var sut = CreateSut(jsRuntime);

		// Act
		await sut.TrackEventAsync(new EventTelemetry { Name = "first" });  // probes JS, determines unavailable
		await sut.TrackEventAsync(new EventTelemetry { Name = "second" }); // fast-path skip
		await sut.TrackEventAsync(new EventTelemetry { Name = "third" });  // fast-path skip

		// Assert — JS was touched only during the initial probe
		Assert.Equal(1, jsRuntime.InvocationCount);
	}

	[Fact]
	public async Task AdaptiveBlazorApplicationInsights_WhenJsAvailable_CallIsForwardedToJs()
	{
		// Arrange
		var jsRuntime = new FakeJSRuntime { ShouldThrow = false };
		var sut = CreateSut(jsRuntime);

		// Act
		await sut.TrackEventAsync(new EventTelemetry { Name = "test" });

		// Assert
		Assert.Equal(1, jsRuntime.InvocationCount);
	}

	[Fact]
	public async Task AdaptiveBlazorApplicationInsights_WhenJsWasAvailableAndThenThrows_ExceptionPropagates()
	{
		// Arrange
		var jsRuntime = new FakeJSRuntime { ShouldThrow = false };
		var sut = CreateSut(jsRuntime);
		await sut.TrackEventAsync(new EventTelemetry { Name = "first" }); // succeeds → _isJsAvailable = true

		// Act — JS starts throwing after availability was already confirmed
		jsRuntime.ShouldThrow = true;

		// Assert — exception must propagate; the catch 'when' guard does not fire for _isJsAvailable == true
		await Assert.ThrowsAsync<InvalidOperationException>(() => sut.TrackEventAsync(new EventTelemetry { Name = "second" }));
	}

	private static AdaptiveBlazorApplicationInsights CreateSut(IJSRuntime jsRuntime)
	{
		var browserImpl = new BrowserBlazorApplicationInsights(jsRuntime);
		return new AdaptiveBlazorApplicationInsights(browserImpl);
	}

	private class FakeJSRuntime : IJSRuntime
	{
		public bool ShouldThrow { get; set; }
		public int InvocationCount { get; private set; }

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
		{
			InvocationCount++;
			if (ShouldThrow)
			{
				throw new InvalidOperationException("JavaScript interop calls cannot be issued at this time.");
			}
			return ValueTask.FromResult(default(TValue));
		}

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args)
		{
			InvocationCount++;
			if (ShouldThrow)
			{
				throw new InvalidOperationException("JavaScript interop calls cannot be issued at this time.");
			}
			return ValueTask.FromResult(default(TValue));
		}
	}
}
