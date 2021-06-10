using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Input with placeholder support.
	/// </summary>
	public interface IInputWithPlaceholder
	{
		/// <summary>
		/// Placeholder for the input.
		/// </summary>
		string Placeholder { get; set; }
	}
}
