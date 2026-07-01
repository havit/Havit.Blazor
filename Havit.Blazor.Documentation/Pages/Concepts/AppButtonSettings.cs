namespace Havit.Blazor.Documentation.Pages.Concepts;

public static class AppButtonSettings
{
	public static ButtonSettings MainButton { get; } = new()
	{
		Color = ThemeColor.Primary,
		Icon = BootstrapIcon.Fullscreen
	};

	public static ButtonSettings SecondaryButton { get; } = new()
	{
		Color = ThemeColor.Secondary,
		Variant = ButtonVariant.Outline
	};

	public static ButtonSettings CloseButton { get; } = new()
	{
		Color = ThemeColor.Danger,
		Variant = ButtonVariant.Outline,
		Icon = BootstrapIcon.XLg
	};
}
