namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public class HxFormValueComponentRenderer : ComponentBase
{
	/// <summary>
	/// Values for the component renderer.
	/// </summary>
	[Parameter] public IFormValueComponent FormValueComponent { get; set; }

	/// <inheritdoc />
	protected override sealed void BuildRenderTree(RenderTreeBuilder builder)
	{
		// no base call

		string cssClass = CssClassHelper.Combine(FormValueComponent.CoreCssClass, FormValueComponent.CssClass);

		// Without any CssClass, Label, or Hint, we will render just the "Value"
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

				// floating labels

				builder.OpenRegion(5);
				BuildRenderInputGroups(builder, BuildRenderValueWithFloatingLabel);
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
		BuildRenderHint(builder);
		builder.CloseRegion();

		builder.OpenRegion(9);
		BuildRenderValidationMessage(builder);
		builder.CloseRegion();

		if (renderDiv)
		{
			builder.CloseElement();
		}
	}

	/// <summary>
	/// Renders the value/input and label inside floating label wrapper.
	/// </summary>
	protected virtual void BuildRenderValueWithFloatingLabel(RenderTreeBuilder builder)
	{
		builder.OpenElement(1, "div");
		builder.AddAttribute(2, "class", "form-floating");

		builder.OpenRegion(3);
		BuildRenderValue(builder);
		builder.CloseRegion();

		builder.OpenRegion(4);
		BuildRenderLabel(builder);
		builder.CloseRegion();

		builder.CloseElement(); // div.form-floating
	}

	/// <summary>
	/// Renders the label when properties are set.
	/// </summary>
	protected virtual void BuildRenderLabel(RenderTreeBuilder builder)
	{
		//  <label for="formGroupExampleInput">Example label</label>
		builder.OpenComponent(1, typeof(HxFormValueComponentRenderer_Label));
		builder.AddAttribute(2, nameof(HxFormValueComponentRenderer_Label.FormValueComponent), FormValueComponent);
		builder.CloseComponent();
	}

	/// <summary>
	/// Renders the input groups (with content).
	/// </summary>
	protected virtual void BuildRenderInputGroups(RenderTreeBuilder builder, RenderFragment content)
	{
		IFormValueComponentWithInputGroups formValueComponentWithInputGroups = FormValueComponent as IFormValueComponentWithInputGroups;
		IInputWithSize formValueComponentWithSize = FormValueComponent as IInputWithSize;

		bool shouldRenderInputGroups = FormValueComponent.ShouldRenderInputGroups();

		if (shouldRenderInputGroups)
		{
			builder.OpenElement(100, "span");
			builder.AddAttribute(101, "class", CssClassHelper.Combine(
				"input-group",
				formValueComponentWithInputGroups.InputGroupCssClass,
				formValueComponentWithSize is not null ? formValueComponentWithSize.InputSizeEffective.AsInputGroupCssClass() : null));

			if (!String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupStartText))
			{
				builder.OpenElement(200, "span");
				builder.AddAttribute(201, "class", "input-group-text");
				builder.AddContent(202, formValueComponentWithInputGroups.InputGroupStartText);
				builder.CloseElement(); // span.input-group-text
			}

			if (formValueComponentWithInputGroups.InputGroupStartTemplate is not null)
			{
				builder.AddContent(300, formValueComponentWithInputGroups.InputGroupStartTemplate);
			}

		}

		builder.OpenRegion(400);
		BuildRenderAdorn(builder, content);
		builder.CloseRegion();

		if (shouldRenderInputGroups)
		{
			if (!String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupEndText))
			{
				builder.OpenElement(500, "span");
				builder.AddAttribute(501, "class", "input-group-text");
				builder.AddContent(600, formValueComponentWithInputGroups.InputGroupEndText);
				builder.CloseElement(); // span.input-group-text
			}
			if (formValueComponentWithInputGroups.InputGroupEndTemplate is not null)
			{
				builder.AddContent(700, formValueComponentWithInputGroups.InputGroupEndTemplate);
			}

			builder.CloseElement(); // span.input-group
		}
	}

	/// <summary>
	/// Renders the form-adorn wrapper (with content) when the component has at least one adornment.
	/// </summary>
	protected virtual void BuildRenderAdorn(RenderTreeBuilder builder, RenderFragment content)
	{
		IFormValueComponentWithAdorns formValueComponentWithAdorns = FormValueComponent as IFormValueComponentWithAdorns;
		IInputWithSize formValueComponentWithSize = FormValueComponent as IInputWithSize;

		bool shouldRenderAdorn = FormValueComponent.ShouldRenderAdorn();

		if (shouldRenderAdorn)
		{
			builder.OpenElement(800, "div");
			builder.AddAttribute(801, "class", CssClassHelper.Combine(
				"form-control",
				"form-adorn",
				formValueComponentWithSize is not null ? formValueComponentWithSize.GetInputSizeCssClass() : null,
				formValueComponentWithAdorns.AdornCssClass));

			if (formValueComponentWithAdorns.AdornStartTemplate is not null)
			{
				builder.OpenElement(810, "div");
				builder.AddAttribute(811, "class", "form-adorn-icon");
				builder.AddContent(812, formValueComponentWithAdorns.AdornStartTemplate);
				builder.CloseElement(); // div.form-adorn-icon
			}

			if (!String.IsNullOrEmpty(formValueComponentWithAdorns.AdornStartText))
			{
				builder.OpenElement(820, "span");
				builder.AddAttribute(821, "class", "form-adorn-text");
				builder.AddContent(822, formValueComponentWithAdorns.AdornStartText);
				builder.CloseElement(); // span.form-adorn-text
			}
		}

		builder.OpenRegion(830);
		content(builder);
		builder.CloseRegion();

		if (shouldRenderAdorn)
		{
			if (!String.IsNullOrEmpty(formValueComponentWithAdorns.AdornEndText))
			{
				builder.OpenElement(840, "span");
				builder.AddAttribute(841, "class", "form-adorn-text");
				builder.AddContent(842, formValueComponentWithAdorns.AdornEndText);
				builder.CloseElement(); // span.form-adorn-text
			}

			if (formValueComponentWithAdorns.AdornEndTemplate is not null)
			{
				builder.OpenElement(850, "div");
				builder.AddAttribute(851, "class", "form-adorn-icon");
				builder.AddContent(852, formValueComponentWithAdorns.AdornEndTemplate);
				builder.CloseElement(); // div.form-adorn-icon
			}

			builder.CloseElement(); // div.form-adorn
		}
	}

	/// <summary>
	/// Renders the value/input.
	/// </summary>
	protected virtual void BuildRenderValue(RenderTreeBuilder builder)
	{
		FormValueComponent.RenderValue(builder);
	}

	/// <summary>
	/// Renders the hint when the HintTemplate property is set.
	/// </summary>
	protected virtual void BuildRenderHint(RenderTreeBuilder builder)
	{
		if (!String.IsNullOrEmpty(FormValueComponent.Hint) || (FormValueComponent.HintTemplate != null))
		{
			builder.OpenElement(1, "div");
			builder.AddAttribute(2, "id", FormValueComponent.HintId);
			builder.AddAttribute(3, "class", FormValueComponent.CoreHintCssClass);
			builder.AddContent(4, FormValueComponent.Hint);
			builder.AddContent(5, FormValueComponent.HintTemplate);
			builder.CloseElement();
		}
	}

	/// <summary>
	/// Renders the validation message.
	/// </summary>
	protected virtual void BuildRenderValidationMessage(RenderTreeBuilder builder)
	{
		FormValueComponent.RenderValidationMessage(builder);
	}
}
