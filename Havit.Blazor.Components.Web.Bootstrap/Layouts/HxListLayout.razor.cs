using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Layouts;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxListLayout
	{
		[Parameter] public bool FilterDrawerOpen { get; set; }

		[Parameter] public string Title { get; set; }

		// TODO: TitleTemplate nebo TitleSection? Bude mít vůbec význam?
		[Parameter] public RenderFragment TitleSection { get; set; }

		// TODO: QuickSearchSection?
		[Parameter] public RenderFragment SearchSection { get; set; }

		// TODO: Search (předchozí) vs. Filter (zde)
		[Parameter] public RenderFragment FilterSection { get; set; }

		[Parameter] public RenderFragment NamedViewsSection { get; set; }

		[Parameter] public RenderFragment DataSection { get; set; }

		[Parameter] public RenderFragment DetailSection { get; set; }

		[Parameter] public RenderFragment CommandsSection { get; set; }
	}
}
