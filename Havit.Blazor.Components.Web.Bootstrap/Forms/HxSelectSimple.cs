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
	public class HxSelectSimple<TItemType> : HxSelectBase<TItemType, TItemType>
	{
		private protected override TItemType GetValueFromItem(TItemType item) => item;
	}
}
