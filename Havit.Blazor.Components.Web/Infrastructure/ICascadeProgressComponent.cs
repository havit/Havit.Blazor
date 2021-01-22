using System;
using System.Collections.Generic;
using System.Text;
using Havit.Blazor.Components.Web.Forms;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Infrastructure
{
	/// <summary>
	/// Component which can receive InProgress in a cascade.
	/// </summary>
	public interface ICascadeProgressComponent
	{
		/// <summary>
		/// Progress state cascading parameter.
		/// </summary>
		public ProgressState ProgressState { get; set; }

		/// <summary>
		/// When null (default), the InProgress value is received from cascading <see cref="ProgressState" />.
		/// To set multiple controls as disabled use <seealso cref="HxProgressState" />.
		/// </summary>
		public bool? InProgress { get; set; }
	}
}
