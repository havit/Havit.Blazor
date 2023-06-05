namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">Bootstrap 5 Dropdown</see> generic component.<br />
/// For buttons with dropdowns use more specific <see cref="HxDropdownButtonGroup"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxDropdown">https://havit.blazor.eu/components/HxDropdown</see>
/// </summary>
public partial class HxDropdown : ComponentBase, IDropdownContainer
{
	/// <summary>
	/// Direction in which the dropdown is opened.
	/// </summary>
	[Parameter] public DropdownDirection Direction { get; set; }

	/// <summary>
	/// By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (<see cref="DropdownAutoClose.True"/>).
	/// You can use the AutoClose parameter to change this behavior of the dropdown.
	/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior">https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior</see>.
	/// </summary>
	[Parameter] public DropdownAutoClose AutoClose { get; set; } = DropdownAutoClose.True;

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <c>div</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	bool IDropdownContainer.IsOpen { get; set; }

	protected string GetDropdownDirectionCssClass()
	{
		return this.Direction switch
		{
			DropdownDirection.Down => "dropdown",
			DropdownDirection.Up => "dropup",
			DropdownDirection.Start => "dropstart",
			DropdownDirection.End => "dropend",
			_ => throw new InvalidOperationException($"Unknown {nameof(DropdownDirection)} value {Direction}.")
		};
	}

	protected virtual string GetCoreCssClass() => null;
}
