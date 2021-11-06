using System;
using Avalonia.Controls;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using ReactiveUI;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddGradeViewModel : ViewModelBase
    {
        private bool _calendarOpen;
        
        private int _selectedDay = Utilities.TodaysDay;
        private MonthRepresentation _selectedMonth = new(Utilities.TodaysMonth);
        private int _selectedYear = Utilities.TodaysYear;
        
        private DateTime? _tempSelectedDate = DateTime.Today;

        private Calendar? _calendar;

        public AddGradeViewModel()
        {
            Instance = this;
        }
        
        internal static AddGradeViewModel? Instance { get; private set; }

        internal MonthRepresentation SelectedMonth
        {
            get => _selectedMonth;
            set 
            {
                this.RaiseAndSetIfChanged(ref _selectedMonth, value); // Important to force the UI to update;

                DateType protocol = DateType.None;
                if (_tempSelectedDate is null || !Utilities.ValidateDate(_selectedDay, value.Month, _selectedYear,
                    out protocol))
                {
                    // TODO: Red outline/border around corresponding box in the view
                    if(protocol.CustomHasFlag(DateType.Month))
                        System.Diagnostics.Trace.WriteLine("Month is invalid!");

                    return;
                }

                var newDate = _tempSelectedDate.Value;
                SetDate(newDate.Day, value.Month, newDate.Year);
            }
        }
        private bool CalendarOpen
        {
            get => _calendarOpen; 
            set => this.RaiseAndSetIfChanged(ref _calendarOpen, value); // Important to force the UI to update
        }

        private int SelectedDay
        {
            get => _selectedDay;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDay, value); // Important to force the UI to update
                
                if (Utilities.ValidateDate(value, _selectedMonth.Month, _selectedYear, out DateType protocol))
                {
                    if (_tempSelectedDate is null) 
                        return;

                    var newDate = _tempSelectedDate.Value;
                    SetDate(value, newDate.Month, newDate.Year);
                    
                    return;
                }
                
                // TODO: Red outline/border around corresponding box in the view
                if(protocol.CustomHasFlag(DateType.Day))
                    System.Diagnostics.Trace.WriteLine("Day is invalid!");
            }
        }
        private int SelectedYear
        {
            get => _selectedYear;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedYear, value); // Important to force the UI to update

                var validDate = Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, value, out var protocol);
                if (!validDate)
                {
                    // TODO: Red outline/border around corresponding box in the view
                    if(protocol.CustomHasFlag(DateType.Year))
                        System.Diagnostics.Trace.WriteLine("Year is invalid!");
                    return;
                }
                
                if (_tempSelectedDate is null) 
                    return;

                var newDate = _tempSelectedDate.Value;
                SetDate(newDate.Day, newDate.Month, value);
            }
        }
        
        private DateTime? TempSelectedDate 
        {
            get => _tempSelectedDate;
            set
            {
                if (!Utilities.ValidateDate(value, out _)) 
                    return;
                this.RaiseAndSetIfChanged(ref _tempSelectedDate, value);
            }
        }
        
        internal void DateChanged(object? sender, SelectionChangedEventArgs args)
        {
            if (sender is not Calendar calendar) 
                return;
            _calendar = calendar;
            TempSelectedDate = calendar.SelectedDate;
        }

        private void ToggleCalendar()
        {
            CalendarOpen = !CalendarOpen;
            
            if (_calendar is null || !Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, _selectedYear, out _))
                return;

            var currentDate = new DateTime(_selectedYear, _selectedMonth.Month, _selectedDay);
            if (_calendarOpen)
            {
                _calendar.SelectedDate = currentDate;
                _calendar.DisplayDate = currentDate;
            }
            else
                TempSelectedDate = currentDate;
        }

        private void SaveCalendar()
        {
            if (_tempSelectedDate is null)
                return;
            var newDate = _tempSelectedDate.Value;

            SelectedDay = newDate.Day;
            SelectedMonth = new MonthRepresentation(newDate.Month);
            SelectedYear = newDate.Year;
            
            CalendarOpen = false;
        }

        private void SetDate(int day, int month, int year)
        {
            if (!Utilities.ValidateDate(day, month, year, out _))
                return;
            TempSelectedDate = new DateTime(year, month, day);
        }
    }
}