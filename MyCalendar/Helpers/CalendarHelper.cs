using System;
using System.Collections.Generic;
using System.Globalization;
using Helpers;
using Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(CalendarHelper))]
namespace Helpers
{
	public class CalendarHelper : ICalendarHelper
	{
		public string[] GetFormattedDateInRange(DateTime startDate, DateTime endDate, string format)
		{
			var ci = CultureInfo.CurrentCulture;
			var selectedDays = new List<string>();
			var currDate = startDate;
			do
			{
				selectedDays.Add(currDate.ToString(format, ci));
				currDate = currDate.AddDays(1);
			} while (currDate.Date <= endDate.Date);

			return selectedDays.ToArray();
		}

		public void GetWeeksInMonth(int month, int year, List<WeekModel> weeks)
		{
			Calendar CurrentCalendar = CultureInfo.CurrentCulture.Calendar;
			var FirstDayOfMonth = new DateTime(year, month, 1);
			for (var d = FirstDayOfMonth; d.Month == month; d = CurrentCalendar.AddWeeks(d, 1))
			{
				weeks.Add(new WeekModel
				{
					WeekOfYear = CurrentCalendar.GetWeekOfYear(d, CalendarWeekRule.FirstDay, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek),
					Year = d.Year
				});
			}
		}

		public string[] GetAbbreviatedDayNames()
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
		}
	}
}
