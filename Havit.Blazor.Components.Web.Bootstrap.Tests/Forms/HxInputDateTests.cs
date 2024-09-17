using System.Globalization;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

[TestClass]
public class HxInputDateTests : BunitTestBase
{
	[TestMethod]
	public void HxInputDate_TryParseDateTimeOffsetFromString()
	{
		// Arrange
		CultureInfo culture = CultureInfo.GetCultureInfo("cs-CZ");

		DateTimeOffset? parsedDateTime;

		// Act + Assert

		// empty (valid)
		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(" ", culture, out parsedDateTime));
		Assert.AreEqual(null, parsedDateTime);

		// standard (valid)
		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("10.02.2020", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(2020, 2, 10), parsedDateTime);

		// whitespaces (valid)
		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("   20. 3. 2020  ", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(2020, 3, 20), parsedDateTime);

		// commas (valid)
		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("05,06,2020", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(2020, 6, 5), parsedDateTime);

		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("07,08", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(DateTime.Now.Year, 8, 7), parsedDateTime);

		// spaces (valid)
		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("05 06 2020", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(2020, 6, 5), parsedDateTime);

		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("07 08", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(DateTime.Now.Year, 8, 7), parsedDateTime);

		// shortcuts (valid)
		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("0203", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(DateTime.Now.Year, 3, 2), parsedDateTime);

		Assert.IsTrue(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("15", culture, out parsedDateTime));
		Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15), parsedDateTime);

		// time (invalid)
		Assert.IsFalse(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("10.02.2020 1:00", culture, out _));

		// shortcut, 13th month (invalid)
		Assert.IsFalse(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("0113", culture, out _));

		// shortcut, 32nd day (invalid)
		Assert.IsFalse(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("32", culture, out _));

		// time 0:00 unfortunately passes
		// Assert.IsFalse(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("10.02.2020 0:00", culture, out _));
	}

	[TestMethod]
	public void HxInputDate_EnabledShouldOverrideFormStateForNestedControls_Issue877()
	{
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
			Assert.IsFalse(button.Find("button").HasAttribute("disabled"), $"Button {button.Instance.Text} should not be disabled.");
		}
		Assert.IsFalse(cut.Markup.Contains("disabled"), "There should be no disabled attribute in the rendered markup.");
	}
}
