using System.Reflection;
using BlazorAppTest.Pages;
using Microsoft.AspNetCore.Components;

namespace BlazorRTLAppTest.Pages;

public partial class AllTests
{
	private RenderFragment GetComponentTestLinks() => builder =>
	{
		List<string?> routes = GetRoutesToRender(typeof(HxAlertTest).Assembly);

		foreach (var route in routes)
		{
			if (route is null)
			{
				continue;
			}

			builder.OpenElement(1, "p");
			builder.SetKey(route);
			builder.OpenElement(2, "a");
			builder.AddAttribute(3, "href", "/all-tests#" + route.Remove(0, 1));
			builder.AddContent(4, route);
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
