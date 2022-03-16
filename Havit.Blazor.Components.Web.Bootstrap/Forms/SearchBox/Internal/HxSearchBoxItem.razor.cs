using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxSearchBoxItem
{
	[Parameter] public string Title { get; set; }
	[Parameter] public string Subtitle { get; set; }
	[Parameter] public IconBase Icon { get; set; }
}
