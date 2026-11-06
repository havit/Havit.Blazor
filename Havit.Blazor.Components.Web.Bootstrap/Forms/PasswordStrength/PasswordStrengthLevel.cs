namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Password strength level used by <see cref="HxPasswordStrength"/>.
/// </summary>
public enum PasswordStrengthLevel
{
	/// <summary>
	/// Weak password (one segment of the meter is filled).
	/// </summary>
	Weak = 0,

	/// <summary>
	/// Fair password (two segments of the meter are filled).
	/// </summary>
	Fair = 1,

	/// <summary>
	/// Good password (three segments of the meter are filled).
	/// </summary>
	Good = 2,

	/// <summary>
	/// Strong password (all four segments of the meter are filled).
	/// </summary>
	Strong = 3
}
