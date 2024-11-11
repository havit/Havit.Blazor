using Havit.Blazor.Documentation.Shared.Components.DocColorMode;

namespace Havit.Blazor.Documentation.Server;

public partial class App(IDocColorModeResolver docColorModeResolver)
{
	private readonly IDocColorModeResolver _docColorModeResolver = docColorModeResolver;
}
