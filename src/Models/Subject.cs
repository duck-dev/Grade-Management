using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class Subject : IElement, IGradable
    {
        private readonly Color _additionalInfoColor = Color.Parse("#999999");
        private readonly Color _lightBackground = Color.Parse("#c7cad1");

        public Subject(string name, float weighting, string subjectColorHex, bool counts)
        {
            this.Name = name;
            this.Weighting = weighting;
            this.Counts = counts;
            this.SubjectColorHex = subjectColorHex;
        }
        
        [JsonConstructor]
        public Subject(string name, float weighting, string subjectColorHex, List<Grade> grades, bool counts)
        {
            this.Name = name;
            this.Weighting = weighting;
            this.Grades = grades;
            this.Counts = counts;
            this.SubjectColorHex = subjectColorHex;
        }
        
        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude] 
        public List<Grade> Grades { get; } = new();

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
        
        internal void Edit(string newName, float newWeighting, string newSubjectColorHex, bool counts)
        {
            this.Name = newName;
            this.Weighting = newWeighting;
            this.Counts = counts;
            this.SubjectColorHex = newSubjectColorHex;
        }
    }
}