using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	public static class CssClassHelper
	{
		public static string Combine(params string[] cssClasses)
		{
			return String.Join(" ", cssClasses.Where(item => !String.IsNullOrEmpty(item)));
		}
	}
}
