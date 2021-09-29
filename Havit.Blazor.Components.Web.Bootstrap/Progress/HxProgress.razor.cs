using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/progress/">Bootstrap 5 Progress</see> component.<br/>
	/// A wrapping component for the <see cref="HxProgressBar" />.
	/// </summary>
	public partial class HxProgress
	{
		/// <summary>
		/// Content consisting of one or multiple <c>HxProgressBar</c> components.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Additional CSS classes for the progress.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Height of all inner progress bars. Default is <c>15</c> pixels.
		/// </summary>
		[Parameter] public int Height { get; set; } = 15;
	}
}
