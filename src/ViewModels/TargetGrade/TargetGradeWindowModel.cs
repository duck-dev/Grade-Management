using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Elements;
using GradeManagement.ViewModels.BaseClasses;
using ReactiveUI;

namespace GradeManagement.ViewModels.TargetGrade
{
    public class TargetGradeWindowModel : ViewModelBase
    {
        private const string TargetGradeTitle = "Calculate Target Grade";
        private const string AverageTitle = "Calculate Average";

        private static readonly Color _greenColor = new(255,0,155,114);
        private static readonly Color _whiteColor = new(255, 255, 255, 255);
        private static readonly Color _blackColor = new(255, 0, 0, 0);
        private static readonly Color _darkerGreenColor = _greenColor.DarkenColor(0.075f);
        private static readonly Color _darkerDefaultColor = Color.Parse("#D8DDE6").DarkenColor(0.075f);

        private readonly List<ViewModelBase> _viewModels = new();
        private ViewModelBase? _content;
        private string _windowTitle = TargetGradeTitle;
        private int _currentButton;

        public TargetGradeWindowModel(IEnumerable<Grade> grades)
        {
            var enumerable = grades as Grade[] ?? grades.ToArray();
            var viewModel = new TargetGradeViewModel(enumerable);
            _content = Content = viewModel;
            _viewModels.SafeAdd(_content);
            
            ClearData();
            
            Grades = enumerable;
            viewModel.Grades = this.Grades;
        }
        
        internal IEnumerable<Grade>? Grades { get; set; }

        private string WindowTitle
        {
            get => _windowTitle;
            set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
        }
        
        private ObservableCollection<SolidColorBrush> ButtonColors { get; } 
            = new() { new SolidColorBrush(_greenColor), new SolidColorBrush(_greenColor,0) };
        
        private ObservableCollection<SolidColorBrush> ButtonColorsHover { get; } 
            = new()
            {
                new SolidColorBrush(_greenColor.DarkenColor(0.075f)),
                new SolidColorBrush(_darkerDefaultColor)
            };
        
        private ObservableCollection<SolidColorBrush> ButtonTextColors { get; } 
            = new() { new SolidColorBrush(_whiteColor), new SolidColorBrush(_blackColor) };
        
        // ReSharper disable once CollectionNeverQueried.Local
        private ObservableCollection<FontWeight> FontWeights { get; } 
            = new() { FontWeight.Bold, FontWeight.Normal };

        private ViewModelBase? Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        internal void ConfigureViewModels(IEnumerable<Grade> grades)
        {
            this.Grades = grades;
            if(GetViewModel(typeof(TargetGradeViewModel)) is TargetGradeViewModel targetGrade)
                targetGrade.Grades = grades;
            if(GetViewModel(typeof(AverageGradeViewModel)) is AverageGradeViewModel averageGrade)
                averageGrade.Grades = grades;
        }

        internal void ClearData()
        {
            if(GetViewModel(typeof(TargetGradeViewModel)) is TargetGradeViewModel targetGrade)
                targetGrade.EraseData();
            if(GetViewModel(typeof(AverageGradeViewModel)) is AverageGradeViewModel averageGrade)
                averageGrade.EraseData();
        }

        private void SwitchCalculator(int selectedButton)
        {
            if (_currentButton == selectedButton)
                return;
            _currentButton = selectedButton;
            
            WindowTitle = _windowTitle.Equals(AverageTitle) ? TargetGradeTitle : AverageTitle;
            
            ChangeButtons(selectedButton);
            ToggleControl(selectedButton);
        }

        private void ChangeButtons(int selectedButton)
        {
            var otherButton = Math.Abs(selectedButton - 1); 

            ButtonColors[selectedButton].Opacity = 1;
            ButtonColors[otherButton].Opacity = 0;

            ButtonColorsHover[selectedButton].Color = _darkerGreenColor;
            ButtonColorsHover[otherButton].Color = _darkerDefaultColor;
            
            ButtonTextColors[selectedButton].Color = _whiteColor;
            ButtonTextColors[otherButton].Color = _blackColor;

            FontWeights[selectedButton] = FontWeight.Bold;
            FontWeights[otherButton] = FontWeight.Normal;
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
            
            Content = viewModel;
            _content?.EraseData();
            
            if (viewModel is ITargetGrade viewModelInterface)
                viewModelInterface.Grades = this.Grades;
        }
        
        private ViewModelBase? GetViewModel(Type type)
            => _viewModels.FirstOrDefault(x => x.GetType() == type);
    }
}