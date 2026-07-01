namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/avatar/">Bootstrap Avatar</see> component.
/// Represents a user or entity with an image or initials.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAvatar">https://havit.blazor.eu/components/HxAvatar</see>
/// </summary>
public partial class HxAvatar
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxAvatar"/> and derived components.
	/// </summary>
	public static AvatarSettings Defaults { get; set; }

	static HxAvatar()
	{
		Defaults = new AvatarSettings()
		{
			Size = AvatarSize.Regular,
			Color = ThemeColor.None,
			Subtle = false,
			CssClass = null
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual AvatarSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public AvatarSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual AvatarSettings GetSettings() => Settings;

	/// <summary>
	/// URL of the avatar image. Renders an <c>img.avatar-img</c> element inside the avatar.
	/// </summary>
	[Parameter] public string ImageSrc { get; set; }

	/// <summary>
	/// Alternate text for the avatar image (<see cref="ImageSrc"/>).
	/// </summary>
	[Parameter] public string ImageAlt { get; set; }

	/// <summary>
	/// Content of the avatar, usually initials (e.g. <c>AB</c>). Can also be used for fully custom content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Size of the avatar. The default is <see cref="AvatarSize.Regular"/>.
	/// </summary>
	[Parameter] public AvatarSize? Size { get; set; }
	protected AvatarSize SizeEffective => Size ?? GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxAvatar) + " has to be set.");

	/// <summary>
	/// Theme color of the avatar (background and contrasting content color), usually combined with initials.
	/// The default is <see cref="ThemeColor.None"/> (the default avatar background).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected ThemeColor ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color ?? throw new InvalidOperationException(nameof(Color) + " default for " + nameof(HxAvatar) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, the avatar uses the subtle variant of the theme color (<c>avatar-subtle</c>), composed with <see cref="Color"/>.
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Subtle { get; set; }
	protected bool SubtleEffective => Subtle ?? GetSettings()?.Subtle ?? GetDefaults().Subtle ?? throw new InvalidOperationException(nameof(Subtle) + " default for " + nameof(HxAvatar) + " has to be set.");

	/// <summary>
	/// Status indicator of the avatar. The default is <see cref="AvatarStatus.None"/> (no indicator rendered).
	/// </summary>
	[Parameter] public AvatarStatus Status { get; set; } = AvatarStatus.None;

	/// <summary>
	/// Accessible label of the status indicator (rendered as <c>aria-label</c>).
	/// The default is the English name of the <see cref="Status"/> (e.g. <c>Online</c>).
	/// </summary>
	[Parameter] public string StatusLabel { get; set; }
	protected string StatusLabelEffective => StatusLabel ?? GetStatusDefaultLabel();

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected virtual string GetCssClass()
	{
		return CssClassHelper.Combine(
			"avatar",
			GetSizeCssClass(),
			SubtleEffective ? "avatar-subtle" : null,
			ColorEffective.ToThemeCss(),
			CssClassEffective);
	}

	private string GetSizeCssClass()
	{
		return SizeEffective switch
		{
			AvatarSize.Regular => null,
			AvatarSize.ExtraSmall => "avatar-xs",
			AvatarSize.Small => "avatar-sm",
			AvatarSize.Large => "avatar-lg",
			AvatarSize.ExtraLarge => "avatar-xl",
			_ => throw new InvalidOperationException($"Unknown {nameof(AvatarSize)} value: {SizeEffective}.")
		};
	}

	private string GetStatusCssClass()
	{
		return Status switch
		{
			AvatarStatus.Online => "status-online",
			AvatarStatus.Offline => "status-offline",
			AvatarStatus.Busy => "status-busy",
			AvatarStatus.Away => "status-away",
			_ => throw new InvalidOperationException($"Unknown {nameof(AvatarStatus)} value: {Status}.")
		};
	}

	private string GetStatusDefaultLabel()
	{
		return Status switch
		{
			AvatarStatus.Online => "Online",
			AvatarStatus.Offline => "Offline",
			AvatarStatus.Busy => "Busy",
			AvatarStatus.Away => "Away",
			_ => throw new InvalidOperationException($"Unknown {nameof(AvatarStatus)} value: {Status}.")
		};
	}
}
