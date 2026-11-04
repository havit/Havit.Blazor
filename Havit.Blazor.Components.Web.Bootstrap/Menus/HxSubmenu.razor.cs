using Havit.Blazor.Components.Web.Infrastructure;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Submenu (nested menu) for the <see cref="HxMenu"/>. Renders a <c>div.submenu</c> wrapper with a <c>button.menu-item</c> trigger
/// and a nested <c>.menu</c> element (with <see cref="ChildContent"/>). Submenus can be nested to any depth.<br />
/// See <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#submenus">Bootstrap 6 submenus</see>.
/// The opening (hover/click), positioning and keyboard navigation are handled by the parent <c>HxMenu</c>'s Bootstrap instance.
/// </summary>
public partial class HxSubmenu : ICascadeEnabledComponent
{
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => FormState; set => FormState = value; }

	/// <summary>
	/// Text of the submenu trigger. Ignored when <see cref="TitleTemplate"/> is set.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Custom content of the submenu trigger. Takes precedence over <see cref="Text"/>.
	/// </summary>
	[Parameter] public RenderFragment TitleTemplate { get; set; }

	/// <summary>
	/// Icon of the submenu trigger (use <see cref="BootstrapIcon" />).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the submenu trigger icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }

	/// <summary>
	/// Theme color variant of the trigger (renders the <c>theme-*</c> class).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }

	/// <summary>
	/// The nested menu content (typically <see cref="HxMenuItem"/>, <see cref="HxMenuItemNavLink"/>, another <see cref="HxSubmenu"/>, etc.).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
	[Parameter] public bool? Enabled { get; set; }

	/// <summary>
	/// Additional CSS class for the underlying <c>.submenu</c> element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the underlying <c>.submenu</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }
}
