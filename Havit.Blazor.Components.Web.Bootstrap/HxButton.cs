using Havit.Blazor.Components.Web.Bootstrap.Forms;
using Havit.Blazor.Components.Web.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Button.
	/// </summary>
	public class HxButton : ComponentBase
	{
		/// <summary>
		/// Form state.
		/// </summary>
		[CascadingParameter] protected FormState FormState { get; set; }

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
		[Parameter] public BootstrapIcon? Icon { get; set; } 

		[Parameter] public string Skin { get; set; } // TODO

		/// <summary>
		/// When null (default), the IsEnabled value is received from cascading FormState.
		/// When value is false, input is rendered as disabled.
		/// To set multiple controls as disabled use <seealso cref="HxFormState" />.
		/// </summary>
		[Parameter] public bool? IsEnabled { get; set; }

		/// <summary>
		/// Effective value of IsEnabled. When IsEnabled is not set, receives value from FormState or defaults to true.
		/// </summary>
		protected bool IsEnabledEffective => IsEnabled ?? FormState?.IsEnabled ?? true;

		/// <summary>
		/// Raised after button is clicked.
		/// </summary>
		[Parameter]
		public EventCallback<MouseEventArgs> OnClick { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call
			builder.OpenElement(0, "button");
			builder.AddAttribute(1, "type", GetButtonType());
			builder.AddAttribute(2, "class", CssClassHelper.Combine("btn btn-primary", CssClass));
			builder.AddAttribute(3, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, HandleClick));
			builder.AddAttribute(4, "disabled", !IsEnabledEffective);
			
			if (Icon != null)
			{
				builder.OpenComponent(5, typeof(HxBootstrapIcon));
				builder.AddAttribute(6, nameof(HxBootstrapIcon.Icon), Icon);
				builder.CloseComponent();
				builder.AddContent(7, " ");

			}

			builder.AddContent(8, Text);
			builder.AddContent(9, ChildContent);
			builder.CloseElement();
		}

		private protected virtual string GetButtonType() => "button";

		private async Task HandleClick(MouseEventArgs mouseEventArgs)
		{
			await OnClick.InvokeAsync(mouseEventArgs);
		}
	}
}
