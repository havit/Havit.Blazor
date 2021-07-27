using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Default <see cref="IFormValueComponent" /> renderer.
	/// </summary>
	public class HxFormValueRenderer : FormValueRenderer
	{
		/// <summary>
		/// Adds <see cref="HxFormValueComponentRenderer"/> to a builder.
		/// </summary>
		public override void Render(int sequence, RenderTreeBuilder builder, IFormValueComponent data)
		{
			builder.OpenRegion(sequence);

			builder.OpenComponent(0, typeof(HxFormValueComponentRenderer));
			builder.AddAttribute(1, nameof(HxFormValueComponentRenderer.FormValueComponent), data);
			builder.CloseComponent();

			builder.CloseRegion();
		}

	}
}
