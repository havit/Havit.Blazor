using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxMessageBox : ComponentBase
	{
		///// <summary>
		///// Header icon.
		///// </summary>
		//[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Header text.
		/// </summary>
		[Parameter] public string HeaderText { get; set; }

		/// <summary>
		/// Header template.
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

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
		/// Buttons to show. Default is <see cref="HxMessageBoxButtons.Ok"/>.
		/// </summary>
		[Parameter] public HxMessageBoxButtons Buttons { get; set; } = HxMessageBoxButtons.Ok;

		/// <summary>
		/// Primary button (if you want to override the default).
		/// </summary>
		[Parameter] public HxMessageBoxButtons? PrimaryButton { get; set; }

		/// <summary>
		/// Text for <see cref="HxMessageBoxButtons.Custom"/>.
		/// </summary>
		[Parameter] public string CustomButtonText { get; set; }

		[Parameter] public string CssClass { get; set; }

		[Parameter] public EventCallback<HxMessageBoxButtons> OnClosed { get; set; }


		[Inject] protected IStringLocalizer<HxMessageBox> Loc { get; set; }

		private HxModal modal;
		private bool shouldSignalClose = false;

		public async Task ShowAsync()
		{
			shouldSignalClose = true;
			await modal.ShowAsync();
		}

		private async Task HandleButtonClick(HxMessageBoxButtons button)
		{
			shouldSignalClose = false;
			await modal.HideAsync();  // fires HxModal.OnClosed => We have shouldSignalClose to prevent raising our OnClose twice
			await OnClosed.InvokeAsync(button);
		}

		private async Task HandleModalClosed()
		{
			if (shouldSignalClose)
			{
				await OnClosed.InvokeAsync(HxMessageBoxButtons.None);
				shouldSignalClose = false;
			}
		}

		private List<ButtonDefintion> GetButtonsToRender()
		{
			var buttons = new List<ButtonDefintion>();
			var primaryButtonEffective = this.PrimaryButton;

			switch (this.Buttons)
			{
				case HxMessageBoxButtons.AbortRetryIgnore:
					// no primary button (if not explicitly requested)
					buttons.Add(new() { Id = HxMessageBoxButtons.Abort, Text = Loc["Abort"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Abort) });
					buttons.Add(new() { Id = HxMessageBoxButtons.Retry, Text = Loc["Retry"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Retry) });
					buttons.Add(new() { Id = HxMessageBoxButtons.Ignore, Text = Loc["Ignore"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Ignore) });
					break;
				case HxMessageBoxButtons.OkCancel:
					primaryButtonEffective ??= HxMessageBoxButtons.Ok;
					buttons.Add(new() { Id = HxMessageBoxButtons.Cancel, Text = Loc["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Cancel) });
					buttons.Add(new() { Id = HxMessageBoxButtons.Ok, Text = Loc["OK"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Ok) });
					break;
				case HxMessageBoxButtons.YesNo:
					primaryButtonEffective ??= HxMessageBoxButtons.Yes;
					buttons.Add(new() { Id = HxMessageBoxButtons.No, Text = Loc["No"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.No) });
					buttons.Add(new() { Id = HxMessageBoxButtons.Yes, Text = Loc["Yes"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Yes) });
					break;
				case HxMessageBoxButtons.RetryCancel:
					primaryButtonEffective ??= HxMessageBoxButtons.Retry;
					buttons.Add(new() { Id = HxMessageBoxButtons.Cancel, Text = Loc["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Cancel) });
					buttons.Add(new() { Id = HxMessageBoxButtons.Retry, Text = Loc["Retry"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Retry) });
					break;
				case HxMessageBoxButtons.CustomCancel:
					primaryButtonEffective ??= HxMessageBoxButtons.Custom;
					buttons.Add(new() { Id = HxMessageBoxButtons.Cancel, Text = Loc["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Cancel) });
					buttons.Add(new() { Id = HxMessageBoxButtons.Custom, Text = this.CustomButtonText, IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Custom) });
					break;
				default:
					if (this.Buttons.HasFlag(HxMessageBoxButtons.Abort))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.Abort, Text = Loc["Abort"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Abort) });
					}
					if (this.Buttons.HasFlag(HxMessageBoxButtons.Retry))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.Retry, Text = Loc["Retry"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Retry) });
					}
					if (this.Buttons.HasFlag(HxMessageBoxButtons.Ignore))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.Ignore, Text = Loc["Ignore"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Ignore) });
					}
					if (this.Buttons.HasFlag(HxMessageBoxButtons.Cancel))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.Cancel, Text = Loc["Cancel"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Cancel) });
					}
					if (this.Buttons.HasFlag(HxMessageBoxButtons.Yes))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.Yes, Text = Loc["Yes"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Yes) });
					}
					if (this.Buttons.HasFlag(HxMessageBoxButtons.No))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.No, Text = Loc["No"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.No) });
					}
					if (this.Buttons.HasFlag(HxMessageBoxButtons.Custom))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.Custom, Text = this.CustomButtonText, IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Custom) });
					}
					if (this.Buttons.HasFlag(HxMessageBoxButtons.Ok))
					{
						buttons.Add(new() { Id = HxMessageBoxButtons.Ok, Text = Loc["Ok"], IsPrimary = primaryButtonEffective?.HasFlag(HxMessageBoxButtons.Ok) });
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

		private class ButtonDefintion
		{
			public HxMessageBoxButtons Id { get; set; }
			public string Text { get; set; }
			public bool? IsPrimary { get; set; }
		}

	}
}
