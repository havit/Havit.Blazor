using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Forms
{
	/// <summary>
	/// Form state as a cascading value &amp; parameter.
	/// </summary>
	public class FormState
	{
		/// <summary>
		/// Indicated enabled/disabled section.
		/// </summary>
		public bool? IsEnabled { get; set; }
	}
}
