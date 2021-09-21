namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Responsive expand setting (breakpoint) for <see cref="HxNavbar"/>.
	/// </summary>
	public enum NavbarExpand
	{
		/// <summary>
		/// Always expanded, never collapses.
		/// </summary>
		Always,

		/// <summary>
		/// Expanded for viewports above "small" breakpoint (<c>576px</c>).
		/// </summary>
		SmallUp,

		/// <summary>
		/// Expanded for viewports above "medium" breakpoint (<c>768px</c>).
		/// </summary>
		MediumUp,

		/// <summary>
		/// Expanded for viewports above "large" breakpoint (<c>992px</c>).
		/// </summary>
		LargeUp,

		/// <summary>
		/// Expanded for viewports above "extra-large" breakpoint (<c>1200px</c>).
		/// </summary>
		ExtraLargeUp,

		/// <summary>
		/// Expanded for viewports above "XXL" breakpoint (<c>1400px</c>).
		/// </summary>
		XxlUp,

		/// <summary>
		/// Never expanded, always collapsed.
		/// </summary>
		Never
	}
}
