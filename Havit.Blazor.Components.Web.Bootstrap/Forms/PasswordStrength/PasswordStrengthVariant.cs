namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Visualization variant of the <see cref="HxPasswordStrength"/> meter.
/// </summary>
public enum PasswordStrengthVariant
{
	/// <summary>
	/// Segmented meter&#8212;four segments that fill progressively as the password strength increases (renders the <c>strength</c> CSS class).
	/// </summary>
	Segments = 0,

	/// <summary>
	/// Single progress bar that grows with the password strength (renders the <c>strength-bar</c> CSS class).
	/// </summary>
	ProgressBar = 1
}
