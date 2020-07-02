using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	/// <summary>
	/// Cell template.
	/// </summary>
	public class CellTemplate
	{
		/// <summary>
		/// Template to render cell.
		/// </summary>
		public RenderFragment Template { get; set; }

		/// <summary>
		/// Css class of the cell.
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public CellTemplate(RenderFragment template, string cssClass = null)
		{
			Template = template;
			CssClass = cssClass;
		}
	}
}
