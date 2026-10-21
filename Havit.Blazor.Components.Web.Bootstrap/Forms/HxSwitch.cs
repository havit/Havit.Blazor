namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Switch input.<br/>
/// (Replaces the former <c>HxInputSwitch</c> component which was dropped in v4.0.0.)<br/>
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSwitch">https://havit.blazor.eu/components/HxSwitch</see>
/// </summary>
public class HxSwitch : HxCheckboxBase
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxSwitch"/> and derived components.
	/// </summary>
	public static SwitchSettings Defaults { get; set; }

	static HxSwitch()
	{
		Defaults = new SwitchSettings();
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override SwitchSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance.
	/// </summary>
	[Parameter] public SwitchSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	protected override SwitchSettings GetSettings() => Settings;

	/// <summary>
	/// [Obsolete] No effect. Since Bootstrap 6, every switch renders the <c>switch</c> attribute (native haptic feedback in mobile Safari, iOS 17.4+).
	/// </summary>
	[Obsolete("No effect. Since Bootstrap 6, every switch renders the switch attribute (native haptic feedback).")]
	[Parameter] public bool? Native { get; set; }

	protected override CheckboxRenderMode RenderModeEffective => CheckboxRenderMode.Switch;

	/// <summary>
	/// Theme color of the switch (renders the <c>theme-*</c> class on the <c>.switch</c> wrapper, new in Bootstrap 6).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected override ThemeColor ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color ?? ThemeColor.None;

	protected override ButtonVariant VariantEffective => throw new NotSupportedException();

	protected override ButtonSize ButtonSizeEffective => throw new NotSupportedException();

	/// <summary>
	/// Size of the switch (<c>switch-sm</c>/<c>switch-lg</c>).<br />
	/// The default is taken from <see cref="HxSetup.Defaults"/> (<see cref="InputSize.Regular"/> if not customized).
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected override InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? HxSetup.Defaults.InputSize;
}
