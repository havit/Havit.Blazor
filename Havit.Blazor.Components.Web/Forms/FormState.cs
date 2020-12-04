using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Forms
{
	/// <summary>
	/// Form state to be used as a cascading value &amp; parameter.
	/// </summary>
	public class FormState
	{
		/// <summary>
		/// Indicates enabled/disabled section.
		/// </summary>
		public bool? Enabled { get; set; }
	}
}
