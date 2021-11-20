using System;
using System.Collections.Generic;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels.TargetGrade
{
    public class AverageGradeViewModel : ViewModelBase, ITargetGrade
    {
        private float _grade;
        private float _weighting;

        public IEnumerable<Grade> Grades { get; set; } = null!;

        private float Grade
        {
            get => _grade;
            set => this.RaiseAndSetIfChanged(ref _grade, value);
        }

        private float Weighting
        {
            get => _weighting;
            set => this.RaiseAndSetIfChanged(ref _weighting, value);
        }

        private float CalculatedAverage
        {
            get
            {
                var newGrades = new List<Grade>(Grades)
                {
                    new Grade("[TempGrade]", _grade, _weighting, DateTime.Today, true)
                };
                return Utilities.GetAverage(newGrades, true);
            }
        }

        protected internal override void EraseData()
        {
            // TODO: Erase data
        }
    }
}