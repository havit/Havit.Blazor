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
		/// Brand logo to be displayed when the <see cref="HxSidebar"/> is collapsed.
		/// </summary>
		[Parameter] public RenderFragment LogoShort { get; set; }

		/// <summary>
		/// Brand short name.
		/// </summary>
		[Parameter] public string BrandNameShort { get; set; }

		/// <summary>
		/// Adjusts the css class for the element to be hidden when the containg sidebar is collapsed.
		/// </summary>
		/// <param name="cssClass"></param>
		/// <returns></returns>
		private string GetCssClass(string cssClass)
		{
			if (LogoShort is not null)
			{
				cssClass += " hx-hide-when-in-collapsed-sidebar";
			}

			return cssClass;
		}
	}
}
