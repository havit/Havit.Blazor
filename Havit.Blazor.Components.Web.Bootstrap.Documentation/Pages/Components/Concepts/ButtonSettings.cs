namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Pages.Components.Concepts;

public static class AppButtonSettings
{
	/// <summary>
	/// We can derive the settings from the defaults using the with statement.
	/// </summary>
	public static ButtonSettings MainButton { get; } = HxButton.Defaults with
	{
		Color = ThemeColor.Primary,
		Icon = BootstrapIcon.Fullscreen
	};

	public static ButtonSettings SecondaryButton { get; } = new()
	{
		Color = ThemeColor.Secondary,
		Outline = true
	};

	public static ButtonSettings CloseButton { get; } = new()
	{
		Color = ThemeColor.Danger,
		Outline = true,
		Icon = BootstrapIcon.XLg
	};
}
