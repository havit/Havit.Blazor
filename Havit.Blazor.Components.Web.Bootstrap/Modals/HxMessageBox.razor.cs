using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Component to display message-boxes.
	/// </summary>
	public partial class HxMessageBox : ComponentBase
	{
		///// <summary>
		///// Header icon.
		///// </summary>
		//[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Title text (Header).
		/// </summary>
		[Parameter] public string Title { get; set; }

		/// <summary>
		/// Title template (Header).
		/// </summary>
		[Parameter] public RenderFragment TitleTemplate { get; set; }

		/// <summary>
		/// Content (body) text.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Content (body) template.
		/// </summary>
		[Parameter] public RenderFragment ContentTemplate { get; set; }

		/// <summary>
		/// Indicates whether to show close button.
		/// </summary>
		[Parameter] public bool ShowCloseButton { get; set; } = true;

		/// <summary>
		/// Buttons to show. Default is <see cref="MessageBoxButtons.Ok"/>.
		/// </summary>
		[Parameter] public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.Ok;

		/// <summary>
		/// Primary button (if you want to override the default).
		/// </summary>
		[Parameter] public MessageBoxButtons? PrimaryButton { get; set; }

		/// <summary>
		/// Text for <see cref="MessageBoxButtons.Custom"/>.
		/// </summary>
		[Parameter] public string CustomButtonText { get; set; }

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Raised when the message box gets closed. Returns the button clicked.
		/// </summary>
		[Parameter] public EventCallback<MessageBoxButtons> OnClosed { get; set; }

		/// <summary>
		/// Triggers the <see cref="OnClosed"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnClosedAsync(MessageBoxButtons button) => OnClosed.InvokeAsync(button);


		[Inject] protected IStringLocalizer<HxMessageBox> MessageBoxLocalizer { get; set; }

		private HxModal modal;
		private MessageBoxButtons? result;

		/// <summary>
		/// Displays the message box.
		/// </summary>
		/// <returns></returns>
		public async Task ShowAsync()
		{
			result = null;
			await modal.ShowAsync();
		}

		private async Task HandleButtonClick(MessageBoxButtons button)
		{
			result = button;
			await modal.HideAsync();  // fires HxModal.OnClose
		}

		private async Task HandleModalClosed()
		{
			await InvokeOnClosedAsync(result ?? MessageBoxButtons.None);
		}

		private List<ButtonDefinition> GetButtonsToRender()
		{
			var buttons = new List<ButtonDefinition>();
			var primaryButtonEffective = this.PrimaryButton;

			switch (this.Buttons)
			{
				case MessageBoxButtons.AbortRetryIgnore:
					// no primary button (if not explicitly requested)
					buttons.Add(new() { Id = MessageBoxButtons.Abort, Text = MessageBoxLocalizer["Abort"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Abort) });
					buttons.Add(new() { Id = MessageBoxButtons.Retry, Text = MessageBoxLocalizer["Retry"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Retry) });
					buttons.Add(new() { Id = MessageBoxButtons.Ignore, Text = MessageBoxLocalizer["Ignore"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ignore) });
					break;
				case MessageBoxButtons.OkCancel:
					primaryButtonEffective ??= MessageBoxButtons.Ok;
					buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = MessageBoxLocalizer["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
					buttons.Add(new() { Id = MessageBoxButtons.Ok, Text = MessageBoxLocalizer["OK"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ok) });
					break;
				case MessageBoxButtons.YesNo:
					primaryButtonEffective ??= MessageBoxButtons.Yes;
					buttons.Add(new() { Id = MessageBoxButtons.No, Text = MessageBoxLocalizer["No"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.No) });
					buttons.Add(new() { Id = MessageBoxButtons.Yes, Text = MessageBoxLocalizer["Yes"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Yes) });
					break;
				case MessageBoxButtons.RetryCancel:
					primaryButtonEffective ??= MessageBoxButtons.Retry;
					buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = MessageBoxLocalizer["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
					buttons.Add(new() { Id = MessageBoxButtons.Retry, Text = MessageBoxLocalizer["Retry"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Retry) });
					break;
				case MessageBoxButtons.CustomCancel:
					primaryButtonEffective ??= MessageBoxButtons.Custom;
					buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = MessageBoxLocalizer["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
					buttons.Add(new() { Id = MessageBoxButtons.Custom, Text = this.CustomButtonText, IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Custom) });
					break;
				default:
					if (this.Buttons.HasFlag(MessageBoxButtons.Abort))
					{
						buttons.Add(new() { Id = MessageBoxButtons.Abort, Text = MessageBoxLocalizer["Abort"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Abort) });
					}
					if (this.Buttons.HasFlag(MessageBoxButtons.Retry))
					{
						buttons.Add(new() { Id = MessageBoxButtons.Retry, Text = MessageBoxLocalizer["Retry"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Retry) });
					}
					if (this.Buttons.HasFlag(MessageBoxButtons.Ignore))
					{
						buttons.Add(new() { Id = MessageBoxButtons.Ignore, Text = MessageBoxLocalizer["Ignore"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ignore) });
					}
					if (this.Buttons.HasFlag(MessageBoxButtons.Cancel))
					{
						buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = MessageBoxLocalizer["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
					}
					if (this.Buttons.HasFlag(MessageBoxButtons.Yes))
					{
						buttons.Add(new() { Id = MessageBoxButtons.Yes, Text = MessageBoxLocalizer["Yes"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Yes) });
					}
					if (this.Buttons.HasFlag(MessageBoxButtons.No))
					{
						buttons.Add(new() { Id = MessageBoxButtons.No, Text = MessageBoxLocalizer["No"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.No) });
					}
					if (this.Buttons.HasFlag(MessageBoxButtons.Custom))
					{
						buttons.Add(new() { Id = MessageBoxButtons.Custom, Text = this.CustomButtonText, IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Custom) });
					}
					if (this.Buttons.HasFlag(MessageBoxButtons.Ok))
					{
						buttons.Add(new() { Id = MessageBoxButtons.Ok, Text = MessageBoxLocalizer["OK"], IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ok) });
					}
					break;
			}

			if (buttons.Count == 1)
			{
				// single button should be primary
				buttons[0].IsPrimary = true;
			}
			else
			{
				// primary button should be always the last one
				buttons.Sort((b1, b2) => ((b1.IsPrimary ?? false) ? 1 : 0) - ((b2.IsPrimary ?? false) ? 1 : 0));
			}

			return buttons;
		}

		private class ButtonDefinition
		{
			public MessageBoxButtons Id { get; set; }
			public string Text { get; set; }
			public bool? IsPrimary { get; set; }
		}

	}
}
