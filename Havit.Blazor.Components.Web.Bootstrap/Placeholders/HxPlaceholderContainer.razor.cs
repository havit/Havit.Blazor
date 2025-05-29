namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Optional container for the <see cref="HxPlaceholder"/> components where you can set the animation and some common properties
/// for all placeholders contained.
/// </summary>
public partial class HxPlaceholderContainer
{
	/// <summary>
	/// Gets or sets the name of the element to render. Default is <code>span</code>.
	/// </summary>
	[Parameter] public string ElementName { get; set; }
	protected string ElementNameEffective => ElementName ?? GetSettings()?.ElementName ?? GetDefaults().ElementName ?? throw new InvalidOperationException(nameof(ElementName) + " default for " + nameof(HxPlaceholderContainer) + " has to be set.");

	/// <summary>
	/// Application-wide defaults for <see cref="HxPlaceholderContainer"/> and derived components.
	/// </summary>
	public static PlaceholderContainerSettings Defaults { get; set; }

	static HxPlaceholderContainer()
	{
		Defaults = new PlaceholderContainerSettings()
		{
			ElementName = "span",
			Animation = PlaceholderAnimation.None,
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual PlaceholderContainerSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public PlaceholderContainerSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual PlaceholderContainerSettings GetSettings() => Settings;


	/// <summary>
	/// Animation of the placeholders in the container.
	/// </summary>
	[Parameter] public PlaceholderAnimation? Animation { get; set; }
	protected PlaceholderAnimation AnimationEffective => Animation ?? GetSettings()?.Animation ?? GetDefaults().Animation ?? throw new InvalidOperationException(nameof(Animation) + " default for " + nameof(HxPlaceholderContainer) + " has to be set.");

	/// <summary>
	/// Size of the placeholders (propagated to the child <see cref="HxPlaceholder"/>s).
	/// </summary>
	[Parameter] public PlaceholderSize? Size { get; set; }

	/// <summary>
	/// Color of the placeholders (propagated to the child <see cref="HxPlaceholder"/>s).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }

	/// <summary>
	/// Content of the placeholder container (put your <see cref="HxPlaceholder"/>s here).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Indicates whether the placeholder is active (if <c>false</c>, the component acts as regular HTML element).
	/// Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool Active { get; set; } = true;

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected virtual string GetCssClass()
	{
		return CssClassHelper.Combine(
			Active ? GetAnimationCssClass() : null,
			CssClassEffective);
	}

	private string GetAnimationCssClass()
	{
		return (AnimationEffective) switch
		{
			PlaceholderAnimation.None => null,
			PlaceholderAnimation.Glow => "placeholder-glow",
			PlaceholderAnimation.Wave => "placeholder-wave",
			_ => throw new InvalidOperationException($"Unknown {nameof(PlaceholderAnimation)} value {AnimationEffective}.")
		};
	}
}
