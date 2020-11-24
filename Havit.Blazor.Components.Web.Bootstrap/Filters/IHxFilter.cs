using Havit.Blazor.Components.Web.Bootstrap.Infrastructure;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public interface IHxFilter : IRenderNotificationComponent
	{
		RenderFragment GetLabelTemplate();

		RenderFragment GetFilterTemplate();

	}
}
