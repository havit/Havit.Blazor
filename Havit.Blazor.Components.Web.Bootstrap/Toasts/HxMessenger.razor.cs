using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Toasts
{
	public partial class HxMessenger : ComponentBase
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		private List<Message> messages = new List<Message>();
		private Messenger messenger;

		public HxMessenger()
		{
			messenger = new Messenger(this);
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			// when used in layout we want to clear all messages during navigation
			messages.Clear();
		}

		public void AddMessage(Message message)
		{
			messages.Add(message);
			StateHasChanged();
		}

		protected void HandleToastHidden(Message message)
		{
			messages.Remove(message);
		}
	}
}
