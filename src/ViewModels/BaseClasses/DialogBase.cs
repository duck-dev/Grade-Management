using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class DialogBase : ViewModelBase
    {
        protected DialogBase(string title, 
                                         IEnumerable<SolidColorBrush> borderColors, 
                                         IEnumerable<SolidColorBrush> buttonTextColors,
                                         IEnumerable<string> buttonTexts)
        {
            this.Title = title;
            this.BorderColors = borderColors.ToArray();
            this.ButtonTextColors = buttonTextColors.ToArray();
            this.ButtonTexts = buttonTexts.ToArray();
        }
        
        protected string Title { get; init; }
        protected SolidColorBrush[] BorderColors { get; init; }
        protected SolidColorBrush[] ButtonTextColors { get; init; }
        protected string[] ButtonTexts { get; init; }
    }
}