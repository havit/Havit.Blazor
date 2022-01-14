namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Brand for the <see cref="HxSidebar.HeaderTemplate"/>.
	/// </summary>
	public partial class HxSidebarBrand
	{
		/// <summary>
		/// Brand long name.
		/// </summary>
		[Parameter] public string BrandName { get; set; }

		/// <summary>
		/// Brand logo.
		/// </summary>
		[Parameter] public RenderFragment Logo { get; set; }

		/// <summary>
		/// Brand short name.
		/// </summary>
		[Parameter] public string BrandNameShort { get; set; }
	}
}
