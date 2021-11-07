using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
        
        // Grade
        private float _elementGrade;
        
        // Name
        private string? _elementName;
        
        // Colors for border (incomplete/complete selection)
        private static readonly Color _incompleteColor = Color.Parse("#D64045");
        private static readonly Color _normalColor = Color.Parse("#009b72");

        public AddGradeViewModel() => Instance = this;

        internal static AddGradeViewModel? Instance { get; private set; }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName)
                                                && !float.IsNaN(_elementGrade)
                                                && !float.IsNaN(ElementWeighting)
                                                && Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, 
                                                                            _selectedYear, out _);

        protected override SolidColorBrush[] BorderBrushes { get; } =
        {
            new(_incompleteColor),
            new(_normalColor), new(_normalColor), new(_normalColor),
            new(_incompleteColor),
            new(_incompleteColor)
        };

        protected override string? ElementName
        {
            get => _elementName;
            set
            {
                _elementName = value;
                BorderBrushes[0].Color = _normalColor;
                if (string.IsNullOrEmpty(value))
                    BorderBrushes[0].Color = _incompleteColor;
                
                this.RaisePropertyChanged(nameof(BorderBrushes));
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        internal MonthRepresentation SelectedMonth
        {
            get => _selectedMonth;
            set 
            {
                this.RaiseAndSetIfChanged(ref _selectedMonth, value); // Important to force the UI to update;
                this.RaisePropertyChanged(nameof(DataComplete));

                DateType protocol = DateType.None;
                if (!Utilities.ValidateDate(_selectedDay, value.Month, _selectedYear, out protocol)
                    || _tempSelectedDate is null)
                {
                    if (!protocol.CustomHasFlag(DateType.Month)) 
                        return;
                    BorderBrushes[2].Color = _incompleteColor;
                    this.RaisePropertyChanged(nameof(BorderBrushes));
                    
                    return;
                }

                BorderBrushes[2].Color = _normalColor;
                this.RaisePropertyChanged(nameof(BorderBrushes));
                
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
                
                BorderBrushes[1].Color = _normalColor;
                
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
                        BorderBrushes[1].Color = _incompleteColor;
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
                
                BorderBrushes[3].Color = _normalColor;

                var validDate = Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, value, out var protocol);
                if (!validDate)
                {
                    if (protocol.CustomHasFlag(DateType.Year))
                        BorderBrushes[3].Color = _incompleteColor;
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

        protected string? ElementGradeString
        {
            get => string.Empty;
            set
            {
                _elementGrade = float.NaN;
                BorderBrushes[4].Color = _normalColor;
                if (float.TryParse(value, out float grade))
                    _elementGrade = grade;
                else
                    BorderBrushes[4].Color = _incompleteColor;

                this.RaisePropertyChanged(nameof(BorderBrushes));
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        protected override string? ElementWeightingString
        {
            get => string.Empty;
            set
            {
                ElementWeighting = float.NaN;
                BorderBrushes[5].Color = _normalColor;
                if (float.TryParse(value, out float weighting))
                    ElementWeighting = weighting;
                else
                    BorderBrushes[5].Color = _incompleteColor;

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

        internal void GradeCountsChecked(object? sender, RoutedEventArgs args) => ElementCounts = !ElementCounts;

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