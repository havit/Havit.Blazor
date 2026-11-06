namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxPasswordStrength"/>.
/// </summary>
public record PasswordStrengthSettings
{
	/// <summary>
	/// Visualization variant of the strength meter (segmented meter or a single progress bar).
	/// </summary>
	public PasswordStrengthVariant? Variant { get; set; }

	/// <summary>
	/// Minimum password length required for the first strength point.
	/// </summary>
	public int? MinLength { get; set; }

	/// <summary>
	/// Point values for the individual criteria of the scoring algorithm.
	/// </summary>
	public PasswordStrengthWeights Weights { get; set; }

	/// <summary>
	/// Score boundaries for the strength levels.
	/// </summary>
	public PasswordStrengthThresholds Thresholds { get; set; }

	/// <summary>
	/// When <c>true</c>, a text feedback element is rendered below the meter.
	/// </summary>
	public bool? ShowText { get; set; }

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Weak"/> level.
	/// </summary>
	public string WeakText { get; set; }

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Fair"/> level.
	/// </summary>
	public string FairText { get; set; }

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Good"/> level.
	/// </summary>
	public string GoodText { get; set; }

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Strong"/> level.
	/// </summary>
	public string StrongText { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the strength meter.
	/// </summary>
	public string CssClass { get; set; }
}
