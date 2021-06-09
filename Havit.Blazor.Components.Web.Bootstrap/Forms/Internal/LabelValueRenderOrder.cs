namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Render order.
	/// </summary>
	public enum LabelValueRenderOrder
	{
		/// <summary>
		/// Render label first, then value/input (majority of components).
		/// </summary>
		LabelValue,

		/// <summary>
		/// Render value/input first, then label (e.g. checkbox).
		/// </summary>
		ValueLabel
	}
}