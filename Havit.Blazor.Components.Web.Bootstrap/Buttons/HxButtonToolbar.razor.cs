namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/button-group/#button-toolbar">Button toolbar</see> component.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxButtonToolbar">https://havit.blazor.eu/components/HxButtonToolbar</see>
/// </summary>
public partial class HxButtonToolbar
{
	/// <summary>
	/// Toolbar's content
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// An explicit label should be set, as most assistive technologies will otherwise not announce them, despite the presence of the correct role attribute. 
	/// </summary>
	[Parameter] public string AriaLabel { get; set; }

	/// <summary>
	/// CSS class(es) to add to the HTML element with the <c>.btn-toolbar</c> class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
}
