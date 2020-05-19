using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	public static class RenderFragmentBuilder
	{
		public static RenderFragment CreateFrom(string content, RenderFragment template)
		{
			return (RenderTreeBuilder builder) =>
			{
				builder.AddContent(0, content); // null check: pokud je string null, použije prázdný řetězec
				builder.AddContent(1, template); // null check: je použit uvnitř metody inside
			};
		}
	}
}
