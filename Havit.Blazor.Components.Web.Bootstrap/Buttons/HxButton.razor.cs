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
		public static ButtonDefaults Defaults { get; set; } = new();

		[CascadingParameter] protected FormState FormState { get; set; }
		FormState ICascadeEnabledComponent.FormState { get => this.FormState; set => this.FormState = value; }

		[Parameter] public EditContext EditContext { get; set; }
		[CascadingParameter] protected EditContext CascadingEditContext { get; set; }
		protected EditContext EditContextEffective => EditContext ?? CascadingEditContext;

		/// <summary>
		/// Custom css class to render with the button.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Label of the button.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Button template.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Icon to render into the button.
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Bootstrap button style - theme color. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/" />.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// Bootstrap button size. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/#sizes" />.
		/// </summary>
		[Parameter] public ButtonSize? Size { get; set; }

		/// <summary>
		/// Bootstrap "outline" button style. See <a href="https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons" />.
		/// </summary>
		[Parameter] public bool? Outline { get; set; }

		/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
		[Parameter] public bool? Enabled { get; set; }

		/// <summary>
		/// Specifies the form the button belongs to.
		/// </summary>
		[Parameter] public string FormId { get; set; }

		/// <summary>
		/// Tooltip text.
		/// </summary>
		[Parameter] public string Tooltip { get; set; }

		/// <summary>
		/// Tooltip placement.
		/// </summary>
		[Parameter] public TooltipPlacement TooltipPlacement { get; set; }

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
		/// Additional attributes to be splatted onto an underlying <code>&lt;button&gt;</code> element.
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
		protected bool SpinnerEffective => this.Spinner ?? clickInProgress;
		protected bool DisabledEffective => !CascadeEnabledComponent.EnabledEffective(this)
			|| (SingleClickProtection && clickInProgress && (OnClick.HasDelegate || OnValidClick.HasDelegate || OnInvalidClick.HasDelegate));

		private bool clickInProgress;

		protected virtual string CoreCssClass => "hx-button";

		protected virtual IconPosition IconPosition => IconPosition.Start;

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
					ThemeColor.Primary => "btn btn-outline-primary",
					ThemeColor.Secondary => "btn btn-outline-secondary",
					ThemeColor.Success => "btn btn-outline-success",
					ThemeColor.Danger => "btn btn-outline-danger",
					ThemeColor.Warning => "btn btn-outline-warning",
					ThemeColor.Info => "btn btn-outline-info",
					ThemeColor.Light => "btn btn-outline-light",
					ThemeColor.Dark => "btn btn-outline-dark",
					ThemeColor.Link => "btn btn-link",
					ThemeColor.None => "btn",
					_ => throw new InvalidOperationException($"Unknown {nameof(HxButton)} color {colorEffective:g}.")
				};
			}
			return colorEffective switch
			{
				ThemeColor.Primary => "btn btn-primary",
				ThemeColor.Secondary => "btn btn-secondary",
				ThemeColor.Success => "btn btn-success",
				ThemeColor.Danger => "btn btn-danger",
				ThemeColor.Warning => "btn btn-warning",
				ThemeColor.Info => "btn btn-info",
				ThemeColor.Light => "btn btn-light",
				ThemeColor.Dark => "btn btn-dark",
				ThemeColor.Link => "btn btn-link",
				ThemeColor.None => "btn",
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
