using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxAccordion
	{
		[Parameter] public string CssClass { get; set; }

		[Parameter] public string ExpandedItemId { get; set; }
		[Parameter] public EventCallback<string> ExpandedItemIdChanged { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		protected internal string Id { get; set; } = "hx" + Guid.NewGuid().ToString("N");

		internal async Task SetExpandedItemIdAsync(string newId)
		{
			if (this.ExpandedItemId != newId)
			{
				ExpandedItemId = newId;
				await ExpandedItemIdChanged.InvokeAsync(newId);
			}
		}
	}
}
