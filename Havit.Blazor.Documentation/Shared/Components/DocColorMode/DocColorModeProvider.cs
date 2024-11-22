using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Shared.Components.DocColorMode;

public class DocColorModeProvider : IDisposable, IDocColorModeProvider
{
	private readonly IHttpContextProxy _httpContextProxy;
	private readonly PersistentComponentState _persistentComponentState;
	private PersistingComponentStateSubscription _persistingComponentStateSubscription;
	private ColorMode? _colorMode;

	public DocColorModeProvider(
		IHttpContextProxy httpContextProxy,
		PersistentComponentState persistentComponentState)
	{
		_httpContextProxy = httpContextProxy;
		_persistentComponentState = persistentComponentState;
		_persistingComponentStateSubscription = _persistentComponentState.RegisterOnPersisting(PersistMode);
	}

	public event ColorModeChangedHandler ColorModeChanged;

	/// <summary>
	/// Raises the <see cref="AuthenticationStateChanged"/> event.
	/// </summary>
	/// <param name="task">A <see cref="Task"/> that supplies the updated <see cref="AuthenticationState"/>.</param>
	public void SetColorMode(ColorMode colorMode)
	{
		_colorMode = colorMode;
		ColorModeChanged?.Invoke(colorMode);
	}

	public ColorMode GetColorMode()
	{
		if (_colorMode == null)
		{
			ResolveInitialColorMode();
		}
		return _colorMode.Value;
	}

	private void ResolveInitialColorMode()
	{
		// prerendering
		if (_httpContextProxy.IsSupported()
			&& _httpContextProxy.GetCookieValue("ColorMode") is string cookie
			&& Enum.TryParse<ColorMode>(cookie, ignoreCase: true, out var mode))
		{
			_colorMode = mode;
		}
		else if (_persistentComponentState.TryTakeFromJson<ColorMode>("ColorMode", out var restored))
		{
			_colorMode = restored;
		}
		else
		{
			_colorMode = ColorMode.Auto;
		}
	}

	private Task PersistMode()
	{
		_persistentComponentState.PersistAsJson("ColorMode", GetColorMode());
		return Task.CompletedTask;
	}

	void IDisposable.Dispose()
	{
		_persistingComponentStateSubscription.Dispose();
	}
}

/// <summary>
/// A handler for the <see cref="AuthenticationStateProvider.AuthenticationStateChanged"/> event.
/// </summary>
/// <param name="task">A <see cref="Task"/> that supplies the updated <see cref="AuthenticationState"/>.</param>
public delegate void ColorModeChangedHandler(ColorMode colorMode);

