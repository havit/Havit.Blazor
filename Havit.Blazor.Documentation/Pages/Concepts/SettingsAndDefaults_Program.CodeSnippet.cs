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
		HxPlaceholder.Defaults.Color = ThemeColor.Secondary;

		HxButton.Defaults.Size = ButtonSize.Small;
		
		HxDrawer.Defaults.Backdrop = DrawerBackdrop.Static;
		HxDrawer.Defaults.HeaderCssClass = "border-bottom";
		HxDrawer.Defaults.FooterCssClass = "border-top";
		
		HxChipList.Defaults.ChipBadgeSettings.Color = ThemeColor.Secondary;
		HxChipList.Defaults.ChipBadgeSettings.TextColor = ThemeColor.Inverse;
		HxChipList.Defaults.ChipBadgeSettings.CssClass = "p-2 rounded-pill";
	}
}