namespace BlazorAppTest.Pages.HxCalendarComponents;

public class ZonedTimeProvider : TimeProvider
{
	private TimeZoneInfo zoneInfo;

	public ZonedTimeProvider(TimeZoneInfo zoneInfo)
	{
		this.zoneInfo = zoneInfo;
	}

	public override TimeZoneInfo LocalTimeZone { get => zoneInfo; }
}