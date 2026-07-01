namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Interface to help keep the dropdown-toggle implementations aligned.
/// </summary>
internal interface IHxDropdownToggle
{
	(int Skidding, int Distance)? DropdownOffset { get; set; }
	string DropdownReference { get; set; }
	EventCallback OnHidden { get; set; }
	EventCallback OnShown { get; set; }
	DropdownAutoClose? AutoClose { get; set; }


	Task HandleJsHidden();
	Task HandleJsShown();
	Task HideAsync();
	Task ShowAsync();
}