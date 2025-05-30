namespace Havit.Blazor.Components.Web.Bootstrap;

// TODO: Should share the same settings with HxCheckbox?

/// <summary>
/// Switch input.<br/>
/// (Replaces the former <c>HxInputSwitch</c> component which was dropped in v4.0.0.)<br/>
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSwitch">https://havit.blazor.eu/components/HxSwitch</see>
/// </summary>
public class HxSwitch : HxCheckbox
{
	/// <inheritdoc />
	public override CheckboxRenderMode RenderMode
	{
		get { return Native ? CheckboxRenderMode.NativeSwitch : CheckboxRenderMode.Switch; }
		set { throw new NotSupportedException($"The {nameof(RenderMode)} property of {nameof(HxSwitch)} cannot be set."); }
	}

	/// <summary>
	/// To render checkbox as a native switch.
	/// </summary>
	[Parameter] public bool Native { get; set; }
}