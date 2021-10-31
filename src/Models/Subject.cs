﻿using System.Collections.Generic;
using Avalonia.Media;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public class Subject : IGradable
    {
        private string _name;
        private float _weighting;
        private readonly string _subjectColorHex;
        private readonly List<Grade> _grades;
        private bool _counts;
        
        private readonly Color _additionalInfoDark = Color.Parse("#7a7a7a");
        private readonly Color _additionalInfoLight = Color.Parse("#b8b8b8");

        /// <summary>
        /// A constructor with no color being passed, which automatically sets a default gray-blue-ish shade by calling
        /// another constructor with 4 parameters.
        /// </summary>
        /// <param name="name">The name of the subject.</param>
        /// <param name="weighting">The weighting of the subject-average for the final year-average.</param>
        /// <param name="grades">The collection of grades included in this subject.</param>
        public Subject(string name, float weighting, IEnumerable<Grade> grades) : this(name, weighting, "#c7cad1", 
        grades, true) {}
        
        /// <summary>
        /// A constructor with 4 parameters:
        /// </summary>
        /// <param name="name">The name of the subject.</param>
        /// <param name="weighting">The weighting of the subject-average for the final year-average.</param>
        /// <param name="color">The color of the subject, being used as the background color of the button, representing
        ///                     this subject in the User Interface.</param>
        /// <param name="grades">The collection of grades included in this subject.</param>
        public Subject(string name, float weighting, string color, IEnumerable<Grade> grades, bool counts)
        {
            _name = name;
            _weighting = weighting;
            _subjectColorHex = color;
            _grades = new List<Grade>(grades);
            _counts = counts;
        }
        
        /// <summary>
        /// The weighting of the subject-average for the final year-average.
        /// </summary>
        public float Weighting => _weighting;
        
        /// <summary>
        /// The hex-representation of the subject-color. The color of this subject is being used as the background
        /// color of the button representing this subject in the User Interface.
        /// </summary>
        public string SubjectColorHex => _subjectColorHex;
        
        /// <summary>
        /// The subject-color wrapped in the Avalonia.Media.Color struct. The color of this subject is being used
        /// as the background color of the button representing this subject in the User Interface.
        /// </summary>
        public Color SubjectColor => Color.Parse(_subjectColorHex);

        /// <summary>
        /// The color of the title, being a bit darker than the <see cref="SubjectColor"/>
        /// if the background is bright and vice-versa.
        /// </summary>
        public SolidColorBrush TitleBrush => 
            new(Utilities.AdjustForegroundBrightness(SubjectColor, DarkSubjectTint, LightSubjectTint));

        /// <summary>
        /// The color of the additional information in the User Interface (grades-count and weighting)
        /// with adjusted brightness, based on the background color, being the <see cref="SubjectColor"/>
        /// </summary>
        public SolidColorBrush AdditionalInfoColor => 
            new(Utilities.AdjustForegroundBrightness(SubjectColor, _additionalInfoDark, _additionalInfoLight));

        /// <summary>
        /// The exact average of all grades in this subject (not rounded).
        /// </summary>
        public float GradeValue => Utilities.GetAverage(_grades, false);

        /// <summary>
        /// Does the subject's average count for the school-year's average?
        /// </summary>
        public bool Counts => _counts;
        
        /// <summary>
        /// The rounded average of all grades in this subject.
        /// </summary>
        internal float RoundedAverage => Utilities.GetAverage(_grades, true);
        
        /// <summary>
        /// The name of this subject.
        /// </summary>
        internal string Name => _name;
        
        /// <summary>
        /// A collection (list) of all <see cref="Grade">grades</see> in this subject.
        /// </summary>
        internal List<Grade> Grades => _grades;

        /// <summary>
        /// Darker tint of the <see cref="SubjectColor"/>
        /// </summary>
        private Color DarkSubjectTint => Utilities.DarkenColor(SubjectColor, 0.2f);
        
        /// <summary>
        /// Brighter tint of the <see cref="SubjectColor"/>
        /// </summary>
        private Color LightSubjectTint => Utilities.BrightenColor(SubjectColor, 0.2f);

        internal void ChangeData(string newName) => ChangeData(newName, _weighting, _counts);
        internal void ChangeData(float newWeighting) => ChangeData(_name, newWeighting, _counts);
        internal void ChangeData(bool counts) => ChangeData(_name, _weighting, counts);
        
        /// <summary>
        /// Change the data of this subject.
        /// </summary>
        /// <param name="newName">The new name of this subject.</param>
        /// <param name="newWeighting">The new weighting of the average.</param>
        /// <param name="counts">Determines whether the subject's average counts for the year's average or not.</param>
        private void ChangeData(string newName, float newWeighting, bool counts)
        {
            _name = newName;
            _weighting = newWeighting;
            _counts = counts;
        }
    }
}