using Avalonia.Controls;
using GradeManagement.Models;
using GradeManagement.Models.Settings;
using GradeManagement.Views;
using ReactiveUI;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class ViewModelBase : ReactiveObject
    {
        protected MainWindow? MainWindowInstance { get; private set; }
        protected Preferences? SettingsRef { get; }
        
        protected Controls? TopbarTexts { get; private set; }
        protected internal Window? CurrentAddWindow { get; internal set; }

        protected ViewModelBase() 
            => this.SettingsRef = SettingsManager.Settings;

        protected void InitializeTopbarElements()
        {
            if (MainWindow.Instance is null)
                return;
            
            this.MainWindowInstance = MainWindow.Instance;
            this.TopbarTexts = this.MainWindowInstance.Get<Grid>("Topbar-Grid").Children;
        }
        
        protected void UpdateVisualOnChange()
        {
            CloseAddWindow();
            DataManager.SaveData();
        }

        protected internal virtual void EraseData() { }

        private void CloseAddWindow()
        {
            CurrentAddWindow?.Close();
            CurrentAddWindow = null;
        }
    }
}