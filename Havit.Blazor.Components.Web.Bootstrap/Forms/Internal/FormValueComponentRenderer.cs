using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public class FormValueComponentRenderer : ComponentBase
	{
		/// <summary>
		/// Values for component renderer.
		/// </summary>
		[Parameter] public IFormValueComponent FormValueComponent { get; set; }

		/// <inheritdoc />
		protected override sealed void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call

			string cssClass = CssClassHelper.Combine(FormValueComponent.CoreCssClass, FormValueComponent.CssClass);

			// pokud nemáme css class, label, ani hint, budeme renderovat jen jako "Value"
			bool renderDiv = !String.IsNullOrEmpty(cssClass)
				|| !String.IsNullOrEmpty(FormValueComponent.Label)
				|| (FormValueComponent.LabelTemplate != null)
				|| !String.IsNullOrEmpty(FormValueComponent.Hint)
				|| (FormValueComponent.HintTemplate != null);

			if (renderDiv)
			{
				builder.OpenElement(1, "div");
				if (!String.IsNullOrEmpty(cssClass))
				{
					builder.AddAttribute(2, "class", cssClass);
				}
			}

			switch (FormValueComponent.RenderOrder)
			{
				case LabelValueRenderOrder.LabelValue:

					// majority component

					builder.OpenRegion(3);
					BuildRenderLabel(builder);
					builder.CloseRegion();

					builder.OpenRegion(4);
					BuildRenderInputGroups(builder, BuildRenderValue);
					builder.CloseRegion();

					break;

				case LabelValueRenderOrder.ValueLabel:

					// checkbox, etc.

					builder.OpenRegion(5);
					BuildRenderInputGroups(builder, BuildRenderValue); // TODO: Lze zde vůbec renderovat InputGroups???
					builder.CloseRegion();

					builder.OpenRegion(6);
					BuildRenderLabel(builder);
					builder.CloseRegion();

					break;

				default: throw new InvalidOperationException($"Unknown RenderOrder: {FormValueComponent.RenderOrder}");
			}

			builder.OpenRegion(7);
			BuildRenderValidationMessage(builder);
			builder.CloseRegion();

			builder.OpenRegion(8);
			BuildRenderHint(builder);
			builder.CloseRegion();

			if (renderDiv)
			{
				builder.CloseElement();
			}
		}

		/// <summary>
		/// Renders label when properties set.
		/// </summary>
		protected virtual void BuildRenderLabel(RenderTreeBuilder builder)
		{
			//  <label for="formGroupExampleInput">Example label</label>
			if (!String.IsNullOrEmpty(FormValueComponent.Label) || (FormValueComponent.LabelTemplate != null))
			{
				builder.OpenElement(1, "label");
				builder.AddAttribute(2, "for", FormValueComponent.LabelFor);
				builder.AddAttribute(3, "class", CssClassHelper.Combine(FormValueComponent.CoreLabelCssClass, FormValueComponent.LabelCssClass));
				builder.AddEventStopPropagationAttribute(4, "onclick", true); // TODO: Chceme onclick:stopPropagation na labelech všech inputů, nebo jen checkboxy? Má to být  nastavitelné?
				if (FormValueComponent.LabelTemplate == null)
				{
					builder.AddContent(5, FormValueComponent.Label);
				}
				builder.AddContent(6, FormValueComponent.LabelTemplate);
				builder.CloseElement();
			}
		}

		/// <summary>
		/// Renders input groups (with content).
		/// </summary>
		protected virtual void BuildRenderInputGroups(RenderTreeBuilder builder, RenderFragment content)
		{
			IFormValueComponentWithInputGroups formValueComponentWithInputGroups = FormValueComponent as IFormValueComponentWithInputGroups;

			bool shouldRenderInputGroups = (formValueComponentWithInputGroups != null)
				&& (!String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupStart)
					|| (formValueComponentWithInputGroups.InputGroupStartTemplate != null)
					|| !String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupEnd)
					|| (formValueComponentWithInputGroups.InputGroupEndTemplate != null));

			if (shouldRenderInputGroups)
			{
				builder.OpenElement(100, "span");
				builder.AddAttribute(101, "class", CssClassHelper.Combine("input-group", formValueComponentWithInputGroups.InputGroupCssClass, GetInputGroupSizeCssClass(formValueComponentWithInputGroups.InputGroupSize)));

				if (!String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupStart))
				{
					builder.OpenElement(200, "span");
					builder.AddAttribute(201, "class", "input-group-text");
					builder.AddContent(202, formValueComponentWithInputGroups.InputGroupStart);
					builder.CloseElement(); // span.input-group-text
				}

				builder.AddContent(300, formValueComponentWithInputGroups.InputGroupStartTemplate);
			}

			builder.OpenRegion(400);
			content(builder);
			builder.CloseRegion();

			if (shouldRenderInputGroups)
			{
				if (!String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupEnd))
				{
					builder.OpenElement(500, "span");
					builder.AddAttribute(501, "class", "input-group-text");
					builder.AddContent(600, formValueComponentWithInputGroups.InputGroupEnd);
					builder.CloseElement(); // span.input-group-text
				}

				builder.AddContent(600, formValueComponentWithInputGroups.InputGroupEndTemplate);

				builder.CloseElement(); // span.input-group
			}
		}

		/// <summary>
		/// Renders value/input.
		/// </summary>
		protected virtual void BuildRenderValue(RenderTreeBuilder builder)
		{
			FormValueComponent.RenderValue(builder);
		}

		/// <summary>
		/// Renders hint when property HintTemplate set.
		/// </summary>
		protected virtual void BuildRenderHint(RenderTreeBuilder builder)
		{
			if (!String.IsNullOrEmpty(FormValueComponent.Hint) || (FormValueComponent.HintTemplate != null))
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(2, "class", FormValueComponent.CoreHintCssClass);
				builder.AddContent(3, FormValueComponent.Hint);
				builder.AddContent(4, FormValueComponent.HintTemplate);
				builder.CloseElement();
			}
		}

		/// <summary>
		/// Renders validation message.
		/// </summary>
		protected virtual void BuildRenderValidationMessage(RenderTreeBuilder builder)
		{
			FormValueComponent.RenderValidationMessage();
		}

		private string GetInputGroupSizeCssClass(InputSize inputGroupSize)
		{
			return inputGroupSize switch
			{
				InputSize.Regular => null,
				InputSize.Small => "input-group-sm",
				InputSize.Large => "input-group-lg",
				_ => throw new InvalidOperationException(inputGroupSize.ToString())
			};
		}

		/// <summary>
		/// Adds <see cref="FormValueComponentRenderer"/> to a builder.
		/// </summary>
		public static void Render(int sequence, RenderTreeBuilder builder, IFormValueComponent data)
		{
			builder.OpenRegion(sequence);

			builder.OpenComponent(0, typeof(FormValueComponentRenderer));
			builder.AddAttribute(1, nameof(FormValueComponentRenderer.FormValueComponent), data);
			builder.CloseComponent();

			builder.CloseRegion();
		}
	}
}
