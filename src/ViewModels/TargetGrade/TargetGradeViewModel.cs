using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels.TargetGrade
{
    public class TargetGradeViewModel : ViewModelBase, ITargetGrade
    {
        private string _targetAverageString = string.Empty;
        private float _targetAverage = float.NaN;
        private string _weightingString = string.Empty;
        private float _weighting = float.NaN;

        public IEnumerable<Grade>? Grades { get; set; }
        
        [Obsolete("Do NOT use this constructor, because it leaves the collection of grades uninitialized " +
                  "and this leads to exceptions and unintended behaviour.")]
        public TargetGradeViewModel() { }

        public TargetGradeViewModel(IEnumerable<Grade> grades) 
            => this.Grades = grades;

        private string TargetAverageString
        {
            get => _targetAverageString;
            set
            {
                _targetAverage = float.TryParse(value, out float average) ? average : float.NaN;
                this.RaiseAndSetIfChanged(ref _targetAverageString, value);
                this.RaisePropertyChanged(nameof(NeededGrade));
            }
        }
        
        private string WeightingString
        {
            get => _weightingString;
            set
            {
                _weighting = float.TryParse(value, out float weighting) ? weighting : float.NaN;
                this.RaiseAndSetIfChanged(ref _weightingString, value);
                this.RaisePropertyChanged(nameof(NeededGrade));
            }
        }

        private string NeededGrade
        {
            get
            {
                if (float.IsNaN(_targetAverage) || float.IsNaN(_weighting) || this.Grades is null)
                    return "-";
                
                var previousGrades = Grades.Sum(x => x.GradeValue * x.Weighting);
                var weightingSum = Grades.Sum(x => x.Weighting);
                var result = (_targetAverage * (weightingSum + _weighting) - previousGrades) / _weighting;
                return float.IsNaN(result) ? "-" : result.ToString("F2");
            }
        }

        protected internal override void EraseData()
        {
            // TODO: Erase data
        }
    }
}