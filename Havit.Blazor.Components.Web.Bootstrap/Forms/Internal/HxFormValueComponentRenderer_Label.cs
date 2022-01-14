namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public class HxFormValueComponentRenderer_Label : ComponentBase
	{
		[Parameter] public IFormValueComponent FormValueComponent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (!String.IsNullOrEmpty(FormValueComponent.Label) || (FormValueComponent.LabelTemplate != null))
			{
				builder.OpenElement(1, "label");
				builder.AddAttribute(2, "for", FormValueComponent.LabelFor);
				builder.AddAttribute(3, "class", CssClassHelper.Combine(FormValueComponent.CoreLabelCssClass, FormValueComponent.LabelCssClass));
				builder.AddEventStopPropagationAttribute(4, "onclick", true);
				if (FormValueComponent.LabelTemplate == null)
				{
					builder.AddContent(5, FormValueComponent.Label);
				}
				builder.AddContent(6, FormValueComponent.LabelTemplate);
				builder.CloseElement();
			}
		}

	}
}
