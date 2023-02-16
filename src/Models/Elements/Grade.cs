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
    public class Grade : ReactiveObject, IElement, ICloneable
    {
        private const int MaxNameLength = 64;
        private const double EnabledOpacity = 1.0;
        private const double DisabledOpacity = 0.4;
        private const double DisabledOpacityGrade = 0.4;
        
        private string _name = string.Empty;
        private float _gradeValue = float.NaN;
        private float? _scoredPoints;
        private float? _maxPoints;
        private float _weighting;
        private DateTime _date;
        private bool _counts;
        private ButtonStyleBase? _buttonStyle;
        
        [JsonConstructor]
        public Grade(string name, float gradeValue, float? scoredPoints, float? maxPoints, float weighting, DateTime date, bool counts)
        {
            if (name.Length > MaxNameLength)
                name = name.Substring(0, MaxNameLength);
            this.Name = name;
            this.GradeValue = gradeValue;
            this.ScoredPoints = scoredPoints;
            this.MaxPoints = maxPoints;
            this.Weighting = weighting;
            this.Date = date;
            this.Counts = counts;
            
            var isGrid = SettingsManager.Settings?.GradeButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
        }

        [JsonInclude]
        public string Name
        {
            get => _name; 
            private set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        [JsonInclude]
        public virtual float GradeValue
        {
            get => _gradeValue;
            private set
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
        public float Weighting
        {
            get => _weighting; 
            private set => this.RaiseAndSetIfChanged(ref _weighting, value);
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
        
        [JsonInclude]
        public bool Counts
        {
            get => _counts;
            private set
            {
                this.RaiseAndSetIfChanged(ref _counts, value);
                this.RaisePropertyChanged(nameof(ElementsOpacity));
                this.RaisePropertyChanged(nameof(GradeTextOpacity));
            }
        }

        [JsonIgnore] 
        public virtual int ElementCount => 1;

        [JsonIgnore]
        public ButtonStyleBase? ButtonStyle
        {
            get => _buttonStyle;
            set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
        }

        internal float RoundedGrade => (float)Math.Round(GradeValue, 2);
        internal string DateString => Date.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);
        internal double ElementsOpacity => Counts ? EnabledOpacity : DisabledOpacity;
        internal double GradeTextOpacity => Counts ? EnabledOpacity : DisabledOpacityGrade;

        internal bool IsMultiGrade => this is GradeGroup;

        internal bool PointsSpecified => !IsMultiGrade && ScoredPoints != null && MaxPoints != null;

        public T? Duplicate<T>(bool save = true) where T : class, IElement
        {
            if (this.Clone() is not Grade duplicate)
                return null;
            
            if(save)
                Save(duplicate);

            return duplicate as T;
        }
        
        public void Save<T>(T? element = null) where T : class, IElement
        {
            Grade grade = element as Grade ?? this;
            IGradesContainer? container = null;
            if (MainWindowViewModel.Instance?.Content is GradeListViewModel viewModel)
                container = viewModel.GradesContainer;
            container?.Grades.Add(grade);
        }
        
        public object Clone() => new Grade(_name, _gradeValue, _scoredPoints, _maxPoints, _weighting, _date, Counts);

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