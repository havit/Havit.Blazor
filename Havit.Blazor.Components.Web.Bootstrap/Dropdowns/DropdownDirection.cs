using static System.Net.WebRequestMethods;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Direction for <see cref="HxDropdown"/>.
	/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/#directions"/>
	/// </summary>
	public enum DropdownDirection
	{
		Down = 0,

		Up,

		/// <summary>
		/// Left (in LTR).
		/// </summary>
		Start,

		/// <summary>
		/// Right (in LTR).
		/// </summary>
		End
	}
}