using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using GradeManagement.UtilityCollection;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class DialogBase : ViewModelBase
    {
        private static readonly Color _darkBorder = Color.Parse("#696969");
        private static readonly Color _lightBorder = Color.Parse("#9c9c9c");
        
        protected DialogBase(string title, 
            IEnumerable<SolidColorBrush> buttonTextColors,
            IEnumerable<string> buttonTexts)
        {
            this.Title = title;
            this.ButtonTextColors = buttonTextColors.ToArray();
            this.ButtonTexts = buttonTexts.ToArray();

            this.BorderColors = ButtonTextColors.Select(x => new SolidColorBrush(
                Utilities.AdjustForegroundBrightness(x.Color, _darkBorder, _lightBorder))).ToArray();
        }
        
        protected string Title { get; init; }

        protected SolidColorBrush[] BorderColors { get; init; }
        protected SolidColorBrush[] ButtonTextColors { get; init; }
        protected string[] ButtonTexts { get; init; }
    }
}