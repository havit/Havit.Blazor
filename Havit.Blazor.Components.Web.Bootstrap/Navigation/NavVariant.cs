namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Variations for <see cref="HxNav"/>.
/// </summary>
public enum NavVariant
{
	/// <summary>
	/// Base <c>nav</c>.
	/// </summary>
	Standard = 0,

	/// <summary>
	/// Tabs. <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/nav/#tabs">https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/nav/#tabs</see>
	/// Remember to set the <c>active</c> tab (<see cref="HxNavLink.CssClass"/>).
	/// </summary>
	Tabs = 1,

	/// <summary>
	/// Pills <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/nav/#pills">https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/nav/#pills</see>.
	/// </summary>
	Pills = 2,

	/// <summary>
	/// Underline <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/nav/#underline">https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/nav/#underline</see>.
	/// </summary>
	Underline = 3
}
