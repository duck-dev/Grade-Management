using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.UtilityCollection;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace GradeManagement.Models.Elements
{
    public class ColorableElement : ReactiveObject
    {
        private readonly Color _additionalInfoColor = Color.Parse("#999999");
        private readonly Color _lightBackground = Color.Parse("#c7cad1");
        private string _elementColorHex = "#c7cad1";
        
        protected ColorableElement(string elementColorHex)
            => this.ElementColorHex = elementColorHex;

        [JsonInclude]
        public string ElementColorHex
        {
            get => _elementColorHex;
            protected set
            {
                if (_elementColorHex.Equals(value))
                    return;
                _elementColorHex = value;
                ApplyChangedColor();

                this.RaisePropertyChanged(nameof(ElementColor));
                this.RaisePropertyChanged(nameof(TitleBrush));
                this.RaisePropertyChanged(nameof(BackgroundBrushHover));
                this.RaisePropertyChanged(nameof(AdditionalInfoColor));
            }
        }
        
        internal Color ElementColor { get; private set; }

        internal SolidColorBrush? TitleBrush { get; private set; }
        
        internal LinearGradientBrush? BackgroundBrushHover { get; private set; }
        
        internal SolidColorBrush? AdditionalInfoColor { get; private set; }
        
        private Color DarkSubjectTint => ElementColor.DarkenColor(0.2f);
        private Color LightSubjectTint => ElementColor.BrightenColor(0.2f);
        
        private Color AdditionalInfoDark => _additionalInfoColor.DarkenColor(0.2f);
        private Color AdditionalInfoLight => _additionalInfoColor.BrightenColor(0.2f);

        private void ApplyChangedColor()
        {
            ElementColor = Color.Parse(_elementColorHex);
                
            var titleTint 
                = Utilities.AdjustForegroundBrightness(ElementColor, DarkSubjectTint, LightSubjectTint);
            TitleBrush = new SolidColorBrush(titleTint);

            var backgroundGradient = Utilities.CreateLinearGradientBrush(
                new RelativePoint(0, 100, RelativeUnit.Relative),
                new RelativePoint(100, 0, RelativeUnit.Relative),
                new[] { ElementColor.DarkenColor(0.075f), _lightBackground.DarkenColor(0.075f) },
                new[] { 0.0, 100.0 });
            BackgroundBrushHover = backgroundGradient;

            var additionalInfoTint 
                = Utilities.AdjustForegroundBrightness(ElementColor, AdditionalInfoDark, AdditionalInfoLight);
            AdditionalInfoColor = new SolidColorBrush(additionalInfoTint);
        }
    }
}