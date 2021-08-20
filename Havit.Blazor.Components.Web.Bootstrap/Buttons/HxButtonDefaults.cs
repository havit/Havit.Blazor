using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Default values for <see cref="HxButton"/>
	/// </summary>
	public class HxButtonDefaults
	{
		/// <summary>
		/// Bootstrap button size. See https://getbootstrap.com/docs/5.0/components/buttons/#sizes
		/// </summary>
		public ButtonSize Size { get; set; } = ButtonSize.Regular;

		/// <summary>
		/// Bootstrap button style - theme color. See https://getbootstrap.com/docs/5.0/components/buttons/.
		/// </summary>
		public ThemeColor? Color { get; set; }
	}
}
