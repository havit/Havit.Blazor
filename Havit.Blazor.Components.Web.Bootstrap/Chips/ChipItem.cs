namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Chip item to be rendered in UI.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/ChipItem">https://havit.blazor.eu/types/ChipItem</see>
	/// </summary>
	public class ChipItem
	{
		/// <summary>
		/// Chip template.
		/// </summary>
		public RenderFragment ChipTemplate { get; init; }

		/// <summary>
		/// <c>True</c> when it is possible to remove the chip.
		/// </summary>
		public bool Removable { get; init; } = false;

		/// <summary>
		/// Remove action called when chip should be removed.
		/// It receives the model from which the chip should be removed.
		/// It is not the same instance as the one from which the chip was generated!
		/// </summary>
		public Action<object> RemoveAction { get; init; }
	}
}
