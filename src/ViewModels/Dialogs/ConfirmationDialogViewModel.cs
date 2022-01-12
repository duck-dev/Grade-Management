using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement.ViewModels.Dialogs
{
    public class ConfirmationDialogViewModel : ConfirmationDialogBase
    {
        public ConfirmationDialogViewModel(string title,
            IEnumerable<SolidColorBrush> borderColors,
            IEnumerable<SolidColorBrush> buttonTextColors,
            IEnumerable<string> buttonTexts) : base(title, borderColors, buttonTextColors, buttonTexts)
        { }

        public ConfirmationDialogViewModel(string title,
            IEnumerable<Color> borderColors,
            IEnumerable<Color> buttonTextColors,
            IEnumerable<string> buttonTexts)
            : this(title, 
                   borderColors.Select(x => new SolidColorBrush(x)), 
                   buttonTextColors.Select(x => new SolidColorBrush(x)),
                   buttonTexts)
        { }
        
        private IElement? ElementToRemove { get; init; }

        private void Confirm()
        {
            if (ElementToRemove is null)
                return;
        }

        private void Cancel()
        {
            if (ElementToRemove is null)
                return;
        }

        private void CloseDialog()
        {
            
        }
    }
}