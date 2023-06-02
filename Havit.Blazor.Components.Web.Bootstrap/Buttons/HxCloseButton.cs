namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.2/components/close-button/">close-button</see> component.<br />
/// A simple close button for dismissing content like modals and alerts.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCloseButton">https://havit.blazor.eu/components/HxCloseButton</see>
/// </summary>
public class HxCloseButton : HxButton
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxCloseButton"/> and derived components.
	/// </summary>
	public static new CloseButtonSettings Defaults { get; set; }

	/// <summary>
	/// Toggles between the light and dark version of the button.
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? White { get; set; }
	protected bool WhiteEffective => White ?? GetDefaults().White ?? throw new InvalidOperationException(nameof(White) + " default for " + nameof(HxCloseButton) + " has to be set.");

	protected override string CoreCssClass => $"{base.CoreCssClass} btn-close " + (WhiteEffective ? "btn-close-white" : string.Empty);

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected new virtual CloseButtonSettings GetDefaults() => Defaults;

	public HxCloseButton()
	{
		AdditionalAttributes ??= new();
		AdditionalAttributes.Add("aria-label", "Close"); // Adding the aria-label for accessibility.
	}

	static HxCloseButton()
	{
		Defaults = new CloseButtonSettings()
		{
			White = false
		};
	}
}
