using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models.Settings;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class DialogBase : ViewModelBase
    {
        private static readonly Color _darkBorder = Color.Parse("#696969");
        private static readonly Color _lightBorder = Color.Parse("#9c9c9c");
        
        protected DialogBase(string title,
            IEnumerable<SolidColorBrush> buttonColors,
            IEnumerable<SolidColorBrush> buttonTextColors,
            IEnumerable<string> buttonTexts)
        {
            this.Title = title;
            this.ButtonColors = buttonColors.ToArray();
            this.ButtonTextColors = buttonTextColors.ToArray();
            this.ButtonTexts = buttonTexts.ToArray();
        }
        
        protected string Title { get; init; }
        
        protected SolidColorBrush[] ButtonColors { get; init; }
        protected SolidColorBrush[] ButtonColorsHover
            => ButtonColors.Select(x => new SolidColorBrush(x.Color.DarkenColor(0.1f))).ToArray();
        protected SolidColorBrush[] ButtonTextColors { get; init; }
        
        protected string[] ButtonTexts { get; init; }
        
        protected bool IgnoreDialog { get; set; }
        
        protected static void CloseDialog()
        {
            if (MainWindowViewModel.Instance is not { } mainInstance)
                return;
            mainInstance.Content.CurrentDialog = null;
        }

        protected void CheckIgnoreDialog()
        {
            if (!IgnoreDialog || SettingsManager.Settings is not { } settings) 
                return;
            settings.ShowRemoveConfirmation = false;
            SettingsManager.SaveSettings();
        }
    }
}