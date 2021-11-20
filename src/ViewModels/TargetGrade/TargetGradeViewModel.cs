using System.Collections.Generic;
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
        private float _targetAverage;
        private float _weighting;

        public IEnumerable<Grade> Grades { get; set; } = null!;
        
        private float TargetAverage
        {
            get => _targetAverage;
            set => this.RaiseAndSetIfChanged(ref _targetAverage, value);
        }
        
        private float Weighting
        {
            get => _weighting;
            set => this.RaiseAndSetIfChanged(ref _weighting, value);
        }

        private float NeededGrade
        {
            get
            {
                var previousGradesAverage = Utilities.GetAverage(Grades, false);
                return (_targetAverage * (Grades.Count() + 1) - previousGradesAverage) / _weighting;
            }
        }

        protected internal override void EraseData()
        {
            // TODO: Erase data
        }
    }
}