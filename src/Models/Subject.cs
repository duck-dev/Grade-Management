using System.Collections.Generic;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public class Subject : IGradable
    {
        private string _name;
        private float _weighting;
        private readonly List<Grade> _grades;
        
        public Subject(string name, float weighting, IEnumerable<Grade> grades)
        {
            _name = name;
            _weighting = weighting;
            _grades = new List<Grade>(grades);
        }

        internal string Name => _name;
        public float GradeValue => Utilities.GetAverage(_grades, false);
        internal float RoundedAverage => (float)System.Math.Round(GradeValue, 2);
        public float Weighting => _weighting;
        internal List<Grade> Grades => _grades;

        internal void ChangeData(string newName) => ChangeData(newName, _weighting);
        internal void ChangeData(float newWeighting) => ChangeData(_name, newWeighting);
        private void ChangeData(string newName, float newWeighting)
        {
            _name = newName;
            _weighting = newWeighting;
        }
    }
}