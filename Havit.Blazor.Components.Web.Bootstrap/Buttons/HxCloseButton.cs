using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A simple close button for dismissing content like modals and alerts.
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
	/// Enables overriding defaults in descandants (use separate set of defaults).
	/// </summary>
	protected new virtual CloseButtonSettings GetDefaults() => Defaults;

	public HxCloseButton()
	{
		if (AdditionalAttributes is null)
		{
			AdditionalAttributes = new();
		}
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
