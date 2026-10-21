namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxOtpInput"/> component.
/// </summary>
public record OtpInputSettings : InputSettings
{
	/// <summary>
	/// The number of digit slots to render (the maximum length of the code).
	/// </summary>
	public int? Length { get; set; }

	/// <summary>
	/// When <c>true</c>, the slots display a dot (<c>•</c>) instead of the entered characters.
	/// </summary>
	public bool? Mask { get; set; }

	/// <summary>
	/// When <c>true</c>, the slots are visually connected into a single cohesive field with shared borders (Bootstrap <c>otp-connected</c> class).
	/// </summary>
	public bool? Connected { get; set; }
}
