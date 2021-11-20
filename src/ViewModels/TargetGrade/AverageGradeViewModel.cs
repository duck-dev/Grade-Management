using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels.TargetGrade
{
    public class AverageGradeViewModel : ViewModelBase
    {
        private float _grade;
        private float _weighting;
        
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
        
        protected internal override void EraseData()
        {
            // TODO: Erase data
        }
    }
}