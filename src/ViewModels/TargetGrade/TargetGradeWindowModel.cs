using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels.TargetGrade
{
    public class TargetGradeWindowModel : ViewModelBase
    {
        private const string TargetGradeTitle = "Calculate Target Grade";
        private const string AverageTitle = "Calculate Average";

        private static readonly Color _whiteColor = new(255, 255, 255, 255);
        private static readonly Color _blackColor = new(255, 0, 0, 0);
        private readonly List<ViewModelBase> _viewModels = new();
        private ViewModelBase _content;
        private string _windowTitle = TargetGradeTitle;
        private int _currentButton;

        public TargetGradeWindowModel(IEnumerable<Grade> grades)
        {
            var enumerable = grades as Grade[] ?? grades.ToArray();
            _content = Content = new TargetGradeViewModel(enumerable);
            _viewModels.SafeAdd(_content);
            Grades = enumerable;
        }
        
        internal IEnumerable<Grade> Grades { get; set; }

        private string WindowTitle
        {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }
        
        private static Color GreenColor { get; } = new(255,0,155,114);
        private SolidColorBrush[] ButtonColors { get; } = { new(GreenColor), new(GreenColor,0) };
        private SolidColorBrush[] ButtonTextColors { get; } = { new(_whiteColor), new(_blackColor) };
        private FontWeight[] FontWeights { get; } = { FontWeight.Bold, FontWeight.Normal };

        private ViewModelBase Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        private void SwitchCalculator(int selectedButton)
        {
            if (_currentButton == selectedButton)
                return;
            _currentButton = selectedButton;
            
            WindowTitle = _windowTitle.Equals(AverageTitle) ? TargetGradeTitle : AverageTitle;
            var otherButton = Math.Abs(selectedButton - 1); 

            ButtonColors[selectedButton].Opacity = 1;
            ButtonColors[otherButton].Opacity = 0;
            
            ButtonTextColors[selectedButton].Color = _whiteColor;
            ButtonTextColors[otherButton].Color = _blackColor;

            FontWeights[selectedButton] = FontWeight.Bold;
            FontWeights[otherButton] = FontWeight.Normal;

            ToggleControl(selectedButton);
        }

        private void UpdateButtonVisual()
        {
            this.RaisePropertyChanged(nameof(ButtonColors));
            this.RaisePropertyChanged(nameof(ButtonTextColors));
            this.RaisePropertyChanged(nameof(FontWeights));
        }

        private void ToggleControl(int selected)
        {
            var type = selected == 0 ? typeof(TargetGradeViewModel) : typeof(AverageGradeViewModel);
            ViewModelBase viewModel;
            if (_viewModels.Any(x => x.GetType() == type))
                viewModel = _viewModels.Find(x => x.GetType() == type)!;
            else
            {
                viewModel = (ViewModelBase)Activator.CreateInstance(type, this.Grades)!;
                _viewModels.Add(viewModel);
            }

            if (viewModel is ITargetGrade viewModelInterface)
                viewModelInterface.Grades = this.Grades;

            Content = viewModel;
            _content.EraseData();
        }
    }
}