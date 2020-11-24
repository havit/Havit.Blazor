using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Event to use for data-binding.
	/// </summary>
	public enum BindEvent
	{
		/// <summary>
		/// "oninput" event is used to retrieved value from the input element.
		/// </summary>
		OnInput,

		/// <summary>
		/// "onchange" event is used to retrieved value from the input element.
		/// </summary>
		OnChange
	}
}
