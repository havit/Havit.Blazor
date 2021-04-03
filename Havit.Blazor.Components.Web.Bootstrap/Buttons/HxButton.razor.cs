using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Button type="button".
	/// </summary>
	public partial class HxButton : ComponentBase, ICascadeEnabledComponent
	{
		/// <inheritdoc />
		[CascadingParameter] public FormState FormState { get; set; }

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
		/// Bootstrap button style - theme color. See https://getbootstrap.com/docs/5.0/components/buttons/.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// Bootstrap button size. See https://getbootstrap.com/docs/5.0/components/buttons/#sizes
		/// </summary>
		[Parameter] public ButtonSize Size { get; set; }

		/// <summary>
		/// Bootstrap "outline" button style. See https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons.
		/// </summary>
		[Parameter] public bool? Outlined { get; set; }

		/// <summary>
		/// Skin of the button. Simplifies usage of button properties.
		/// </summary>
		[Parameter] public ButtonSkin Skin { get; set; }

		/// <inheritdoc />
		[Parameter] public bool? Enabled { get; set; }

		/// <summary>
		/// Specifies the form the button belongs to.
		/// </summary>
		[Parameter] public string FormId { get; set; }

		/// <summary>
		/// Raised after the button is clicked.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

		/// <summary>
		/// Stop onClick-event propagation. Deafult is <c>true</c>.
		/// </summary>
		[Parameter] public bool OnClickStopPropagation { get; set; } = true;

		/// <summary>
		/// Set <c>true</c> if you want to display a <see cref="HxSpinner"/> while the <see cref="HxButton.OnClick"/> handler is running.
		/// </summary>
		[Parameter] public bool? Spinner { get; set; }

		/// <summary>
		/// Set <c>false</c> if you want to allow multiple <see cref="OnClick"/> handlers in parallel. Default is <c>true</c>.
		/// </summary>
		[Parameter] public bool SingleClickProtection { get; set; } = true;

		/// <summary>
		/// Localization service.
		/// </summary>
		[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

		protected IconBase IconEffective => this.Icon ?? this.Skin?.Icon;
		protected bool SpinnerEffective => this.Spinner ?? this.Skin?.Spinner ?? false;

		private bool clickInProgress;

		protected string GetText()
		{
			if (!String.IsNullOrEmpty(Text))
			{
				return this.Text;
			}
			else if (!String.IsNullOrEmpty(Skin?.Text))
			{
				return StringLocalizerFactory.GetLocalizedValue(Skin.Text, Skin.ResourceType);
			}
			return null;
		}

		protected string GetColorCssClass()
		{
			var outlined = this.Outlined ?? Skin?.Outlined ?? false;
			var style = this.Color ?? Skin?.Color ?? throw new InvalidOperationException($"Button {nameof(Color)} has to be set - either from {nameof(Skin)} or explicitly.");

			if (outlined)
			{
				return style switch
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
					ThemeColor.None => null,
					_ => throw new InvalidOperationException($"Unknown button style {style:g}.")
				};
			}
			return style switch
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
				ThemeColor.None => null,
				_ => throw new InvalidOperationException($"Unknown button style {style:g}.")
			};
		}

		protected string GetSizeCssClass()
		{
			return this.Size switch
			{
				ButtonSize.Regular => null,
				ButtonSize.Small => "btn-sm",
				ButtonSize.Large => "btn-lg",
				_ => throw new InvalidOperationException($"Unknown button Size value {this.Size:g}.")
			};
		}

		private protected virtual string GetButtonType() => "button";

		private async Task HandleClick(MouseEventArgs mouseEventArgs)
		{
			if (!clickInProgress || !SingleClickProtection)
			{
				clickInProgress = true;
				if (SpinnerEffective)
				{
					await Task.Yield(); // when OnClick is handled by longrunning SYNCHRONOUS task, spinner would not show - we need to return not-completed asynchronous task, so we use Task.Yield here.
				}
				await OnClick.InvokeAsync(mouseEventArgs);
				clickInProgress = false;
			}
		}
	}
}
