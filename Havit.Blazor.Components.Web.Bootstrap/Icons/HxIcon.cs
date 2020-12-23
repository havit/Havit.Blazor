using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Displays an icon.
	/// </summary>
	public class HxIcon : ComponentBase
	{
		/// <summary>
		/// Icon to display.
		/// </summary>
		[Parameter]
		public IconBase Icon { get; set; }

		/// <summary>
		/// CSS Class to combine with basic icon CSS class.
		/// </summary>
		[Parameter]
		public string CssClass { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent(1, Icon.RendererComponentType);
			builder.AddAttribute(2, "Icon", Icon);
			builder.AddAttribute(2, "CssClass", CssClass);
			builder.CloseComponent();
		}
	}
}
