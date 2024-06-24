﻿namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/badge/">Bootstrap Badge</see> component.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxBadge">https://havit.blazor.eu/components/HxBadge</see>
/// </summary>
public partial class HxBadge
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxBadge"/> and derived components.
	/// </summary>
	public static BadgeSettings Defaults { get; set; }

	static HxBadge()
	{
		Defaults = new BadgeSettings()
		{
			Color = null,
			TextColor = ThemeColor.None,
			Type = BadgeType.Regular,
			CssClass = null
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual BadgeSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public BadgeSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual BadgeSettings GetSettings() => Settings;

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Badge color (background).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected ThemeColor ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color ?? throw new InvalidOperationException(nameof(Color) + " for " + nameof(HxBadge) + " has to be set.");

	/// <summary>
	/// Color of badge text. Use <see cref="Color"/> for the background color.
	/// The default is <see cref="ThemeColor.None"/> (color automatically selected to work with the chosen background color).
	/// </summary>
	[Parameter] public ThemeColor? TextColor { get; set; }
	protected ThemeColor TextColorEffective => TextColor ?? GetSettings()?.TextColor ?? GetDefaults().TextColor ?? throw new InvalidOperationException(nameof(TextColor) + " default for " + nameof(HxBadge) + " has to be set.");

	/// <summary>
	/// Badge type - Regular or rounded-pills. The default is <see cref="BadgeType.Regular"/>.
	/// </summary>
	[Parameter] public BadgeType? Type { get; set; }
	protected BadgeType TypeEffective => Type ?? GetSettings()?.Type ?? GetDefaults().Type ?? throw new InvalidOperationException(nameof(Type) + " default for " + nameof(HxBadge) + " has to be set.");

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		Contract.Requires<InvalidOperationException>(Color != ThemeColor.None, $"Parameter {nameof(Color)} of {nameof(HxBadge)} is required.");
	}

	private string GetBackgroundColorCss()
	{
		// if TextColor is not set, we use the combined -text-bg-{color} class (Bootstrap 5.3.3)
		if (TextColorEffective == ThemeColor.None)
		{
			return ColorEffective.ToTextBackgroundColorCss();
		}
		return ColorEffective.ToBackgroundColorCss();
	}

	protected string GetTypeCss()
	{
		return TypeEffective switch
		{
			BadgeType.Regular => null,
			BadgeType.RoundedPill => "rounded-pill",
			_ => throw new InvalidOperationException($"Unknown {nameof(BadgeType)} value: {TypeEffective}.")
		};
	}
}
