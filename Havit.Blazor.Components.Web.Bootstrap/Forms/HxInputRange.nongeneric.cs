namespace Havit.Blazor.Components.Web.Bootstrap;

public static class HxInputRange
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputRange{TValue}"/>.
	/// </summary>
	public static InputRangeSettings Defaults { get; set; }

	static HxInputRange()
	{
		Defaults = new InputRangeSettings()
		{
			BindEvent = BindEvent.OnChange
		};
	}
}
