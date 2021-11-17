using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Views;
using ReactiveUI;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class ViewModelBase : ReactiveObject
    {
        protected MainWindow? MainWindowInstance { get; private set; }
        protected Controls? TopbarTexts { get; private set; }
        protected internal Window? CurrentAddWindow { get; internal set; }

        protected void InitializeTopbarElements()
        {
            if (MainWindow.Instance is null)
                return;
            
            this.MainWindowInstance = MainWindow.Instance;
            this.TopbarTexts = this.MainWindowInstance.Get<Grid>("Topbar-Grid").Children;
        }
        
        protected internal void UpdateVisualOnChange<T>(IListViewModel<T>? viewModel, IEnumerable<T>? collection) 
            where T : IElement
        {
            if(viewModel is not null && collection is not null)
                viewModel.Items = new ObservableCollection<T>(collection);
            
            CloseAddWindow();
            DataManager.SaveData();
        }
        
        private void CloseAddWindow()
        {
            CurrentAddWindow?.Close();
            CurrentAddWindow = null;
        }
    }
}