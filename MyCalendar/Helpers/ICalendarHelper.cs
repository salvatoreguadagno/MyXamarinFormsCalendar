using System;
using System.Collections.Generic;
using Models;

namespace Helpers
{
	public interface ICalendarHelper
	{
		string[] GetFormattedDateInRange(DateTime startDate, DateTime endDate, string format);
		void GetWeeksInMonth(int month, int year, List<WeekModel> weeks);
		string[] GetAbbreviatedDayNames();
	}
}
