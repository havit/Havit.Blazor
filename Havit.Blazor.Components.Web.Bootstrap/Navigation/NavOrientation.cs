﻿using static System.Net.WebRequestMethods;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Orientation for <see cref="HxNav"/>.
	/// </summary>
	public enum NavOrientation
	{
		Horizontal = 0,

		/// <summary>
		/// <see href="https://getbootstrap.com/docs/5.1/components/navs-tabs/#vertical"/>
		/// </summary>
		Vertical = 1
	}
}