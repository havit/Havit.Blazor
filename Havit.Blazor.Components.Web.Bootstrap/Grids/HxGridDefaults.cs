namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxGrid{TItem}"/>.
	/// </summary>
	public class HxGridDefaults
	{
		/// <summary>
		/// Icon to display ascending sort direction.
		/// </summary>
		public IconBase SortAscendingIcon { get; set; } = BootstrapIcon.SortAlphaDownAlt;

		/// <summary>
		/// Icon to display descending sort direction.
		/// </summary>
		public IconBase SortDescendingIcon { get; set; } = BootstrapIcon.SortAlphaDown;
	}
}