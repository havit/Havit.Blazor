using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Input with sizing support.
	/// </summary>
	public interface IInputWithLabelType
	{
		/// <summary>
		/// Label type.
		/// </summary>
		LabelType LabelType { get; set; }
	}
}
