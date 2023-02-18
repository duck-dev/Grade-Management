using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels;
using GradeManagement.ViewModels.Lists;

namespace GradeManagement.Models.Elements
{
    public class GradeGroup : Grade, IGradesContainer, ICloneable
    {
        [JsonConstructor]
        public GradeGroup(string name, List<Grade> grades, float weighting, DateTime date, bool counts) 
            : base(name, Utilities.GetAverage(grades, false), null, null, weighting, date, counts)
        {
            this.Grades = grades;
        }

        public GradeGroup(string name, ICollection<Grade> grades, float weighting, DateTime date, bool counts)
            : base(name, Utilities.GetAverage(grades, false), null, null, weighting, date, counts)
        {
            this.Grades = grades.ToList();
        }

        [JsonIgnore]
        public IGradesContainer? ParentContainer { get; set; }

        [JsonInclude]
        public List<Grade> Grades { get; set; }

        [JsonIgnore]
        public override float GradeValue => Utilities.GetAverage(Grades, true);

        [JsonIgnore]
        public override int ElementCount => Grades.Count;

        public new object Clone() => new GradeGroup(Name, Grades.Clone().ToList(), Weighting, Date, Counts);

        public new void Save<T>(T? element = null) where T : Element
        {
            GradeGroup grade = element as GradeGroup ?? this;
            IGradesContainer? container = null;
            if (MainWindowViewModel.Instance?.Content is GradeListViewModel viewModel)
                container = viewModel.GradesContainer;
            container?.Grades.Add(grade);
        }
    }
}