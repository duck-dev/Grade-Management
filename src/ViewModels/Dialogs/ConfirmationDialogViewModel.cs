using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement.ViewModels.Dialogs
{
    public class ConfirmationDialogViewModel : DialogBase
    {
        private enum ActionType
        {
            Confirm,
            Cancel
        }
        
        private readonly Action? _confirmAction;
        private readonly Action? _cancelAction;

        public ConfirmationDialogViewModel(string title,
            IEnumerable<SolidColorBrush> buttonColors,
            IEnumerable<SolidColorBrush> buttonTextColors,
            IEnumerable<string> buttonTexts,
            Action? confirmAction,
            Action? cancelAction = null) : base(title, buttonColors, buttonTextColors, buttonTexts)
        {
            _confirmAction = confirmAction;
            _cancelAction = cancelAction;
        }

        public ConfirmationDialogViewModel(string title,
            IEnumerable<Color> buttonColors,
            IEnumerable<Color> buttonTextColors,
            IEnumerable<string> buttonTexts,
            Action? confirmAction,
            Action? cancelAction = null)
            : this(title,
                   buttonColors.Select(x => new SolidColorBrush(x)),
                   buttonTextColors.Select(x => new SolidColorBrush(x)),
                   buttonTexts,
                   confirmAction,
                   cancelAction)
        { }

        private void Command(ActionType actionType)
        {
            CloseDialog();
            if (actionType == ActionType.Confirm)
                _confirmAction?.Invoke();
            else if(actionType == ActionType.Cancel)
                _cancelAction?.Invoke();
        }

        private void CloseDialog()
        {
            if (MainWindowViewModel.Instance is not { } mainInstance)
                return;
            mainInstance.Content.CurrentDialog = null;
        }
    }
}