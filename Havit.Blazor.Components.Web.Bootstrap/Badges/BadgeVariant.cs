namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Visual variant of the <see cref="HxBadge"/> (composed with a theme color, see <see cref="HxBadge.Color"/>).
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/badge/">Bootstrap 6 badges</see>.
/// </summary>
public enum BadgeVariant
{
	/// <summary>
	/// Solid (filled) badge. Default.
	/// </summary>
	Solid = 0,

	/// <summary>
	/// Badge with a subtle (light) background (<c>badge-subtle</c>).
	/// </summary>
	Subtle,

	/// <summary>
	/// Outlined badge (<c>badge-outline</c>).
	/// </summary>
	Outline
}
