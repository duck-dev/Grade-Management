using System;
using System.Collections.Generic;
using System.Globalization;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels.TargetGrade
{
    public class AverageGradeViewModel : ViewModelBase, ITargetGrade
    {
        private string _gradeString = string.Empty;
        private float _grade = float.NaN;
        private string _weightingString = string.Empty;
        private float _weighting = float.NaN;

        [Obsolete("Do NOT use this constructor, because it leaves the collection of grades uninitialized " +
                  "and this leads to exceptions and unintended behaviour.")]
        public AverageGradeViewModel() { }
        
        public AverageGradeViewModel(IEnumerable<Grade> grades)
            => this.Grades = grades;
        
        public IEnumerable<Grade> Grades { get; set; } = null!;

        private string GradeString
        {
            get => _gradeString;
            set
            {
                _grade = float.TryParse(value, out float grade) ? grade : float.NaN;
                this.RaiseAndSetIfChanged(ref _gradeString, value);
                this.RaisePropertyChanged(nameof(CalculatedAverage));
            }
        }

        private string WeightingString
        {
            get => _weightingString;
            set
            {
                _weighting = float.TryParse(value, out float weighting) ? weighting : float.NaN;
                this.RaiseAndSetIfChanged(ref _weightingString, value);
                this.RaisePropertyChanged(nameof(CalculatedAverage));
            }
        }

        private string CalculatedAverage
        {
            get
            {
                if (float.IsNaN(_grade) || float.IsNaN(_weighting))
                    return "-";
                
                var newGrades = new List<Grade>(Grades)
                {
                    new Grade("[TempGrade]", _grade, _weighting, DateTime.Today, true)
                };
                var result = Utilities.GetAverage(newGrades, true);
                return float.IsNaN(result) ? "-" : result.ToString(CultureInfo.InvariantCulture);
            }
        }

        protected internal override void EraseData()
        {
            // TODO: Erase data
        }
    }
}