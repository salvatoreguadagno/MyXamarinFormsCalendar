using System;
using ViewModels;
using Xamarin.Forms;
using Helpers;
using Messages;

namespace MyCalendar
{
	public partial class MyCalendarPage : ContentPage
	{
		private CalendarViewModel _calendarVM;
		private ICalendarHelper _calendarHelper;

		public MyCalendarPage()
		{
			InitializeComponent();
			_calendarHelper = DependencyService.Get<ICalendarHelper>();
			_calendarVM = (CalendarViewModel)BindingContext;
			_calendarVM.PropertyChanged += (sender, e) =>
			{
				PopulateCalendarGrid();
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			SubscribeMessages();
			PopulateCalendarHeaderGrid();
			PopulateCalendarGrid();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			UnSubscribeMessages();
		}

		private void SubscribeMessages()
		{
			MessagingCenter.Subscribe<ShowDatesMessage>(this, string.Empty, async (msg) =>
			{
				var selectedDates = _calendarHelper.GetFormattedDateInRange(msg.DateFrom, msg.DateTo, "d");
				await DisplayActionSheet("SELECTED DAYS", "Cancel", null, selectedDates);
			});
			MessagingCenter.Subscribe<ShowErrorMessage>(this, string.Empty, async (msg) =>
			{
				await DisplayAlert("Warning", msg.Message, "Cancel");
			});
		}

		private void UnSubscribeMessages()
		{
			MessagingCenter.Unsubscribe<ShowDatesMessage>(this, string.Empty);
			MessagingCenter.Unsubscribe<ShowErrorMessage>(this, string.Empty);
		}

		private void PopulateCalendarHeaderGrid()
		{
			var shortDayName = _calendarHelper.GetAbbreviatedDayNames();
			calendarGrid.Children.Add(new Label { Text = shortDayName[0] }, 6, 0);
			for (var i = 1; i < shortDayName.Length; i++)
			{
				calendarGrid.Children.Add(new Label { Text = shortDayName[i] }, i - 1, 0);
			}
		}

		private void PopulateCalendarGrid()
		{
			var headerFirstRow = 1;
			calendarGrid.Children.Clear();
			PopulateCalendarHeaderGrid();
			for (var row = 0; row < _calendarVM.WeeksOfSelectedMonth.Count; row++)
			{
				foreach (var day in _calendarVM.WeeksOfSelectedMonth[row].Days)
				{
					var dayCell = new Label { Text = day.Day.ToString(), BindingContext = BindingContext };

					if (day.Date >= _calendarVM.SelectedStartDate.Date && day.Date <= _calendarVM.SelectedEndDate.Date)
					{
						dayCell.Style = (Style)Application.Current.Resources["SelectedDayStyle"];
					}
					else if (day.Date.Month == _calendarVM.SelectedMonth.Month)
					{
						dayCell.Style = (Style)Application.Current.Resources["CurrentMonthDayStyle"];
					}
					else
					{
						dayCell.Style = (Style)Application.Current.Resources["OtherMonthDayStyle"];
					}

					var col = day.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)day.DayOfWeek - 1;
					calendarGrid.Children.Add(dayCell, col, row + headerFirstRow);
				}
			}
		}
	}
}
