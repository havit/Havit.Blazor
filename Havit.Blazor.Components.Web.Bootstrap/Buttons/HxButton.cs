using Havit.Blazor.Components.Web.Bootstrap.Infrastructure;
using Havit.Blazor.Components.Web.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		/// Custom css class to render with button.
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
		/// Icon to render into button.
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Skin of the button. Simplifies usage of button properties.
		/// </summary>
		[Parameter] public ButtonSkin Skin { get; set; }

		/// <inheritdoc />
		[Parameter] public bool? IsEnabled { get; set; }

		/// <summary>
		/// Localization service.
		/// </summary>
		[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

		/// <summary>
		/// Raised after button is clicked.
		/// </summary>
		[Parameter]
		public EventCallback<MouseEventArgs> OnClick { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "type", GetButtonType());
			builder.AddAttribute(2, "class", CssClassHelper.Combine("btn btn-primary", Skin?.CssClass, CssClass));
			builder.AddAttribute(3, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, HandleClick));
			builder.AddAttribute(4, "disabled", !this.IsEnabledEffective());

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

		private protected virtual string GetButtonType() => "button";

		private async Task HandleClick(MouseEventArgs mouseEventArgs)
		{
			await OnClick.InvokeAsync(mouseEventArgs);
		}
	}
}
