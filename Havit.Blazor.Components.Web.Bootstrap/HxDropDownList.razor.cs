using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.GoranG3.Web.Client.Pages.Prototyping
{
    public class HxDropDownList<T> : ComponentBase
    {
        [Parameter]
		public T Value { get; set; }

		[Parameter]
		public EventCallback<EventArgs> OnValueChanged { get; set; }

		[Parameter]
		public IEnumerable<T> Data { get; set; }
	}
}
