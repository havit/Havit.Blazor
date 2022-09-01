using System.Reflection;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Havit.Blazor.Components.Web.Infrastructure;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Non-generic API for the <see cref="HxInputBase{TValue}"/> component.
	/// </summary>
	public sealed class HxInputBase
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxInputBase{TValue}"/> and derived components.
		/// </summary>
		public static InputsSettings Defaults { get; set; }

		static HxInputBase()
		{
			Defaults = new InputsSettings()
			{
				ValidationMessageMode = ValidationMessageMode.Tooltip
			};
		}
	}

}