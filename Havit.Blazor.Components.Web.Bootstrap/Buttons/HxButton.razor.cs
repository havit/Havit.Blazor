using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Havit.Blazor.Components.Web.Infrastructure;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Button (<c>&lt;button type="button"&gt;</c>). See also <see href="https://getbootstrap.com/docs/5.1/components/buttons/">Bootstrap Buttons</see>.
	/// </summary>
	public partial class HxButton : ComponentBase, ICascadeEnabledComponent
	{
		/// <summary>
		/// Application-wide defaults for <see cref="HxButton"/> and derived components.
		/// </summary>
		public static ButtonDefaults Defaults { get; set; } = new();

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

		/// <summary>
		/// Position of the icon within the button. Default is <see cref="ButtonIconPlacement.Start" /> (configurable through <see cref="HxButton.Defaults"/>).
		/// </summary>
		[Parameter] public ButtonIconPlacement? IconPlacement { get; set; }

		/// <summary>
		/// Bootstrap button style - theme color.<br />
		/// Default is taken from <see cref="HxButton.Defaults"/> (<see cref="ThemeColor.None"/> if not customized).
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// Button size. Default is <see cref="ButtonSize.Regular"/>.
		/// </summary>
		[Parameter] public ButtonSize? Size { get; set; }

		/// <summary>
		/// <see href="https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons">Bootstrap "outline" button</see> style.
		/// </summary>
		[Parameter] public bool? Outline { get; set; }

		/// <summary>
		/// Custom CSS class to render with the <c>&lt;button /&gt;</c>.<br />
		/// When using <see cref="Tooltip"/> you might want to use <see cref="TooltipWrapperCssClass"/> instead of <see cref="CssClass" /> to get the desired result.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
		[Parameter] public bool? Enabled { get; set; }

		[CascadingParameter] protected FormState FormState { get; set; }
		FormState ICascadeEnabledComponent.FormState { get => this.FormState; set => this.FormState = value; }

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
		/// If set, a <c>span</c> wrapper will be rendered around the <c>&lt;button /&gt;</c>. For most scenarios you will then use <see cref="TooltipWrapperCssClass"/> instead of <see cref="CssClass"/>.
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
		/// Custom CSS class to render with the tooltip <c>span</c> wrapper of the <c>&lt;button /&gt;</c>.<br />
		/// If set, the <c>span</c> wrapper will be rendered no matter the <see cref="Tooltip"/> text is set or not.
		/// </summary>
		[Parameter] public string TooltipWrapperCssClass { get; set; }

		/// <summary>
		/// Raised after the button is clicked.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

		/// <summary>
		/// Raised after the button is clicked and EditContext validation succeeds.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnValidClick { get; set; }

		/// <summary>
		/// Raised after the button is clicked and EditContext validation fails.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnInvalidClick { get; set; }

		/// <summary>
		/// Stop onClick-event propagation. Deafult is <c>true</c>.
		/// </summary>
		[Parameter] public bool OnClickStopPropagation { get; set; } = true;

		/// <summary>
		/// Prevents the default action for the onclick event. Deafult is <c>false</c>.
		/// </summary>
		[Parameter] public bool OnClickPreventDefault { get; set; }

		/// <summary>
		/// Set state of the embedded <see cref="HxSpinner"/>.
		/// Leave <c>null</c> if you want automated spinner when any of the <see cref="HxButton.OnClick"/> handlers is running.
		/// You can set an explicit <c>false</c> constant to disable (override) the spinner automation.
		/// </summary>
		[Parameter] public bool? Spinner { get; set; }

		/// <summary>
		/// Set <c>false</c> if you want to allow multiple <see cref="OnClick"/> handlers in parallel. Default is <c>true</c>.
		/// </summary>
		[Parameter] public bool SingleClickProtection { get; set; } = true;

		/// <summary>
		/// Additional attributes to be splatted onto an underlying <c>&lt;button&gt;</c> element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		/// <summary>
		/// Localization service.
		/// </summary>
		[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

		/// <summary>
		/// Return <see cref="HxButton"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual ButtonDefaults GetDefaults() => Defaults;

		protected IconBase IconEffective => this.Icon ?? GetDefaults().Icon;
		protected ButtonIconPlacement IconPlacementEffective => this.IconPlacement ?? GetDefaults().IconPlacement;
		protected bool SpinnerEffective => this.Spinner ?? clickInProgress;
		protected bool DisabledEffective => !CascadeEnabledComponent.EnabledEffective(this)
			|| (SingleClickProtection && clickInProgress && (OnClick.HasDelegate || OnValidClick.HasDelegate || OnInvalidClick.HasDelegate));

		protected bool clickInProgress;
		protected ElementReference buttonElementReference;

		/// <summary>
		/// Gets basic CSS class(es) which get rendered to every single button. <br/>
		/// Default implementation is <c>"hx-button btn"</c>.
		/// </summary>
		protected virtual string CoreCssClass => "hx-button btn";


		protected virtual string GetButtonCssClass()
		{
			return CssClassHelper.Combine(CoreCssClass, GetColorCssClass(), GetSizeCssClass(), this.CssClass ?? GetDefaults().CssClass);
		}

		protected string GetColorCssClass()
		{
			var outlineEffective = this.Outline ?? GetDefaults().Outline;
			var colorEffective = this.Color ?? GetDefaults().Color ?? throw new InvalidOperationException($"Button {nameof(Color)} has to be set - either from {nameof(ButtonDefaults)} or explicitly.");

			if (outlineEffective)
			{
				return colorEffective switch
				{
					ThemeColor.Primary => "btn-outline-primary",
					ThemeColor.Secondary => "btn-outline-secondary",
					ThemeColor.Success => "btn-outline-success",
					ThemeColor.Danger => "btn-outline-danger",
					ThemeColor.Warning => "btn-outline-warning",
					ThemeColor.Info => "btn-outline-info",
					ThemeColor.Light => "btn-outline-light",
					ThemeColor.Dark => "btn-outline-dark",
					ThemeColor.Link => "btn-link",
					ThemeColor.None => null,
					_ => throw new InvalidOperationException($"Unknown {nameof(HxButton)} color {colorEffective:g}.")
				};
			}
			return colorEffective switch
			{
				ThemeColor.Primary => "btn-primary",
				ThemeColor.Secondary => "btn-secondary",
				ThemeColor.Success => "btn-success",
				ThemeColor.Danger => "btn-danger",
				ThemeColor.Warning => "btn-warning",
				ThemeColor.Info => "btn-info",
				ThemeColor.Light => "btn-light",
				ThemeColor.Dark => "btn-dark",
				ThemeColor.Link => "btn-link",
				ThemeColor.None => null,
				_ => throw new InvalidOperationException($"Unknown {nameof(HxButton)} color {colorEffective:g}.")
			};
		}

		protected string GetSizeCssClass()
		{
			var sizeEffective = this.Size ?? GetDefaults().Size;

			return sizeEffective switch
			{
				ButtonSize.Regular => null,
				ButtonSize.Small => "btn-sm",
				ButtonSize.Large => "btn-lg",
				_ => throw new InvalidOperationException($"Unknown {nameof(HxButton)} {nameof(Size)}: {sizeEffective}.")
			};
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
			if (OnClick.HasDelegate)
			{
				Contract.Requires<InvalidOperationException>(!OnValidClick.HasDelegate, $"Cannot use both {nameof(OnClick)} and {nameof(OnValidClick)} parameters.");
				Contract.Requires<InvalidOperationException>(!OnInvalidClick.HasDelegate, $"Cannot use both {nameof(OnClick)} and {nameof(OnInvalidClick)} parameters.");

				await OnClick.InvokeAsync(mouseEventArgs);
			}
			else if (OnValidClick.HasDelegate || OnInvalidClick.HasDelegate)
			{
				Contract.Requires<InvalidOperationException>(EditContextEffective != null, $"{nameof(EditContext)} has to be supplied as cascading value or explicit parameter.");

				var isValid = EditContextEffective.Validate(); // Original .NET comment: This will likely become ValidateAsync later

				if (isValid && OnValidClick.HasDelegate)
				{
					await OnValidClick.InvokeAsync(mouseEventArgs);
				}

				if (!isValid && OnInvalidClick.HasDelegate)
				{
					await OnInvalidClick.InvokeAsync(mouseEventArgs);
				}
			}
		}
	}
}
