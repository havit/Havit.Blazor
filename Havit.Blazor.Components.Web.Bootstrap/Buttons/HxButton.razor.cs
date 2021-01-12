using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Havit.Blazor.Components.Web.Forms;
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
		/// Raised after the button is clicked.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

		/// <summary>
		/// Stop onClick-event propagation. Deafult is <c>true</c>.
		/// </summary>
		[Parameter] public bool OnClickStopPropagation { get; set; } = true;

		/// <summary>
		/// Localization service.
		/// </summary>
		[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

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

		protected string GetColorCss()
		{
			var outlined = this.Outlined ?? Skin?.Outlined ?? false;
			var style = this.Color ?? Skin?.Color ?? throw new InvalidOperationException($"Button {nameof(Color)} has to be set - either from {nameof(Skin)} or explicitly.");

			if (outlined)
			{
				return style switch
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
					_ => throw new InvalidOperationException($"Unknown button style {style:g}.")
				};
			}
			return style switch
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
				_ => throw new InvalidOperationException($"Unknown button style {style:g}.")
			};
		}

		private protected virtual string GetButtonType() => "button";

		private async Task HandleClick(MouseEventArgs mouseEventArgs)
		{
			await OnClick.InvokeAsync(mouseEventArgs);
		}
	}
}
