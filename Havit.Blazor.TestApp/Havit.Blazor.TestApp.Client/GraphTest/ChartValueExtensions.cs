using System.Globalization;
using Havit.Blazor.Components.Web.ECharts;

namespace Havit.Blazor.TestApp.Client.GraphTest;

public static class ChartValueExtensions
{
	public static HxEChart.JSFunc CreateSimpleFormatterJsFunc(this ChartValueType chartValueType, string unit, bool round)
	{
		return CreateCoreFormatterJsFunc(chartValueType, unit, round, valueLocator: ".value");
	}

	public static HxEChart.JSFunc CreateSimpleValueFormatterJsFunc(this ChartValueType chartValueType, string unit, bool round)
	{
		return CreateCoreFormatterJsFunc(chartValueType, unit, round, valueLocator: null);
	}

	private static HxEChart.JSFunc CreateCoreFormatterJsFunc(ChartValueType chartValueType, string unit, bool round, string valueLocator)
	{
		return (chartValueType, round) switch
		{
			(ChartValueType.Money, _) =>
				new HxEChart.JSFunc($$"""
                    function (arg) {
                        return Math.floor(arg{{valueLocator}}).toLocaleString('{{CultureInfo.CurrentUICulture.Name}}') + ' {{unit}}';
                    }
                    """),
			(ChartValueType.Hours, true) =>
				new HxEChart.JSFunc($$"""
                    function (arg) {
                        const [hours, minutes] = [Math.floor(arg{{valueLocator}}), Math.floor((arg{{valueLocator}} % 1) * 60)];
                        return hours.toLocaleString('{{CultureInfo.CurrentUICulture.Name}}') + ' {{unit}}';
                    }
                    """),
			(ChartValueType.Hours, false) =>
				new HxEChart.JSFunc($$"""
                    function (arg) {
                        const [hours, minutes] = [Math.floor(arg{{valueLocator}}), Math.floor((arg{{valueLocator}} % 1) * 60)];
                        return hours.toLocaleString('{{CultureInfo.CurrentUICulture.Name}}') + `:${String(minutes).padStart(2, '0')}` + ' {{unit}}';
                    }
                    """),
			(ChartValueType.Percent, _) =>
				new HxEChart.JSFunc($$"""
                    function (arg) {
                        return arg{{valueLocator}}.toLocaleString('{{CultureInfo.CurrentUICulture.Name}}') + ' {{unit}}';
                    }
                    """),
			_ => throw new InvalidOperationException($"ChartValueType {chartValueType} is unknown."),
		};
	}
}
