namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Render order.
	/// </summary>
	public enum InputRenderOrder
	{
		/// <summary>
		/// Render Label first, then input (majority of components).
		/// </summary>
		LabelInput,

		/// <summary>
		/// Render Input first, then label (e.g. checkbox).
		/// </summary>
		InputLabel
	}
}