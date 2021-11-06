using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class Subject : IGradable
    {
        private string _name;
        private float _weighting;
        private readonly string _subjectColorHex;
        private readonly List<Grade> _grades;
        private bool _counts;

        private readonly Color _additionalInfoColor = Color.Parse("#999999");
        private readonly Color _lightBackground = Color.Parse("#c7cad1"); // TODO: Will probably be in a special class for all the colors and variations of them
        
        public Subject(string name, float weighting, IEnumerable<Grade> grades) : this(name, weighting, "#c7cad1", 
        grades, true) {}
        
        public Subject(string name, float weighting, string color, IEnumerable<Grade> grades, bool counts)
        {
            _name = name;
            _weighting = weighting;
            _subjectColorHex = color;
            _grades = new List<Grade>(grades);
            _counts = counts;
        }
        
        public float Weighting => _weighting;
        
        public string SubjectColorHex => _subjectColorHex;
        public Color SubjectColor => Color.Parse(_subjectColorHex);
        public SolidColorBrush TitleBrush => 
            new(Utilities.AdjustForegroundBrightness(SubjectColor, DarkSubjectTint, LightSubjectTint));

        public LinearGradientBrush BackgroundBrush
        {
            get
            {
                return Utilities.CreateLinearGradientBrush(new RelativePoint(0, 100, RelativeUnit.Relative),
                    new RelativePoint(100, 0, RelativeUnit.Relative),
                    new[] {SubjectColor, _lightBackground},
                    new[] {0.0, 0.9});
            }
        }

        public LinearGradientBrush BackgroundBrushHover
        {
            get
            {
                return Utilities.CreateLinearGradientBrush(new RelativePoint(0, 100, RelativeUnit.Relative),
                    new RelativePoint(100, 0, RelativeUnit.Relative),
                    new[] {Utilities.DarkenColor(SubjectColor, 0.1f), Utilities.DarkenColor(_lightBackground, 0.1f)},
                    new[] {0.0, 0.9});
            }
        }
        
        public SolidColorBrush AdditionalInfoColor => 
            new(Utilities.AdjustForegroundBrightness(SubjectColor, AdditionalInfoDark, AdditionalInfoLight));
        
        public float GradeValue => Utilities.GetAverage(_grades, false);
        
        public bool Counts => _counts;
        
        internal float RoundedAverage => Utilities.GetAverage(_grades, true);
        
        internal string Name => _name;
        
        internal List<Grade> Grades => _grades;
        
        private Color DarkSubjectTint => Utilities.DarkenColor(SubjectColor, 0.2f);
        private Color LightSubjectTint => Utilities.BrightenColor(SubjectColor, 0.2f);
        
        private Color AdditionalInfoDark => Utilities.DarkenColor(_additionalInfoColor, 0.3f);
        private Color AdditionalInfoLight => Utilities.BrightenColor(_additionalInfoColor, 0.3f);

        internal void ChangeData(string newName) => ChangeData(newName, _weighting, _counts);
        internal void ChangeData(float newWeighting) => ChangeData(_name, newWeighting, _counts);
        internal void ChangeData(bool counts) => ChangeData(_name, _weighting, counts);
        private void ChangeData(string newName, float newWeighting, bool counts)
        {
            _name = newName;
            _weighting = newWeighting;
            _counts = counts;
        }
    }
}