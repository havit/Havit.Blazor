namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxMultiSelect{TValue, TItem}"/>.
/// </summary>
public class HxMultiSelect
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxMultiSelect{TValue, TItem}"/>.
	/// </summary>
	public static MultiSelectSettings Defaults { get; set; }

	static HxMultiSelect()
	{
		Defaults = new MultiSelectSettings()
		{
			InputSize = InputSize.Regular,
			AllowFiltering = false,
			AllowSelectAll = false,
			ClearFilterOnHide = true
		};
	}
}
