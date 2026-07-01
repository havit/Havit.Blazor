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
		
		HxChipList.Defaults.Color = ThemeColor.Secondary;
		HxChipList.Defaults.CssClass = "my-chip-list";
	}
}