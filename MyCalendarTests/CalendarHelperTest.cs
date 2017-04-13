using Helpers;
using NUnit.Framework;
using Models;
using System.Collections.Generic;
using System;

namespace MyCalendarTests
{
	[TestFixture()]
	public class CalendarHelperTest
	{
		ICalendarHelper _calendarHelper;

		[SetUp]
		public void Setup()
		{
			_calendarHelper = new CalendarHelper();
		}

		[Test()]
		public void WeeksInMonthTest()
		{
			var weekDays = new List<WeekModel>();
			_calendarHelper.GetWeeksInMonth(4, 2017, weekDays);
			Assert.AreEqual(weekDays.Count, 5, string.Format("April 2017 spans on 5 weeks but found {0}", weekDays.Count));
			Assert.IsTrue(weekDays[0].Days[0].Date == new DateTime(2017, 3, 27), "First week of April 2017 should begin Monday March 27th");
			Assert.IsTrue(weekDays[0].Days[6].Date == new DateTime(2017, 4, 2), "First week of April 2017 should end Sunday April 2nd");
		}

		[Test()]
		public void GetFormattedDateInRangeTest()
		{
			var daysAddedForTest = 10;
			var datetimeFormat = "d";
			var dateFrom = new DateTime(2017, 4, 1);
			var dateTo = dateFrom.AddDays(daysAddedForTest);
			var datesInRange = _calendarHelper.GetFormattedDateInRange(dateFrom, dateTo, datetimeFormat);
			Assert.AreEqual(datesInRange.Length, daysAddedForTest + 1, "days from {0} to {1} should be {2} rather than {3}", dateFrom, dateTo, daysAddedForTest, datesInRange.Length);
			Assert.AreEqual(datesInRange[0], dateFrom.ToString(datetimeFormat), "starting date should be {0} and not {1}", dateFrom.Day.ToString(), datesInRange[0]);
		}
	}
}
