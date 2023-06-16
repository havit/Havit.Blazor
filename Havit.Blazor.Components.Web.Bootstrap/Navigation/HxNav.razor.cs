namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/navs-tabs/">Bootstrap Nav</see> component.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxNav">https://havit.blazor.eu/components/HxNav</see>
/// </summary>
public partial class HxNav
{
	/// <summary>
	/// Orientation of the nav.
	/// Default is <see cref="NavOrientation.Horizontal"/>.
	/// </summary>
	[Parameter] public NavOrientation Orientation { get; set; } = NavOrientation.Horizontal;

	/// <summary>
	/// The visual variant of the nav items.
	/// Default is <see cref="NavVariant.Standard"/>.
	/// </summary>
	[Parameter] public NavVariant Variant { get; set; } = NavVariant.Standard;

	/// <summary>
	/// ID of the nav which can be used for <see cref="HxScrollspy.TargetId"/>.
	/// </summary>
	[Parameter] public string Id { get; set; } = "hx-" + Guid.NewGuid().ToString("N");

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Content of the nav.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	protected virtual string GetCoreCssClass()
	{
		if (NavbarContainer is not null)
		{
			return "navbar-nav";
		}
		return "nav";
	}

	protected virtual string GetOrientationCssClass()
	{
		return this.Orientation switch
		{
			NavOrientation.Horizontal => null,
			NavOrientation.Vertical => "flex-column",
			_ => throw new InvalidOperationException($"Unknown {nameof(NavOrientation)} value {this.Orientation}.")
		};
	}

	protected virtual string GetVariantCssClass()
	{
		return this.Variant switch
		{
			NavVariant.Standard => null,
			NavVariant.Pills => "nav-pills",
			NavVariant.Tabs => "nav-tabs",
			NavVariant.Underline => "nav-underline",
			_ => throw new InvalidOperationException($"Unknown {nameof(NavVariant)} value {this.Variant}.")
		};
	}
}
