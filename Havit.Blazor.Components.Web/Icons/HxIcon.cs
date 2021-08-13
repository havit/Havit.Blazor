using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Displays an icon.
	/// Currently supports <a href="https://icons.getbootstrap.com/" target="_blank">Bootstrap icons</a> through <code>BootstrapIcon</code> class.
	/// You can add your own icon-set easily.
	/// </summary>
	public class HxIcon : ComponentBase
	{
		/// <summary>
		/// Icon to display.
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// CSS Class to combine with basic icon CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

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
