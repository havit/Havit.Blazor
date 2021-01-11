using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Infrastructure;
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
	public class HxButton : ComponentBase, ICascadeEnabledComponent
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
		/// Bootstrap button style. See https://getbootstrap.com/docs/5.0/components/buttons/.
		/// </summary>
		[Parameter] public ButtonStyle? Style { get; set; }

		/// <summary>
		/// Bootstrap outline button style. See https://getbootstrap.com/docs/5.0/components/buttons/#outline-buttons.
		/// </summary>
		[Parameter] public bool? Outline { get; set; }

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
		/// Localization service.
		/// </summary>
		[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "type", GetButtonType());
			builder.AddAttribute(2, "class", CssClassHelper.Combine("btn", GetStyleCss(), this.CssClass ?? Skin?.CssClass));
			builder.AddAttribute(3, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, HandleClick));
			builder.AddAttribute(4, "disabled", !this.EnabledEffective());

			IconBase icon = Icon ?? Skin?.Icon;
			if (icon != null)
			{
				builder.OpenComponent(5, typeof(HxIcon));
				builder.AddAttribute(6, nameof(HxIcon.Icon), icon);
				builder.CloseComponent();
				builder.AddContent(7, " ");

			}

			if (!String.IsNullOrEmpty(Text))
			{
				builder.AddContent(8, Text);
			}
			else if (!String.IsNullOrEmpty(Skin?.Text))
			{
				builder.AddContent(9, StringLocalizerFactory.GetLocalizedValue(Skin.Text, Skin.ResourceType));
			}

			builder.AddContent(10, ChildContent);
			builder.CloseElement();
		}

		protected string GetStyleCss()
		{
			var outline = this.Outline ?? Skin?.Outline ?? false;
			var style = this.Style ?? Skin?.Style ?? throw new InvalidOperationException("Button Style has to be set - either from Skin or explicitly.");

			if (outline)
			{
				return style switch
				{
					ButtonStyle.Primary => "btn-outline-primary",
					ButtonStyle.Secondary => "btn-outline-secondary",
					ButtonStyle.Success => "btn-outline-success",
					ButtonStyle.Danger => "btn-outline-danger",
					ButtonStyle.Warning => "btn-outline-warning",
					ButtonStyle.Info => "btn-outline-info",
					ButtonStyle.Light => "btn-outline-light",
					ButtonStyle.Dark => "btn-outline-dark",
					ButtonStyle.Link => "btn-link",
					ButtonStyle.None => null,
					_ => throw new InvalidOperationException($"Unknown button style {style:g}.")
				};
			}
			return style switch
			{
				ButtonStyle.Primary => "btn-primary",
				ButtonStyle.Secondary => "btn-secondary",
				ButtonStyle.Success => "btn-success",
				ButtonStyle.Danger => "btn-danger",
				ButtonStyle.Warning => "btn-warning",
				ButtonStyle.Info => "btn-info",
				ButtonStyle.Light => "btn-light",
				ButtonStyle.Dark => "btn-dark",
				ButtonStyle.Link => "btn-link",
				ButtonStyle.None => null,
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
