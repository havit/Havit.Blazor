namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">Bootstrap 5 Dropdown</see> component for dropdown buttons.<br />
/// For generic dropdowns, use <see cref="HxDropdown"/>.
/// </summary>
public class HxDropdownButtonGroup : HxDropdown
{
	/// <summary>
	/// Set <c>true</c> to create a <a href="https://getbootstrap.com/docs/5.3/components/dropdowns/#split-button">split dropdown</a>
	/// (using a <c>btn-group</c>).
	/// </summary>
	[Parameter] public bool Split { get; set; }

	protected override string GetCoreCssClass()
	{
		/*
			.btn-group
			The basic.dropdown class only brings the position:relative requirement (+ directions within other variations).
			.btn-group is needed for split buttons and brings default in-row positioning to behave like regular buttons.
			It is used in almost all Bootstrap samples. It might be replaced with a more appropriate class if needed.
			.btn-group cannot be used in the Navbar as it breaks responsiveness.
		*/
		return "btn-group";
	}
}
