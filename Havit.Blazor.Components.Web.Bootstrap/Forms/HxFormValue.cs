using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxFormValue : ComponentBase, IFormValueComponent
	{
		#region IFormValueComponent properties
		/// <summary>
		/// Custom CSS class to render with wrapping div.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Label to render before input/value (or after input for Checkbox).		
		/// </summary>
		[Parameter] public string Label { get; set; }

		/// <summary>
		/// Label to render before input/value (or after input for Checkbox).
		/// </summary>
		[Parameter] public RenderFragment LabelTemplate { get; set; }

		/// <summary>
		/// Custom CSS class to render with the label.
		/// </summary>
		[Parameter] public string LabelCssClass { get; set; }

		/// <summary>
		/// Hint to render after value/input as form-text.
		/// </summary>
		[Parameter] public string Hint { get; set; }

		/// <summary>
		/// Hint to render after element/input as form-text.
		/// </summary>
		[Parameter] public RenderFragment HintTemplate { get; set; }
		#endregion

		/// <summary>
		/// Template to render value.
		/// </summary>
		[Parameter] public RenderFragment ValueTemplate { get; set; }

		/// <summary>
		/// Custom CSS class to render with the value.
		/// </summary>
		[Parameter] public string ValueCssClass { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);
			FormValueComponentRenderer.Render(0, builder, this);
		}

		/// <inheritdoc />
		public void RenderValue(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "div");
			builder.AddAttribute(1, "class", CssClassHelper.Combine("form-control", ValueCssClass));
			builder.AddContent(2, ValueTemplate);
			builder.CloseElement();
		}
	}
}
