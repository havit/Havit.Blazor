namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Interface to help keep the menu-toggle implementations aligned.
/// </summary>
internal interface IHxMenuToggle
{
	(int Skidding, int Distance)? MenuOffset { get; set; }
	string MenuReference { get; set; }
	EventCallback OnHidden { get; set; }
	EventCallback OnShown { get; set; }
	MenuAutoClose? AutoClose { get; set; }


	Task HandleJsHidden();
	Task HandleJsShown();
	Task HideAsync();
	Task ShowAsync();
}
