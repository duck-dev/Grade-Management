using Avalonia.Media;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class TargetGradeViewModel : ViewModelBase
    {
        private int _currentButton;

        private static Color GreenColor { get; } = new(255,0,155,114);
        private SolidColorBrush[] ButtonColors { get; } = { new(GreenColor), new(GreenColor,0) };

        protected override void EraseData()
        {
            // TODO: Erase data
        }

        private void SwitchCalculator(int selectedButton)
        {
            if (_currentButton == selectedButton)
                return;
            _currentButton = selectedButton;

            ButtonColors[selectedButton].Opacity = 1;
            ButtonColors[selectedButton == 0 ? 1 : 0].Opacity = 0;

            this.RaisePropertyChanged(nameof(ButtonColors));
        }
    }
}