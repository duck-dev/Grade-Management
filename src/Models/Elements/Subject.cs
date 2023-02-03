using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models.Elements
{
    public class Subject : ColorableElement, IElement, ICloneable, IGradesContainer
    {
        private const int MaxNameLength = 64;
        private const double EnabledOpacity = 1.0;
        private const double DisabledOpacity = 0.6;
        private const double DisabledOpacityGrade = 0.4;
        
        private string _name = string.Empty;
        private List<Grade> _grades = new();
        private float _weighting;
        private bool _counts;
        private ButtonStyleBase? _buttonStyle;

        public Subject(string name, float weighting, string elementColorHex, bool counts) : base(elementColorHex)
        {
            if (name.Length > MaxNameLength)
                name = name.Substring(0, MaxNameLength);
            this.Name = name;
            this.Weighting = weighting;
            this.Counts = counts;

            var isGrid = SettingsManager.Settings?.SubjectButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
            AdjustTextColors(isGrid);
        }
        
        [JsonConstructor]
        public Subject(string name, float weighting, string elementColorHex, List<Grade> grades, bool counts) 
            : this(name, weighting, elementColorHex, counts)
        {
            this.Grades = grades;
        }

        [JsonInclude]
        public string Name
        {
            get => _name; 
            private set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        [JsonIgnore]
        public IGradesContainer? ParentContainer { get; }

        [JsonInclude]
        public List<Grade> Grades
        {
            get => _grades;
            private set
            {
                if (value.SequenceEqual(_grades))
                    return;
                this.RaiseAndSetIfChanged(ref _grades, value);
                this.RaisePropertyChanged(nameof(ElementCount));
                this.RaisePropertyChanged(nameof(RoundedAverage));
            }
        }

        [JsonInclude]
        public float Weighting
        {
            get => _weighting; 
            private set => this.RaiseAndSetIfChanged(ref _weighting, value);
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
        public float GradeValue => Utilities.GetAverage(Grades, false);

        [JsonIgnore]
        public int ElementCount => Grades.Count;
        
        [JsonIgnore]
        public ButtonStyleBase? ButtonStyle
        {
            get => _buttonStyle;
            set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
        }
        
        protected override int GridThresholdAdditionalInfo => 120;
        protected override int ListThresholdAdditionalInfo => 135;
        
        internal float RoundedAverage => Utilities.GetAverage(Grades, true);
        internal double ElementsOpacity => Counts ? EnabledOpacity : DisabledOpacity;
        internal double GradeTextOpacity => Counts ? EnabledOpacity : DisabledOpacityGrade;

        public T? Duplicate<T>(bool save = true) where T : class, IElement
        {
            if (Clone() is not Subject duplicate)
                return null;

            if (save)
                Save(duplicate);

            return duplicate as T;
        }

        public void Save<T>(T? element = null) where T : class, IElement
        {
            Subject subject = element as Subject ?? this;
            var currentYear = MainWindowViewModel.CurrentYear;
            currentYear?.Subjects.Add(subject);
        }

        public object Clone() => new Subject(_name, _weighting, ElementColorHex, _grades.Clone().ToList(), Counts);

        internal void Edit(string newName, float newWeighting, string newSubjectColorHex, bool counts)
        {
            var oldCounts = this.Counts;
            var oldWeighting = this.Weighting;
            
            this.Name = newName;
            this.Weighting = newWeighting;
            this.Counts = counts;
            this.ElementColorHex = newSubjectColorHex;

            if (oldCounts == counts && Math.Abs(oldWeighting - newWeighting) < 0.001f) 
                return;
            var mainInstance = MainWindowViewModel.Instance;
            mainInstance?.UpdateAverage();
        }
    }
}