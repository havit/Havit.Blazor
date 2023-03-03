namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap Dropdown menu which opens when triggered.
/// </summary>
public partial class HxDropdownMenu
{
	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Parameter] public DropdownMenuAlignment? Alignment { get; set; }

	protected virtual string ElementName => "ul";

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <c>ul</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		AdditionalAttributes ??= new();
		AdditionalAttributes["class"] = GetCssClass();
	}

	protected string GetCssClass() =>
		CssClassHelper.Combine(
			"dropdown-menu",
			((DropdownContainer as IDropdownContainer)?.IsOpen ?? false) ? "show" : null,
			this.Alignment?.GetCssClass(),
			this.CssClass
			);
}
