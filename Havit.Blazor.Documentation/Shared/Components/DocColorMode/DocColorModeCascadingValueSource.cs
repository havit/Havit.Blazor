namespace Havit.Blazor.Documentation.Shared.Components.DocColorMode;

// Reference implementation in AuthenticationStateCascadingValueSource
// https://github.com/dotnet/aspnetcore/blob/79d06db8a4be29165e24eb841054a337161bd09a/src/Components/Authorization/src/CascadingAuthenticationStateServiceCollectionExtensions.cs#L29-L56
public class DocColorModeCascadingValueSource : CascadingValueSource<ColorMode>, IDisposable
{
	private readonly IDocColorModeProvider _docColorModeStateProvider;

	public DocColorModeCascadingValueSource(IDocColorModeProvider docColorModeStateProvider)
		: base(docColorModeStateProvider.GetColorMode, isFixed: false)
	{
		_docColorModeStateProvider = docColorModeStateProvider;
		_docColorModeStateProvider.ColorModeChanged += HandleColorModeChanged;
	}

	private void HandleColorModeChanged(ColorMode colorMode)
	{
		// It's OK to discard the task because this only represents the duration of the dispatch to sync context.
		// It handles any exceptions internally by dispatching them to the renderer within the context of whichever
		// component threw when receiving the update. This is the same as how a CascadingValue doesn't get notified
		// about exceptions that happen inside the recipients of value notifications.
		_ = NotifyChangedAsync(colorMode);
	}

	public void Dispose()
	{
		_docColorModeStateProvider.ColorModeChanged -= HandleColorModeChanged;
	}
}
