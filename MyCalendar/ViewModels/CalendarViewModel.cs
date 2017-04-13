using System;
using System.Collections.Generic;
using Models;
using Helpers;
using Xamarin.Forms;
using System.Windows.Input;
using Messages;

namespace ViewModels
{
	public class CalendarViewModel : BaseViewModel
	{
		private ICalendarHelper _calHelper;

		#region properties
		public List<WeekModel> WeeksOfSelectedMonth { get; set; } = new List<WeekModel>();
		public ICommand ViewSelectedDatesCommand { get; private set; }
		public ICommand PreviousMonthCommand { get; private set; }
		public ICommand NextMonthCommand { get; private set; }

		private DateTime _selectedStartDate = DateTime.Now;
		public DateTime SelectedStartDate
		{
			get { return _selectedStartDate; }
			set
			{
				if (value.Date > SelectedEndDate.Date)
				{
					MessagingCenter.Send<ShowErrorMessage>(new ShowErrorMessage { Message = "Start date cannot be greater than end date" }, string.Empty);
					OnPropertyChanged(nameof(SelectedStartDate));
					return;
				}
				_selectedStartDate = value;
				OnPropertyChanged(nameof(SelectedStartDate));
			}
		}

		private DateTime _selectedEndDate = DateTime.Now;
		public DateTime SelectedEndDate
		{
			get { return _selectedEndDate; }
			set
			{
				if (value.Date < SelectedStartDate.Date)
				{
					MessagingCenter.Send<ShowErrorMessage>(new ShowErrorMessage { Message = "Start date has to be greater than end date" }, string.Empty);
					OnPropertyChanged(nameof(SelectedEndDate));
					return;
				}
				_selectedEndDate = value;
				OnPropertyChanged(nameof(SelectedEndDate));
			}
		}

		private DateTime _selectedMonth = DateTime.Now;
		public DateTime SelectedMonth
		{
			get { return _selectedMonth; }
			set
			{
				_selectedMonth = value;
				UpdateWeeksOfSelectedMonth();
				OnPropertyChanged(nameof(SelectedMonth));
			}
		}
		#endregion

		public CalendarViewModel()
		{
			_calHelper = DependencyService.Get<ICalendarHelper>();
			SetCommands();
			UpdateWeeksOfSelectedMonth();
		}

		private void SetCommands()
		{
			ViewSelectedDatesCommand = new Command(() =>
			{
				MessagingCenter.Send<ShowDatesMessage>(new ShowDatesMessage { DateFrom = _selectedStartDate, DateTo = _selectedEndDate }, string.Empty);
			});
			PreviousMonthCommand = new Command(() =>
			{
				SelectedMonth = SelectedMonth.AddMonths(-1);
			});
			NextMonthCommand = new Command(() =>
			{
				SelectedMonth = SelectedMonth.AddMonths(1);
			});
		}

		private void UpdateWeeksOfSelectedMonth()
		{
			WeeksOfSelectedMonth.Clear();
			_calHelper.GetWeeksInMonth(SelectedMonth.Month, SelectedMonth.Year, WeeksOfSelectedMonth);
		}
	}
}
