using System;
using Avalonia.Media;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class TargetGradeViewModel : ViewModelBase
    {
        private const string TargetGradeTitle = "Calculate Target Grade";
        private const string AverageTitle = "Calculate Average";

        private static readonly SolidColorBrush _whiteColorBrush = new(new Color(255, 255, 255, 255));
        private static readonly SolidColorBrush _blackColorBrush = new(new Color(255, 0, 0, 0));
        private string _windowTitle = TargetGradeTitle;
        private int _currentButton;

        private string WindowTitle
        {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }
        
        private static Color GreenColor { get; } = new(255,0,155,114);
        private SolidColorBrush[] ButtonColors { get; } = { new(GreenColor), new(GreenColor,0) };
        private SolidColorBrush[] ButtonTextColors { get; } = { _whiteColorBrush, _blackColorBrush };
        private FontWeight[] FontWeights { get; } = { FontWeight.Bold, FontWeight.Normal };

        protected override void EraseData()
        {
            // TODO: Erase data
        }

        private void SwitchCalculator(int selectedButton)
        {
            if (_currentButton == selectedButton)
                return;
            _currentButton = selectedButton;

            var otherButton = Math.Abs(selectedButton - 1);

            ButtonColors[selectedButton].Opacity = 1;
            ButtonColors[otherButton].Opacity = 0;

            ButtonTextColors[selectedButton] = _whiteColorBrush;
            ButtonTextColors[otherButton] = _blackColorBrush;

            FontWeights[selectedButton] = FontWeight.Bold;
            FontWeights[otherButton] = FontWeight.Normal;

            WindowTitle = _windowTitle.Equals(AverageTitle) ? TargetGradeTitle : AverageTitle;

            this.RaisePropertyChanged(nameof(ButtonColors));
            this.RaisePropertyChanged(nameof(ButtonTextColors));
            this.RaisePropertyChanged(nameof(FontWeights));
        }
    }
}