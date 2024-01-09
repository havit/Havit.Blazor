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
	/// Tabs. <see href="https://getbootstrap.com/docs/5.3/components/navs-tabs/#tabs">https://getbootstrap.com/docs/5.3/components/navs-tabs/#tabs</see>
	/// Remember to set the <c>active</c> tab (<see cref="HxNavLink.CssClass"/>).
	/// </summary>
	Tabs = 1,

	/// <summary>
	/// Pills <see href="https://getbootstrap.com/docs/5.3/components/navs-tabs/#pills">https://getbootstrap.com/docs/5.3/components/navs-tabs/#pills</see>.
	/// </summary>
	Pills = 2,

	/// <summary>
	/// Underline <see href="https://getbootstrap.com/docs/5.3/components/navs-tabs/#underline">https://getbootstrap.com/docs/5.3/components/navs-tabs/#underline</see>.
	/// </summary>
	Underline = 3
}
