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
	public abstract class HxInputBase<TValue> : InputBase<TValue>
	{
		[CascadingParameter] protected FormState CascadingFormState { get; set; }

		[Parameter]
		public string Label { get; set; }

		[Parameter]
		public RenderFragment LabelTemplate { get; set; }

		[Parameter]
		public new string CssClass { get; set; }

		[Parameter]
		public bool ShowValidationMessage { get; set; } = true;

		[Parameter]
		public bool? IsEnabled { get; set; }

		protected bool IsEnabledEfective => IsEnabled ?? CascadingFormState?.IsEnabled ?? true;

		protected virtual string CoreInputCssClass => "form-control";

		protected string InputId { get; private set; }

		protected virtual InputRenderOrder RenderOrder => InputRenderOrder.LabelInputValidator;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call

			switch (RenderOrder)
			{
				case InputRenderOrder.LabelInputValidator:
					builder.OpenRegion(1);
					BuildRenderLabel(builder);
					builder.CloseRegion();

					builder.OpenRegion(2);
					BuildRenderInput(builder);
					builder.CloseRegion();

					builder.OpenRegion(3);
					BuildRenderValidationMessage(builder);
					builder.CloseRegion();
					break;

				case InputRenderOrder.InputLabelValidator:
					builder.OpenRegion(1);
					BuildRenderInput(builder);
					builder.CloseRegion();

					builder.OpenRegion(2);
					BuildRenderLabel(builder);
					builder.CloseRegion();

					builder.OpenRegion(3);
					BuildRenderValidationMessage(builder);
					builder.CloseRegion();
					break;

				default: throw new InvalidOperationException(RenderOrder.ToString());
			}
		}

		protected virtual void BuildRenderLabel(RenderTreeBuilder builder)
		{
			//  <label for="formGroupExampleInput">Example label</label>
			if (!String.IsNullOrEmpty(Label) || (LabelTemplate != null))
			{
				EnsureInputId();

				builder.OpenElement(1, "label");
				builder.AddAttribute(2, "for", InputId);
				builder.AddContent(3, Label);
				builder.AddContent(4, LabelTemplate);
				builder.CloseElement();
			}
		}

		protected abstract void BuildRenderInput(RenderTreeBuilder builder);

		protected virtual void BuildRenderInput_AddCommonAttributes(RenderTreeBuilder builder, string typeValue)
		{
			builder.AddMultipleAttributes(1, AdditionalAttributes);
			builder.AddAttribute(2, "id", InputId);
			builder.AddAttribute(3, "type", typeValue);
			builder.AddAttribute(4, "class", GetCssClassToRender());
			builder.AddAttribute(5, "disabled", !IsEnabledEfective);
		}

		protected virtual void BuildRenderValidationMessage(RenderTreeBuilder builder)
		{
			if (ShowValidationMessage)
			{
				//<div class="invalid-feedback">
				//Please provide a valid city.
				//</div>
				builder.OpenComponent<HxValidationMessage<TValue>>(1);
				builder.AddAttribute(2, nameof(HxValidationMessage<TValue>.For), ValueExpression);
				builder.CloseComponent();
			}
		}

		private void EnsureInputId()
		{
			if (String.IsNullOrEmpty(InputId))
			{
				InputId = "el" + Guid.NewGuid().ToString("N");
			}
		}

		protected virtual string GetCssClassToRender()
		{
			string validationCssClass = EditContext.GetValidationMessages(FieldIdentifier).Any() ? "is-invalid" : null /*"is-valid"*/;

			return String.Join(" ", new[] { CoreInputCssClass, CssClass, validationCssClass }.Where(item => !String.IsNullOrEmpty(item)));
		}

	}
}