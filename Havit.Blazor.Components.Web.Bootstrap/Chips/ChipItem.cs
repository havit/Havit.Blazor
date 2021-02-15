using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class ChipItem
	{
		public FieldIdentifier FieldIdentifier { get; init; } // TODO: Do potomka?
		public RenderFragment ChipTemplate { get; init; }
		public bool CanBeRemoved { get; init; } = false;
	}
}
