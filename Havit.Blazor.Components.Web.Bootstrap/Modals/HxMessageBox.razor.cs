using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Component for displaying message boxes.<br/>
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
	/// Returns the defaults for <see cref="HxMessageBox"/>.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual MessageBoxSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxMessageBox.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public MessageBoxSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual MessageBoxSettings GetSettings() => Settings;

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
	/// The default is taken from the underlying <see cref="HxModal"/> (<c>true</c>).
	/// </summary>
	[Parameter] public bool? ShowCloseButton { get; set; }

	/// <summary>
	/// Buttons to show. The default is <see cref="MessageBoxButtons.Ok"/>.
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
	protected ButtonSettings PrimaryButtonSettingsEffective => PrimaryButtonSettings ?? GetSettings()?.PrimaryButtonSettings ?? GetDefaults().PrimaryButtonSettings ?? throw new InvalidOperationException(nameof(PrimaryButtonSettings) + " default for " + nameof(HxMessageBox) + " has to be set.");

	/// <summary>
	/// Settings for dialog secondary button(s).
	/// </summary>
	[Parameter] public ButtonSettings SecondaryButtonSettings { get; set; }
	protected ButtonSettings SecondaryButtonSettingsEffective => SecondaryButtonSettings ?? GetSettings()?.SecondaryButtonSettings ?? GetDefaults().SecondaryButtonSettings ?? throw new InvalidOperationException(nameof(SecondaryButtonSettings) + " default for " + nameof(HxMessageBox) + " has to be set.");

	/// <summary>
	/// Settings for the underlying <see cref="HxModal"/> component.
	/// </summary>
	[Parameter] public ModalSettings ModalSettings { get; set; }
	protected ModalSettings ModalSettingsEffective => ModalSettings ?? GetSettings()?.ModalSettings ?? GetDefaults().ModalSettings ?? throw new InvalidOperationException(nameof(ModalSettings) + " default for " + nameof(HxMessageBox) + " has to be set.");

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

	private HxModal _modal;
	private MessageBoxButtons? _result;

	/// <summary>
	/// Displays the message box.
	/// </summary>
	/// <returns></returns>
	public async Task ShowAsync()
	{
		_result = null;
		await _modal.ShowAsync();
	}

	private async Task HandleButtonClick(MessageBoxButtons button)
	{
		_result = button;
		await _modal.HideAsync();  // fires HxModal.OnClose
	}

	private async Task HandleModalClosed()
	{
		await InvokeOnClosedAsync(_result ?? MessageBoxButtons.None);
	}

	private List<ButtonDefinition> GetButtonsToRender()
	{
		var buttons = new List<ButtonDefinition>();
		var primaryButtonEffective = PrimaryButton;

		switch (Buttons)
		{
			case MessageBoxButtons.AbortRetryIgnore:
				// no primary button (if not explicitly requested)
				buttons.Add(new() { Id = MessageBoxButtons.Abort, Text = GetButtonTextEffective(MessageBoxButtons.Abort), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Abort) });
				buttons.Add(new() { Id = MessageBoxButtons.Retry, Text = GetButtonTextEffective(MessageBoxButtons.Retry), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Retry) });
				buttons.Add(new() { Id = MessageBoxButtons.Ignore, Text = GetButtonTextEffective(MessageBoxButtons.Ignore), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ignore) });
				break;
			case MessageBoxButtons.OkCancel:
				primaryButtonEffective ??= MessageBoxButtons.Ok;
				buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = GetButtonTextEffective(MessageBoxButtons.Cancel), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
				buttons.Add(new() { Id = MessageBoxButtons.Ok, Text = GetButtonTextEffective(MessageBoxButtons.Ok), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ok) });
				break;
			case MessageBoxButtons.YesNo:
				primaryButtonEffective ??= MessageBoxButtons.Yes;
				buttons.Add(new() { Id = MessageBoxButtons.No, Text = GetButtonTextEffective(MessageBoxButtons.No), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.No) });
				buttons.Add(new() { Id = MessageBoxButtons.Yes, Text = GetButtonTextEffective(MessageBoxButtons.Yes), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Yes) });
				break;
			case MessageBoxButtons.RetryCancel:
				primaryButtonEffective ??= MessageBoxButtons.Retry;
				buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = GetButtonTextEffective(MessageBoxButtons.Cancel), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
				buttons.Add(new() { Id = MessageBoxButtons.Retry, Text = GetButtonTextEffective(MessageBoxButtons.Retry), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Retry) });
				break;
			case MessageBoxButtons.CustomCancel:
				primaryButtonEffective ??= MessageBoxButtons.Custom;
				buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = GetButtonTextEffective(MessageBoxButtons.Cancel), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
				buttons.Add(new() { Id = MessageBoxButtons.Custom, Text = GetButtonTextEffective(MessageBoxButtons.Custom), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Custom) });
				break;
			default:
				if (Buttons.HasFlag(MessageBoxButtons.Abort))
				{
					buttons.Add(new() { Id = MessageBoxButtons.Abort, Text = GetButtonTextEffective(MessageBoxButtons.Abort), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Abort) });
				}
				if (Buttons.HasFlag(MessageBoxButtons.Retry))
				{
					buttons.Add(new() { Id = MessageBoxButtons.Retry, Text = GetButtonTextEffective(MessageBoxButtons.Retry), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Retry) });
				}
				if (Buttons.HasFlag(MessageBoxButtons.Ignore))
				{
					buttons.Add(new() { Id = MessageBoxButtons.Ignore, Text = GetButtonTextEffective(MessageBoxButtons.Ignore), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ignore) });
				}
				if (Buttons.HasFlag(MessageBoxButtons.Cancel))
				{
					buttons.Add(new() { Id = MessageBoxButtons.Cancel, Text = GetButtonTextEffective(MessageBoxButtons.Cancel), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Cancel) });
				}
				if (Buttons.HasFlag(MessageBoxButtons.Yes))
				{
					buttons.Add(new() { Id = MessageBoxButtons.Yes, Text = GetButtonTextEffective(MessageBoxButtons.Yes), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Yes) });
				}
				if (Buttons.HasFlag(MessageBoxButtons.No))
				{
					buttons.Add(new() { Id = MessageBoxButtons.No, Text = GetButtonTextEffective(MessageBoxButtons.No), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.No) });
				}
				if (Buttons.HasFlag(MessageBoxButtons.Custom))
				{
					buttons.Add(new() { Id = MessageBoxButtons.Custom, Text = GetButtonTextEffective(MessageBoxButtons.Custom), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Custom) });
				}
				if (Buttons.HasFlag(MessageBoxButtons.Ok))
				{
					buttons.Add(new() { Id = MessageBoxButtons.Ok, Text = GetButtonTextEffective(MessageBoxButtons.Ok), IsPrimary = primaryButtonEffective?.HasFlag(MessageBoxButtons.Ok) });
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

	private string GetButtonTextEffective(MessageBoxButtons button)
	{
		return button switch
		{
			MessageBoxButtons.Ok => GetSettings()?.OkButtonText ?? GetDefaults()?.OkButtonText ?? MessageBoxLocalizer["OK"],
			MessageBoxButtons.Cancel => GetSettings()?.CancelButtonText ?? GetDefaults()?.CancelButtonText ?? MessageBoxLocalizer["Cancel"],
			MessageBoxButtons.Retry => GetSettings()?.RetryButtonText ?? GetDefaults()?.RetryButtonText ?? MessageBoxLocalizer["Retry"],
			MessageBoxButtons.Ignore => GetSettings()?.IgnoreButtonText ?? GetDefaults()?.IgnoreButtonText ?? MessageBoxLocalizer["Ignore"],
			MessageBoxButtons.Abort => GetSettings()?.AbortButtonText ?? GetDefaults()?.AbortButtonText ?? MessageBoxLocalizer["Abort"],
			MessageBoxButtons.Yes => GetSettings()?.YesButtonText ?? GetDefaults()?.YesButtonText ?? MessageBoxLocalizer["Yes"],
			MessageBoxButtons.No => GetSettings()?.NoButtonText ?? GetDefaults()?.NoButtonText ?? MessageBoxLocalizer["No"],
			MessageBoxButtons.Custom => CustomButtonText,
			_ => throw new InvalidOperationException("Unsupported button type."),
		};
	}

	private class ButtonDefinition
	{
		public MessageBoxButtons Id { get; set; }
		public string Text { get; set; }
		public bool? IsPrimary { get; set; }
	}
}
