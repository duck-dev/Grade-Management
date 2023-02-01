using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models.Elements
{
    public class GradeGroup : Grade
    {
        [JsonConstructor]
        public GradeGroup(string name, IEnumerable<Grade> grades, float weighting, DateTime date, bool counts) 
            : base(name, Utilities.GetAverage(grades, false), weighting, date, counts)
        {
            this.Grades = grades.ToList();
        }

        [JsonInclude]
        public List<Grade> Grades { get; set; }

        [JsonInclude]
        public override float GradeValue => Utilities.GetAverage(Grades, true);

        [JsonIgnore]
        public override int ElementCount => Grades.Count;
    }
}