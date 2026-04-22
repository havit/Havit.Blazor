namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxGridColumn{TItem}"/> and derived components.
/// </summary>
public record GridColumnSettings
{
	/// <summary>
	/// Adds tabindex to column
	/// </summary>
	public int? TabIndex { get; set; }
}
