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
	public void HxInputDateRange_RequireFromLessOrEqualTo_ValidatesWhenFromIsGreaterThanTo()
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
			builder.AddAttribute(4, nameof(HxInputDateRange.RequireFromLessOrEqualTo), true);
			builder.CloseComponent();
		};

		// Act
		var cut = Render(componentRenderer);

		// Assert
		Assert.IsTrue(cut.Markup.Contains("is-invalid"), "Component should have validation error when from > to");
	}

	[TestMethod]
	public void HxInputDateRange_RequireFromLessOrEqualTo_PassesWhenFromIsLessThanOrEqualToTo()
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

		// Assert
		Assert.IsFalse(cut.Markup.Contains("is-invalid"), "Component should not have validation error when from <= to");
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
}
