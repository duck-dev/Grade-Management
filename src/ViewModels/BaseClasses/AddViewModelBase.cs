using Avalonia.Media;
using ReactiveUI;

namespace GradeManagement.ViewModels.BaseClasses
{
    public class AddViewModelBase : ViewModelBase
    {
        private string? _elementName;
        
        protected virtual bool DataComplete { get; }
        protected virtual SolidColorBrush[]? BorderBrushes { get; }
        
        protected virtual string? ElementName
        {
            get => _elementName;
            set
            {
                _elementName = value;
                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }

        protected float ElementWeighting { get; set; } = float.NaN;
        protected virtual string? ElementWeightingString
        {
            get => string.Empty;
            set
            {
                ElementWeighting = float.NaN;
                if (float.TryParse(value, out float weighting))
                    ElementWeighting = weighting;

                this.RaisePropertyChanged(nameof(DataComplete));
            }
        }
        protected bool ElementCounts { get; set; } = true;

        protected virtual void CreateElement()
        {
            
        }
    }
}