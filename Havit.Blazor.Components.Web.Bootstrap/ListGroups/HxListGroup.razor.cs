using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// List groups are a flexible and powerful component for displaying a series of content.
	/// </summary>
	public partial class HxListGroup
	{
		/// <summary>
		/// Content of the list group component.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// If set to true, removes borders and rounded corners to render list group items edge-to-edge.
		/// </summary>
		[Parameter] public bool Flush { get; set; }

		/// <summary>
		/// Set to true to opt into numbered list group items. The list group changes from an unordered list to an ordered list.
		/// </summary>
		[Parameter] public bool Numbered { get; set; }

		/// <summary>
		/// If true, changes the layout of the list group items from vertical to horizontal across all breakpoints. Cannot be combined with flush list groups.
		/// </summary>
		[Parameter] public bool Horizontal { get; set; }

		/// <summary>
		/// If true, items will have equal width. Use only when the <code>HxListGroup</code> is horizontal.
		/// </summary>
		[Parameter] public bool EqualWidthItems { get; set; }

		private string GetClasses()
		{
			return CssClassHelper.Combine(
				"list-group",
				Flush ? "list-group-flush" : null,
				Numbered ? "list-group-numbered" : null,
				Horizontal && !Flush ? "list-group-horizontal" : null, // Horizontal cannot be combined with Flush
				EqualWidthItems && Horizontal ? "flex-fill" : null); // EqualWidthItems has to be combined with Horizontal
		}
	}
}
