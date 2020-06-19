using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	/// <summary>
	/// <seealso cref="BindEvent" /> extensions.
	/// </summary>
	public static class BindEventExtensions
	{
		/// <summary>
		/// Gets the name of event as string.
		/// </summary>
		public static string ToEventName(this BindEvent value)
		{
			return value.ToString().ToLower();
		}
	}
}
