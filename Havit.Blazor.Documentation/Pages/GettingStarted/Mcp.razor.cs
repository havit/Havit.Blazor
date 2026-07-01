namespace Havit.Blazor.Documentation.Pages.GettingStarted;

public partial class Mcp
{
	private IdeChoice _ide = IdeChoice.VsCode;

	private enum IdeChoice { VsCode, VisualStudio, ClaudeCode, Other }
}
