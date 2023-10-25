public class AppButtonSettings
{
	public static ButtonSettings MainButton { get; } = new()
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
