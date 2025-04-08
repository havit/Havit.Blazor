namespace Havit.Blazor.Components.Web.ECharts;
public static class EChartClickEventArgsExtensions
{
	public static decimal? GetValueAsDecimal(this EChartClickEventArgs args)
	{
		ArgumentNullException.ThrowIfNull(args);

		if (args.Value == null)
		{
			return null;
		}

		return Decimal.Parse(args.Value);
	}

	public static int? GetValueAsInt32(this EChartClickEventArgs args)
	{
		ArgumentNullException.ThrowIfNull(args);

		if (args.Value == null)
		{
			return null;
		}

		return Int32.Parse(args.Value);
	}
}
