namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Score boundaries for the <see cref="HxPasswordStrength"/> strength levels
/// (mirrors the <c>thresholds</c> option of the underlying Bootstrap Strength plugin).
/// Scores above <see cref="Good"/> are evaluated as <see cref="PasswordStrengthLevel.Strong"/>.
/// </summary>
public record PasswordStrengthThresholds
{
	/// <summary>
	/// Maximum score evaluated as <see cref="PasswordStrengthLevel.Weak"/>.
	/// The default value is <c>2</c>.
	/// </summary>
	public int Weak { get; set; } = 2;

	/// <summary>
	/// Maximum score evaluated as <see cref="PasswordStrengthLevel.Fair"/>.
	/// The default value is <c>4</c>.
	/// </summary>
	public int Fair { get; set; } = 4;

	/// <summary>
	/// Maximum score evaluated as <see cref="PasswordStrengthLevel.Good"/>.
	/// The default value is <c>6</c>.
	/// </summary>
	public int Good { get; set; } = 6;
}
