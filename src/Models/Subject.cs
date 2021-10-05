using System.Collections.Generic;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public class Subject : IGradable
    {
        private string _name;
        private float _weighting;
        private string _subjectColorHex;
        private readonly List<Grade> _grades;

        /// <summary>
        /// A constructor with no color being passed, which automatically sets a default gray-blue-ish shade by calling
        /// another constructor with 4 parameters.
        /// </summary>
        /// <param name="name">The name of the subject.</param>
        /// <param name="weighting">The weighting of the subject-average for the final year-average.</param>
        /// <param name="grades">The collection of grades included in this subject.</param>
        public Subject(string name, float weighting, IEnumerable<Grade> grades) : this(name, weighting, "#c7cad1", grades) {}
        
        /// <summary>
        /// A constructor with 4 parameters:
        /// </summary>
        /// <param name="name">The name of the subject.</param>
        /// <param name="weighting">The weighting of the subject-average for the final year-average.</param>
        /// <param name="color">The color of the subject, being used as the background color of the button, representing
        ///                     this subject in the User Interface.</param>
        /// <param name="grades">The collection of grades included in this subject.</param>
        public Subject(string name, float weighting, string color, IEnumerable<Grade> grades)
        {
            _name = name;
            _weighting = weighting;
            _subjectColorHex = color;
            _grades = new List<Grade>(grades);
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
        /// The exact average of all grades in this subject (not rounded).
        /// </summary>
        public float GradeValue => Utilities.GetAverage(_grades, false);
        
        /// <summary>
        /// The rounded average of all grades in this subject.
        /// </summary>
        internal float RoundedAverage => (float)System.Math.Round(GradeValue, 2);
        
        /// <summary>
        /// The name of this subject.
        /// </summary>
        internal string Name => _name;
        
        /// <summary>
        /// A collection (list) of all <see cref="Grade">grades</see> in this subject.
        /// </summary>
        internal List<Grade> Grades => _grades;

        internal void ChangeData(string newName) => ChangeData(newName, _weighting);
        internal void ChangeData(float newWeighting) => ChangeData(_name, newWeighting);
        
        /// <summary>
        /// Change the data of this subject.
        /// </summary>
        /// <param name="newName">The new name of this subject.</param>
        /// <param name="newWeighting">The new weighting of the average.</param>
        private void ChangeData(string newName, float newWeighting)
        {
            _name = newName;
            _weighting = newWeighting;
        }
    }
}