using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputDateRangeDateOnlyTests : BunitTestBase
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Binding_PreservesParentFieldAndModelValidation(bool useWrapper)
	{
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US"));
		var model = new RangeModel();
		var editContext = new EditContext(model);
		var changedFields = new List<FieldIdentifier>();
		editContext.OnFieldChanged += (_, args) => changedFields.Add(args.FieldIdentifier);
		var cut = RenderForm(model, editContext, useWrapper);

		cut.FindAll("input")[0].Change("2/13/2024");

		Assert.Equal(new DateOnly(2024, 2, 13), model.Period.StartDate);
		var parentField = new FieldIdentifier(model, nameof(RangeModel.Period));
		Assert.Contains(parentField, changedFields);
		Assert.True(editContext.IsModified(parentField));
		Assert.Contains("Start date cannot be the thirteenth.", editContext.GetValidationMessages(parentField));
		Assert.Contains("Start date cannot be the thirteenth.", cut.Markup);
		Assert.All(cut.FindAll("input"), input => Assert.Contains("is-invalid", input.ClassList));
		Assert.All(changedFields, field => Assert.Same(model, field.Model));

		cut.FindAll("input")[0].Change("2/29/2024");
		Assert.Equal(new DateOnly(2024, 2, 29), model.Period.StartDate);
		Assert.Empty(editContext.GetValidationMessages(parentField));
		Assert.DoesNotContain("is-invalid", cut.Markup);
	}

	[Fact]
	public void Typing_SupportsOpenEmptyLeapDayAndYearBoundaryRanges()
	{
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US"));
		var model = new RangeModel();
		var cut = RenderForm(model, new EditContext(model));

		cut.FindAll("input")[1].Change("1/1/2025");
		Assert.Equal(new DateOnlyRange(null, new DateOnly(2025, 1, 1)), model.Period);
		cut.FindAll("input")[0].Change("12/31/2024");
		Assert.Equal(new DateOnlyRange(new DateOnly(2024, 12, 31), new DateOnly(2025, 1, 1)), model.Period);
		cut.FindAll("input")[0].Change("2/29/2024");
		Assert.Equal(new DateOnly(2024, 2, 29), model.Period.StartDate);
		cut.FindAll("input")[1].Change("");
		Assert.Null(model.Period.EndDate);
		cut.FindAll("input")[0].Change("");
		Assert.Equal(default, model.Period);
		Assert.DoesNotContain("is-invalid", cut.Markup);
	}

	[Theory]
	[InlineData(0, "2/30/2024")]
	[InlineData(1, "invalid")]
	[InlineData(0, "1/1/2025")]
	[InlineData(1, "1/1/2023")]
	public void InvalidInput_PreservesModelAndShowsValidation(int inputIndex, string text)
	{
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US"));
		var original = new DateOnlyRange(new DateOnly(2024, 2, 29), new DateOnly(2024, 12, 31));
		var model = new RangeModel { Period = original };
		var editContext = new EditContext(model);
		var cut = RenderForm(model, editContext);

		cut.FindAll("input")[inputIndex].Change(text);

		Assert.Equal(original, model.Period);
		Assert.NotEmpty(editContext.GetValidationMessages());
		Assert.Contains("is-invalid", cut.Markup);
	}

	[Fact]
	public async Task CalendarsAndPredefinedRanges_UseSharedSettingsAndClearEndpoints()
	{
		var model = new RangeModel();
		var selectedRange = new DateOnlyRange(new DateOnly(2024, 2, 29), new DateOnly(2024, 3, 1));
		var settings = new InputDateRangeSettings
		{
			MinDate = new DateTime(2024, 2, 1),
			MaxDate = new DateTime(2024, 3, 31),
			PredefinedDateRanges = [new() { Label = "Leap day range", DateRange = new(selectedRange.StartDate?.ToDateTime(TimeOnly.MinValue), selectedRange.EndDate?.ToDateTime(TimeOnly.MinValue)) }]
		};
		var cut = RenderForm(model, new EditContext(model), settings: settings);
		var calendars = cut.FindComponents<HxCalendar>();
		Assert.All(calendars, calendar =>
		{
			Assert.Equal(settings.MinDate, calendar.Instance.MinDate);
			Assert.Equal(settings.MaxDate, calendar.Instance.MaxDate);
		});

		await cut.InvokeAsync(() => calendars[0].Instance.ValueChanged.InvokeAsync(new DateTime(2024, 2, 29)));
		Assert.Equal(new DateOnlyRange(new DateOnly(2024, 2, 29), null), model.Period);
		cut.FindComponents<HxButton>().First(button => button.Instance.Text == "Leap day range").Find("button").Click();
		Assert.Equal(selectedRange, model.Period);
		Assert.Equal(new DateTime(2024, 2, 29), cut.FindComponents<HxCalendar>()[0].Instance.Value);

		cut.FindComponents<HxButton>().First(button => button.Instance.Text == "Clear").Find("button").Click();
		Assert.Equal(new DateOnlyRange(null, selectedRange.EndDate), model.Period);
		cut.FindComponents<HxButton>().Last(button => button.Instance.Text == "Clear").Find("button").Click();
		Assert.Equal(default, model.Period);
	}

	[Fact]
	public void RequireDateOrder_CanBeDisabled()
	{
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US"));
		var model = new RangeModel { Period = new(new DateOnly(2024, 1, 1), new DateOnly(2024, 2, 1)) };
		var cut = RenderForm(model, new EditContext(model), settings: new() { RequireDateOrder = false });
		cut.FindAll("input")[0].Change("12/31/2024");
		Assert.Equal(new DateOnly(2024, 12, 31), model.Period.StartDate);
		Assert.DoesNotContain("is-invalid", cut.Markup);
	}

	[Fact]
	public void UnsupportedValueType_IsRejectedClearly()
	{
		var value = 0;
		var exception = Assert.Throws<InvalidOperationException>(() => Render<HxInputDateRange<int>>(parameters => parameters
			.Add(component => component.ValueExpression, () => value)));
		Assert.Contains("Use DateTimeRange or DateOnlyRange", exception.Message);
	}

	[Theory]
	[InlineData("en-US", "3/31/2024", 3, 31)]
	[InlineData("cs-CZ", "27.10.2024", 10, 27)]
	public void WithoutEditForm_PreservesCalendarDatesAcrossTimeZones(string cultureName, string input, int month, int day)
	{
		using var culture = CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo(cultureName));
		var value = default(DateOnlyRange);
		var cut = Render<HxInputDateRange<DateOnlyRange>>(parameters => parameters
			.Add(component => component.Value, value)
			.Add(component => component.ValueChanged, newValue => value = newValue)
			.Add(component => component.ValueExpression, () => value)
			.Add(component => component.TimeProvider, new CalendarTimeProvider()));

		cut.FindAll("input")[0].Change(input);
		Assert.Equal(new DateOnlyRange(new DateOnly(2024, month, day), null), value);
		var calendarDate = cut.FindComponents<HxCalendar>()[0].Instance.Value.Value;
		Assert.Equal(new DateTime(2024, month, day), calendarDate);
		Assert.Equal(DateTimeKind.Unspecified, calendarDate.Kind);
	}

	private sealed class CalendarTimeProvider : TimeProvider
	{
		public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.CreateCustomTimeZone("Test +14", TimeSpan.FromHours(14), "Test +14", "Test +14");
	}

	private IRenderedComponent<EditForm> RenderForm(RangeModel model, EditContext editContext, bool useWrapper = false, InputDateRangeSettings settings = null)
	{
		return Render<EditForm>(parameters => parameters
			.Add(component => component.EditContext, editContext)
			.Add(component => component.ChildContent, _ => builder =>
			{
				builder.OpenComponent<DataAnnotationsValidator>(0);
				builder.CloseComponent();
				builder.OpenComponent(1, useWrapper ? typeof(DateOnlyRangeWrapper) : typeof(HxInputDateRange<DateOnlyRange>));
				builder.AddAttribute(2, "Value", model.Period);
				builder.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<DateOnlyRange>(this, value => model.Period = value));
				builder.AddAttribute(4, "ValueExpression", (Expression<Func<DateOnlyRange>>)(() => model.Period));
				builder.AddAttribute(5, "Settings", settings);
				builder.AddAttribute(6, "Label", "Period");
				builder.AddAttribute(7, "ValidationMessageMode", ValidationMessageMode.Regular);
				builder.CloseComponent();
			}));
	}

	public class RangeModel
	{
		[CustomValidation(typeof(RangeModel), nameof(ValidatePeriod))]
		public DateOnlyRange Period { get; set; }

		public static ValidationResult ValidatePeriod(DateOnlyRange value)
		{
			return value.StartDate?.Day == 13 ? new ValidationResult("Start date cannot be the thirteenth.") : ValidationResult.Success;
		}
	}

	public class DateOnlyRangeWrapper : ComponentBase
	{
		[Parameter] public DateOnlyRange Value { get; set; }
		[Parameter] public EventCallback<DateOnlyRange> ValueChanged { get; set; }
		[Parameter] public Expression<Func<DateOnlyRange>> ValueExpression { get; set; }
		[Parameter] public InputDateRangeSettings Settings { get; set; }
		[Parameter] public string Label { get; set; }
		[Parameter] public ValidationMessageMode? ValidationMessageMode { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<HxInputDateRange<DateOnlyRange>>(0);
			builder.AddAttribute(1, nameof(Value), Value);
			builder.AddAttribute(2, nameof(ValueChanged), ValueChanged);
			builder.AddAttribute(3, nameof(ValueExpression), ValueExpression);
			builder.AddAttribute(4, nameof(Settings), Settings);
			builder.AddAttribute(5, nameof(Label), Label);
			builder.AddAttribute(6, nameof(ValidationMessageMode), ValidationMessageMode);
			builder.CloseComponent();
		}
	}
}
