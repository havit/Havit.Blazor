namespace Havit.Blazor.Documentation.Pages;

public partial class HeroCard
{
	[Parameter] public string Title { get; set; }

	[Parameter] public string Description { get; set; }

	[Parameter] public BootstrapIcon Icon { get; set; }

	[Parameter] public ThemeColor Color { get; set; }
}