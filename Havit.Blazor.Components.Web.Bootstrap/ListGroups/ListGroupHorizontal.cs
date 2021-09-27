namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Responsive horizontal setting (breakpoint) for <see cref="HxListGroup"/>.
	/// Default is <see cref="Never"/>.
	/// </summary>
	public enum ListGroupHorizontal
	{
		/// <summary>
		/// Never Horizontal, always vertical.
		/// </summary>
		Never = 0,

		/// <summary>
		/// Horizontal for viewports above "small" breakpoint (<c>576px</c>).
		/// </summary>
		SmallUp,

		/// <summary>
		/// Horizontal for viewports above "medium" breakpoint (<c>768px</c>).
		/// </summary>
		MediumUp,

		/// <summary>
		/// Horizontal for viewports above "large" breakpoint (<c>992px</c>).
		/// </summary>
		LargeUp,

		/// <summary>
		/// Horizontal for viewports above "extra-large" breakpoint (<c>1200px</c>).
		/// </summary>
		ExtraLargeUp,

		/// <summary>
		/// Horizontal for viewports above "XXL" breakpoint (<c>1400px</c>).
		/// </summary>
		XxlUp,

		/// <summary>
		/// Always horizontal, never vertical.
		/// </summary>
		Always
	}
}
