using Avalonia.Controls;
using GradeManagement.Views;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected MainWindow? MainWindowInstance { get; private set; }
        protected Controls? TopbarTexts { get; private set; }

        protected void InitializeTopbarElements()
        {
            if (MainWindow.Instance is null)
                return;
            
            this.MainWindowInstance = MainWindow.Instance;
            this.TopbarTexts = this.MainWindowInstance.Get<Grid>("Topbar-Grid").Children;
        }

        internal virtual void ChangeTopbar()
        {
            if (TopbarTexts is null)
                throw new System.ArgumentNullException();
        }
    }
}