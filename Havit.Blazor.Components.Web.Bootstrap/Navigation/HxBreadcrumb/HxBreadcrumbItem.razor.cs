using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxBreadcrumbItem
	{
		/// <summary>
		/// Determines whether the <c>HxBreadcrumbItem</c> is active (use for a page that the user is currently on).
		/// </summary>
		[Parameter] public bool Active { get; set; }
		/// <summary>
		/// The link of the breadcrumb (a page where the user will be led on click).
		/// </summary>
		[Parameter] public string Link { get; set; }
		/// <summary>
		/// Text of the link (usually a name of the page).
		/// </summary>
		[Parameter] public string Text { get; set; }
	}
}
