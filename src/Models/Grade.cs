using System;
using GradeManagement.Interfaces;

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
        public float Weighting { get; init; }
        internal string Name { get; init; }
        internal DateTime Date { get; init; }
        internal bool Counts { get; init; }
    }
}