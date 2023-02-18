using System.Text.Json.Serialization;
using Avalonia;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.UtilityCollection;
using ReactiveUI;

// ReSharper disable once CheckNamespace
namespace GradeManagement.Models.Elements
{
    public abstract class ColorableElement : Element
    {
        private const float TitleDarkenFactor = 0.3f;
        private const float TitleBrightenFactor = 0.5f;
        private const float AdditionalInfoDarkenFactor = 0.25f;
        private const float AdditionalInfoBrightenFactor = 0.3f;
        
        private const int GridThresholdTitle = 110;
        private const int ListThresholdTitle = 135;

        private const int GridThresholdAdditionalInfo = 120;
        private const int ListThresholdAdditionalInfo = 135;
        
        private readonly Color _additionalInfoBaseColor = Color.Parse("#999999");
        private readonly Color _lightBackground = Color.Parse("#c7cad1");

        private SolidColorBrush? _additionalInfoGrid;
        private SolidColorBrush? _additionalInfoList;
        private SolidColorBrush? _titleBrushGrid;
        private SolidColorBrush? _titleBrushList;
        private SelectedButtonStyle _buttonStyle;
        
        private string _elementColorHex = "#c7cad1";
        private SolidColorBrush? _additionalInfoBrush;
        
        private bool _darkSymbols;

        protected ColorableElement(string elementColorHex, string name, float weighting = 1f, bool counts = true) : base(name, weighting, counts) 
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
                this.RaisePropertyChanged(nameof(BackgroundBrush));
                this.RaisePropertyChanged(nameof(BackgroundBrushHover));
                this.RaisePropertyChanged(nameof(AdditionalInfoColor));
            }
        }

        internal Color ElementColor { get; private set; }

        internal SolidColorBrush? TitleBrush { get; private set; }
        
        internal LinearGradientBrush? BackgroundBrush { get; private set; }
        
        internal LinearGradientBrush? BackgroundBrushHover { get; private set; }

        internal SolidColorBrush? AdditionalInfoColor
        {
            get => _additionalInfoBrush; 
            private set => this.RaiseAndSetIfChanged(ref _additionalInfoBrush, value);
        }
        
        protected bool DarkSymbols
        {
            get => _darkSymbols;
            set => this.RaiseAndSetIfChanged(ref _darkSymbols, value);
        }
        
        private Color DarkTitleTint => ElementColor.DarkenColor(TitleDarkenFactor);
        private Color LightTitleTint => ElementColor.BrightenColor(TitleBrightenFactor);
        
        private Color AdditionalInfoDark => _additionalInfoBaseColor.DarkenColor(AdditionalInfoDarkenFactor);
        private Color AdditionalInfoLight => _additionalInfoBaseColor.BrightenColor(AdditionalInfoBrightenFactor);
        
        internal void AdjustTextColors(bool isGrid, bool changeButtonStyle = true)
        {
            if(changeButtonStyle)
                _buttonStyle = isGrid ? SelectedButtonStyle.Grid : SelectedButtonStyle.List;
            AdditionalInfoColor = isGrid ? _additionalInfoGrid : _additionalInfoList;
            TitleBrush = isGrid ? _titleBrushGrid : _titleBrushList;
            
            int threshold = isGrid ? GridThresholdAdditionalInfo : ListThresholdAdditionalInfo;
            DarkSymbols = ElementColor.PerceivedBrightness() > threshold;
        }

        private void ApplyChangedColor()
        {
            ElementColor = Color.Parse(_elementColorHex);

            var backgroundGradient = Utilities.CreateLinearGradientBrush(
                new RelativePoint(0, 1, RelativeUnit.Relative),
                new RelativePoint(1, 0, RelativeUnit.Relative),
                new[] { ElementColor, _lightBackground },
                new[] { 0.2, 1.0 });
            BackgroundBrush = backgroundGradient;
            
            var backgroundGradientHover = Utilities.CreateLinearGradientBrush(
                new RelativePoint(0, 1, RelativeUnit.Relative),
                new RelativePoint(1, 0, RelativeUnit.Relative),
                new[] { ElementColor.DarkenColor(0.075f), _lightBackground.DarkenColor(0.075f) },
                new[] { 0.2, 1.0 });
            BackgroundBrushHover = backgroundGradientHover;
            
            SetAdditionalInfoColor();
        }

        private void SetAdditionalInfoColor()
        {
            var gridColor = ElementColor.AdjustForegroundBrightness(AdditionalInfoDark, AdditionalInfoLight, GridThresholdAdditionalInfo);
            _additionalInfoGrid = new SolidColorBrush(gridColor);
            var listColor = ElementColor.AdjustForegroundBrightness(AdditionalInfoDark, AdditionalInfoLight, ListThresholdAdditionalInfo);
            _additionalInfoList = new SolidColorBrush(listColor);

            gridColor = ElementColor.AdjustForegroundBrightness(DarkTitleTint, LightTitleTint, GridThresholdTitle);
            _titleBrushGrid = new SolidColorBrush(gridColor);
            listColor = ElementColor.AdjustForegroundBrightness(DarkTitleTint, LightTitleTint, ListThresholdTitle);
            _titleBrushList = new SolidColorBrush(listColor);
            
            bool isGrid = _buttonStyle == SelectedButtonStyle.Grid;
            AdjustTextColors(isGrid, false);
        }
    }
}