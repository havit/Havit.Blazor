namespace Havit.Blazor.Documentation.Shared.Components.DocColorMode;

public class DocColorModeClientResolver : IDocColorModeResolver
{
	public ColorMode GetColorMode()
	{
		return ColorMode.Auto; // client always resolves to auto, cookie used for server prerendering
	}
}
