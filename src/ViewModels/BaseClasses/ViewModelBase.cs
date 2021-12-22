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
        
        protected void UpdateVisualOnChange()
        {
            CloseAddWindow();
            DataManager.SaveData();
        }
        
        protected T? ShowDialog<T>(T window, Window? parentWindow, ViewModelBase callerViewModel)
            where T : Window
        {
            if (parentWindow is null)
                return null;
            window.ShowDialog(parentWindow);
            CatchClosingWindow(window, callerViewModel);
            return window;
        }
        
        protected internal virtual void EraseData() { }

        private static void CatchClosingWindow(Window window, ViewModelBase callerViewModel)
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

        private void CloseAddWindow()
        {
            CurrentAddWindow?.Close();
            CurrentAddWindow = null;
        }
    }
}