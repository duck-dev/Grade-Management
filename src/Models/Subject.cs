using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class Subject : IGradable
    {
        private readonly string _subjectColorHex;

        private readonly Color _additionalInfoColor = Color.Parse("#999999");
        private readonly Color _lightBackground = Color.Parse("#c7cad1");
        
        public Subject(string name, float weighting, IEnumerable<Grade> grades) : this(name, weighting, "#c7cad1", 
        grades, true) {}
        
        public Subject(string name, float weighting, string color, IEnumerable<Grade> grades, bool counts)
        {
            this.Name = name;
            this.Weighting = weighting;
            this.Grades = new List<Grade>(grades);
            this.Counts = counts;
            _subjectColorHex = color;
        }
        
        [JsonInclude]
        public string Name { get; private set; }
        
        [JsonInclude]
        public List<Grade> Grades { get; }

        [JsonInclude]
        public float Weighting { get; private set; }
        
        [JsonInclude]
        public bool Counts { get; private set; }

        [JsonInclude]
        public string SubjectColorHex => _subjectColorHex;
        
        [JsonIgnore]
        public float GradeValue => Utilities.GetAverage(Grades, false);
        
        internal Color SubjectColor => Color.Parse(_subjectColorHex);
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
                    new[] {Utilities.DarkenColor(SubjectColor, 0.075f), Utilities.DarkenColor(_lightBackground, 0.075f)},
                    new[] {0.0, 90.0});
            }
        }
        
        internal SolidColorBrush AdditionalInfoColor => 
            new(Utilities.AdjustForegroundBrightness(SubjectColor, AdditionalInfoDark, AdditionalInfoLight));
        
        internal float RoundedAverage => Utilities.GetAverage(Grades, true);

        private Color DarkSubjectTint => Utilities.DarkenColor(SubjectColor, 0.2f);
        private Color LightSubjectTint => Utilities.BrightenColor(SubjectColor, 0.2f);
        
        private Color AdditionalInfoDark => Utilities.DarkenColor(_additionalInfoColor, 0.3f);
        private Color AdditionalInfoLight => Utilities.BrightenColor(_additionalInfoColor, 0.3f);

        internal void ChangeData(string newName) => ChangeData(newName, Weighting, Counts);
        internal void ChangeData(float newWeighting) => ChangeData(Name, newWeighting, Counts);
        internal void ChangeData(bool counts) => ChangeData(Name, Weighting, counts);
        private void ChangeData(string newName, float newWeighting, bool counts)
        {
            this.Name = newName;
            this.Weighting = newWeighting;
            this.Counts = counts;
        }
    }
}