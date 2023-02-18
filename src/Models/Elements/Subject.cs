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
using JetBrains.Annotations;
using ReactiveUI;

namespace GradeManagement.Models.Elements
{
    public class Subject : ColorableElement, ICloneable, IGradesContainer
    {
        private List<Grade> _grades = new();

        public Subject(string name, float weighting, string elementColorHex, bool counts) : base(elementColorHex, name, weighting, counts)
        {
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

        [JsonIgnore] [UsedImplicitly] 
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

        [JsonIgnore]
        public override float GradeValue => Utilities.GetAverage(Grades, false);

        [JsonIgnore]
        public override int ElementCount => Grades.Count;
        
        internal float RoundedAverage => Utilities.GetAverage(Grades, true);

        protected internal override void Save<T>(T? element = null) where T : class
        {
            Subject subject = element as Subject ?? this;
            var currentYear = MainWindowViewModel.CurrentYear;
            currentYear?.Subjects.Add(subject);
        }

        public override object Clone() => new Subject(Name, Weighting, ElementColorHex, Grades.Clone().ToList(), Counts);

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