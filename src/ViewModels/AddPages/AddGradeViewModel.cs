using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Models.Elements;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;
using ReactiveUI;
using Calendar = Avalonia.Controls.Calendar;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddGradeViewModel : AddViewModelBase, IAddViewModel<Grade>
    {
        private bool _isEditing;
        
        // Date
        private int _selectedDay = Utilities.TodaysDay;
        private MonthRepresentation _selectedMonth = new(Utilities.TodaysMonth);
        private int _selectedYear = Utilities.TodaysYear;
        
        private DateTime? _tempSelectedDate = DateTime.Today;
        private Calendar? _calendar;
        private bool _calendarOpen;
        
        // Other parameters
        private float _elementGrade = float.NaN;
        private float? _elementScoredPoints;
        private float? _elementMaxPoints;
        
        private string? _elementGradeStr;
        private string? _elementScoredPointsStr;
        private string? _elementMaxPointsStr;

        private bool _specifyPoints;
        private bool _isMultiGrade;

        public AddGradeViewModel()
        {
            Instance = this;
            BorderBrushes = new SolidColorBrush[]
            {
                new(IncompleteColor),
                new(NormalColor), new(NormalColor), new(NormalColor),
                new(IncompleteColor),
                new(IncompleteColor),
                new(InactiveColor), new(InactiveColor)
            };
            NameIndex = 0;
            WeightingIndex = 5;
            
            EditPageText(AddPageAction.Create, "Grade");
        }

        internal static AddGradeViewModel? Instance { get; private set; }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName) && !string.IsNullOrWhiteSpace(ElementName)
                                                && (IsMultiGrade || !float.IsNaN(_elementGrade))
                                                && !float.IsNaN(ElementWeighting)
                                                && (!SpecifyPoints || _elementScoredPoints <= _elementMaxPoints)
                                                && Utilities.ValidateDate(_selectedDay, _selectedMonth.Month, _selectedYear, out _)
                                                && DataChanged();
        
        internal IGradesContainer? GradesContainer { get; set; }

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
        internal MonthRepresentation SelectedMonth
        {
            get => _selectedMonth;
            set 
            {
                this.RaiseAndSetIfChanged(ref _selectedMonth, value); // Important to force the UI to update
                this.RaisePropertyChanged(nameof(DataComplete));
                
                BorderBrushes![2].Color = NormalColor;
                this.RaisePropertyChanged(nameof(BorderBrushes));

                if (!Utilities.ValidateDate(_selectedDay, value.Month, _selectedYear, out DateType protocol)
                    || _tempSelectedDate is null)
                {
                    SelectedDay = _selectedDay; // Update Day, due to different amount of days in months (possibly valid/invalid day)
                    if (!protocol.CustomHasFlag(DateType.Month)) 
                        return;
                    BorderBrushes![2].Color = IncompleteColor;
                    this.RaisePropertyChanged(nameof(BorderBrushes));
                    
                    return;
                }
                
                SelectedDay = _selectedDay; // Update Day, due to different amount of days in months (possibly valid/invalid day)

                var newDate = _tempSelectedDate.Value;
                SetDate(newDate.Day, value.Month, newDate.Year);
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
                    
                    SelectedDay = _selectedDay; // Update Day, because of leap year (possibly valid/invalid day)
                    
                    return;
                }
                
                this.RaisePropertyChanged(nameof(BorderBrushes));
                SelectedDay = _selectedDay; // Update Day, because of leap year (possibly valid/invalid day)

                if (_tempSelectedDate is null) 
                    return;

                var newDate = _tempSelectedDate.Value;
                SetDate(newDate.Day, newDate.Month, value);
            }
        }
        
        private string? ElementGradeString
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

        private string? ElementScoredPointsString
        {
            get => _elementScoredPointsStr;
            set => HandlePointsChange(value, nameof(ElementScoredPointsString), 6, ref _elementScoredPointsStr, ref _elementScoredPoints);
        }

        private string? ElementMaxPointsString
        {
            get => _elementMaxPointsStr;
            set => HandlePointsChange(value, nameof(ElementMaxPointsString), 7, ref _elementMaxPointsStr, ref _elementMaxPoints);
        }

        private bool SpecifyPoints
        {
            get => _specifyPoints;
            set
            {
                if (_specifyPoints == true && value == false) // Prevent erasing saved values at the beginning (only erase if changing from true to false, which can only be done manually)
                {
                    ElementScoredPointsString = string.Empty;
                    ElementMaxPointsString = string.Empty;
                }
                BorderBrushes![6].Color = value == true ? IncompleteColor : InactiveColor;
                BorderBrushes![7].Color = value == true ? IncompleteColor : InactiveColor;
                
                this.RaiseAndSetIfChanged(ref _specifyPoints, value);
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }
        
        private bool IsMultiGrade
        {
            get => _isMultiGrade;
            set
            {
                this.RaiseAndSetIfChanged(ref _isMultiGrade, value);
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        private Grade? EditedGrade { get; set; }

        private bool IsEditing
        {
            get => _isEditing; 
            set => this.RaiseAndSetIfChanged(ref _isEditing, value);
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
            SpecifyPoints = false;

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
        
        public void EditElement(Grade grade)
        {
            EditedGrade = grade;
            IsEditing = true;
            EditPageText(AddPageAction.Edit, "Grade", grade.Name);

            ElementName = grade.Name;
            ElementGradeString = grade.IsMultiGrade ? string.Empty : grade.GradeValue.ToString(CultureInfo.CurrentCulture);
            SpecifyPoints = grade.ScoredPoints != null && grade.MaxPoints != null;
            ElementScoredPointsString = grade.ScoredPoints.ToString();
            ElementMaxPointsString = grade.MaxPoints.ToString();
            ElementWeightingString = grade.Weighting.ToString(CultureInfo.CurrentCulture);
            ElementCounts = grade.Counts;
            IsMultiGrade = grade.IsMultiGrade;
            
            SelectedDay = grade.Date.Day;
            SelectedMonth = new MonthRepresentation(grade.Date.Month);
            SelectedYear = grade.Date.Year;
        }

        private void CreateElement()
        {
            if (ElementName is null || TempSelectedDate is null)
                return;
            
            if (EditedGrade is null)
            {
                if (GradesContainer != null)
                {
                    var viewModel = GradeListViewModel.Instance;
                    Grade grade = IsMultiGrade ? new GradeGroup(ElementName, Array.Empty<Grade>(), ElementWeighting, TempSelectedDate.Value, ElementCounts) 
                        : new Grade(ElementName, _elementGrade, _elementScoredPoints, _elementMaxPoints, ElementWeighting, TempSelectedDate.Value, ElementCounts);
                
                    GradesContainer?.Grades.Add(grade);
                    viewModel?.Items?.Add(grade);
                }
            }
            else
                EditedGrade.Edit(ElementName, _elementGrade, _elementScoredPoints, _elementMaxPoints, ElementWeighting, TempSelectedDate.Value, ElementCounts);
            
            UpdateVisualOnChange();
            EditedGrade = null;
        }

        private bool DataChanged()
        {
            if (EditedGrade is null)
                return true;
            
            var date = EditedGrade.Date;
            return ElementName is not null && (!ElementName.Trim().Equals(EditedGrade.Name.Trim())
                                               || !ElementWeighting.Equals(EditedGrade.Weighting)
                                               || (!IsMultiGrade && !_elementGrade.Equals(EditedGrade.GradeValue))
                                               || !_elementScoredPoints.AlmostEquals(EditedGrade.ScoredPoints, Utilities.EqualityTolerance)
                                               || !_elementMaxPoints.AlmostEquals(EditedGrade.MaxPoints, Utilities.EqualityTolerance)
                                               || _selectedDay != date.Day
                                               || _selectedMonth.Month != date.Month
                                               || _selectedYear != date.Year
                                               || ElementCounts != EditedGrade.Counts);
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

        private void HandlePointsChange(string? strValue, string propertyName, int brushIndex, ref string? strVariable, ref float? floatVariable)
        {
            if (!SpecifyPoints)
                return;

            if (strValue != strVariable)
            {
                strVariable = strValue;
                this.RaisePropertyChanged(propertyName);
            }
            
            floatVariable = null;
            BorderBrushes![brushIndex].Color = NormalColor;

            bool parsed = float.TryParse(strValue, CultureInfo.InvariantCulture, out float max);
            if (parsed)
                floatVariable = max;
            else
                BorderBrushes[brushIndex].Color = IncompleteColor;

            bool valueSizeCondition = _elementScoredPoints > _elementMaxPoints;
            BorderBrushes[6].Color = _elementScoredPoints is null || valueSizeCondition ? IncompleteColor : NormalColor;
            BorderBrushes[7].Color = _elementMaxPoints is null || valueSizeCondition ? IncompleteColor : NormalColor;
                
            this.RaisePropertyChanged(nameof(BorderBrushes));
            this.RaisePropertyChanged(nameof(DataComplete));
        }
    }
}