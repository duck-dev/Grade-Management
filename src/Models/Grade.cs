using System;
using System.Globalization;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public class Grade : IGradable
    {
        public Grade(string name, float value, float weighting, DateTime date, bool counts)
        {
            this.Name = name;
            this.GradeValue = value;
            this.Weighting = weighting;
            this.Date = date.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture); // TODO: Use selected culture
            this.Counts = counts;
        }
        
        public bool Counts { get; private set; }
        public float GradeValue { get; private set; }
        public float Weighting { get; private set; }
        internal float RoundedGrade => (float)Math.Round(GradeValue, 2);
        internal string Name { get; private set; }
        internal string Date { get; private set; }
    }
}