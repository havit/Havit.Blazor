namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxMessageBox"/> and derived components.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/MessageBoxSettings">https://havit.blazor.eu/types/MessageBoxSettings</see>
	/// </summary>
	public record MessageBoxSettings
	{
		/// <summary>
		/// Settings for the dialog primary button.
		/// </summary>
		public ButtonSettings PrimaryButtonSettings { get; set; }

		/// <summary>
		/// Settings for dialog secondary button(s).
		/// </summary>
		public ButtonSettings SecondaryButtonSettings { get; set; }

		/// <summary>
		/// Settings for underlying <see cref="HxModal"/> component.
		/// </summary>
		public ModalSettings ModalSettings { get; set; }
	}
}
