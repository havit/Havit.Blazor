using System;

public class ZonedTimeProvider
{
	// Create a time provider that work with a time zone different than the local time zone 
	public class ZonedTimeProvider : TimeProvider
	{
		private TimeZoneInfo _zoneInfo;
		public ZonedTimeProvider(TimeZoneInfo zoneInfo) : base()
		{
			_zoneInfo = zoneInfo ?? TimeZoneInfo.Local;
		}
		public override TimeZoneInfo LocalTimeZone { get => _zoneInfo; }
	}
}
