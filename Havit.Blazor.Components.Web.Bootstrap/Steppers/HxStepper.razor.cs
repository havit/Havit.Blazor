namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/stepper/">Bootstrap Stepper</see> component (new in Bootstrap 6).<br />
/// Displays progress through a multi-step workflow (wizards, timelines, step-by-step progress bars) as a sequence of numbered steps (<see cref="HxStepperItem"/>s).
/// The component is CSS-only, there is no step-switching logic (wire up your own state).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxStepper">https://havit.blazor.eu/components/HxStepper</see>
/// </summary>
public partial class HxStepper
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxStepper"/> and derived components.
	/// </summary>
	public static StepperSettings Defaults { get; set; }

	static HxStepper()
	{
		Defaults = new StepperSettings();
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual StepperSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public StepperSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual StepperSettings GetSettings() => Settings;

	/// <summary>
	/// Content of the stepper (<see cref="HxStepperItem"/>s).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Changes the layout of the stepper items from vertical to horizontal, including the responsive breakpoint
	/// (Bootstrap 6 responsive <c>{breakpoint}:stepper-horizontal</c> classes are container query based,
	/// the component renders the required <c>contains-inline</c> wrapper automatically).
	/// The default is <see cref="StepperHorizontal.Never"/> (vertical stepper).
	/// </summary>
	[Parameter] public StepperHorizontal? Horizontal { get; set; }
	protected StepperHorizontal HorizontalEffective => Horizontal ?? GetSettings()?.Horizontal ?? GetDefaults().Horizontal ?? StepperHorizontal.Never;

	/// <summary>
	/// Theme color of the stepper (renders the <c>theme-*</c> class), applies to all active items.
	/// To color individual steps, use <see cref="HxStepperItem.Color"/>.
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected ThemeColor? ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color;

	/// <summary>
	/// When <c>true</c>, the stepper is wrapped in a <c>stepper-overflow</c> container which enables horizontal scrolling
	/// when the (horizontal) stepper overflows its parent. The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Overflow { get; set; }
	protected bool OverflowEffective => Overflow ?? GetSettings()?.Overflow ?? GetDefaults().Overflow ?? false;

	/// <summary>
	/// Additional CSS class(es) for the stepper.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element
	/// (e.g. use the <c>style</c> attribute to customize the gap by overriding the <c>--bs-stepper-gap</c> CSS variable).
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	/// <summary>
	/// Indicates whether any of the items is rendered as an anchor (has <see cref="HxStepperItem.Href"/> set).
	/// </summary>
	internal bool HasAnchorItems => _hasAnchorItems;
	private bool _hasAnchorItems;

	/// <summary>
	/// Called by <see cref="HxStepperItem"/> with <see cref="HxStepperItem.Href"/> set.
	/// Anchor items cannot be nested in an ordered list, so the stepper re-renders as a <c>div</c> element.
	/// </summary>
	internal void RegisterAnchorItem()
	{
		if (!_hasAnchorItems)
		{
			_hasAnchorItems = true;
			StateHasChanged();
		}
	}

	/// <summary>
	/// Responsive horizontal steppers use container queries, which require a <c>contains-inline</c> parent element
	/// (when the <c>stepper-overflow</c> wrapper is rendered, it establishes the query container itself).
	/// </summary>
	private bool RenderContainsInlineWrapper => !OverflowEffective
		&& (HorizontalEffective is not (StepperHorizontal.Never or StepperHorizontal.Always));

	protected virtual string GetClasses()
	{
		return CssClassHelper.Combine(
			"stepper",
			GetHorizontalCssClass(),
			GetColorCssClass(),
			CssClassEffective);
	}

	protected virtual string GetHorizontalCssClass()
	{
		return HorizontalEffective switch
		{
			StepperHorizontal.Never => null,
			StepperHorizontal.Always => "stepper-horizontal",
			StepperHorizontal.SmallUp => "sm:stepper-horizontal",
			StepperHorizontal.MediumUp => "md:stepper-horizontal",
			StepperHorizontal.LargeUp => "lg:stepper-horizontal",
			StepperHorizontal.ExtraLargeUp => "xl:stepper-horizontal",
			StepperHorizontal.XxlUp => "2xl:stepper-horizontal",
			_ => throw new InvalidOperationException($"Unknown {nameof(StepperHorizontal)} value {HorizontalEffective}.")
		};
	}

	private string GetColorCssClass()
	{
		return ColorEffective switch
		{
			null => null,
			ThemeColor.None => null,
			_ => ColorEffective.Value.ToThemeCss()
		};
	}
}
