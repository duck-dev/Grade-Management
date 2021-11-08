using Avalonia.Media;
using ReactiveUI;

namespace GradeManagement.ViewModels.BaseClasses
{
    public class AddViewModelBase : ViewModelBase
    {
        private string? _elementName;
        private string? _elementWeightingStr;

        protected virtual bool DataComplete { get; }
        protected SolidColorBrush[]? BorderBrushes { get; init; }
        
        // Indexes of the elements in the UI
        protected int WeightingIndex { get; init; }
        protected int NameIndex { get; init; }
        
        // Colors for border (incomplete/complete selection)
        protected static Color IncompleteColor { get; } = Color.Parse("#D64045");
        protected static Color NormalColor { get; } = Color.Parse("#009b72");

        protected string? ElementName
        {
            get => _elementName;
            set
            {
                _elementName = value;
                BorderBrushes![NameIndex].Color = NormalColor;
                if (string.IsNullOrEmpty(value))
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
                _elementWeightingStr = value;
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
        protected bool ElementCounts { get; set; } = true;

        protected virtual void CreateElement()
        {
            
        }
    }
}