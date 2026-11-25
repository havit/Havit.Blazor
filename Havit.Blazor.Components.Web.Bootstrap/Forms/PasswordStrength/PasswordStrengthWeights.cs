namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Point values for the individual criteria of the <see cref="HxPasswordStrength"/> scoring algorithm
/// (mirrors the <c>weights</c> option of the underlying Bootstrap Strength plugin).
/// Set a weight to <c>0</c> to disable the corresponding criterion.
/// </summary>
public record PasswordStrengthWeights
{
	/// <summary>
	/// Points for meeting the minimum length (<see cref="HxPasswordStrength.MinLength"/>, <c>8</c> characters by default).
	/// The default value is <c>1</c>.
	/// </summary>
	public int MinLength { get; set; } = 1;

	/// <summary>
	/// Points for exceeding the minimum length by 4 or more characters.
	/// The default value is <c>1</c>.
	/// </summary>
	public int ExtraLength { get; set; } = 1;

	/// <summary>
	/// Points for containing lowercase letters.
	/// The default value is <c>1</c>.
	/// </summary>
	public int Lowercase { get; set; } = 1;

	/// <summary>
	/// Points for containing uppercase letters.
	/// The default value is <c>1</c>.
	/// </summary>
	public int Uppercase { get; set; } = 1;

	/// <summary>
	/// Points for containing numbers.
	/// The default value is <c>1</c>.
	/// </summary>
	public int Numbers { get; set; } = 1;

	/// <summary>
	/// Points for containing special characters.
	/// The default value is <c>1</c>.
	/// </summary>
	public int Special { get; set; } = 1;

	/// <summary>
	/// Points for containing multiple special characters.
	/// The default value is <c>1</c>.
	/// </summary>
	public int MultipleSpecial { get; set; } = 1;

	/// <summary>
	/// Points for a long password (16 or more characters).
	/// The default value is <c>1</c>.
	/// </summary>
	public int LongPassword { get; set; } = 1;
}
