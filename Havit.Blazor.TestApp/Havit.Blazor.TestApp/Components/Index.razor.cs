using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Havit.Blazor.TestApp.Components;

public partial class Index
{
	private IEnumerable<string> GetTestPages()
	{
		return [
			.. GetRoutesToRender(typeof(Havit.Blazor.TestApp.Client._Imports).Assembly),
			.. GetRoutesToRender(typeof(Havit.Blazor.TestApp.Components._Imports).Assembly)
			];
	}

	public static List<string> GetRoutesToRender(Assembly assembly)
	{
		// Get all the components whose base class is ComponentBase
		var components = assembly
			.ExportedTypes
			.Where(t => t.IsSubclassOf(typeof(ComponentBase)))
			.Where(t => t.Name.EndsWith("Test"));

		var routes = components
			.SelectMany(component => GetRoutesFromComponent(component))
			.Where(config => config is not null)
			.ToList();

		return routes;
	}

	private static IEnumerable<string> GetRoutesFromComponent(Type component)
	{
		var attributes = component.GetCustomAttributes(inherit: true);

		var routeAttributes = attributes.OfType<RouteAttribute>();

		if (routeAttributes is null)
		{
			yield return null;
		}

		foreach (var routeAttribute in routeAttributes)
		{
			if (String.IsNullOrWhiteSpace(routeAttribute.Template))
			{
				continue;
			}

			if (routeAttribute.Template.Contains('{'))
			{
				continue;
			}

			yield return routeAttribute.Template;
		}
	}
}
