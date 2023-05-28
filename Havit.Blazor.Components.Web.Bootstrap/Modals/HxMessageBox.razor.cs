using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Component to display message-boxes.<br/>
/// Usually used via <see cref="HxMessageBoxService"/> and <see cref="HxMessageBoxHost"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxMessageBox">https://havit.blazor.eu/components/HxMessageBox</see>
/// </summary>
public partial class HxMessageBox : ComponentBase
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxButton"/> and derived components.
	/// </summary>
	public static MessageBoxSettings Defaults { get; set; }

	static HxMessageBox()
	{
		Defaults = new MessageBoxSettings()
		{
			PrimaryButtonSettings = new ButtonSettings()
			{
				Color = ThemeColor.Primary,
			},
			SecondaryButtonSettings = new ButtonSettings()
			{
				Color = ThemeColor.Secondary,
			},
			ModalSettings = new()
			{
				Backdrop = ModalBackdrop.Static
			}
		};
	}

	/// <summary>
	/// Returns <see cref="HxMessageBox"/> defaults.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected virtual MessageBoxSettings GetDefaults() => Defaults;

	///// <summary>
	///// Header icon.
	///// </summary>
	//[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Title text (Header).
	/// </summary>
	[Parameter] public string Title { get; set; }

	/// <summary>
	/// Header template (Header).
	/// </summary>
	[Parameter] public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// Content (body) text.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Body (content) template.
	/// </summary>
	[Parameter] public RenderFragment BodyTemplate { get; set; }

	/// <summary>
	/// Indicates whether to show the close button.
	/// Default is taken from the underlying <see cref="HxModal"/> (<c>true</c>).
	/// </summary>
	[Parameter] public bool? ShowCloseButton { get; set; }

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
	/// Settings for the dialog primary button.
	/// </summary>
	[Parameter] public ButtonSettings PrimaryButtonSettings { get; set; }
	protected ButtonSettings PrimaryButtonSettingsEffective => this.PrimaryButtonSettings ?? GetDefaults().PrimaryButtonSettings ?? throw new InvalidOperationException(nameof(PrimaryButtonSettings) + " default for " + nameof(HxMessageBox) + " has to be set.");

	/// <summary>
	/// Settings for dialog secondary button(s).
	/// </summary>
	[Parameter] public ButtonSettings SecondaryButtonSettings { get; set; }
	protected ButtonSettings SecondaryButtonSettingsEffective => this.SecondaryButtonSettings ?? GetDefaults().SecondaryButtonSettings ?? throw new InvalidOperationException(nameof(SecondaryButtonSettings) + " default for " + nameof(HxMessageBox) + " has to be set.");

	/// <summary>
	/// Settings for underlying <see cref="HxModal"/> component.
	/// </summary>
	[Parameter] public ModalSettings ModalSettings { get; set; }
	protected ModalSettings ModalSettingsEffective => this.ModalSettings ?? GetDefaults().ModalSettings ?? throw new InvalidOperationException(nameof(ModalSettings) + " default for " + nameof(HxMessageBox) + " has to be set.");

	/// <summary>
	/// Raised when the message box gets closed. Returns the button clicked.
	/// </summary>
	[Parameter] public EventCallback<MessageBoxButtons> OnClosed { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClosed"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClosedAsync(MessageBoxButtons button) => OnClosed.InvokeAsync(button);

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <see cref="HxModal"/>.
	/// </summary>
	[Parameter] public Dictionary<string, object> AdditionalAttributes { get; set; }


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
