using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        
        protected void UpdateVisualOnChange<T>(IListViewModel<T>? viewModel, IEnumerable<T>? collection) 
            where T : IElement
        {
            if(viewModel is not null && collection is not null)
                viewModel.Items = new ObservableCollection<T>(collection);
            
            CloseAddWindow();
            DataManager.SaveData();
        }
        
        protected T? ShowDialog<T>(T window, Window? parentWindow, ViewModelBase callerViewModel, int index)
            where T : Window
        {
            if (parentWindow is null)
                return null;
            window.ShowDialog(parentWindow);
            CatchClosingWindow(window, callerViewModel);
            return window;
        }

        private void CatchClosingWindow(Window window, ViewModelBase callerViewModel)
        {
            EventHandler<CancelEventArgs>? closingDel = null;
            closingDel = delegate
            {
                window.Closing -= closingDel;
                if (window.DataContext is not ViewModelBase viewModel)
                    return;
                
                viewModel.EraseData();
            };
            window.Closing += closingDel;
        }

        protected internal virtual void EraseData() { }
        
        private void CloseAddWindow()
        {
            CurrentAddWindow?.Close();
            CurrentAddWindow = null;
        }
    }
}