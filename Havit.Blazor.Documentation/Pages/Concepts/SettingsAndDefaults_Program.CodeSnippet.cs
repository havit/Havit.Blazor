public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRazorPages();

		services.AddHxServices();
		services.AddHxMessenger();
		services.AddHxMessageBoxHost();

		SetHxComponents();
	}

	private static void SetHxComponents()
	{
		HxPlaceholderContainer.Defaults.Animation = PlaceholderAnimation.Glow;
		HxPlaceholder.Defaults.Color = ThemeColor.Light;

		HxButton.Defaults.Size = ButtonSize.Small;
		
		HxOffcanvas.Defaults.Backdrop = OffcanvasBackdrop.Static;
		HxOffcanvas.Defaults.HeaderCssClass = "border-bottom";
		HxOffcanvas.Defaults.FooterCssClass = "border-top";
		
		HxChipList.Defaults.ChipBadgeSettings.Color = ThemeColor.Light;
		HxChipList.Defaults.ChipBadgeSettings.TextColor = ThemeColor.Dark;
		HxChipList.Defaults.ChipBadgeSettings.CssClass = "p-2 rounded-pill";
	}
}