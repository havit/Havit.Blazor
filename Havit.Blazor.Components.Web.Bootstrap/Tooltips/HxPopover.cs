using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/popovers/">Bootstrap Popover</see> component.<br />
/// Rendered as a <c>span</c> wrapper to fully support popovers on disabled elements (see example in <see href="https://getbootstrap.com/docs/5.3/components/popovers/#disabled-elements">Disabled elements</see> in the Bootstrap popover documentation).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxPopover">https://havit.blazor.eu/components/HxPopover</see>
/// </summary>
public class HxPopover : HxTooltipInternalBase
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxPopover"/> and derived components.
	/// </summary>
	public static PopoverSettings Defaults { get; }

	static HxPopover()
	{
		Defaults = new PopoverSettings()
		{
			Animation = true,
			Container = "body"
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected override PopoverSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public PopoverSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected override PopoverSettings GetSettings() => this.Settings;

	/// <summary>
	/// Popover title.
	/// </summary>
	[Parameter]
	public string Title
	{
		get => base.TitleInternal;
		set => base.TitleInternal = value;
	}

	/// <summary>
	/// Popover content.
	/// </summary>
	[Parameter]
	public string Content
	{
		get => base.ContentInternal;
		set => base.ContentInternal = value;
	}

	/// <summary>
	/// Popover placement. Default is <see cref="PopoverPlacement.Right"/>.
	/// </summary>
	[Parameter]
	public PopoverPlacement Placement
	{
		get => (PopoverPlacement)base.PlacementInternal;
		set => base.PlacementInternal = (TooltipPlacement)value;
	}

	/// <summary>
	/// Popover trigger(s). Default is <see cref="PopoverTrigger.Click"/>.
	/// </summary>
	[Parameter]
	public PopoverTrigger Trigger
	{
		get => (PopoverTrigger)base.TriggerInternal;
		set => base.TriggerInternal = (TooltipTrigger)value;
	}

	protected override string JsModuleName => nameof(HxPopover);
	protected override string DataBsToggle => "popover";

	public HxPopover()
	{
		this.Placement = PopoverPlacement.Right;
		this.Trigger = PopoverTrigger.Click;
	}
	protected override Dictionary<string, string> GetNewContentForUpdate()
	{
		return new()
		{
			{ ".popover-header", this.Title },
			{ ".popover-body", this.Content }
		};
	}

}
