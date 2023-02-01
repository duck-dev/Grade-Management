using System;
using System.Collections.ObjectModel;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.Models;
using GradeManagement.ResourcesNamespace;
using GradeManagement.ViewModels.AddPages;
using ReactiveUI;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class AddViewModelBase : ViewModelBase
    {
        private string? _elementName;
        private string? _elementWeightingStr;
        private string? _buttonText;
        private string? _title;
        private bool _elementCounts = true;
        private ColorRepresentation _selectedColor;

        private readonly Color[] _elementColors =
        {
            DefaultElementColor, Color.Parse("#FFAE03"), Color.Parse("#EB8934"), Color.Parse("#D64045"), 
            Color.Parse("#FF85FB"), Color.Parse("#A326C9"), Color.Parse("#5F8BB0"), Color.Parse("#6FB3BF"), 
            Color.Parse("#A5B1CC"), Color.Parse("#009B72"), Color.Parse("#74CC31"), Color.Parse("#A8744F")
        };

        protected AddViewModelBase()
        {
            foreach (var color in _elementColors)
            {
                var colorRepresentation = new ColorRepresentation(color);
                ElementColorsCollection.Add(colorRepresentation);
            }

            _selectedColor = SelectedColor = ElementColorsCollection[0];
            SelectedColor.Selected = true;
        }
        
        // Colors for border (incomplete/complete selection)
        protected static Color IncompleteColor => Resources.InvalidColor;

        protected static Color NormalColor => Resources.AppGreen;

        protected virtual bool DataComplete { get; }
        protected SolidColorBrush[]? BorderBrushes { get; init; }
        
        // Indexes of the elements in the UI
        protected int WeightingIndex { get; init; }
        protected int NameIndex { get; init; }

        protected string? Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        protected string? ButtonText
        {
            get => _buttonText;
            set => this.RaiseAndSetIfChanged(ref _buttonText, value);
        }

        protected string? ElementName
        {
            get => _elementName;
            set
            {
                this.RaiseAndSetIfChanged(ref _elementName, value);
                BorderBrushes![NameIndex].Color = NormalColor;
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    BorderBrushes[NameIndex].Color = IncompleteColor;
                
                this.RaisePropertyChanged(nameof(BorderBrushes));
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        protected float ElementWeighting { get; private set; } = float.NaN;
        protected string? ElementWeightingString
        {
            get => _elementWeightingStr;
            set
            {
                this.RaiseAndSetIfChanged(ref _elementWeightingStr, value);
                ElementWeighting = float.NaN;
                BorderBrushes![WeightingIndex].Color = NormalColor;

                if (float.TryParse(value, out float weighting))
                    ElementWeighting = weighting;
                else
                    BorderBrushes![WeightingIndex].Color = IncompleteColor;

                this.RaisePropertyChanged(nameof(BorderBrushes));
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        protected bool ElementCounts
        {
            get => _elementCounts;
            set
            {
                this.RaiseAndSetIfChanged(ref _elementCounts, value);
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        protected ObservableCollection<ColorRepresentation> ElementColorsCollection { get; } = new();

        protected ColorRepresentation SelectedColor
        {
            get => _selectedColor;
            private set
            {
                _selectedColor = value;
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        private static Color DefaultElementColor => Resources.CurrentTheme.ElementBackground;
        
        protected internal override void EraseData()
        {
            ElementName = string.Empty;
            ElementWeightingString = string.Empty;
            ElementCounts = true;
            SelectedColor = ElementColorsCollection[0];
        }

        protected void ChangeColor(ColorRepresentation colorRepresentation)
        {
            SelectedColor.Selected = false;
            colorRepresentation.Selected = true;
            SelectedColor = colorRepresentation;
        }

        internal void EditPageText(AddPageAction action, Type pageType, string suffix = "")
        {
            string type = pageType switch
            {
                { } yearType when yearType == typeof(AddYearViewModel) => "School Year",
                { } subjectType when subjectType == typeof(AddSubjectViewModel) => "Subject",
                { } gradeType when gradeType == typeof(AddGradeViewModel) => "Grade",
                _ => throw new ArgumentException($"Value of parameter \"{nameof(pageType)}\" should be an `AddViewModelBase` inheritor.",
                    nameof(pageType))
            };
            EditPageText(action, type, suffix);
        }

        internal void EditPageText(AddPageAction action, string type, string suffix = "")
        {
            string prefix = action.ToString();
            Title = $"{prefix} {(string.IsNullOrEmpty(suffix) ? type : ($"\"{suffix}\""))}:";
            ButtonText = $"{prefix} {type}";
        }
    }
}