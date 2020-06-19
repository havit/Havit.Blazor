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
	/// <summary>
	/// A base class for form input components. This base class automatically integrates
	/// with an Microsoft.AspNetCore.Components.Forms.EditContext, which must be supplied
	/// as a cascading parameter.
	/// Extends <seealso cref="HxInputBase{TValue}" /> class.
	/// Adds support for input groups, https://v5.getbootstrap.com/docs/5.0/forms/input-group/
	/// </summary>
	public abstract class HxInputBaseWithInputGroups<TValue> : HxInputBase<TValue>
	{
		/// <summary>
		/// Input-group before input.
		/// </summary>
		[Parameter]
		public RenderFragment InputGroupBeforeTemplate { get; set; }

		/// <summary>
		/// Input-group before input.
		/// </summary>
		[Parameter]
		public RenderFragment InputGroupAfterTemplate { get; set; }

		/// <inheritdoc />
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

		/// <inheritdoc />
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