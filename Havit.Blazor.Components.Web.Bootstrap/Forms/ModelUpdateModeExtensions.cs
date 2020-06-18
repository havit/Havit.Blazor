using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	public static class ModelUpdateModeExtensions
	{
		public static string ToEventName(this ModelUpdateMode value)
		{
			return value.ToString().ToLower();
		}
	}
}
