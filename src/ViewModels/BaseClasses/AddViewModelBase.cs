using System;
using Avalonia.Media;
using GradeManagement.ViewModels.AddPages;
using ReactiveUI;

namespace GradeManagement.ViewModels.BaseClasses
{
    internal enum AddPageAction
    {
        Create,
        Edit
    }
    
    public abstract class AddViewModelBase : ViewModelBase
    {
        private string? _elementName;
        private string? _elementWeightingStr;
        private string? _buttonText;
        private string? _title;
        private bool _elementCounts = true;
        
        // Colors for border (incomplete/complete selection)
        protected static Color IncompleteColor { get; } = Color.Parse("#D64045");
        protected static Color NormalColor { get; } = Color.Parse("#009b72");

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

        protected internal string? ElementName
        {
            get => _elementName;
            set
            {
                this.RaiseAndSetIfChanged(ref _elementName, value);
                BorderBrushes![NameIndex].Color = NormalColor;
                if (string.IsNullOrEmpty(value))
                    BorderBrushes[NameIndex].Color = IncompleteColor;
                
                this.RaisePropertyChanged(nameof(BorderBrushes));
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        protected float ElementWeighting { get; private set; } = float.NaN;
        protected internal string? ElementWeightingString
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

        protected internal bool ElementCounts
        {
            get => _elementCounts;
            set
            {
                this.RaiseAndSetIfChanged(ref _elementCounts, value);
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        internal void EditPageText(AddPageAction action, Type pageType, string suffix = "")
        {
            string type = pageType switch
            {
                { } yearType when yearType == typeof(AddYearViewModel) => "School Year",
                { } subjectType when subjectType == typeof(AddSubjectViewModel) => "Subject",
                { } gradeType when gradeType == typeof(AddGradeViewModel) => "Grade",
                _ => throw new ArgumentException("The passed `pageType` should be an `AddViewModelBase` inheritor.",
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

        protected internal override void EraseData()
        {
            ElementName = string.Empty;
            ElementWeightingString = string.Empty;
            ElementCounts = true;
        }
    }
}