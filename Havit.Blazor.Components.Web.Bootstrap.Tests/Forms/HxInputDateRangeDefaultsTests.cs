using Bunit;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[CollectionDefinition("Date range defaults", DisableParallelization = true)]
public class DateRangeDefaultsCollection
{
}

[Collection("Date range defaults")]
public class HxInputDateRangeDefaultsTests : BunitTestBase
{
	[Fact]
	public void Defaults_AreSharedAndConvertPresetsWithoutChangingCalendarDates()
	{
		var originalDefaults = HxInputDateRange.Defaults;
		try
		{
			var preset = new DateTimeRange(new DateTime(2024, 2, 29, 12, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 1, 18, 0, 0, DateTimeKind.Utc));
			HxInputDateRange.Defaults = originalDefaults with
			{
				FromPlaceholder = "Shared from",
				ShowClearButton = false,
				MinDate = new DateTime(2024, 1, 1),
				PredefinedDateRanges = [new() { Label = "Shared preset", DateRange = preset }]
			};
			var dateTimeValue = default(DateTimeRange);
			var dateOnlyValue = default(DateOnlyRange);
			var dateTimeInput = Render<HxInputDateRange<DateTimeRange>>(parameters => parameters
				.Add(component => component.ValueExpression, () => dateTimeValue)
				.Add(component => component.ValueChanged, value => dateTimeValue = value));
			var dateOnlyInput = Render<HxInputDateRange<DateOnlyRange>>(parameters => parameters
				.Add(component => component.ValueExpression, () => dateOnlyValue)
				.Add(component => component.ValueChanged, value => dateOnlyValue = value));

			Assert.Equal("Shared from", dateTimeInput.Find("input").GetAttribute("placeholder"));
			Assert.Equal("Shared from", dateOnlyInput.Find("input").GetAttribute("placeholder"));
			Assert.All(dateOnlyInput.FindComponents<HxCalendar>(), calendar => Assert.Equal(HxInputDateRange.Defaults.MinDate, calendar.Instance.MinDate));
			Assert.DoesNotContain(dateOnlyInput.FindComponents<HxButton>(), button => button.Instance.Text == "Clear");
			dateTimeInput.FindComponents<HxButton>().First(button => button.Instance.Text == "Shared preset").Find("button").Click();
			dateOnlyInput.FindComponents<HxButton>().First(button => button.Instance.Text == "Shared preset").Find("button").Click();
			Assert.Equal(preset, dateTimeValue);
			Assert.Equal(new DateOnlyRange(new DateOnly(2024, 2, 29), new DateOnly(2024, 3, 1)), dateOnlyValue);

			var customPreset = new DateOnlyRange(null, new DateOnly(2025, 1, 1));
			var overridden = Render<HxInputDateRange<DateOnlyRange>>(parameters => parameters
				.Add(component => component.ValueExpression, () => dateOnlyValue)
				.Add(component => component.ValueChanged, value => dateOnlyValue = value)
				.Add(component => component.Settings, new() { FromPlaceholder = "Settings from", PredefinedDateRanges = [new() { Label = "Settings preset", DateRange = new(null, new DateTime(2025, 1, 1, 18, 0, 0, DateTimeKind.Utc)) }] })
				.Add(component => component.FromPlaceholder, "Parameter from"));
			Assert.Equal("Parameter from", overridden.Find("input").GetAttribute("placeholder"));
			Assert.DoesNotContain(overridden.FindComponents<HxButton>(), button => button.Instance.Text == "Shared preset");
			overridden.FindComponents<HxButton>().First(button => button.Instance.Text == "Settings preset").Find("button").Click();
			Assert.Equal(customPreset, dateOnlyValue);

			var parameterPreset = new DateOnlyRange(new DateOnly(2025, 2, 1), null);
			var parameterInput = Render<HxInputDateRange<DateOnlyRange>>(parameters => parameters
				.Add(component => component.ValueExpression, () => dateOnlyValue)
				.Add(component => component.ValueChanged, value => dateOnlyValue = value)
				.Add(component => component.Settings, overridden.Instance.Settings)
				.Add(component => component.PredefinedDateRanges, [new() { Label = "Parameter preset", DateRange = parameterPreset }]));
			Assert.DoesNotContain(parameterInput.FindComponents<HxButton>(), button => button.Instance.Text == "Settings preset");
			parameterInput.FindComponents<HxButton>().First(button => button.Instance.Text == "Parameter preset").Find("button").Click();
			Assert.Equal(parameterPreset, dateOnlyValue);
		}
		finally
		{
			HxInputDateRange.Defaults = originalDefaults;
		}
	}
}
