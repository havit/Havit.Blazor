using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	public abstract class HxInputBaseWithInputGroups<TValue> : HxInputBase<TValue>
	{
		[Parameter]
		public RenderFragment InputGroupBeforeTemplate { get; set; }
		
		[Parameter]
		public RenderFragment InputGroupAfterTemplate { get; set; }

		protected override void BuildRenderInputAndValidationMessage(RenderTreeBuilder builder)
		{
			if ((InputGroupBeforeTemplate == null) && (InputGroupAfterTemplate == null))
			{
				base.BuildRenderInputAndValidationMessage(builder);
				return;
			}

			builder.OpenElement(1, "div");
			builder.AddAttribute(2, "class", "input-group");
			
			builder.OpenRegion(3);
			base.BuildRenderInputAndValidationMessage(builder);
			builder.CloseRegion();

			builder.CloseElement();

		}

		protected override void BuildRenderInputDecorated(RenderTreeBuilder builder)
		{
			if (InputGroupBeforeTemplate != null)
			{
				builder.OpenElement(1, "span");
				builder.AddAttribute(2, "class", "input-group-text");
				builder.AddContent(3, InputGroupBeforeTemplate);
				builder.CloseElement();
			}

			builder.OpenRegion(4);
			base.BuildRenderInputDecorated(builder);
			builder.CloseRegion();

			if (InputGroupAfterTemplate != null)
			{
				builder.OpenElement(5, "span");
				builder.AddAttribute(6, "class", "input-group-text");
				builder.AddContent(7, InputGroupAfterTemplate);
				builder.CloseElement();
			}
		}
	}
}