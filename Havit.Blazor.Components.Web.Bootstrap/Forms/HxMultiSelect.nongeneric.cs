namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxMultiSelect{TValue, TItem}"/>.
/// </summary>
public sealed class HxMultiSelect
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxMultiSelect{TValue, TItem}"/>.
	/// </summary>
	public static MultiSelectSettings Defaults { get; set; }

	static HxMultiSelect()
	{
		Defaults = new MultiSelectSettings()
		{
			AllowFiltering = false,
			AllowSelectAll = false,
			ClearFilterOnHide = true
		};
	}
}
