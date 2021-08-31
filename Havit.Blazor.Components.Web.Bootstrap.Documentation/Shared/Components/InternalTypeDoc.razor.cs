using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public partial class InternalTypeDoc
	{
		[Parameter] public string TypeText { get; set; }
		[Parameter] public Type Type { get; set; }

		protected override void OnParametersSet()
		{
			try { Type = Type.GetType($"Havit.Blazor.Components.Web.Bootstrap.{TypeText}, Havit.Blazor.Components.Web.Bootstrap"); } catch { }
		}

		public static bool DetermineIfTypeIsInternal(string typeText)
		{
			try
			{
				Type type = Type.GetType($"Havit.Blazor.Components.Web.Bootstrap.{typeText}, Havit.Blazor.Components.Web.Bootstrap");
				if (type is null)
				{
					return false;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
