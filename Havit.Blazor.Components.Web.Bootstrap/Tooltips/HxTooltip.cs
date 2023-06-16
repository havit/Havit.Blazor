using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/tooltips/">Bootstrap Tooltip</see> component, activates on hover.<br />
/// Rendered as a <c>span</c> wrapper to fully support tooltips on disabled elements (see example in <see href="https://getbootstrap.com/docs/5.3/components/tooltips/#disabled-elements">Disabled elements</see> in the Bootstrap tooltip documentation).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxTooltip">https://havit.blazor.eu/components/HxTooltip</see>
/// </summary>
public class HxTooltip : HxTooltipInternalBase
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxTooltip"/> and derived components.
	/// </summary>
	public static TooltipSettings Defaults { get; }

	static HxTooltip()
	{
		Defaults = new TooltipSettings()
		{
			Animation = true,
			Container = "body"
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected override TooltipSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public TooltipSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected override TooltipSettings GetSettings() => this.Settings;

	/// <summary>
	/// Tooltip text.
	/// </summary>
	[Parameter]
	public string Text
	{
		get => base.TitleInternal;
		set => base.TitleInternal = value;
	}

	/// <summary>
	/// Tooltip placement. Default is <see cref="TooltipPlacement.Top"/>.
	/// </summary>
	[Parameter]
	public TooltipPlacement Placement
	{
		get => base.PlacementInternal;
		set => base.PlacementInternal = value;
	}

	/// <summary>
	/// Tooltip trigger(s). Default is <c><see cref="TooltipTrigger.Hover"/> | <see cref="TooltipTrigger.Focus"/></c>.
	/// </summary>
	[Parameter]
	public TooltipTrigger Trigger
	{
		get => base.TriggerInternal;
		set => base.TriggerInternal = value;
	}


	protected override string JsModuleName => nameof(HxTooltip);
	protected override string DataBsToggle => "tooltip";

	public HxTooltip()
	{
		this.Placement = TooltipPlacement.Top;
		this.Trigger = TooltipTrigger.Hover | TooltipTrigger.Focus;
	}

	protected override Dictionary<string, string> GetNewContentForUpdate()
	{
		return new()
		{
			{ ".tooltip-inner", this.Text }
		};
	}
}
