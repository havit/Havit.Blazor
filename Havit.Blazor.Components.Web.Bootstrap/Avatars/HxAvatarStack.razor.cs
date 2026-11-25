namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/avatar/#avatar-stack">Bootstrap Avatar stack</see> component.
/// Groups multiple <see cref="HxAvatar"/> components with an overlapping effect.
/// Avatars are rendered in reverse order, so the first avatar appears on top.
/// To show a count of additional users, add an initials avatar (e.g. <c>&lt;HxAvatar Color="ThemeColor.Secondary"&gt;+5&lt;/HxAvatar&gt;</c>) as the last item.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAvatarStack">https://havit.blazor.eu/components/HxAvatarStack</see>
/// </summary>
public partial class HxAvatarStack
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxAvatarStack"/> and derived components.
	/// </summary>
	public static AvatarStackSettings Defaults { get; set; }

	static HxAvatarStack()
	{
		Defaults = new AvatarStackSettings()
		{
			Size = AvatarSize.Regular,
			CssClass = null
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual AvatarStackSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public AvatarStackSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual AvatarStackSettings GetSettings() => Settings;

	/// <summary>
	/// Content of the avatar stack (put your <see cref="HxAvatar"/>s here).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Size of the avatars in the stack (a shorthand for setting the size of all contained avatars).
	/// The default is <see cref="AvatarSize.Regular"/>.
	/// </summary>
	[Parameter] public AvatarSize? Size { get; set; }
	protected AvatarSize SizeEffective => Size ?? GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxAvatarStack) + " has to be set.");

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
			"avatar-stack",
			GetSizeCssClass(),
			CssClassEffective);
	}

	private string GetSizeCssClass()
	{
		return SizeEffective switch
		{
			AvatarSize.Regular => null,
			AvatarSize.ExtraSmall => "avatar-stack-xs",
			AvatarSize.Small => "avatar-stack-sm",
			AvatarSize.Large => "avatar-stack-lg",
			AvatarSize.ExtraLarge => "avatar-stack-xl",
			_ => throw new InvalidOperationException($"Unknown {nameof(AvatarSize)} value: {SizeEffective}.")
		};
	}
}
