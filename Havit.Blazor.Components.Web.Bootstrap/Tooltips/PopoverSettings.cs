using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

public record class PopoverSettings : TooltipInternalSettings
{
	/// <summary>
	/// Popover trigger(s).
	/// </summary>
	public PopoverTrigger? Trigger { get; set; }
}
