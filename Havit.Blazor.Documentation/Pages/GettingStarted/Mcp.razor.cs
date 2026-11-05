namespace Havit.Blazor.Documentation.Pages.GettingStarted;

public partial class Mcp
{
	private IdeChoice _ide = IdeChoice.VsCode;

	private static readonly IdeChoice[] _ideChoices = [IdeChoice.VsCode, IdeChoice.VisualStudio, IdeChoice.ClaudeCode, IdeChoice.Other];

	private static string GetIdeChoiceText(IdeChoice ide) => ide switch
	{
		IdeChoice.VsCode => "VS Code",
		IdeChoice.VisualStudio => "Visual Studio",
		IdeChoice.ClaudeCode => "Claude Code",
		IdeChoice.Other => "Other",
		_ => ide.ToString()
	};


	private enum IdeChoice { VsCode, VisualStudio, ClaudeCode, Other }
}
