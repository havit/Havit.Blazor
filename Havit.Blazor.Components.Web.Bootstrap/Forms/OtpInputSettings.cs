namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxOtpInput"/> component.
/// </summary>
public record OtpInputSettings : InputSettings
{
	/// <summary>
	/// The number of inputs (digits) to render.
	/// </summary>
	public int? Length { get; set; }

	/// <summary>
	/// When <c>true</c>, the inputs render as <c>type="password"</c> fields to hide the entered values.
	/// </summary>
	public bool? Mask { get; set; }

	/// <summary>
	/// When <c>true</c>, the inputs are visually connected into a single cohesive field (Bootstrap <c>input-group</c> styles).
	/// </summary>
	public bool? Connected { get; set; }
}
