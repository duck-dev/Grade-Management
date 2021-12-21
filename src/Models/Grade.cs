using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.ViewModels;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models
{
    public class Grade : ReactiveObject, IElement, IGradable, ICloneable
    {
        private ButtonStyleBase? _buttonStyle;
        
        [JsonConstructor]
        public Grade(string name, float gradeValue, float weighting, DateTime date, bool counts)
        {
            this.Name = name;
            this.GradeValue = gradeValue;
            this.Weighting = weighting;
            this.Date = date;
            this.Counts = counts;
            
            var isGrid = SettingsManager.Settings?.GradeButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
        }
        
        [JsonInclude]
        public string Name { get; private set; }
        
        [JsonInclude]
        public float GradeValue { get; private set; }
        
        [JsonInclude]
        public float Weighting { get; private set; }
        
        [JsonInclude]
        public DateTime Date { get; private set; }
        
        [JsonInclude]
        public bool Counts { get; private set; }

        [JsonIgnore] 
        public int ElementCount => 1; // TODO: When partial grades are implemented, return count of grades

        [JsonIgnore]
        public ButtonStyleBase? ButtonStyle
        {
            get => _buttonStyle;
            internal set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
        }

        internal float RoundedGrade => (float)Math.Round(GradeValue, 2);
        internal string DateString => Date.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);

        public IEnumerable<T>? Duplicate<T>() where T : IElement
        {
            if (Clone() is not Grade duplicate)
                return null;
            var currentSubject = MainWindowViewModel.CurrentSubject;
            currentSubject?.Grades.Add(duplicate);

            return currentSubject?.Grades as IEnumerable<T>;
        }

        public object Clone() => this.MemberwiseClone();

        internal void Edit(string newName, float newGrade, float newWeighting, DateTime newDate, bool counts)
        {
            this.Name = newName;
            this.GradeValue = newGrade;
            this.Weighting = newWeighting;
            this.Date = newDate;
            this.Counts = counts;
        }
    }
}