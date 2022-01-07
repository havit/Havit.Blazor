using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public interface IInputSettingsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		InputSize? InputSize { get; }
	}
}
