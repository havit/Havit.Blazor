namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Checkbox input.<br />
/// (Replaces the former <c>HxInputCheckbox</c> component which was dropped in v 4.0.0.)
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCheckbox">https://havit.blazor.eu/components/HxCheckbox</see>
/// </summary>
public class HxCheckbox : HxCheckboxBase
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxCheckbox"/> and derived components.
	/// </summary>
	public static CheckboxSettings Defaults { get; set; }

	static HxCheckbox()
	{
		Defaults = new CheckboxSettings()
		{
			Color = ThemeColor.None,
			Outline = false,
			RenderMode = CheckboxRenderMode.Checkbox
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override CheckboxSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance.
	/// </summary>
	[Parameter] public CheckboxSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	protected override CheckboxSettings GetSettings() => Settings;

	/// <summary>
	/// Checkbox render mode.
	/// </summary>
	[Parameter] public CheckboxRenderMode? RenderMode { get; set; }
	protected override CheckboxRenderMode RenderModeEffective => RenderMode ?? GetSettings()?.RenderMode ?? GetDefaults().RenderMode ?? throw new InvalidOperationException(nameof(RenderMode) + " default for " + nameof(HxCheckbox) + " has to be set.");

	/// <summary>
	/// Bootstrap button style - theme color.<br />
	/// The default is taken from <see cref="HxButton.Defaults"/> (<see cref="ThemeColor.None"/> if not customized).
	/// For <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected override ThemeColor ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color ?? throw new InvalidOperationException(nameof(Color) + " default for " + nameof(HxCheckbox) + " has to be set.");

	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" button</see> style.
	/// For <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	[Parameter] public bool? Outline { get; set; }
	protected override bool OutlineEffective => Outline ?? GetSettings()?.Outline ?? GetDefaults().Outline ?? throw new InvalidOperationException(nameof(Outline) + " default for " + nameof(HxCheckbox) + " has to be set.");

}
