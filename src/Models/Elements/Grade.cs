using System;
using System.Globalization;
using System.Text.Json.Serialization;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.ViewModels;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models.Elements
{
    public class Grade : ReactiveObject, IElement, IGradable, ICloneable
    {
        private const int MaxNameLength = 35;
        
        private string _name = string.Empty;
        private float _gradeValue;
        private float _weighting;
        private DateTime _date;
        //private bool _counts; TODO: Will be used once there is a UI representation of this bool on the element
        private ButtonStyleBase? _buttonStyle;
        
        [JsonConstructor]
        public Grade(string name, float gradeValue, float weighting, DateTime date, bool counts)
        {
            if (name.Length > MaxNameLength)
                name = name.Substring(0, MaxNameLength);
            this.Name = name;
            this.GradeValue = gradeValue;
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
        public float GradeValue
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
        public bool Counts { get; private set; } // TODO: Update UI

        [JsonIgnore] 
        public int ElementCount => 1; // TODO: When partial grades are implemented, return count of grades

        [JsonIgnore]
        public ButtonStyleBase? ButtonStyle
        {
            get => _buttonStyle;
            set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
        }

        internal float RoundedGrade => (float)Math.Round(GradeValue, 2);
        internal string DateString => Date.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);

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
            var currentSubject = MainWindowViewModel.CurrentSubject;
            currentSubject?.Grades.Add(grade);
        }
        
        public object Clone() => new Grade(_name, _gradeValue, _weighting, _date, Counts);

        internal void Edit(string newName, float newGrade, float newWeighting, DateTime newDate, bool counts)
        {
            var oldGrade = this.GradeValue;
            var oldCounts = this.Counts;
            var oldWeighting = this.Weighting;
            
            this.Name = newName;
            this.GradeValue = newGrade;
            this.Weighting = newWeighting;
            this.Date = newDate;
            this.Counts = counts;
            
            if (oldCounts == counts && Math.Abs(oldWeighting - newWeighting) < 0.001f && Math.Abs(oldGrade - newGrade) < 0.001f) 
                return;
            var mainInstance = MainWindowViewModel.Instance;
            mainInstance?.UpdateAverage();
        }
    }
}