using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Button (<c>&lt;button type="button"&gt;</c>). See also <see href="https://getbootstrap.com/docs/5.3/components/buttons/">Bootstrap Buttons</see>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxButton">https://havit.blazor.eu/components/HxButton</see>
/// </summary>
public partial class HxButton : ComponentBase, ICascadeEnabledComponent
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxButton"/> and derived components.
	/// </summary>
	public static ButtonSettings Defaults { get; set; }

	static HxButton()
	{
		Defaults = new ButtonSettings()
		{
			Size = ButtonSize.Regular,
			IconPlacement = ButtonIconPlacement.Start,
			Color = ThemeColor.None,
			CssClass = null,
			Outline = false,
			Icon = null
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual ButtonSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public ButtonSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ButtonSettings GetSettings() => Settings;

	/// <summary>
	/// Text of the button.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Button content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Icon to render into the button.
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }
	protected IconBase IconEffective => Icon ?? GetSettings()?.Icon ?? GetDefaults().Icon;

	/// <summary>
	/// Position of the icon within the button. The default is <see cref="ButtonIconPlacement.Start" /> (configurable through <see cref="HxButton.Defaults"/>).
	/// </summary>
	[Parameter] public ButtonIconPlacement? IconPlacement { get; set; }
	protected ButtonIconPlacement IconPlacementEffective => IconPlacement ?? GetSettings()?.IconPlacement ?? GetDefaults()?.IconPlacement ?? throw new InvalidOperationException(nameof(IconPlacement) + " default for " + nameof(HxButton) + " has to be set.");

	/// <summary>
	/// Bootstrap button style - theme color.<br />
	/// The default is taken from <see cref="HxButton.Defaults"/> (<see cref="ThemeColor.None"/> if not customized).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected ThemeColor ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color ?? throw new InvalidOperationException(nameof(Color) + " default for " + nameof(HxButton) + " has to be set.");

	/// <summary>
	/// Button size. The default is <see cref="ButtonSize.Regular"/>.
	/// </summary>
	[Parameter] public ButtonSize? Size { get; set; }
	protected ButtonSize SizeEffective => Size ?? GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxButton) + " has to be set.");

	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" button</see> style.
	/// </summary>
	[Parameter] public bool? Outline { get; set; }
	protected bool OutlineEffective => Outline ?? GetSettings()?.Outline ?? GetDefaults().Outline ?? throw new InvalidOperationException(nameof(Outline) + " default for " + nameof(HxButton) + " has to be set.");

	/// <summary>
	/// Custom CSS class to render with the <c>&lt;button /&gt;</c>.<br />
	/// When using <see cref="Tooltip"/> you might want to use <see cref="TooltipWrapperCssClass"/> instead of <see cref="CssClass" /> to get the desired result.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// CSS class to be rendered with the button icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }
	protected string IconCssClassEffective => IconCssClass ?? GetSettings()?.IconCssClass ?? GetDefaults().IconCssClass;

	/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
	[Parameter] public bool? Enabled { get; set; }

	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => FormState; set => FormState = value; }

	/// <summary>
	/// Associated <see cref="EditContext"/>.
	/// </summary>
	[Parameter] public EditContext EditContext { get; set; }
	[CascadingParameter] protected EditContext CascadingEditContext { get; set; }
	protected EditContext EditContextEffective => EditContext ?? CascadingEditContext;

	/// <summary>
	/// Specifies the form the button belongs to.
	/// </summary>
	[Parameter] public string FormId { get; set; }

	/// <summary>
	/// Tooltip text.<br/>
	/// If set, a <c>span</c> wrapper will be rendered around the <c>&lt;button /&gt;</c>. For most scenarios, you will then use <see cref="TooltipWrapperCssClass"/> instead of <see cref="CssClass"/>.
	/// </summary>
	[Parameter] public string Tooltip { get; set; }

	/// <summary>
	/// Tooltip placement.
	/// </summary>
	[Parameter] public TooltipPlacement TooltipPlacement { get; set; }

	/// <summary>
	/// Custom CSS class to render with the tooltip.
	/// </summary>
	[Parameter] public string TooltipCssClass { get; set; }

	/// <summary>
	/// Appends the tooltip to a specific element. Default is <c>body</c>.
	/// </summary>
	[Parameter] public string TooltipContainer { get; set; }

	/// <summary>
	/// Custom CSS class to render with the tooltip <c>span</c> wrapper of the <c>&lt;button /&gt;</c>.<br />
	/// If set, the <c>span</c> wrapper will be rendered no matter whether the <see cref="Tooltip"/> text is set or not.
	/// </summary>
	[Parameter] public string TooltipWrapperCssClass { get; set; }

	/// <summary>
	/// Raised after the button is clicked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Raised after the button is clicked and EditContext validation succeeds.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnValidClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnValidClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnValidClickAsync(MouseEventArgs args) => OnValidClick.InvokeAsync(args);

	/// <summary>
	/// Raised after the button is clicked and EditContext validation fails.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnInvalidClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnInvalidClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnInvalidClickAsync(MouseEventArgs args) => OnInvalidClick.InvokeAsync(args);

	/// <summary>
	/// Stops onClick-event propagation. Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool OnClickStopPropagation { get; set; } = true;

	/// <summary>
	/// Prevents the default action for the onclick event. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool OnClickPreventDefault { get; set; }

	/// <summary>
	/// Sets the state of the embedded <see cref="HxSpinner"/>.
	/// Leave <c>null</c> if you want automated spinner when any of the <see cref="HxButton.OnClick"/> handlers is running.
	/// You can set an explicit <c>false</c> constant to disable (override) the spinner automation.
	/// </summary>
	[Parameter] public bool? Spinner { get; set; }

	/// <summary>
	/// Sets <c>false</c> if you want to allow multiple <see cref="OnClick"/> handlers in parallel. Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool SingleClickProtection { get; set; } = true;

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <c>&lt;button&gt;</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected bool SpinnerEffective => Spinner ?? clickInProgress;
	protected bool DisabledEffective => !CascadeEnabledComponent.EnabledEffective(this)
		|| (SingleClickProtection && clickInProgress && (OnClick.HasDelegate || OnValidClick.HasDelegate || OnInvalidClick.HasDelegate));

	protected bool clickInProgress;
	protected ElementReference buttonElementReference;
	private HxTooltip _tooltipComponent;

	/// <summary>
	/// Gets the basic CSS class(es) which get rendered to every single button. <br/>
	/// The default implementation is <c>"hx-button btn"</c>.
	/// </summary>
	protected virtual string CoreCssClass => "hx-button btn";

	protected virtual string GetButtonCssClass()
	{
		return CssClassHelper.Combine(
			CoreCssClass,
			ColorEffective.ToButtonColorCss(OutlineEffective),
			SizeEffective.ToButtonSizeCssClass(),
			CssClassEffective);
	}

	protected string GetTooltipWrapperCssClass()
	{
		bool tooltipWillRenderSpan = !String.IsNullOrEmpty(Tooltip) || !String.IsNullOrWhiteSpace(TooltipWrapperCssClass);
		if (tooltipWillRenderSpan)
		{
			return CssClassHelper.Combine("d-inline-block", TooltipWrapperCssClass);
		}
		return null;
	}

	private protected virtual string GetButtonType() => "button";

	private async Task HandleClick(MouseEventArgs mouseEventArgs)
	{
		if (!clickInProgress || !SingleClickProtection)
		{
			clickInProgress = true;
			await HandleClickCore(mouseEventArgs);
			clickInProgress = false;
		}
	}

	private async Task HandleClickCore(MouseEventArgs mouseEventArgs)
	{
		// #209 [HxButton] Tooltip does not hide when the button opens HxModal
		// We disable the button (SingleClickProtection) and disabled buttons do not raise any events (the tooltip won't receive <c>mouseout</c> and stays visible).
		// UPDATE 06/2024: As of now, only Firefox does not hide the tooltip when the button is disabled.
		// Removing the HideAsync workaround as it causes #817 [HxButton] Tooltip behavior on mobile devices
		// await _tooltipComponent.HideAsync();

		if (OnClick.HasDelegate)
		{
			Contract.Requires<InvalidOperationException>(!OnValidClick.HasDelegate, $"[{this.GetType().Name}] Cannot use both {nameof(OnClick)} and {nameof(OnValidClick)} parameters.");
			Contract.Requires<InvalidOperationException>(!OnInvalidClick.HasDelegate, $"[{this.GetType().Name}] Cannot use both {nameof(OnClick)} and {nameof(OnInvalidClick)} parameters.");

			await InvokeOnClickAsync(mouseEventArgs);
		}
		else if (OnValidClick.HasDelegate || OnInvalidClick.HasDelegate)
		{
			Contract.Requires<InvalidOperationException>(EditContextEffective != null, $"[{this.GetType().Name}] To use {nameof(OnValidClick)}/{nameof(OnInvalidClick)}, {nameof(EditContext)} must be supplied as a cascading value or explicit parameter.");

			var isValid = EditContextEffective.Validate(); // Original .NET comment: This will likely become ValidateAsync later

			if (isValid && OnValidClick.HasDelegate)
			{
				await InvokeOnValidClickAsync(mouseEventArgs);
			}

			if (!isValid && OnInvalidClick.HasDelegate)
			{
				await InvokeOnInvalidClickAsync(mouseEventArgs);
			}
		}
	}
}
