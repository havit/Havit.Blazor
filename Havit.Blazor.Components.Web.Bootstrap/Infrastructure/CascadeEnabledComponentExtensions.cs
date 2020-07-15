using System;
using System.Collections.Generic;
using System.Text;
using Havit.Blazor.Components.Web.Forms;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Infrastructure
{
	/// <summary>
	/// Extensions to <see cref="ICascadeEnabledComponent"/>.
	/// </summary>
	public static class CascadeEnabledComponentExtensions
	{
		/// <summary>
		/// Effective value of IsEnabled. When IsEnabled is not set, receives value from FormState or defaults to true.
		/// </summary>
		public static bool IsEnabledEffective(this ICascadeEnabledComponent component)
		{
			return component.FormState?.IsEnabled ?? component.IsEnabled ?? true;
		}
	}
}
