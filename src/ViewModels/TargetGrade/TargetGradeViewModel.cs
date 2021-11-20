using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels.TargetGrade
{
    public class TargetGradeViewModel : ViewModelBase
    {
        private float _targetAverage;
        private float _weighting;
        
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
        
        protected internal override void EraseData()
        {
            // TODO: Erase data
        }
    }
}