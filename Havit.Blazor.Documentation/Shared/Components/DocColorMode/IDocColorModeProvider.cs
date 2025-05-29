namespace Havit.Blazor.Documentation.Shared.Components.DocColorMode;

public interface IDocColorModeProvider
{
	event ColorModeChangedHandler ColorModeChanged;

	ColorMode GetColorMode();
	void SetColorMode(ColorMode colorMode);
}