namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxSelect{TValue, TItem}"/>.
/// </summary>
public class HxSelect
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxSelect{TValue, TItem}"/> (<see cref="HxSelectBase{TValue, TItem}"/> and derived components, respectively).
	/// </summary>
	public static SelectSettings Defaults { get; set; }

	static HxSelect()
	{
		Defaults = new SelectSettings()
		{
			InputSize = InputSize.Regular,
		};
	}
}
