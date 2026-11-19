namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public class HxFormValueComponentRenderer_Label : ComponentBase
{
	[Parameter] public IFormValueComponent FormValueComponent { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (!String.IsNullOrEmpty(FormValueComponent.Label) || (FormValueComponent.LabelTemplate != null))
		{
			builder.OpenElement(1, "label");
			builder.AddAttribute(2, "id", FormValueComponent.LabelId);
			builder.AddAttribute(3, "for", FormValueComponent.LabelFor);
			builder.AddAttribute(4, "class", CssClassHelper.Combine(FormValueComponent.CoreLabelCssClass, FormValueComponent.LabelCssClass));
			builder.AddEventStopPropagationAttribute(5, "onclick", true);
			if (FormValueComponent.LabelTemplate == null)
			{
				builder.AddContent(6, FormValueComponent.Label);
			}
			builder.AddContent(7, FormValueComponent.LabelTemplate);
			builder.CloseElement();
		}
	}

}
