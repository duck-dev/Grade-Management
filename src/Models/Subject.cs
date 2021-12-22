using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models
{
    public class Subject : ReactiveObject, IElement, IGradable, ICloneable
    {
        private readonly Color _additionalInfoColor = Color.Parse("#999999");
        private readonly Color _lightBackground = Color.Parse("#c7cad1");

        private string _name = string.Empty;
        private List<Grade> _grades = new();
        private float _weighting;
        private string _subjectColorHex = string.Empty;
        private ButtonStyleBase? _buttonStyle;

        public Subject(string name, float weighting, string subjectColorHex, bool counts)
        {
            this.Name = name;
            this.Weighting = weighting;
            this.Counts = counts;
            this.SubjectColorHex = subjectColorHex;
            
            var isGrid = SettingsManager.Settings?.SubjectButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
        }
        
        [JsonConstructor]
        public Subject(string name, float weighting, string subjectColorHex, List<Grade> grades, bool counts) 
            : this(name, weighting, subjectColorHex, counts)
        {
            this.Grades = grades;
        }

        [JsonInclude]
        public string Name
        {
            get => _name; 
            private set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        [JsonInclude]
        public List<Grade> Grades
        {
            get => _grades;
            private set
            {
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
        public bool Counts { get; private set; } // TODO: Update UI

        [JsonInclude]
        public string SubjectColorHex
        {
            get => _subjectColorHex;
            private set
            {
                if (_subjectColorHex.Equals(value))
                    return;
                _subjectColorHex = value;
                this.RaisePropertyChanged(nameof(SubjectColor));
                this.RaisePropertyChanged(nameof(TitleBrush));
                this.RaisePropertyChanged(nameof(BackgroundBrushHover));
                this.RaisePropertyChanged(nameof(AdditionalInfoColor));
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

        internal Color SubjectColor => Color.Parse(SubjectColorHex);
        internal SolidColorBrush TitleBrush => 
            new(Utilities.AdjustForegroundBrightness(SubjectColor, DarkSubjectTint, LightSubjectTint));

        internal LinearGradientBrush BackgroundBrush
        {
            get
            {
                return Utilities.CreateLinearGradientBrush(new RelativePoint(0, 100, RelativeUnit.Relative),
                    new RelativePoint(100, 0, RelativeUnit.Relative),
                    new[] {SubjectColor, _lightBackground},
                    new[] {0.0, 90.0});
            }
        }
        internal LinearGradientBrush BackgroundBrushHover
        {
            get
            {
                return Utilities.CreateLinearGradientBrush(new RelativePoint(0, 100, RelativeUnit.Relative),
                    new RelativePoint(100, 0, RelativeUnit.Relative),
                    new[] {SubjectColor.DarkenColor(0.075f), SubjectColor.DarkenColor(0.075f)},
                    new[] {0.0, 90.0});
            }
        }
        
        internal SolidColorBrush AdditionalInfoColor => 
            new(Utilities.AdjustForegroundBrightness(SubjectColor, AdditionalInfoDark, AdditionalInfoLight));
        
        internal float RoundedAverage => Utilities.GetAverage(Grades, true);

        private Color DarkSubjectTint => SubjectColor.DarkenColor(0.2f);
        private Color LightSubjectTint => SubjectColor.BrightenColor(0.2f);
        
        private Color AdditionalInfoDark => _additionalInfoColor.DarkenColor(0.3f);
        private Color AdditionalInfoLight => _additionalInfoColor.BrightenColor(0.3f);

        public T? Duplicate<T>() where T : class, IElement
        {
            if (Clone() is not Subject duplicate)
                return null;

            var currentYear = MainWindowViewModel.CurrentYear;
            currentYear?.Subjects.Add(duplicate);

            return duplicate as T;
        }

        public object Clone() => new Subject(_name, _weighting, _subjectColorHex, _grades.Clone().ToList(), Counts);

        internal void Edit(string newName, float newWeighting, string newSubjectColorHex, bool counts)
        {
            this.Name = newName;
            this.Weighting = newWeighting;
            this.Counts = counts;
            this.SubjectColorHex = newSubjectColorHex;
        }
    }
}