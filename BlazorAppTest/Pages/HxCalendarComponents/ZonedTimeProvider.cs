namespace BlazorAppTest.Pages.HxCalendarComponents;

public class ZonedTimeProvider : TimeProvider
{
	private readonly TimeZoneInfo _zoneInfo;

	public ZonedTimeProvider(TimeZoneInfo zoneInfo)
	{
		_zoneInfo = zoneInfo;
	}

	public override TimeZoneInfo LocalTimeZone { get => _zoneInfo; }
}