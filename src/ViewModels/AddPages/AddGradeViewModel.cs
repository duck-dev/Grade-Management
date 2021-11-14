using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;
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
            
            EditPageText(AddPageAction.Create, this.GetType());
        }

        internal static AddGradeViewModel? Instance { get; private set; }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName)
                                                && !float.IsNaN(_elementGrade)
                                                && !float.IsNaN(ElementWeighting)
                                                && Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, 
                                                                            _selectedYear, out _)
                                                && DataChanged();
        
        internal Grade? EditedGrade { get; set; } // When editing year, overwrite this property with `Grade`

        internal int SelectedDay
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
        internal int SelectedYear
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
        
        internal string? ElementGradeString
        {
            get => _elementGradeStr;
            set
            {
                this.RaiseAndSetIfChanged(ref _elementGradeStr, value);
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
        
        internal void DateChanged(object? sender, SelectionChangedEventArgs args)
        {
            if (sender is not Calendar calendar) 
                return;
            _calendar = calendar;
            TempSelectedDate = calendar.SelectedDate;
        }
        
        protected internal override void EraseData()
        {
            base.EraseData();
            ElementGradeString = string.Empty;

            var today = DateTime.Today;
            TempSelectedDate = DateTime.Today;
            SelectedDay = today.Day;
            SelectedMonth = new MonthRepresentation(today.Month);
            SelectedYear = today.Year;
            
            EditedGrade = null;
        }

        private bool CalendarOpen
        {
            get => _calendarOpen; 
            set => this.RaiseAndSetIfChanged(ref _calendarOpen, value);
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

        private void CreateElement()
        {
            if (ElementName is null || _tempSelectedDate is null)
                return;
            
            if (EditedGrade is null)
            {
                var currentSubject = MainWindowViewModel.CurrentSubject;
                if (currentSubject is null)
                    return;

                var grade = new Grade(ElementName, _elementGrade, ElementWeighting, _tempSelectedDate.Value, ElementCounts);
                currentSubject.Grades.Add(grade);

                var viewModel = GradeListViewModel.Instance;
                if(viewModel is not null)
                    viewModel.Items = new ObservableCollection<Grade>(currentSubject.Grades);
            }
            else
                EditedGrade.Edit(ElementName, _elementGrade, ElementWeighting, _tempSelectedDate.Value, ElementCounts);

            EditedGrade = null;
            CloseAddWindow();
            DataManager.SaveData();
        }

        private bool DataChanged()
        {
            if (EditedGrade is null)
                return true;
            
            var date = EditedGrade.Date;
            return ElementName is not null && (!ElementName.Trim().Equals(EditedGrade.Name.Trim()) 
                                               || !ElementWeighting.Equals(EditedGrade.Weighting)
                                               || !_elementGrade.Equals(EditedGrade.GradeValue)
                                               || _selectedDay != date.Day || _selectedMonth.Month != date.Month || 
                                               _selectedYear != date.Year 
                                               || ElementCounts != EditedGrade.Counts); 
            // TODO: Investigate why changing the status of whether it counts or not doesn't return true.
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