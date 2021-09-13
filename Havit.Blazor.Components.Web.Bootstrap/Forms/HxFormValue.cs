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
		/// <inheritdoc cref="IFormValueComponent.CssClass" />
		[Parameter] public string CssClass { get; set; }

		/// <inheritdoc cref="IFormValueComponent.Label" />
		[Parameter] public string Label { get; set; }

		/// <inheritdoc cref="IFormValueComponent.LabelTemplate" />
		[Parameter] public RenderFragment LabelTemplate { get; set; }

		/// <inheritdoc cref="IFormValueComponent.LabelCssClass" />
		[Parameter] public string LabelCssClass { get; set; }

		/// <inheritdoc cref="IFormValueComponent.Hint" />
		[Parameter] public string Hint { get; set; }

		/// <inheritdoc cref="IFormValueComponent.HintTemplate" />
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

		/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupStartText" />
		[Parameter] public string InputGroupStartText { get; set; }

		/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupStartTemplate" />
		[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

		/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupEndText"/>
		[Parameter] public string InputGroupEndText { get; set; }

		/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupEndTemplate" />
		[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenRegion(0);
			base.BuildRenderTree(builder);
			builder.CloseRegion();

			HxFormValueRenderer.Current.Render(1, builder, this);
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
