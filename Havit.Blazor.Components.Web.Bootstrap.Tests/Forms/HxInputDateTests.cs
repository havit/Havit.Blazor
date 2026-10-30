using System.Globalization;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputDateTests : BunitTestBase
{
	[Fact]
	public void HxInputDate_EnabledShouldOverrideFormStateForNestedControls_Issue877()
	{
		// https://github.com/havit/Havit.Blazor/issues/877

		// Arrange
		var myValue = new DateTime(2020, 2, 10);

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxFormState>(0);
			builder.AddAttribute(1, nameof(HxFormState.Enabled), false);

			builder.AddAttribute(1, nameof(HxFormState.ChildContent), (RenderFragment)((builder2) =>
			{
				builder2.OpenComponent<HxInputDate<DateTime>>(0);
				builder2.AddAttribute(1, "Value", myValue);
				builder2.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime>(this, (value) => { myValue = value; }));
				builder2.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime>>)(() => myValue));
				builder2.AddAttribute(4, nameof(HxInputDate<DateTime>.Enabled), true);
				builder2.CloseComponent();
			}));

			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		var buttons = cut.FindComponents<HxButton>();
		foreach (var button in buttons)
		{
			Assert.False(button.Find("button").HasAttribute("disabled"), $"Button {button.Instance.Text} should not be disabled.");
		}
		Assert.DoesNotContain("disabled", cut.Markup);
	}

	[Fact]
	public void HxInputDate_TypeValidDate_UpdatesBoundValue()
	{
		// Arrange
		DateTime? myValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime?>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime?>>)(() => myValue));
			builder.CloseComponent();
		};

		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			var cut = Render(componentRenderer);

			// Act
			cut.Find("input").Change("2/10/2020");
		}

		// Assert
		Assert.Equal(new DateTime(2020, 2, 10), myValue);
	}

	[Fact]
	public void HxInputDate_TypeInvalidDate_ShowsValidationError()
	{
		// Arrange
		DateTime? myValue = null;

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime?>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime?>>)(() => myValue));
			builder.AddAttribute(4, "ParsingErrorMessage", "TestParsingErrorMessage");
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		cut.Find("input").Change("not-a-date");

		// Assert
		Assert.Null(myValue);
		Assert.True(cut.Find("input").ClassList.Contains(HxInputBase<object>.InvalidCssClass), "Input element should have the invalid CSS class.");
		Assert.Equal("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent);
	}

	[Fact]
	public void HxInputDate_ClearInput_ResetsValueToNull()
	{
		// Arrange
		DateTime? myValue = new DateTime(2020, 2, 10);

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime?>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime?>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime?>>)(() => myValue));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);

		// Act
		cut.Find("input").Change("");

		// Assert
		Assert.Null(myValue);
	}

	[Fact]
	public void HxInputDate_NonNullable_EmptyInputShouldRaiseParsingError_Issue892()
	{
		// https://github.com/havit/Havit.Blazor/issues/892

		// Arrange
		var myValue = new DateTime(2020, 2, 10);

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDate<DateTime>>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTime>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTime>>)(() => myValue));
			builder.AddAttribute(4, "ParsingErrorMessage", "TestParsingErrorMessage");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		cut.Find("input").Change("");

		// Assert
		Assert.Equal(new DateTime(2020, 2, 10), myValue);
		Assert.Equal("", cut.Find("input").GetAttribute("value"));
		Assert.NotNull(cut.Find($"div.{HxInputBase<object>.InvalidCssClass}"));
		Assert.Equal("TestParsingErrorMessage", cut.Find("div.invalid-feedback").TextContent);
	}

	[Fact]
	public async Task HxInputDate_CalendarNavigation_IsKeptOnParentRerender()
	{
		// Arrange
		DateTime? myValue = new DateTime(2020, 2, 10);

		var cut = Render<HxInputDate<DateTime?>>(parameters => parameters
			.Add(p => p.Value, myValue)
			.Add(p => p.ValueChanged, (DateTime? value) => { myValue = value; })
			.Add(p => p.ValueExpression, () => myValue));

		// Act
		await cut.InvokeAsync(() => cut.FindAll("button.hx-calendar-navigation-button")[1].Click()); // next month
		cut.Render(parameters => parameters.Add(p => p.Value, myValue));

		// Assert
		Assert.Equal("2", cut.Find("select[aria-label='Month']").GetAttribute("value")); // March
		Assert.Equal("2020", cut.Find("select[aria-label='Year']").GetAttribute("value"));
	}

	[Fact]
	public void HxInputDate_ClearValue_CalendarShowsCalendarDisplayMonth()
	{
		// Arrange
		DateTime? myValue = new DateTime(2020, 2, 10);

		var cut = Render<HxInputDate<DateTime?>>(parameters => parameters
			.Add(p => p.Value, myValue)
			.Add(p => p.ValueChanged, (DateTime? value) => { myValue = value; })
			.Add(p => p.ValueExpression, () => myValue)
			.Add(p => p.CalendarDisplayMonth, new DateTime(2019, 6, 1)));

		// Act
		cut.Render(parameters => parameters.Add(p => p.Value, null));

		// Assert
		Assert.Equal("5", cut.Find("select[aria-label='Month']").GetAttribute("value")); // June
		Assert.Equal("2019", cut.Find("select[aria-label='Year']").GetAttribute("value"));
	}

	[Fact]
	public async Task HxInputDate_MinDateChangedAfterNavigation_CalendarShowsMinDateMonth()
	{
		// Arrange
		DateTime? myValue = null;
		var minDate = DateTime.Today.AddYears(2);

		var cut = Render<HxInputDate<DateTime?>>(parameters => parameters
			.Add(p => p.Value, myValue)
			.Add(p => p.ValueChanged, (DateTime? value) => { myValue = value; })
			.Add(p => p.ValueExpression, () => myValue));

		await cut.InvokeAsync(() => cut.FindAll("button.hx-calendar-navigation-button")[1].Click()); // next month

		// Act
		cut.Render(parameters => parameters.Add(p => p.MinDate, minDate));

		// Assert
		Assert.Equal((minDate.Month - 1).ToString(), cut.Find("select[aria-label='Month']").GetAttribute("value"));
		Assert.Equal(minDate.Year.ToString(), cut.Find("select[aria-label='Year']").GetAttribute("value"));
	}

	[Fact]
	public async Task HxInputDate_MaxDateChangedAfterNavigation_CalendarShowsMaxDateMonth()
	{
		// Arrange
		DateTime? myValue = null;
		var maxDate = DateTime.Today.AddYears(-2);

		var cut = Render<HxInputDate<DateTime?>>(parameters => parameters
			.Add(p => p.Value, myValue)
			.Add(p => p.ValueChanged, (DateTime? value) => { myValue = value; })
			.Add(p => p.ValueExpression, () => myValue));

		await cut.InvokeAsync(() => cut.FindAll("button.hx-calendar-navigation-button")[0].Click()); // previous month

		// Act
		cut.Render(parameters => parameters.Add(p => p.MaxDate, maxDate));

		// Assert
		Assert.Equal((maxDate.Month - 1).ToString(), cut.Find("select[aria-label='Month']").GetAttribute("value"));
		Assert.Equal(maxDate.Year.ToString(), cut.Find("select[aria-label='Year']").GetAttribute("value"));
	}

	[Fact]
	public async Task HxInputDate_CalendarNavigation_IsResetWhenDropdownHidden()
	{
		// Arrange
		DateTime? myValue = new DateTime(2020, 2, 10);

		var cut = Render<HxInputDate<DateTime?>>(parameters => parameters
			.Add(p => p.Value, myValue)
			.Add(p => p.ValueChanged, (DateTime? value) => { myValue = value; })
			.Add(p => p.ValueExpression, () => myValue));

		await cut.InvokeAsync(() => cut.FindAll("button.hx-calendar-navigation-button")[1].Click()); // next month
		Assert.Equal("2", cut.Find("select[aria-label='Month']").GetAttribute("value")); // March

		// Act
		var dropdownToggleElement = cut.FindComponent<HxDropdownToggleElement>().Instance;
		await cut.InvokeAsync(dropdownToggleElement.HandleJsHidden);

		// Assert
		Assert.Equal("1", cut.Find("select[aria-label='Month']").GetAttribute("value")); // February
	}
}
