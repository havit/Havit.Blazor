namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <a href="https://getbootstrap.com/docs/5.1/components/dropdowns/#headers">Dropdown menu header</a> for <see cref="HxDropdownMenu"/>.
	/// </summary>
	public partial class HxDropdownHeader
	{
		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }
	}
}
