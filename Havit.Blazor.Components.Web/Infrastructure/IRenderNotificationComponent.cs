namespace Havit.Blazor.Components.Web.Infrastructure
{
	public interface IRenderNotificationComponent
	{
		RenderedEventHandler Rendered { get; set; }
	}

	public delegate void RenderedEventHandler(Microsoft.AspNetCore.Components.ComponentBase component, bool firstRender);

}