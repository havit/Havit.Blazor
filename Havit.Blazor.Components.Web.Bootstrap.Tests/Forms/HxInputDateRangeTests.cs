using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputDateRangeTests : BunitTestBase
{
	// https://github.com/havit/Havit.Blazor/issues/877
	[Fact]
	public void HxInputDateRange_EnabledShouldOverrideFormStateForNestedControls()
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
			Assert.False(button.Find("button").HasAttribute("disabled"), $"Button {button.Instance.Text} should not be disabled.");
		}
		Assert.DoesNotContain("disabled", cut.Markup);
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_DefaultIsTrue()
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
				// RequireDateOrder not set, should default to true
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Try to set invalid range - "from" after "to"
			inputs[0].Change("12/31/2024"); // After June 30, 2024 (using en-US MM/dd/yyyy format)

			// Assert - should NOT allow because validation is enabled by default
			Assert.Equal(new DateTime(2024, 1, 1), myValue.StartDate);
			Assert.Equal(new DateTime(2024, 6, 30), myValue.EndDate);
			Assert.Contains("is-invalid", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_BlocksInvalidFromDate()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");
			Assert.Equal(2, inputs.Count());

			// Try to set "from" date to be after "to" date
			inputs[0].Change("12/31/2024"); // December 31, 2024 - after June 30, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.Equal(new DateTime(2024, 1, 1), myValue.StartDate);
			Assert.Equal(new DateTime(2024, 6, 30), myValue.EndDate);
			Assert.Contains("is-invalid", cut.Markup);
			Assert.Contains("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_BlocksInvalidToDate()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");
			Assert.Equal(2, inputs.Count());

			// Try to set "to" date to be before "from" date
			inputs[1].Change("1/1/2024"); // January 1, 2024 - before June 1, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.Equal(new DateTime(2024, 6, 1), myValue.StartDate);
			Assert.Equal(new DateTime(2024, 12, 31), myValue.EndDate);
			Assert.Contains("is-invalid", cut.Markup);
			Assert.Contains("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_AllowsValidFromDate()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Set "from" date to a valid date (before "to" date)
			inputs[0].Change("6/1/2024"); // June 1, 2024 - before December 31, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.Equal(new DateTime(2024, 6, 1), myValue.StartDate);
			Assert.Equal(new DateTime(2024, 12, 31), myValue.EndDate);
			Assert.DoesNotContain("is-invalid", cut.Markup);
			Assert.DoesNotContain("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_AllowsValidToDate()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Set "to" date to a valid date (after "from" date)
			inputs[1].Change("12/31/2024"); // December 31, 2024 - after January 1, 2024 (using en-US MM/dd/yyyy format)

			// Assert
			Assert.Equal(new DateTime(2024, 1, 1), myValue.StartDate);
			Assert.Equal(new DateTime(2024, 12, 31), myValue.EndDate);
			Assert.DoesNotContain("is-invalid", cut.Markup);
			Assert.DoesNotContain("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_AllowsEqualDates()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Set "from" date to equal "to" date
			inputs[0].Change("12/31/2024"); // Same as end date (using en-US MM/dd/yyyy format)

			// Assert
			Assert.Equal(new DateTime(2024, 12, 31), myValue.StartDate);
			Assert.Equal(new DateTime(2024, 12, 31), myValue.EndDate);
			Assert.DoesNotContain("is-invalid", cut.Markup);
			Assert.DoesNotContain("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_AllowsWhenOnlyFromDateSet()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Change "from" date when "to" is null - should not validate
			inputs[0].Change("6/1/2024"); // Using en-US MM/dd/yyyy format

			// Assert
			Assert.Equal(new DateTime(2024, 6, 1), myValue.StartDate);
			Assert.Null(myValue.EndDate);
			Assert.DoesNotContain("is-invalid", cut.Markup);
			Assert.DoesNotContain("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_AllowsWhenOnlyToDateSet()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Change "to" date when "from" is null - should not validate
			inputs[1].Change("6/30/2024"); // Using en-US MM/dd/yyyy format

			// Assert
			Assert.Null(myValue.StartDate);
			Assert.Equal(new DateTime(2024, 6, 30), myValue.EndDate);
			Assert.DoesNotContain("is-invalid", cut.Markup);
			Assert.DoesNotContain("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_AllowsWhenBothDatesNull()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// Set "from" date when both are null - should not validate
			inputs[0].Change("6/1/2024"); // Using en-US MM/dd/yyyy format

			// Assert
			Assert.Equal(new DateTime(2024, 6, 1), myValue.StartDate);
			Assert.Null(myValue.EndDate);
			Assert.DoesNotContain("is-invalid", cut.Markup);
			Assert.DoesNotContain("TestDateOrderErrorMessage", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_AllowsChangingToNullNull()
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
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
			builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);
		var inputs = cut.FindAll("input");

		// Clear "from" date
		inputs[0].Change("");
		// Re-query inputs after first change to get fresh DOM references
		inputs = cut.FindAll("input");
		// Clear "to" date
		inputs[1].Change("");

		// Assert
		Assert.Null(myValue.StartDate);
		Assert.Null(myValue.EndDate);
		Assert.DoesNotContain("is-invalid", cut.Markup);
		Assert.DoesNotContain("TestDateOrderErrorMessage", cut.Markup);
	}

	[Fact]
	public void HxInputDateRange_RequireDateOrder_DoesNotDuplicateErrorMessageOnMultipleInvalidChanges()
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
				builder.AddAttribute(4, nameof(HxInputDateRange.RequireDateOrder), true);
				builder.AddAttribute(5, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			// First invalid change - "from" after "to"
			inputs[0].Change("12/31/2024"); // December 31, 2024 - after June 30, 2024 (using en-US MM/dd/yyyy format)

			// Re-query inputs after first change to get fresh DOM references
			inputs = cut.FindAll("input");

			// Second invalid change - another "from" after "to"
			inputs[0].Change("11/30/2024"); // November 30, 2024 - still after June 30, 2024 (using en-US MM/dd/yyyy format)

			// Assert - Count validation message elements (span elements containing the error message)
			// Validation message: <span title="TestDateOrderErrorMessage">TestDateOrderErrorMessage</span>
			// Each validation message is rendered as a single <span> element, so we count those
			int errorSpanCount = Regex.Matches(cut.Markup, @"<span[^>]*>TestDateOrderErrorMessage</span>").Count;
			Assert.Equal(1, errorSpanCount);
			Assert.Contains("is-invalid", cut.Markup);
		}
	}

	[Fact]
	public void HxInputDateRange_MultipleInstances_FromValidationError_DoesNotShowOnOtherInstances()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var formModel = new FormModel();

			RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
			{
				builder.OpenComponent<EditForm>(0);
				builder.AddAttribute(1, "Model", formModel);
				builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)((context) => (builder2) =>
				{
					// First HxInputDateRange instance
					builder2.OpenComponent<HxInputDateRange>(0);
					builder2.AddAttribute(1, "Value", formModel.A.Range);
					builder2.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { formModel.A.Range = value; }));
					builder2.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => formModel.A.Range));
					builder2.AddAttribute(4, nameof(HxInputDateRange.Label), "Range A");
					builder2.AddAttribute(5, nameof(HxInputDateRange.FromParsingErrorMessage), "TestFromParsingErrorMessage");
					builder2.CloseComponent();

					// Second HxInputDateRange instance
					builder2.OpenComponent<HxInputDateRange>(10);
					builder2.AddAttribute(11, "Value", formModel.B.Range);
					builder2.AddAttribute(12, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { formModel.B.Range = value; }));
					builder2.AddAttribute(13, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => formModel.B.Range));
					builder2.AddAttribute(14, nameof(HxInputDateRange.Label), "Range B");
					builder2.AddAttribute(5, nameof(HxInputDateRange.FromParsingErrorMessage), "TestFromParsingErrorMessage");
					builder2.CloseComponent();
				}));
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");
			Assert.Equal(4, inputs.Count());

			// Trigger validation error on first range's From field by entering invalid date
			inputs[0].Change("invalid");

			// Assert
			// Count occurrences of the From field validation error message
			// The error message should only appear once, under the first range
			int fromErrorCount = Regex.Matches(cut.Markup, @"<span[^>]*>TestFromParsingErrorMessage</span>").Count;
			Assert.Equal(1, fromErrorCount);

			// Verify the error is NOT shown for Range B => (the single occurence found above is for the Range A)
			var rangeBContainer = cut.FindAll(".hx-input-date-range").Skip(1).SingleOrDefault();
			Assert.NotNull(rangeBContainer);
			Assert.DoesNotContain("TestFromParsingErrorMessage", rangeBContainer.InnerHtml);
		}
	}

	[Fact]
	public void HxInputDateRange_MultipleInstances_ToValidationError_DoesNotShowOnOtherInstances()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var formModel = new FormModel();

			RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
			{
				builder.OpenComponent<EditForm>(0);
				builder.AddAttribute(1, "Model", formModel);
				builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)((context) => (builder2) =>
				{
					// First HxInputDateRange instance
					builder2.OpenComponent<HxInputDateRange>(0);
					builder2.AddAttribute(1, "Value", formModel.A.Range);
					builder2.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { formModel.A.Range = value; }));
					builder2.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => formModel.A.Range));
					builder2.AddAttribute(4, nameof(HxInputDateRange.Label), "Range A");
					builder2.AddAttribute(5, nameof(HxInputDateRange.ToParsingErrorMessage), "TestToParsingErrorMessage");
					builder2.CloseComponent();

					// Second HxInputDateRange instance
					builder2.OpenComponent<HxInputDateRange>(10);
					builder2.AddAttribute(11, "Value", formModel.B.Range);
					builder2.AddAttribute(12, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { formModel.B.Range = value; }));
					builder2.AddAttribute(13, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => formModel.B.Range));
					builder2.AddAttribute(14, nameof(HxInputDateRange.Label), "Range B");
					builder2.AddAttribute(5, nameof(HxInputDateRange.ToParsingErrorMessage), "TestToParsingErrorMessage");
					builder2.CloseComponent();
				}));
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");
			Assert.Equal(4, inputs.Count());

			// Trigger validation error on first range's To field by entering invalid date
			inputs[1].Change("invalid");

			// Assert
			// Count occurrences of the To field validation error message
			// The error message should only appear once, under the first range
			int toErrorCount = Regex.Matches(cut.Markup, @"<span[^>]*>TestToParsingErrorMessage</span>").Count;
			Assert.Equal(1, toErrorCount);

			// Verify the error is NOT shown for Range B => (the single occurence found above is for the Range A)
			var rangeBContainer = cut.FindAll(".hx-input-date-range").Skip(1).SingleOrDefault();
			Assert.NotNull(rangeBContainer);
			Assert.DoesNotContain("TestToParsingErrorMessage", rangeBContainer.InnerHtml);
		}
	}

	[Fact]
	public void HxInputDateRange_MultipleInstances_DateRangeValidationError_DoesNotShowOnOtherInstances()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var formModel = new FormModel
			{
				A = new MyRange { Range = new DateTimeRange { StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2024, 6, 30) } },
				B = new MyRange { Range = new DateTimeRange { StartDate = new DateTime(2024, 7, 1), EndDate = new DateTime(2024, 12, 31) } }
			};

			RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
			{
				builder.OpenComponent<EditForm>(0);
				builder.AddAttribute(1, "Model", formModel);
				builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)((context) => (builder2) =>
				{
					// First HxInputDateRange instance
					builder2.OpenComponent<HxInputDateRange>(0);
					builder2.AddAttribute(1, "Value", formModel.A.Range);
					builder2.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { formModel.A.Range = value; }));
					builder2.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => formModel.A.Range));
					builder2.AddAttribute(4, nameof(HxInputDateRange.Label), "Range A");
					builder2.AddAttribute(5, nameof(HxInputDateRange.RequireDateOrder), true);
					builder2.AddAttribute(6, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
					builder2.CloseComponent();

					// Second HxInputDateRange instance
					builder2.OpenComponent<HxInputDateRange>(10);
					builder2.AddAttribute(11, "Value", formModel.B.Range);
					builder2.AddAttribute(12, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { formModel.B.Range = value; }));
					builder2.AddAttribute(13, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => formModel.B.Range));
					builder2.AddAttribute(14, nameof(HxInputDateRange.Label), "Range B");
					builder2.AddAttribute(15, nameof(HxInputDateRange.RequireDateOrder), true);
					builder2.AddAttribute(16, nameof(HxInputDateRange.DateOrderErrorMessage), "TestDateOrderErrorMessage");
					builder2.CloseComponent();
				}));
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");
			Assert.Equal(4, inputs.Count());

			// Trigger date order validation error on first range by setting From after To
			inputs[0].Change("12/31/2024"); // December 31, 2024 - after June 30, 2024

			// Assert
			// Count occurrences of the date order validation error message
			// The error message should only appear once, under the first range
			int dateOrderErrorCount = Regex.Matches(cut.Markup, @"<span[^>]*>TestDateOrderErrorMessage</span>").Count;
			Assert.Equal(1, dateOrderErrorCount);

			// Verify the error is NOT shown for Range B => (the single occurence found above is for the Range A)
			var rangeBContainer = cut.FindAll(".hx-input-date-range").Skip(1).SingleOrDefault();
			Assert.NotNull(rangeBContainer);
			Assert.DoesNotContain("TestDateOrderErrorMessage", rangeBContainer.InnerHtml);
		}
	}

	[Fact]
	public void HxInputDateRange_Render_ShowsFromAndToInputs()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var myValue = new DateTimeRange();

			RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
			{
				builder.OpenComponent<HxInputDateRange>(0);
				builder.AddAttribute(1, "Value", myValue);
				builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
				builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);

			// Assert
			var inputs = cut.FindAll("input");
			Assert.Equal(2, inputs.Count());
			Assert.Equal("From", inputs[0].GetAttribute("placeholder"));
			Assert.Equal("To", inputs[1].GetAttribute("placeholder"));
		}
	}

	[Fact]
	public void HxInputDateRange_TypeValidRange_UpdatesBoundValue()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange
			var myValue = new DateTimeRange();

			RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
			{
				builder.OpenComponent<HxInputDateRange>(0);
				builder.AddAttribute(1, "Value", myValue);
				builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<DateTimeRange>(this, (value) => { myValue = value; }));
				builder.AddAttribute(3, "ValueExpression", (Expression<Func<DateTimeRange>>)(() => myValue));
				builder.CloseComponent();
			};

			// Act
			var cut = Render(componentRenderer);
			var inputs = cut.FindAll("input");

			inputs[0].Change("1/1/2025"); // From date (using en-US MM/dd/yyyy format)

			inputs = cut.FindAll("input"); // Re-query after re-render
			inputs[1].Change("6/30/2025"); // To date (using en-US MM/dd/yyyy format)

			// Assert
			Assert.Equal(new DateTime(2025, 1, 1), myValue.StartDate);
			Assert.Equal(new DateTime(2025, 6, 30), myValue.EndDate);
		}
	}

	public record class FormModel
	{
		public MyRange A { get; set; } = new MyRange();
		public MyRange B { get; set; } = new MyRange();
	}

	public class MyRange
	{
		public DateTimeRange Range { get; set; }
	}
}
