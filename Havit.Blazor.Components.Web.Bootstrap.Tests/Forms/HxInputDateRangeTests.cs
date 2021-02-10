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
	public class HxInputDateRangeTests
	{
		[TestMethod]
		public void HxInputDateRange_TryParseValueFromString_StartDateEndDatePattern()
		{
			// Arrange
			CultureInfo culture = CultureInfo.GetCultureInfo("cs-CZ");
			DateTimeRange parsedValue;

			string startDateText = "od";
			string endDateText = "do";

			// Act + Assert
			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_StartDateEndDatePattern("1.1.2019-31.12.2020", culture, startDateText, endDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = new DateTime(2019, 1, 1), EndDate = new DateTime(2020, 12, 31) }, parsedValue);

			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_StartDateEndDatePattern("  31.12.2020 - 1.1.2019   ", culture, startDateText, endDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = new DateTime(2019, 1, 1), EndDate = new DateTime(2020, 12, 31) }, parsedValue);

			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_StartDateEndDatePattern("od 1.1.2019 do 31.12.2020", culture, startDateText, endDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = new DateTime(2019, 1, 1), EndDate = new DateTime(2020, 12, 31) }, parsedValue);

			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_StartDateEndDatePattern("   od    31.12.2020  do  1.1.2019   ", culture, startDateText, endDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = new DateTime(2019, 1, 1), EndDate = new DateTime(2020, 12, 31) }, parsedValue);

			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_StartDateEndDatePattern("od31.12.2020do1.1.2019", culture, startDateText, endDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = new DateTime(2019, 1, 1), EndDate = new DateTime(2020, 12, 31) }, parsedValue);

			Assert.IsFalse(HxInputDateRange.TryParseValueFromString_StartDateEndDatePattern("- -", culture, startDateText, endDateText, out _));
		}

		[TestMethod]
		public void HxInputDateRange_TryParseValueFromString_StartDatePattern()
		{
			// Arrange
			CultureInfo culture = CultureInfo.GetCultureInfo("cs-CZ");
			DateTimeRange parsedValue;

			string startDateText = "od";

			// Act + Assert
			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_StartDatePattern("od 1.1.2019", culture, startDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = new DateTime(2019, 1, 1), EndDate = null }, parsedValue);

			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_StartDatePattern("1.1.2019-", culture, startDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = new DateTime(2019, 1, 1), EndDate = null }, parsedValue);

			Assert.IsFalse(HxInputDateRange.TryParseValueFromString_StartDatePattern("-", culture, startDateText, out _));
		}

		[TestMethod]
		public void HxInputDateRange_TryParseValueFromString_EndDatePattern()
		{
			// Arrange
			CultureInfo culture = CultureInfo.GetCultureInfo("cs-CZ");
			DateTimeRange parsedValue;

			string endDateText = "do";

			// Act + Assert
			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_EndDatePattern("do 31.12.2020", culture, endDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = null, EndDate = new DateTime(2020, 12, 31) }, parsedValue);

			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_EndDatePattern("-31.12.2020", culture, endDateText, out parsedValue));
			Assert.AreEqual(new DateTimeRange { StartDate = null, EndDate = new DateTime(2020, 12, 31) }, parsedValue);

			Assert.IsFalse(HxInputDateRange.TryParseValueFromString_EndDatePattern("-", culture, endDateText, out _));
		}

		[TestMethod]
		public void HxInputDateRange_TryParseValueFromString_NoDatePattern()
		{
			// Arrange
			CultureInfo culture = CultureInfo.GetCultureInfo("cs-CZ");
			DateTimeRange parsedValue;

			// Act + Assert
			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_NoDatePattern("", out parsedValue));
			Assert.AreEqual(default, parsedValue);

			Assert.IsTrue(HxInputDateRange.TryParseValueFromString_NoDatePattern("-", out parsedValue));
			Assert.AreEqual(default, parsedValue);

			Assert.IsFalse(HxInputDateRange.TryParseValueFromString_NoDatePattern("- -", out _));
		}
	}
}
