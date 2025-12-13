using System.Globalization;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputDateRangeTests : BunitTestBase
{
	[TestMethod]
	public void HxInputDateRange_EnabledShouldOverrideFormStateForNestedControls_Issue877()
	{
		// Arrange
		var myValue = new DateTimeRange();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxFormState>(0);
			builder.AddAttribute(1, nameof(HxFormState.Enabled), false);

			builder.AddAttribute(1, nameof(HxFormState.ChildContent), (RenderFragment)((builder2) =>
			{
				builder2.OpenComponent<HxInputDateRange>(0);
				builder2.AddAttribute(1, "Value", myValue);
				builder2.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
				builder2.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
				builder2.AddAttribute(4, nameof(HxInputDateRange.Enabled), true);
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
			Assert.IsFalse(button.Find("button").HasAttribute("disabled"), $"Button {button.Instance.Text} should not be disabled.");
		}
		Assert.DoesNotContain("disabled", cut.Markup, "There should be no disabled attribute in the rendered markup.");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_DefaultIsFalse()
	{
		// Arrange
		var myValue = new DateTimeRange
		{
			StartDate = new DateTime(2024, 12, 31),
			EndDate = new DateTime(2024, 1, 1)
		};

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDateRange>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
			// RequireFromLessOrEqualTo not set, should default to false
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		Assert.IsFalse(cut.Markup.Contains("is-invalid"), "Component should not have validation error when RequireFromLessOrEqualTo is not enabled");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_BlocksInvalidFromDate()
	{
		// Arrange
		var myValue = new DateTimeRange
		{
			StartDate = new DateTime(2024, 1, 1),
			EndDate = new DateTime(2024, 6, 30)
		};

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDateRange>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireFromLessOrEqualTo), true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		var inputs = cut.FindAll("input");
		Assert.AreEqual(2, inputs.Count, "Should have two input fields (from and to)");
		
		// Try to set "from" date to be after "to" date
		inputs[0].Change("12/31/2024"); // December 31, 2024 - after June 30, 2024

		// Assert
		Assert.AreEqual(new DateTime(2024, 1, 1), myValue.StartDate, "StartDate should not change when validation fails");
		Assert.AreEqual(new DateTime(2024, 6, 30), myValue.EndDate, "EndDate should remain unchanged");
		Assert.IsTrue(cut.Markup.Contains("is-invalid"), "Component should show validation error");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_BlocksInvalidToDate()
	{
		// Arrange
		var myValue = new DateTimeRange
		{
			StartDate = new DateTime(2024, 6, 1),
			EndDate = new DateTime(2024, 12, 31)
		};

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDateRange>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireFromLessOrEqualTo), true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		var inputs = cut.FindAll("input");
		Assert.AreEqual(2, inputs.Count, "Should have two input fields (from and to)");
		
		// Try to set "to" date to be before "from" date
		inputs[1].Change("1/1/2024"); // January 1, 2024 - before June 1, 2024

		// Assert
		Assert.AreEqual(new DateTime(2024, 6, 1), myValue.StartDate, "StartDate should remain unchanged");
		Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should not change when validation fails");
		Assert.IsTrue(cut.Markup.Contains("is-invalid"), "Component should show validation error");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsValidFromDate()
	{
		// Arrange
		var myValue = new DateTimeRange
		{
			StartDate = new DateTime(2024, 1, 1),
			EndDate = new DateTime(2024, 12, 31)
		};

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDateRange>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireFromLessOrEqualTo), true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		var inputs = cut.FindAll("input");
		
		// Set "from" date to a valid date (before "to" date)
		inputs[0].Change("6/1/2024"); // June 1, 2024 - before December 31, 2024

		// Assert
		Assert.AreEqual(new DateTime(2024, 6, 1), myValue.StartDate, "StartDate should update to valid date");
		Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should remain unchanged");
		Assert.IsFalse(cut.Markup.Contains("is-invalid"), "Component should not show validation error");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsValidToDate()
	{
		// Arrange
		var myValue = new DateTimeRange
		{
			StartDate = new DateTime(2024, 1, 1),
			EndDate = new DateTime(2024, 6, 30)
		};

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDateRange>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireFromLessOrEqualTo), true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		var inputs = cut.FindAll("input");
		
		// Set "to" date to a valid date (after "from" date)
		inputs[1].Change("12/31/2024"); // December 31, 2024 - after January 1, 2024

		// Assert
		Assert.AreEqual(new DateTime(2024, 1, 1), myValue.StartDate, "StartDate should remain unchanged");
		Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should update to valid date");
		Assert.IsFalse(cut.Markup.Contains("is-invalid"), "Component should not show validation error");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsEqualDates()
	{
		// Arrange
		var myValue = new DateTimeRange
		{
			StartDate = new DateTime(2024, 1, 1),
			EndDate = new DateTime(2024, 12, 31)
		};

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDateRange>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireFromLessOrEqualTo), true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		var inputs = cut.FindAll("input");
		
		// Set "from" date to equal "to" date
		inputs[0].Change("12/31/2024"); // Same as end date

		// Assert
		Assert.AreEqual(new DateTime(2024, 12, 31), myValue.StartDate, "StartDate should update to equal EndDate");
		Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should remain unchanged");
		Assert.IsFalse(cut.Markup.Contains("is-invalid"), "Component should not show validation error when dates are equal");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsWhenOnlyOneDateSet()
	{
		// Arrange
		var myValue = new DateTimeRange
		{
			StartDate = new DateTime(2024, 1, 1),
			EndDate = null
		};

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxInputDateRange>(0);
			builder.AddAttribute(1, "Value", myValue);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireFromLessOrEqualTo), true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		var inputs = cut.FindAll("input");
		
		// Change "from" date when "to" is null - should not validate
		inputs[0].Change("6/1/2024");

		// Assert
		Assert.AreEqual(new DateTime(2024, 6, 1), myValue.StartDate, "StartDate should update when EndDate is null");
		Assert.IsNull(myValue.EndDate, "EndDate should remain null");
		Assert.IsFalse(cut.Markup.Contains("is-invalid"), "Component should not show validation error when only one date is set");
	}
}
