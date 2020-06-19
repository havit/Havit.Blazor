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
		public const string InvalidCssClass = "is-invalid";

		[CascadingParameter] protected FormState CascadingFormState { get; set; }

		[Parameter]
		public string Label { get; set; }

		[Parameter]
		public RenderFragment LabelTemplate { get; set; }

		[Parameter]
		public RenderFragment HintTemplate { get; set; }

		[Parameter]
		public new string CssClass { get; set; }

		[Parameter]
		public string LabelCssClass { get; set; }

		[Parameter]
		public string InputCssClass { get; set; }

		[Parameter]
		public bool ShowValidationMessage { get; set; } = true;

		[Parameter]
		public bool? IsEnabled { get; set; }

		protected bool IsEnabledEfective => IsEnabled ?? CascadingFormState?.IsEnabled ?? true;

		private protected virtual string CoreCssClass => "";
		private protected virtual string CoreInputCssClass => "form-control";
		private protected virtual string CoreLabelCssClass => "form-label";
		private protected virtual string HintCoreCssClass => "form-text";

		protected string InputId { get; private set; }

		protected virtual InputRenderOrder RenderOrder => InputRenderOrder.LabelInputValidatorHint;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call
			string cssClass = CssClassHelper.Combine(CoreCssClass, CssClass);
			bool renderDiv = !String.IsNullOrEmpty(cssClass) || !String.IsNullOrEmpty(Label) || (LabelTemplate != null) || (HintTemplate != null);

			if (renderDiv)
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(1, "class", cssClass);
			}

			switch (RenderOrder)
			{
				case InputRenderOrder.LabelInputValidatorHint:
					builder.OpenRegion(100);
					BuildRenderLabel(builder);
					builder.CloseRegion();

					builder.OpenRegion(101);
					BuildRenderInput(builder);
					builder.CloseRegion();

					break;

				case InputRenderOrder.InputLabelValidatorHint:
					builder.OpenRegion(200);
					BuildRenderInput(builder);
					builder.CloseRegion();

					builder.OpenRegion(201);
					BuildRenderLabel(builder);
					builder.CloseRegion();

					break;

				default: throw new InvalidOperationException(RenderOrder.ToString());
			}

			builder.OpenRegion(102);
			BuildRenderValidationMessage(builder);
			builder.CloseRegion();

			builder.OpenRegion(103);
			BuildRenderHint(builder);
			builder.CloseRegion();

			if (renderDiv)
			{
				builder.CloseElement();
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
				builder.AddAttribute(3, "class", CssClassHelper.Combine(CoreLabelCssClass, LabelCssClass));
				if (LabelTemplate == null)
				{
					builder.AddContent(3, Label);
				}
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
			builder.AddAttribute(4, "class", GetInputCssClassToRender());
			builder.AddAttribute(5, "disabled", !IsEnabledEfective);
		}

		protected virtual void BuildRenderHint(RenderTreeBuilder builder)
		{
			if (HintTemplate != null)
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(2, "class", HintCoreCssClass);
				builder.AddContent(3, HintTemplate);
				builder.CloseElement();
			}
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

		protected virtual string GetInputCssClassToRender()
		{
			string validationCssClass = EditContext.GetValidationMessages(FieldIdentifier).Any() ? InvalidCssClass : null;
			return CssClassHelper.Combine(CoreInputCssClass, InputCssClass, validationCssClass);
		}
	}
}