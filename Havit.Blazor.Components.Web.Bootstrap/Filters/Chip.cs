using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public class Chip
	{
		public RenderFragment ChipTemplate { get; set; }
		public Func<Task> RemoveCallback { get; set; }
	}
}
