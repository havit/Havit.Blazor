namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Visual variant of the <see cref="HxCard"/>.
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/card/">Bootstrap 6 cards</see>.
/// </summary>
public enum CardVariant
{
	/// <summary>
	/// Regular card. Default.
	/// </summary>
	Regular = 0,

	/// <summary>
	/// Card with a themed subtle background (<c>card-subtle</c>, typically combined with <see cref="HxCard.Color"/>).
	/// </summary>
	Subtle,

	/// <summary>
	/// Frosted-glass translucent card (<c>card-translucent</c>).
	/// </summary>
	Translucent
}
