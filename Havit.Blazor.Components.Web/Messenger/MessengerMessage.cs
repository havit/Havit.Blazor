namespace Havit.Blazor.Components.Web;

/// <summary>
/// Messenger message.
/// </summary>
public class MessengerMessage
{
	/// <summary>
	/// Key. Used for component pairing during rendering (@key).
	/// </summary>
	public string Key { get; } = Guid.NewGuid().ToString("N");

	/// <summary>
	/// Message icon (header).
	/// </summary>
	public IconBase Icon { get; set; }

	/// <summary>
	/// CSS class.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Delay in milliseconds to auto-hide message.
	/// </summary>
	public int? AutohideDelay { get; set; }

	/// <summary>
	/// Message title (header).
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	/// Custom message header.
	/// </summary>
	public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// Message text (body).
	/// </summary>
	public string Text { get; set; }

	/// <summary>
	/// Custom message body (content).
	/// </summary>
	public RenderFragment ContentTemplate { get; set; }
}
