using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Build render fragments for specific scenarios.
	/// </summary>
	public static class RenderFragmentBuilder
	{
		/// <summary>
		/// Returns RenderFragment to render "nothing". Implementation returns <c>null</c>.
		/// </summary>
		public static RenderFragment Empty()
		{
			return null;
		}

		/// <summary>
		/// Returns RenderFragment which renders content and template (it is expected at least one of argument is null).		
		/// If both are <c>null</c>, returns <see cref="Empty"/>.
		/// </summary>
		public static RenderFragment CreateFrom(string content, RenderFragment template)
		{
			if (content is null && template is null)
			{
				return Empty();
			}

			return (RenderTreeBuilder builder) =>
			{
				builder.AddContent(0, content); // null check: pokud je string null, použije prázdný řetězec
				builder.AddContent(1, template); // null check: je použit uvnitř metody inside
			};
		}
	}
}
