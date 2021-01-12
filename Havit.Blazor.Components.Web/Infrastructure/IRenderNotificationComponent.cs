using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Infrastructure
{
	// TODO: Naming!
	public interface IRenderNotificationComponent
	{
		// TODO: Naming!
		RenderedEventHandler Rendered { get; set; }
	}

	// TODO: Naming!
	public delegate void RenderedEventHandler(Microsoft.AspNetCore.Components.ComponentBase component, bool firstRender);

}