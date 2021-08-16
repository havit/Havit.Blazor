using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxInputTags
	{
		[Parameter] public AutosuggestDataProviderDelegate<string> NewItemSuggestionsDataProvider { get; set; }
	}
}
