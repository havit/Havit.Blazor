namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <seealso cref="BindEvent" /> extensions.
/// </summary>
public static class BindEventExtensions
{
	/// <summary>
	/// Gets the name of the event as a string.
	/// </summary>
	public static string ToEventName(this BindEvent value)
	{
		return value.ToString().ToLower();
	}
}
