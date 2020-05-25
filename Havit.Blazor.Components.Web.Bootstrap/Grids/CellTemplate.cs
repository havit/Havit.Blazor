using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public class CellTemplate
	{
		public RenderFragment Template { get; set; }
		public string CssClass { get; set; }

		public CellTemplate(RenderFragment template, string cssClass = null)
		{
			Template = template;
			CssClass = cssClass;
		}
	}
}
