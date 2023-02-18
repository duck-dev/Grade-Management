using System;
using System.Globalization;
using System.Text.Json.Serialization;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.ViewModels;
using GradeManagement.ViewModels.Lists;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models.Elements
{
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor)]
    [JsonDerivedType(typeof(Grade), "grade")]
    [JsonDerivedType(typeof(GradeGroup), "gradeGroup")]
    public class Grade : Element, ICloneable
    {
        private float _gradeValue = float.NaN;
        private float? _scoredPoints;
        private float? _maxPoints;
        private DateTime _date;
        
        [JsonConstructor]
        public Grade(string name, float gradeValue, float? scoredPoints, float? maxPoints, float weighting, DateTime date, bool counts) 
            : base(name, weighting, counts)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.GradeValue = gradeValue;
            this.ScoredPoints = scoredPoints;
            this.MaxPoints = maxPoints;
            this.Date = date;
            
            var isGrid = SettingsManager.Settings?.GradeButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
        }

        [JsonInclude]
        public override float GradeValue
        {
            get => _gradeValue;
            protected set
            {
                if (value.Equals(_gradeValue))
                    return;
                _gradeValue = value;
                this.RaisePropertyChanged(nameof(RoundedGrade));
            }
        }

        [JsonInclude]
        public float? ScoredPoints
        {
            get => _scoredPoints;
            private set => this.RaiseAndSetIfChanged(ref _scoredPoints, value);
        }
        
        [JsonInclude]
        public float? MaxPoints
        {
            get => _maxPoints;
            private set => this.RaiseAndSetIfChanged(ref _maxPoints, value);
        }

        [JsonInclude]
        public DateTime Date
        {
            get => _date;
            private set
            {
                if(value.Equals(_date))
                    return;
                _date = value;
                this.RaisePropertyChanged(nameof(DateString));
            }
        }

        [JsonIgnore] 
        public override int ElementCount => 1;

        internal float RoundedGrade => (float)Math.Round(GradeValue, 2);
        internal string DateString => Date.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);

        internal bool IsMultiGrade => this is GradeGroup;

        internal bool PointsSpecified => !IsMultiGrade && ScoredPoints != null && MaxPoints != null;

        protected internal override void Save<T>(T? element = null) where T : class
        {
            Grade grade = element as Grade ?? this;
            IGradesContainer? container = null;
            if (MainWindowViewModel.Instance?.Content is GradeListViewModel viewModel)
                container = viewModel.GradesContainer;
            container?.Grades.Add(grade);
        }
        
        public override object Clone() => new Grade(Name, GradeValue, ScoredPoints, MaxPoints, Weighting, Date, Counts);

        internal void Edit(string newName, float newGrade, float? newScoredPoints, float? newMaxPoints, float newWeighting, 
            DateTime newDate, bool counts)
        {
            var oldGrade = this.GradeValue;
            var oldCounts = this.Counts;
            var oldWeighting = this.Weighting;
            
            this.Name = newName;
            this.GradeValue = newGrade;
            this.ScoredPoints = newScoredPoints;
            this.MaxPoints = newMaxPoints;
            this.Weighting = newWeighting;
            this.Date = newDate;
            this.Counts = counts;
            this.RaisePropertyChanged(nameof(PointsSpecified));
            
            if (oldCounts == counts && Math.Abs(oldWeighting - newWeighting) < 0.001f && Math.Abs(oldGrade - newGrade) < 0.001f) 
                return;
            var mainInstance = MainWindowViewModel.Instance;
            mainInstance?.UpdateAverage();
        }
    }
}