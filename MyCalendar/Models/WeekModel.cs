using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
	public class WeekModel
	{
		private static Calendar _currCalendar = CultureInfo.CurrentCulture.Calendar;

		public WeekModel()
		{
			_year = DateTime.Now.Year;
		}

		private int _weekOfYear;
		public int WeekOfYear
		{
			get { return _weekOfYear; }
			set
			{
				if (_weekOfYear != value)
				{
					_weekOfYear = value;
					UpdateDaysInWeek();
				}
			}
		}

		private int _year;
		public int Year
		{
			get { return _year; }
			set
			{
				if (_year != value)
				{
					_year = value;
					UpdateDaysInWeek();
				}
			}
		}

		private List<DateTime> _days = new List<DateTime>();
		public List<DateTime> Days
		{
			get { return _days; }
		}

		private void UpdateDaysInWeek()
		{
			_days.Clear();
			var NewYearsEve = new DateTime(_year, 1, 1);
			var d = _currCalendar.AddWeeks(NewYearsEve, WeekOfYear - 1);
			var monday = d.AddDays(-(int)d.DayOfWeek + 1);
			for (var i = 0; i < 7; i++)
			{
				_days.Add(monday.AddDays(i));
			}
		}

	}
}
