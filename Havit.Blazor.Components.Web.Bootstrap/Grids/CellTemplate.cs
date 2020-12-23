using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Cell template.
	/// </summary>
	public class CellTemplate
	{
		/// <summary>
		/// Template to render cell.
		/// </summary>
		public RenderFragment Template { get; init; }

		/// <summary>
		/// Css class of the cell.
		/// </summary>
		public string CssClass { get; init; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public CellTemplate()
		{
		}

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
