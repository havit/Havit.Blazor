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
		Defaults = new SwitchSettings()
		{
			Native = false
		};
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
	/// To render switch as a native switch.
	/// </summary>
	[Parameter] public bool? Native { get; set; }

	private protected bool NativeEffective => Native ?? GetSettings()?.Native ?? GetDefaults().Native ?? throw new InvalidOperationException(nameof(Native) + " default for " + nameof(HxSwitch) + " has to be set.");

	protected override CheckboxRenderMode RenderModeEffective => NativeEffective ? CheckboxRenderMode.NativeSwitch : CheckboxRenderMode.Switch;

	protected override ThemeColor ColorEffective => throw new NotSupportedException();

	protected override bool OutlineEffective => throw new NotSupportedException();
}