namespace Havit.Blazor.Components.Web;

public struct MessageBoxRequest
{
	/// <summary>
	/// Title in the header.
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	/// Template for the header.
	/// </summary>
	public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// Content (body) text.
	/// </summary>
	public string Text { get; set; }

	/// <summary>
	/// Body (content) template.
	/// </summary>
	public RenderFragment BodyTemplate { get; set; }

	/// <summary>
	/// Indicates whether to show the close button.
	/// </summary>
	public bool? ShowCloseButton { get; set; }

	/// <summary>
	/// Buttons to show.
	/// </summary>
	public MessageBoxButtons Buttons { get; set; }

	/// <summary>
	/// Primary button (if you want to override the default).
	/// </summary>
	public MessageBoxButtons? PrimaryButton { get; set; }

	/// <summary>
	/// Text for the <see cref="MessageBoxButtons.Custom"/> button.
	/// </summary>
	public string CustomButtonText { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying UI component (Bootstrap: HxMessageBox -> HxModal).
	/// </summary>
	public Dictionary<string, object> AdditionalAttributes { get; set; }
}
