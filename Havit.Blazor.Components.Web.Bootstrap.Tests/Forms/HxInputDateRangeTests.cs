using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

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
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
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
				// RequireFromLessOrEqualTo not set, should default to false
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Try to set invalid range - "from" after "to"
			inputs[0].Change("12/31/2024"); // After June 30, 2024 (using en-US MM/dd/yyyy format)

			// Assert - should allow because validation is not enabled by default
			Assert.AreEqual(new DateTime(2024, 12, 31), myValue.StartDate, "StartDate should update even though it's after EndDate (validation disabled by default)"); Assert.AreEqual(new DateTime(2024, 12, 31), myValue.StartDate, "StartDate should update even though it's after EndDate (validation disabled by default)");
			Assert.AreEqual(new DateTime(2024, 6, 30), myValue.EndDate, "EndDate should remain unchanged");
			Assert.DoesNotContain("is-invalid", cut.Markup, "Component should not have validation error when RequireFromLessOrEqualTo is not enabled");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_BlocksInvalidFromDate()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
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
			Assert.HasCount(2, inputs, "Should have two input fields (from and to)");

			// Try to set "from" date to be after "to" date
			inputs[0].Change("12/31/2024"); // December 31, 2024 - after June 30, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.AreEqual(new DateTime(2024, 1, 1), myValue.StartDate, "StartDate should not change when validation fails");
			Assert.AreEqual(new DateTime(2024, 6, 30), myValue.EndDate, "EndDate should remain unchanged");
			Assert.Contains("is-invalid", cut.Markup, "Component should show validation error");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_BlocksInvalidToDate()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
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
			Assert.HasCount(2, inputs, "Should have two input fields (from and to)");

			// Try to set "to" date to be before "from" date
			inputs[1].Change("1/1/2024"); // January 1, 2024 - before June 1, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.AreEqual(new DateTime(2024, 6, 1), myValue.StartDate, "StartDate should remain unchanged");
			Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should not change when validation fails");
			Assert.Contains("is-invalid", cut.Markup, "Component should show validation error");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsValidFromDate()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
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
			inputs[0].Change("6/1/2024"); // June 1, 2024 - before December 31, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.AreEqual(new DateTime(2024, 6, 1), myValue.StartDate, "StartDate should update to valid date");
			Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should remain unchanged");
			Assert.DoesNotContain("is-invalid", cut.Markup, "Component should not show validation error");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsValidToDate()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
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
			inputs[1].Change("12/31/2024"); // December 31, 2024 - after January 1, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.AreEqual(new DateTime(2024, 1, 1), myValue.StartDate, "StartDate should remain unchanged");
			Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should update to valid date");
			Assert.DoesNotContain("is-invalid", cut.Markup, "Component should not show validation error");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsEqualDates()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
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
			inputs[0].Change("12/31/2024"); // Same as end date (using en-US MM/dd/yyyy format)

			// Assert
			Assert.AreEqual(new DateTime(2024, 12, 31), myValue.StartDate, "StartDate should update to equal EndDate");
			Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should remain unchanged");
			Assert.DoesNotContain("is-invalid", cut.Markup, "Component should not show validation error when dates are equal");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsWhenOnlyFromDateSet()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
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
			inputs[0].Change("6/1/2024"); // Using en-US MM/dd/yyyy format

			// Assert
			Assert.AreEqual(new DateTime(2024, 6, 1), myValue.StartDate, "StartDate should update when EndDate is null");
			Assert.IsNull(myValue.EndDate, "EndDate should remain null");
			Assert.DoesNotContain("is-invalid", cut.Markup, "Component should not show validation error when only from date is set");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsWhenOnlyToDateSet()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var myValue = new DateTimeRange
			{
				StartDate = null,
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

			// Change "to" date when "from" is null - should not validate
			inputs[1].Change("6/30/2024"); // Using en-US MM/dd/yyyy format

			// Assert
			Assert.IsNull(myValue.StartDate, "StartDate should remain null");
			Assert.AreEqual(new DateTime(2024, 6, 30), myValue.EndDate, "EndDate should update when StartDate is null");
			Assert.DoesNotContain("is-invalid", cut.Markup, "Component should not show validation error when only to date is set");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsWhenBothDatesNull()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var myValue = new DateTimeRange
			{
				StartDate = null,
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

			// Set "from" date when both are null - should not validate
			inputs[0].Change("6/1/2024"); // Using en-US MM/dd/yyyy format

			// Assert
			Assert.AreEqual(new DateTime(2024, 6, 1), myValue.StartDate, "StartDate should update");
			Assert.IsNull(myValue.EndDate, "EndDate should remain null");
			Assert.DoesNotContain("is-invalid", cut.Markup, "Component should not show validation error when both dates were null");
		}
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_AllowsChangingToNullNull()
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

		// Clear "from" date
		inputs[0].Change("");

		// Assert
		Assert.IsNull(myValue.StartDate, "StartDate should be null after clearing");
		Assert.AreEqual(new DateTime(2024, 12, 31), myValue.EndDate, "EndDate should remain unchanged after clearing StartDate");
	}
}
