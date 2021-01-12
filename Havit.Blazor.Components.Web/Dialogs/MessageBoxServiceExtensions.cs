using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web
{
	public static class MessageBoxServiceExtensions
	{
		public static Task<MessageBoxButtons> ShowAsync(this IHxMessageBoxService messageBoxService, string title, string text, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxButtons? primaryButton = null, string customButtonText = null)
		{
			return messageBoxService.ShowAsync(new MessageBoxRequest()
			{
				Title = title,
				Text = text,
				Buttons = buttons,
				PrimaryButton = primaryButton,
				CustomButtonText = customButtonText
			});
		}
	}
}
