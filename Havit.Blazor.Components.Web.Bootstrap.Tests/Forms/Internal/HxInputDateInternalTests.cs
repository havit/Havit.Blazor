using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms.Internal;

[TestClass]
public class HxInputDateInternalTests
{
	[TestMethod]
	public void HxInputDateInternal_GetValueFromDateTimeOffset()
	{
		// Arrange
		DateTimeOffset dateTimeOffset = DateTimeOffset.Now.Date;

		// Act + Assert

		// DateTime
		Assert.AreEqual(dateTimeOffset.DateTime, HxInputDateInternal<DateTime>.GetValueFromDateTimeOffset(dateTimeOffset));
		Assert.AreEqual(default(DateTime), HxInputDateInternal<DateTime>.GetValueFromDateTimeOffset(null));

		// DateTime?
		Assert.AreEqual(dateTimeOffset.DateTime, HxInputDateInternal<DateTime?>.GetValueFromDateTimeOffset(dateTimeOffset));
		Assert.AreEqual(null, HxInputDateInternal<DateTime?>.GetValueFromDateTimeOffset(null));

		// DateTimeOffset
		Assert.AreEqual(dateTimeOffset, HxInputDateInternal<DateTimeOffset>.GetValueFromDateTimeOffset(dateTimeOffset));
		Assert.AreEqual(default(DateTimeOffset), HxInputDateInternal<DateTimeOffset>.GetValueFromDateTimeOffset(null));

		// DateTimeOffset?
		Assert.AreEqual(dateTimeOffset, HxInputDateInternal<DateTimeOffset?>.GetValueFromDateTimeOffset(dateTimeOffset));
		Assert.AreEqual(null, HxInputDateInternal<DateTimeOffset?>.GetValueFromDateTimeOffset(null));
	}
}
