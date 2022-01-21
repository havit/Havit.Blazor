using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap Collapse toggle triggering the <see cref="HxCollapse"/> to toggle.
	/// </summary>
	public class HxCollapseToggleElement : ComponentBase
	{
		/// <summary>
		/// Gets or sets the name of the element to render.
		/// </summary>
		[Parameter] public string ElementName { get; set; } = "span";

		/// <summary>
		/// Custom CSS class to render with the toggle element.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public string Text { get; set; }

		[Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

		/// <summary>
		/// Target selector of the toggle.
		/// Use <c>#id</c> to reference single <see cref="HxCollapse"/> or <c>.class</c> for multiple <see cref="HxCollapse"/>s.
		/// </summary>
		[Parameter] public string CollapseTarget { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, ElementName);

			builder.AddAttribute(1, "class", GetCssClass());
			builder.AddAttribute(2, "data-bs-toggle", "collapse");
			builder.AddAttribute(3, "aria-expanded", false);

			if (!string.IsNullOrWhiteSpace(CollapseTarget))
			{
				builder.AddAttribute(4, "data-bs-target", CollapseTarget);

				if (CollapseTarget.StartsWith("#"))
				{
					builder.AddAttribute(5, "aria-controls", CollapseTarget.Substring(1));
				}
			}

			builder.AddMultipleAttributes(6, AdditionalAttributes);
			builder.AddMarkupContent(7, Text);
			builder.AddContent(8, ChildContent);

			builder.CloseElement();

			base.BuildRenderTree(builder);
		}

		protected virtual string GetCssClass()
		{
			return CssClass;
		}
	}
}
