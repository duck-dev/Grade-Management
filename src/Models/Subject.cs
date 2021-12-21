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

namespace GradeManagement.Models
{
    public class Subject : IElement, IGradable, ICloneable
    {
        private readonly Color _additionalInfoColor = Color.Parse("#999999");
        private readonly Color _lightBackground = Color.Parse("#c7cad1");

        public Subject(string name, float weighting, string subjectColorHex, bool counts)
        {
            this.Name = name;
            this.Weighting = weighting;
            this.Counts = counts;
            this.SubjectColorHex = subjectColorHex;
            
            var isGrid = SettingsManager.Settings?.GradeButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
        }
        
        [JsonConstructor]
        public Subject(string name, float weighting, string subjectColorHex, List<Grade> grades, bool counts) 
            : this(name, weighting, subjectColorHex, counts)
        {
            this.Grades = grades;
        }
        
        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude] 
        public List<Grade> Grades { get; private set; } = new();

        [JsonInclude]
        public float Weighting { get; private set; }
        
        [JsonInclude]
        public bool Counts { get; private set; }

        [JsonInclude]
        public string SubjectColorHex { get; private set; }
        
        [JsonIgnore]
        public float GradeValue => Utilities.GetAverage(Grades, false);

        [JsonIgnore]
        public int ElementCount => Grades.Count;
        
        [JsonIgnore] 
        public ButtonStyleBase? ButtonStyle { get; internal set; }

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

        public IEnumerable<T>? Duplicate<T>() where T : IElement
        {
            if (Clone() is not Subject duplicate)
                return null;
            duplicate.Grades = duplicate.Grades.Clone().ToList();
            
            var currentYear = MainWindowViewModel.CurrentYear;
            currentYear?.Subjects.Add(duplicate);

            return currentYear?.Subjects as IEnumerable<T>;
        }

        public object Clone() => this.MemberwiseClone();

        internal void Edit(string newName, float newWeighting, string newSubjectColorHex, bool counts)
        {
            this.Name = newName;
            this.Weighting = newWeighting;
            this.Counts = counts;
            this.SubjectColorHex = newSubjectColorHex;
        }
    }
}