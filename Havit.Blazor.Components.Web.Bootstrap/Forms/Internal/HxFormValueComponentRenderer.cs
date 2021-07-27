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
	public class HxFormValueComponentRenderer : ComponentBase
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

					if (FormValueComponent.ShouldRenderInputGroups())
					{
						throw new InvalidOperationException($"Cannot use Input Groups when {nameof(FormValueComponent.RenderOrder)} is {nameof(LabelValueRenderOrder.ValueLabel)}.");
					}

					builder.OpenRegion(5);
					BuildRenderInputGroups(builder, BuildRenderValue);
					builder.CloseRegion();

					builder.OpenRegion(6);
					BuildRenderLabel(builder);
					builder.CloseRegion();

					break;

				case LabelValueRenderOrder.ValueOnly:

					// autosuggest with floating label
					builder.OpenRegion(7);
					BuildRenderInputGroups(builder, BuildRenderValue);
					builder.CloseRegion();

					break;

				default: throw new InvalidOperationException($"Unknown RenderOrder: {FormValueComponent.RenderOrder}");
			}

			builder.OpenRegion(8);
			BuildRenderValidationMessage(builder);
			builder.CloseRegion();

			builder.OpenRegion(9);
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
			builder.OpenComponent(1, typeof(HxFormValueComponentRenderer_Label));
			builder.AddAttribute(2, nameof(HxFormValueComponentRenderer_Label.FormValueComponent), FormValueComponent);
			builder.CloseComponent();
		}

		/// <summary>
		/// Renders input groups (with content).
		/// </summary>
		protected virtual void BuildRenderInputGroups(RenderTreeBuilder builder, RenderFragment content)
		{
			IFormValueComponentWithInputGroups formValueComponentWithInputGroups = FormValueComponent as IFormValueComponentWithInputGroups;
			bool shouldRenderInputGroups = FormValueComponent.ShouldRenderInputGroups();

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
			FormValueComponent.RenderValidationMessage(builder);
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
	}
}
