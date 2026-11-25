namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Visual variant of the <see cref="HxButton"/> (composed with a theme color, see <see cref="HxButton.Color"/>).
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/button/">Bootstrap 6 buttons</see>.
/// </summary>
public enum ButtonVariant
{
	/// <summary>
	/// Solid (filled) button. Default.
	/// </summary>
	Solid = 0,

	/// <summary>
	/// Outlined button.
	/// </summary>
	Outline,

	/// <summary>
	/// Button with a subtle (light) background.
	/// </summary>
	Subtle,

	/// <summary>
	/// Text-only button (no background or border).
	/// </summary>
	Text,

	/// <summary>
	/// Link-styled button.
	/// </summary>
	Link
}
