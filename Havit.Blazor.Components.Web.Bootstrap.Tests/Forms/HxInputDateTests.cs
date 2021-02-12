using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms
{
	[TestClass]
	public class HxInputDateTests
	{
		[TestMethod]
		public void HxInputDate_GetValueFromDateTimeOffset()
		{
			// Arrange
			DateTimeOffset dateTimeOffset = DateTimeOffset.Now.Date;

			// Act + Assert

			// DateTime
			Assert.AreEqual(dateTimeOffset.DateTime, HxInputDate<DateTime>.GetValueFromDateTimeOffset(dateTimeOffset));
			Assert.AreEqual(default(DateTime), HxInputDate<DateTime>.GetValueFromDateTimeOffset(null));

			// DateTime?
			Assert.AreEqual(dateTimeOffset.DateTime, HxInputDate<DateTime?>.GetValueFromDateTimeOffset(dateTimeOffset));
			Assert.AreEqual(null, HxInputDate<DateTime?>.GetValueFromDateTimeOffset(null));

			// DateTimeOffset
			Assert.AreEqual(dateTimeOffset, HxInputDate<DateTimeOffset>.GetValueFromDateTimeOffset(dateTimeOffset));
			Assert.AreEqual(default(DateTimeOffset), HxInputDate<DateTimeOffset>.GetValueFromDateTimeOffset(null));

			// DateTimeOffset?
			Assert.AreEqual(dateTimeOffset, HxInputDate<DateTimeOffset?>.GetValueFromDateTimeOffset(dateTimeOffset));
			Assert.AreEqual(null, HxInputDate<DateTimeOffset?>.GetValueFromDateTimeOffset(null));
		}

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

			// čas 0:00 bohužel projde
			// Assert.IsFalse(HxInputDate<DateTime>.TryParseDateTimeOffsetFromString("10.02.2020 0:00", culture, out _));

		}

	}
}
