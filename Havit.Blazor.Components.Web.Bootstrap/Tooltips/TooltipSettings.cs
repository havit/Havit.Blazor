using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

public record class TooltipSettings : TooltipInternalSettings
{
	/// <summary>
	/// Tooltip trigger(s).
	/// </summary>
	public TooltipTrigger? Trigger { get; set; }
}
