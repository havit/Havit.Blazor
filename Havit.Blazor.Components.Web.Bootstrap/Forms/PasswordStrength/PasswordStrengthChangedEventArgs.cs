namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Arguments for the <see cref="HxPasswordStrength.OnStrengthChanged"/> event.
/// </summary>
public class PasswordStrengthChangedEventArgs
{
	/// <summary>
	/// Current strength level of the password (<c>null</c> when the password is empty).
	/// </summary>
	public PasswordStrengthLevel? Strength { get; set; }

	/// <summary>
	/// Score computed by the scoring algorithm.
	/// </summary>
	public int Score { get; set; }
}
