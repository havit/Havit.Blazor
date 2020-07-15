using System;
using System.Collections.Generic;
using System.Text;
using Havit.Blazor.Components.Web.Forms;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Infrastructure
{
	/// <summary>
	/// Component which can be enabled/disabled in cascade.
	/// </summary>
	public interface ICascadeEnabledComponent
	{
		/// <summary>
		/// Form state cascading parameter.
		/// </summary>
		public FormState FormState { get; set; }

		/// <summary>
		/// When null (default), the IsEnabled value is received from cascading FormState.
		/// When value is false, input is rendered as disabled.
		/// To set multiple controls as disabled use <seealso cref="HxFormState" />.
		/// </summary>
		public bool? IsEnabled { get; set; }
	}
}
