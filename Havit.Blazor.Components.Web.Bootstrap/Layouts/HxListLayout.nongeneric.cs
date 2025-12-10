namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxListLayout{TFilterModel}" />.
/// </summary>
public sealed class HxListLayout
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxListLayout{TFilterModel}"/> and derived components.
	/// </summary>
	public static ListLayoutSettings Defaults { get; set; }

	static HxListLayout()
	{
		Defaults = new ListLayoutSettings()
		{
			CardSettings = new CardSettings(),
			FilterSubmitButtonSettings = new ButtonSettings()
			{
				Color = ThemeColor.Primary,
			},
			FilterOpenButtonSettings = new ButtonSettings()
			{
				Icon = BootstrapIcon.Filter,
				Color = ThemeColor.Light,
			},
			FilterOffcanvasSettings = new OffcanvasSettings(),
		};
	}

	/// <summary>
	/// Can be used for TFilterModelType to indicate that there is no filter in the <see cref="HxListLayout"/> component.
	/// </summary>
	public sealed class NoFilter { }
}
