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
	public class HxFormValue : ComponentBase, IFormValueComponent, IFormValueComponentWithInputGroups
	{
		#region IFormValueComponent properties
		/// <inheritdoc />
		[Parameter] public string CssClass { get; set; }

		/// <inheritdoc />
		[Parameter] public string Label { get; set; }

		/// <inheritdoc />
		[Parameter] public RenderFragment LabelTemplate { get; set; }

		/// <inheritdoc />
		[Parameter] public string LabelCssClass { get; set; }

		/// <inheritdoc />
		[Parameter] public string Hint { get; set; }

		/// <inheritdoc />
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
		[Parameter] public string InputGroupStart { get; set; }

		/// <inheritdoc />
		[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

		/// <inheritdoc />
		[Parameter] public string InputGroupEnd { get; set; }

		/// <inheritdoc />
		[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);
			HxFormValueComponentRenderer.Render(0, builder, this);
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
