using System.Reflection;
using BlazorAppTest.Pages;
using Microsoft.AspNetCore.Components;

namespace BlazorRTLAppTest.Pages;

public partial class AllTests
{
	private RenderFragment GetComponentTestLinks() => builder =>
	{
		int sequence = 1;

		List<string?> routes = GetRoutesToRender(typeof(HxAlertTest).Assembly);

		foreach (var route in routes)
		{
			if (route is null)
			{
				continue;
			}

			builder.OpenElement(sequence++, "p");
			builder.OpenElement(sequence++, "a");
			builder.AddAttribute(sequence++, "href", "/all-tests#" + route.Remove(0, 1));
			builder.AddContent(sequence++, route);
			builder.CloseElement();
			builder.CloseElement();
		}
	};

	public static List<string?> GetRoutesToRender(Assembly assembly)
	{
		// Get all the components whose base class is ComponentBase
		var components = assembly
			.ExportedTypes
			.Where(t => t.IsSubclassOf(typeof(ComponentBase)));

		var routes = components
			.Select(component => GetRouteFromComponent(component))
			.Where(config => config is not null)
			.ToList();

		return routes;
	}

	private static string? GetRouteFromComponent(Type component)
	{
		var attributes = component.GetCustomAttributes(inherit: true);

		var routeAttribute = attributes.OfType<RouteAttribute>().FirstOrDefault();

		if (routeAttribute is null)
		{
			// Only map routable components
			return null;
		}

		var route = routeAttribute.Template;

		if (string.IsNullOrEmpty(route))
		{
			throw new Exception($"RouteAttribute in component '{component}' has empty route template");
		}

		return route;
	}
}
