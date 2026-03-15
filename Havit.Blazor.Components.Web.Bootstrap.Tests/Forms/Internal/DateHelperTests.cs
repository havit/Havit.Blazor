using System.Globalization;
using Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;
using Moq;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms.Internal;

/// <summary>
/// Cultures and their short date formats:
/// <ul>
///		<li>cs-CZ: dd.MM.yyyy</li>
///		<li>en-GB: dd/MM/yyyy</li>
///		<li>en-US: M/d/yyyy</li>
///		<li>ko-KR: yyyy.M.d.</li>
/// </ul>
/// </summary>
[TestClass]
public class DateHelperTests
{
	[TestMethod]
	public void DateHelper_TryParseDateFromString_DayMonthYear_Relaxed()
	{
		// Arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", "10.02.1980", expectedResult: true, expectedParsedDate: new DateTime(1980, 02, 10));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "   20. 3. 2020  ", expectedResult: true, expectedParsedDate: new DateTime(2020, 03, 20));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "05,06,2020", expectedResult: true, expectedParsedDate: new DateTime(2020, 06, 05));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "05 06 1980", expectedResult: true, expectedParsedDate: new DateTime(1980, 06, 05));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "31 02 2000", expectedResult: false, expectedParsedDate: default);

		fixture.ExecuteTest<DateTime?>("en-GB", "10/02/1980", expectedResult: true, expectedParsedDate: new DateTime(1980, 02, 10));
		fixture.ExecuteTest<DateTime?>("en-GB", "   20/ 3/ 2020  ", expectedResult: true, expectedParsedDate: new DateTime(2020, 3, 20));
		fixture.ExecuteTest<DateTime?>("en-GB", "05,06,2020", expectedResult: true, expectedParsedDate: new DateTime(2020, 06, 05));
		fixture.ExecuteTest<DateTime?>("en-GB", "05 06 1980", expectedResult: true, expectedParsedDate: new DateTime(1980, 06, 05));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "31 02 2000", expectedResult: false, expectedParsedDate: default);

		fixture.ExecuteTest<DateTime?>("en-US", "02/10/1980", expectedResult: true, expectedParsedDate: new DateTime(1980, 02, 10));
		fixture.ExecuteTest<DateTime?>("en-US", "   3/ 20/ 2020  ", expectedResult: true, expectedParsedDate: new DateTime(2020, 3, 20));
		fixture.ExecuteTest<DateTime?>("en-US", "06,05,2020", expectedResult: true, expectedParsedDate: new DateTime(2020, 06, 05));
		fixture.ExecuteTest<DateTime?>("en-US", "06 05 1980", expectedResult: true, expectedParsedDate: new DateTime(1980, 06, 05));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "02 31 2000", expectedResult: false, expectedParsedDate: default);

		fixture.ExecuteTest<DateTime?>("ko-KR", "1980.02.10", expectedResult: true, expectedParsedDate: new DateTime(1980, 2, 10));
		fixture.ExecuteTest<DateTime?>("ko-KR", "   2020 . 03. 10  ", expectedResult: true, expectedParsedDate: new DateTime(2020, 3, 10));
		fixture.ExecuteTest<DateTime?>("ko-KR", "2020,06,05", expectedResult: true, expectedParsedDate: new DateTime(2020, 06, 05));
		fixture.ExecuteTest<DateTime?>("ko-KR", "1980 06 05", expectedResult: true, expectedParsedDate: new DateTime(1980, 06, 05));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "2000 02 31", expectedResult: false, expectedParsedDate: default);
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_DayMonth_Strict()
	{
		// Arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", "0708", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", "0708", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", "0807", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", "0807", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));

		fixture.ExecuteTest<DateTime?>("cs-CZ", "078", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-GB", "078", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-US", "087", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("ko-KR", "087", expectedResult: false, expectedParsedDate: default);
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_DayMonth_Relaxed()
	{
		// Arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", "07.08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("cs-CZ", " 7. 8. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "07,08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "  7, 8, ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "07 08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "07-08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "07-08-", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));

		fixture.ExecuteTest<DateTime?>("en-GB", "07.08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", " 7. 8. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", "07,08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", " 7, 8 ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", "07 08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", "07-08", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", "07-08-", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));

		fixture.ExecuteTest<DateTime?>("en-US", "08.07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", " 8. 7. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", "08,07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", " 8, 7, ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", "08 07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", "08-07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", "08-07-", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));

		fixture.ExecuteTest<DateTime?>("ko-KR", "08.07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", " 8. 7. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", "08,07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", " 8, 7, ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", "08 07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", "08-07", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", "08-07-", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, 08, 07));
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_DayMonthYear_Strict()
	{
		// Arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", "070824", expectedResult: true, expectedParsedDate: new DateTime(2024, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-GB", "070824", expectedResult: true, expectedParsedDate: new DateTime(2024, 08, 07));
		fixture.ExecuteTest<DateTime?>("en-US", "080724", expectedResult: true, expectedParsedDate: new DateTime(2024, 08, 07));
		fixture.ExecuteTest<DateTime?>("ko-KR", "240807", expectedResult: true, expectedParsedDate: new DateTime(2024, 08, 07));

		fixture.ExecuteTest<DateTime?>("cs-CZ", "7824", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-GB", "7824", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-US", "8724", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("ko-KR", "2487", expectedResult: false, expectedParsedDate: default);
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_Day_Strict()
	{
		// arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", "15", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 15));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "01", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 1));
		fixture.ExecuteTest<DateTime?>("cs-CZ", "4", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 4));

		fixture.ExecuteTest<DateTime?>("en-GB", "15", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 15));
		fixture.ExecuteTest<DateTime?>("en-GB", "4", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 4));

		fixture.ExecuteTest<DateTime?>("en-US", "15", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 15));
		fixture.ExecuteTest<DateTime?>("en-US", "4", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 4));

		fixture.ExecuteTest<DateTime?>("ko-KR", "15", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 15));
		fixture.ExecuteTest<DateTime?>("ko-KR", "4", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 4));
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_Day_Relaxed()
	{
		// arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", " 03. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("cs-CZ", " 3, ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("cs-CZ", " -03- ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));

		fixture.ExecuteTest<DateTime?>("en-GB", " 03. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("en-GB", " 3, ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("en-GB", " -03- ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));

		fixture.ExecuteTest<DateTime?>("en-US", " 03. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("en-US", " 3, ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("en-US", " -03- ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));

		fixture.ExecuteTest<DateTime?>("ko-KR", " 03. ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("ko-KR", " 3, ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
		fixture.ExecuteTest<DateTime?>("ko-KR", " -03- ", expectedResult: true, expectedParsedDate: new DateTime(fixture.CurrentYear, fixture.CurrentMonth, 3));
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_DateTimeNullable_ShouldAcceptEmptyInputAsNull()
	{
		// arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", String.Empty, expectedResult: true, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("cs-CZ", " ", expectedResult: true, expectedParsedDate: default);
	}

	[TestMethod]
	public void DateHelper_TryParseDateFromString_DateTimeNonNullable_ShouldNotAcceptEmptyInput()
	{
		// arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime>("cs-CZ", String.Empty, expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime>("cs-CZ", " ", expectedResult: false, expectedParsedDate: default);
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

	[TestMethod]
	public void DateHelper_TryParseDateFromString_UnsupportedPatterns()
	{
		// Arrange
		var fixture = new Fixture();

		// Act + Assert
		fixture.ExecuteTest<DateTime?>("cs-CZ", "002/07", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("cs-CZ", "5a5a2025", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("cs-CZ", "5a5a20", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("cs-CZ", "5a5", expectedResult: false, expectedParsedDate: default);

		fixture.ExecuteTest<DateTime?>("en-GB", "002/07", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-GB", "5a5a2025", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-GB", "5a5a20", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-GB", "5a5", expectedResult: false, expectedParsedDate: default);

		fixture.ExecuteTest<DateTime?>("en-US", "002/07", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-US", "2025a5a5", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-US", "20a5a5", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("en-US", "5a5", expectedResult: false, expectedParsedDate: default);

		fixture.ExecuteTest<DateTime?>("ko-KR", "002/07", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("ko-KR", "2025a5a5", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("ko-KR", "20a5a5", expectedResult: false, expectedParsedDate: default);
		fixture.ExecuteTest<DateTime?>("ko-KR", "5a5", expectedResult: false, expectedParsedDate: default);
	}

	private class Fixture
	{
		public int CurrentYear { get; } = 2022;
		public int CurrentMonth { get; } = 8;
		public int CurrentDate { get; } = 7;

		private TimeProvider _timeProvider;

		public Fixture()
		{
			var timeProviderMock = new Mock<TimeProvider>(MockBehavior.Strict);
			timeProviderMock.CallBase = true;
			timeProviderMock.Setup(x => x.LocalTimeZone).Returns(TimeZoneInfo.Local);
			timeProviderMock.Setup(x => x.GetUtcNow()).Returns(new DateTimeOffset(new DateTime(CurrentYear, CurrentMonth, CurrentDate), TimeSpan.Zero));
			_timeProvider = timeProviderMock.Object;
		}

		public void ExecuteTest<TValue>(string culture, string input, bool expectedResult, TValue expectedParsedDate)
		{
			// Arrange
			bool result = default;
			TValue parsedDate = default;

			using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo(culture)))
			{
				// Act
				result = DateHelper.TryParseDateFromString<TValue>(input, _timeProvider, out parsedDate);
			}

			// Assert
			Assert.AreEqual(expectedResult, result, message: $"Culture {culture}, input '{input}'.");
			Assert.AreEqual(expectedParsedDate, parsedDate, message: $"Culture {culture}, input '{input}'.");

		}
	}
}
