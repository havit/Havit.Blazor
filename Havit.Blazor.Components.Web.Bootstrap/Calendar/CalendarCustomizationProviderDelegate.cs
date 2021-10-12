using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Calendar display customization provider.
	/// </summary>
	public delegate CalendarCustomizationResult CalendarCustomizationProviderDelegate(CalendarCustomizationRequest request);
}
