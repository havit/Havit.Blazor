using System;
using System.Collections.Generic;
using System.Text;
using Havit.Blazor.Components.Web.Forms;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Infrastructure
{
	/// <summary>
	/// <see cref="ICascadeEnabledComponent"/> helper method.
	/// </summary>
	public static class CascadeEnabledComponent
	{
		/// <summary>
		/// Effective value of Enabled. When Enabled is not set, receives value from FormState or defaults to true.
		/// </summary>
		public static bool EnabledEffective(ICascadeEnabledComponent component)
		{
			return component.FormState?.Enabled ?? component.Enabled ?? true;
		}
	}
}
