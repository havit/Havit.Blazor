using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorAppTest.Pages.HxCalendarComponents;

public class ZonedTimeProvider : TimeProvider, IDisposable
{
	private TimeZoneInfo zoneInfo;
	private readonly NavigationManager? navigationManager;

	public ZonedTimeProvider(TimeZoneInfo zoneInfo)
	{
		this.zoneInfo = zoneInfo;
	}

	public ZonedTimeProvider(NavigationManager navigationManager) : base()
	{
		this.navigationManager = navigationManager;
		navigationManager.LocationChanged += NavigationManager_LocationChanged;
		Update();
	}

	private void NavigationManager_LocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
	{
		Update();
	}

	private void Update()
	{ 
		TimeZoneInfo? zoneInfo = null;
		try
		{
			var uri = navigationManager?.ToAbsoluteUri(navigationManager.Uri);
			if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("tz", out var vals))
			{
				zoneInfo = TimeZoneInfo.FindSystemTimeZoneById(vals.FirstOrDefault() ?? "Not Found");
			}
		}
		catch (Exception)
		{
			// Ignore and fallback to local timezone
		}
		this.zoneInfo = zoneInfo ?? TimeZoneInfo.Local;
	}
	public override TimeZoneInfo LocalTimeZone { get => zoneInfo; }
	public static TimeProvider FromLocalTimeZone(TimeZoneInfo zoneInfo) => new ZonedTimeProvider(zoneInfo);

	public void Dispose()
	{
		if (navigationManager != null)
		{
			navigationManager.LocationChanged -= NavigationManager_LocationChanged;
		}
	}
}