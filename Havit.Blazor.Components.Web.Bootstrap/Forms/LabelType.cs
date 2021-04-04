using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Label type.
	/// </summary>
	public enum LabelType
	{
		/// <summary>
		/// Regular.
		/// </summary>
		Regular = 0,

		/// <summary>
		///  Floating label. Not supported on all components.
		/// https://getbootstrap.com/docs/5.0/forms/floating-labels/
		/// </summary>
		Floating
	}
}
