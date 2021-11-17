using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Renders an element with the specified name, attributes and child-content.
	/// </summary>
	public class HxDynamicElement : ComponentBase
	{
		/// <summary>
		/// Gets or sets the name of the element to render.
		/// </summary>
		[Parameter] public string ElementName { get; set; } = "span";

		/// <summary>
		/// Raised after the element is clicked.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

		/// <summary>
		/// Stop onClick-event propagation. Deafult is <c>false</c>.
		/// </summary>
		[Parameter] public bool OnClickStopPropagation { get; set; }

		/// <summary>
		/// Prevents the default action for the onclick event. Deafult is <c>false</c>.
		/// </summary>
		[Parameter] public bool OnClickPreventDefault { get; set; }

		/// <summary>
		/// Element reference.
		/// </summary>
		[Parameter] public ElementReference ElementRef { get; set; }

		/// <summary>
		/// Action (synchronnous, not an EventCallback) called when the element's reference got captured.
		/// </summary>
		[Parameter] public Action<ElementReference> ElementRefChanged { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter(CaptureUnmatchedValues = true)]
		public IDictionary<string, object> AdditionalAttributes { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, ElementName);

			builder.AddEventPreventDefaultAttribute(1, "onclick", OnClickPreventDefault);
			builder.AddEventStopPropagationAttribute(2, "onclick", OnClickStopPropagation);
			builder.AddMultipleAttributes(3, AdditionalAttributes);
			builder.AddElementReferenceCapture(4, capturedRef =>
			{
				ElementRef = capturedRef;
				ElementRefChanged?.Invoke(ElementRef);
			});
			builder.AddContent(5, ChildContent);

			builder.CloseElement();
		}
	}
}