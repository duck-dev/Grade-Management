using Avalonia.Media;
using Avalonia.VisualTree;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class TargetGradeViewModel : ViewModelBase
    {
        private IVisual? _currentButton;
        
        private Color GreenColor { get; } = new(255,0,155,114);
        private SolidColorBrush[] ButtonColors 
            => new [] { new SolidColorBrush(GreenColor), new SolidColorBrush(GreenColor) };

        private SolidColorBrush ButtonC => new SolidColorBrush(GreenColor, 1);

        protected override void EraseData()
        {
            // TODO: Erase data
        }

        private void SwitchCalculator(IVisual button)
        {
            if (_currentButton == button)
                return;
            _currentButton = button;
            
            var secondElement = ButtonColors[1];
            var opacity = secondElement.Opacity;

            // Doesn't work yet, will be functional in the next commit
            ButtonColors[0].Opacity = opacity;
            secondElement.Opacity = opacity == 0 ? 1 : 0;

            this.RaisePropertyChanged(nameof(ButtonColors));
        }
    }
}