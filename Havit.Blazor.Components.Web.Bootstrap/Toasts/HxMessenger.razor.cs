using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Component wrapper displaying <see cref="HxToast"/> to show messages.
	/// </summary>
	public partial class HxMessenger : ComponentBase
	{
		/// <summary>
		/// Content to render.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		private List<Message> messages = new List<Message>();
		private MessengerCascadingValue messenger;

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxMessenger()
		{
			messenger = new MessengerCascadingValue(this);
		}

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			// when used in layout we want to clear all messages during navigation
			messages.Clear();
		}

		/// <summary>
		/// Add and shows the message. Not intented to be used in user code. 
		/// See <see cref="IMessenger"/> and <see cref="MessengerExtensions"/> for methods to show message.
		/// </summary>
		public void AddMessage(Message message)
		{
			messages.Add(message);
			StateHasChanged();
		}

		/// <summary>
		/// Receive notification from javascript when message is hidden.
		/// </summary>
		protected void HandleToastHidden(Message message)
		{
			messages.Remove(message);
		}
	}
}
