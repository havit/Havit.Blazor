namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxPlaceholder"/> and derived components.
	/// </summary>
	public record PlaceholderSettings
	{
		/// <summary>
		/// Size of the placeholder.
		/// </summary>
		public PlaceholderSize? Size { get; set; }

		/// <summary>
		/// Explicit color of the placeholder.
		/// </summary>
		public ThemeColor? Color { get; set; }

		/// <summary>
		/// Additional CSS class for <see cref="HxPlaceholder"/>.
		/// </summary>
		public string CssClass { get; set; }
	}
}
