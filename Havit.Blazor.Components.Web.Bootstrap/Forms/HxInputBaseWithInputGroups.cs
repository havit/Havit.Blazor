using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// A base class for form input components. This base class automatically integrates
	/// with an Microsoft.AspNetCore.Components.Forms.EditContext, which must be supplied
	/// as a cascading parameter.
	/// Extends <seealso cref="HxInputBase{TValue}" /> class.
	/// Adds support for input groups, https://v5.getbootstrap.com/docs/5.0/forms/input-group/
	/// </summary>
	public abstract class HxInputBaseWithInputGroups<TValue> : HxInputBase<TValue>, IInputWithSize
	{
		/// <summary>
		/// Input-group at the beginning of the input.
		/// </summary>
		[Parameter] public string InputGroupStart { get; set; }

		/// <summary>
		/// Input-group at the beginning of the input.
		/// </summary>
		[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

		/// <summary>
		/// Input-group at the end of the input.
		/// </summary>
		[Parameter] public string InputGroupEnd { get; set; }

		/// <summary>
		/// Input-group at the end of the input.
		/// </summary>
		[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

		/// <inheritdoc />
		[Parameter] public InputSize InputSize { get; set; }

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (FloatingLabelEffective
				&& (!String.IsNullOrEmpty(InputGroupStart) || (InputGroupStartTemplate != null) || !String.IsNullOrEmpty(InputGroupEnd) || (InputGroupEndTemplate != null)))
			{
				throw new InvalidOperationException($"Cannot use input groups ({nameof(InputGroupStart)}, {nameof(InputGroupStartTemplate)}, {nameof(InputGroupEnd)}, {nameof(InputGroupEndTemplate)}) with floating labels.");
			}
		}

		/// <inheritdoc />
		protected override void BuildRenderInputAndValidationMessage(RenderTreeBuilder builder)
		{
			if (String.IsNullOrEmpty(InputGroupStart) && (InputGroupStartTemplate == null)
				&& String.IsNullOrEmpty(InputGroupEnd) && (InputGroupEndTemplate == null))
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
			if ((InputGroupStartTemplate != null) || (!String.IsNullOrEmpty(InputGroupStart)))
			{
				builder.OpenElement(1, "span");
				builder.AddAttribute(2, "class", "input-group-text");
				builder.AddContent(3, InputGroupStart);
				builder.AddContent(4, InputGroupStartTemplate);
				builder.CloseElement();
			}

			builder.OpenRegion(4);
			base.BuildRenderInputDecorated(builder);
			builder.CloseRegion();

			if ((InputGroupEndTemplate != null) || (!String.IsNullOrEmpty(InputGroupEnd)))
			{
				builder.OpenElement(5, "span");
				builder.AddAttribute(6, "class", "input-group-text");
				builder.AddContent(7, InputGroupEnd);
				builder.AddContent(8, InputGroupEndTemplate);
				builder.CloseElement();
			}
		}
	}
}