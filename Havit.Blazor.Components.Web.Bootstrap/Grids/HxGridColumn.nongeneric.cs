namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for the <see cref="HxGridColumn{TItem}"/> component.
/// </summary>
/// <remarks>
/// Marker for resources for <see cref="HxGridColumn{TItem}"/>.
/// It is unfriendly to create resources for generic classes.
/// </remarks>
public sealed class HxGridColumn
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxGridColumn{TItem}"/> and derived components.
	/// </summary>
	public static GridColumnSettings Defaults { get; set; }

	static HxGridColumn()
	{
		Defaults = new GridColumnSettings()
		{
			TabIndex = null
		};
	}
}
