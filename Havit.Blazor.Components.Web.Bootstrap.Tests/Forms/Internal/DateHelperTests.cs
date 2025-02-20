using System.Globalization;
using Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms.Internal;

[TestClass]
public class DateHelperTests
{
	[TestMethod]
	public void DateHelper_TryParseDateFromString_FullDate_CZ()
	{
		// arrange
		var fixture = new Fixture().WithCulture("cs-CZ");

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("10.02.2020", expectedResult: true, expectedParsedDate: new DateTime(2020, 02, 10));
		fixture.ExecuteTest<DateTime?>("   20. 3. 2020  ", expectedResult: true, expectedParsedDate: new DateTime(2020, 03, 20));
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_VariousDelimiters_CZ()
	{
		// arrange
		var fixture = new Fixture().WithCulture("cs-CZ");

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("05,06,2020", expectedResult: true, expectedParsedDate: new DateTime(2020, 06, 05));
		fixture.ExecuteTest<DateTime?>("05 06 2020", expectedResult: true, expectedParsedDate: new DateTime(2020, 06, 05));
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_ShortDayMonth_CZ()
	{
		// arrange
		var fixture = new Fixture().WithCulture("cs-CZ");

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("07.08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("07,08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("07 08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("0708", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		// TODO "078"
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_ShortDayMonthYear_CZ()
	{
		// arrange
		var fixture = new Fixture().WithCulture("cs-CZ");

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("070824", expectedResult: true, expectedParsedDate: new DateTime(2024, 08, 07));
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_ShortDayOnly_CZ()
	{
		// arrange
		var fixture = new Fixture().WithCulture("cs-CZ");

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("15", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 15));
		fixture.ExecuteTest<DateTime?>("01", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 1));
		// TODO "1"
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_DateTimeNullable_ShouldAcceptEmptyInputAsNull()
	{
		// arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>(String.Empty, expectedResult: true, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>(" ", expectedResult: true, expectedParsedDate: default);
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_DateTimeNonNullable_ShouldNotAcceptEmptyInput()
	{
		// arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime>(String.Empty, expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime>(" ", expectedResult: false, expectedParsedDate: default);
	}

	[TestMethod]
	public void DateHelper_GetValueFromDateTimeOffset()
	{
		// Arrange
		DateTimeOffset dateTimeOffset = DateTimeOffset.Now.Date;

		// Act + Assert

		// DateTime
		Assert.AreEqual(dateTimeOffset.DateTime, DateHelper.GetValueFromDateTimeOffset<DateTime>(dateTimeOffset));
		Assert.AreEqual(default(DateTime), DateHelper.GetValueFromDateTimeOffset<DateTime>(null));

		// DateTime?
		Assert.AreEqual(dateTimeOffset.DateTime, DateHelper.GetValueFromDateTimeOffset<DateTime?>(dateTimeOffset));
		Assert.IsNull(DateHelper.GetValueFromDateTimeOffset<DateTime?>(null));

		// DateTimeOffset
		Assert.AreEqual(dateTimeOffset, DateHelper.GetValueFromDateTimeOffset<DateTimeOffset>(dateTimeOffset));
		Assert.AreEqual(default(DateTimeOffset), DateHelper.GetValueFromDateTimeOffset<DateTimeOffset>(null));

		// DateTimeOffset?
		Assert.AreEqual(dateTimeOffset, DateHelper.GetValueFromDateTimeOffset<DateTimeOffset?>(dateTimeOffset));
		Assert.IsNull(DateHelper.GetValueFromDateTimeOffset<DateTimeOffset?>(null));
	}

	private class Fixture
	{
		public int CurrentYear { get; } = DateTime.Today.Year; // DateTime.Parse("08 07") uses DateTime.Now to supply the year :-(
		public int CurrentMonth { get; } = 8;
		public int CurrentDate { get; } = 7;

		private CultureInfo _cultureInfo;
		private TimeProvider _timeProvider;

		public Fixture()
		{
			_cultureInfo = CultureInfo.InvariantCulture;

			var timeProviderMock = new Mock<TimeProvider>(MockBehavior.Strict);
			timeProviderMock.CallBase = true;
			timeProviderMock.Setup(x => x.LocalTimeZone).Returns(TimeZoneInfo.Local);
			timeProviderMock.Setup(x => x.GetUtcNow()).Returns(new DateTimeOffset(new DateTime(CurrentYear, CurrentMonth, CurrentDate), TimeSpan.Zero));
			_timeProvider = timeProviderMock.Object;
		}

		public Fixture WithCulture(string culture)
		{
			_cultureInfo = CultureInfo.GetCultureInfo(culture);
			return this;
		}

		public void ExecuteTest<TValue>(string input, bool expectedResult, TValue expectedParsedDate)
		{
			var thread = new Thread(() =>
			{
				// Act
				bool result = DateHelper.TryParseDateFromString<TValue>(input, _timeProvider, out var parsedDate);

				// Assert
				Assert.AreEqual(expectedResult, result);
				Assert.AreEqual(expectedParsedDate, parsedDate);
			});

			thread.CurrentCulture = _cultureInfo;
			thread.CurrentUICulture = _cultureInfo;
			thread.Start();
			thread.Join();
		}
	}
}
