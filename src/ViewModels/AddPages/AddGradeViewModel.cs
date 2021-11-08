using System;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;
using Calendar = Avalonia.Controls.Calendar;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddGradeViewModel : AddViewModelBase
    {
        // Date
        private int _selectedDay = Utilities.TodaysDay;
        private MonthRepresentation _selectedMonth = new(Utilities.TodaysMonth);
        private int _selectedYear = Utilities.TodaysYear;
        
        private DateTime? _tempSelectedDate = DateTime.Today;
        private Calendar? _calendar;
        private bool _calendarOpen;
        
        // Other parameters
        private float _elementGrade;
        private string? _elementGradeStr;

        public AddGradeViewModel()
        {
            Instance = this;
            BorderBrushes = new SolidColorBrush[]
            {
                new(IncompleteColor),
                new(NormalColor), new(NormalColor), new(NormalColor),
                new(IncompleteColor),
                new(IncompleteColor)
            };
            NameIndex = 0;
            WeightingIndex = 5;
        }

        internal static AddGradeViewModel? Instance { get; private set; }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName)
                                                && !float.IsNaN(_elementGrade)
                                                && !float.IsNaN(ElementWeighting)
                                                && Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, 
                                                                            _selectedYear, out _);
        
        internal MonthRepresentation SelectedMonth
        {
            get => _selectedMonth;
            set 
            {
                this.RaiseAndSetIfChanged(ref _selectedMonth, value); // Important to force the UI to update;
                this.RaisePropertyChanged(nameof(DataComplete));
                
                BorderBrushes![2].Color = NormalColor;
                this.RaisePropertyChanged(nameof(BorderBrushes));

                if (!Utilities.ValidateDate(_selectedDay, value.Month, _selectedYear, out DateType protocol)
                    || _tempSelectedDate is null)
                {
                    if (!protocol.CustomHasFlag(DateType.Month)) 
                        return;
                    BorderBrushes![2].Color = IncompleteColor;
                    this.RaisePropertyChanged(nameof(BorderBrushes));
                    
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
                this.RaisePropertyChanged(nameof(DataComplete));
                
                BorderBrushes![1].Color = NormalColor;
                
                if (Utilities.ValidateDate(value, _selectedMonth.Month, _selectedYear, out DateType protocol))
                {
                    if (_tempSelectedDate is null)
                    {
                        this.RaisePropertyChanged(nameof(BorderBrushes));
                        return;
                    }

                    var newDate = _tempSelectedDate.Value;
                    SetDate(value, newDate.Month, newDate.Year);
                }
                else
                {
                    if (protocol.CustomHasFlag(DateType.Day))
                        BorderBrushes[1].Color = IncompleteColor;
                }
                
                this.RaisePropertyChanged(nameof(BorderBrushes));
            }
        }
        private int SelectedYear
        {
            get => _selectedYear;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedYear, value); // Important to force the UI to update
                this.RaisePropertyChanged(nameof(DataComplete));
                
                BorderBrushes![3].Color = NormalColor;

                var validDate = Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, value, out var protocol);
                if (!validDate)
                {
                    if (protocol.CustomHasFlag(DateType.Year))
                        BorderBrushes![3].Color = IncompleteColor;
                    this.RaisePropertyChanged(nameof(BorderBrushes));
                    
                    return;
                }
                
                this.RaisePropertyChanged(nameof(BorderBrushes));
                
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

        private string? ElementGradeString
        {
            get => _elementGradeStr;
            set
            {
                _elementGradeStr = value;
                _elementGrade = float.NaN;
                BorderBrushes![4].Color = NormalColor;
                
                if (float.TryParse(value, out float grade))
                    _elementGrade = grade;
                else
                    BorderBrushes[4].Color = IncompleteColor;

                this.RaisePropertyChanged(nameof(BorderBrushes));
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        protected override void CreateElement()
        {
            
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