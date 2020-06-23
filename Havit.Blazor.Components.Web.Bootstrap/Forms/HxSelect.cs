using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	// TODO: Kosmetika
	public class HxSelect<TValueType, TItemType> : HxSelectBase<TValueType, TItemType>
	{
		[Parameter] public Func<TItemType, TValueType> ValueSelector { get; set; } // TODO: Pojmenování?

		private protected override TValueType GetValueFromItem(TItemType item) => ValueSelector(item);
	}
}
