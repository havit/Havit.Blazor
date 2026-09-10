namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxGridColumn{TItem}"/> and derived components.
/// </summary>
public record GridColumnSettings
{
	/// <summary>
	/// The <c>tabindex</c> applied to the column's sortable header cell, controlling its keyboard tab order.
	/// Has no effect on non-sortable columns, where the header is not focusable.
	/// </summary>
	public int? TabIndex { get; set; }
}
