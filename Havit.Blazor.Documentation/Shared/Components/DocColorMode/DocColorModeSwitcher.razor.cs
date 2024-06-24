using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Shared.Components.DocColorMode;

/// <summary>
/// Provides switcher for color mode (theme).
/// </summary>
/// <remarks>
/// The current mode is persisted in a "ColorMode" cookie.
/// The cookie is used for server prerendering (see DocColorModeServerResolver).
/// The server-prerendering value is passed to WASM client by PersistentComponentState.
/// The client-side component uses JS to switch the color mode and save the new value to the cookie.
/// The auto mode is resolved by color-mode-auto.js startup script (see _Layout.cshtml).
/// </remarks>
public partial class DocColorModeSwitcher : IDisposable
{
	[Inject] protected IDocColorModeResolver DocColorModeResolver { get; set; }
	[Inject] protected PersistentComponentState PersistentComponentState { get; set; }
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private PersistingComponentStateSubscription _persistingSubscription;
	private IJSObjectReference _jsModule;
	private ColorMode _mode = ColorMode.Auto;

	protected override void OnInitialized()
	{
		_persistingSubscription = PersistentComponentState.RegisterOnPersisting(PersistMode);

		if (PersistentComponentState.TryTakeFromJson<ColorMode>("ColorMode", out var restored))
		{
			_mode = restored;
		}
		else
		{
			_mode = DocColorModeResolver.GetColorMode();
		}
	}
	private Task PersistMode()
	{
		PersistentComponentState.PersistAsJson("ColorMode", _mode);
		return Task.CompletedTask;
	}

	private async Task HandleClick()
	{
		_mode = _mode switch
		{
			ColorMode.Auto => ColorMode.Dark,
			ColorMode.Dark => ColorMode.Light,
			ColorMode.Light => ColorMode.Auto,
			_ => ColorMode.Auto // fallback
		};

		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("setColorMode", _mode.ToString("g").ToLowerInvariant());
	}

	private async Task EnsureJsModule()
	{
		_jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/Components/DocColorMode/DocColorModeSwitcher.razor.js");
	}

	private IconBase GetIcon()
	{
		return _mode switch
		{
			ColorMode.Auto => BootstrapIcon.CircleHalf,
			ColorMode.Light => BootstrapIcon.Sun,
			ColorMode.Dark => BootstrapIcon.Moon,
			_ => throw new InvalidOperationException($"Unknown color mode '{_mode}'.")
		};
	}

	private string GetTooltip()
	{
		return _mode switch
		{
			ColorMode.Auto => "Auto color mode (theme). Click to switch to Dark.",
			ColorMode.Dark => "Dark color mode (theme). Click to switch to Light.",
			ColorMode.Light => "Light color mode (theme). Click to switch to Auto.",
			_ => "Click to switch color mode (theme) to Auto." // fallback
		};
	}

	void IDisposable.Dispose()
	{
		_persistingSubscription.Dispose();
	}
}
