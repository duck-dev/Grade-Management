using System;
using GradeManagement.Interfaces;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class Grade : IGradable
    {
        public Grade(string name, float value) : this(name, value, 1, DateTime.Today, true) { }

        public Grade(string name, float value, float weighting, DateTime date, bool counts)
        {
            this.Name = name;
            this.GradeValue = value;
            this.Weighting = weighting;
            this.Date = date;
            this.Counts = counts;
        }
        
        public float GradeValue { get; private set; }
        public float RoundedGrade => (float)Math.Round(GradeValue, 2);
        public float Weighting { get; private set; }
        public bool Counts { get; private set; }
        internal string Name { get; private set; }
        internal DateTime Date { get; private set; }
    }
}