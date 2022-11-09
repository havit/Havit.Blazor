namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public abstract record class TooltipInternalSettings
{
	/// <summary>
	/// Apply a CSS fade transition to the tooltip (enable/disable).
	/// </summary>
	public bool? Animation { get; set; }

	/// <summary>
	/// Custom CSS class to add.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with the <c>span</c> wrapper of the child-content.
	/// </summary>
	public string WrapperCssClass { get; set; }

	/// <summary>
	/// Offset of the component relative to its target (ChildContent).
	/// </summary>
	public (int X, int Y)? Offset { get; set; }
}