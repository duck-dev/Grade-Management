using System;
using System.ComponentModel;
using Avalonia.Controls;
using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement.UtilityCollection
{
    public static partial class Utilities
    {
        public static TWindow? ShowAddPage<TWindow, TViewModel>(out TViewModel? viewModel, Window? parentWindow) 
            where TWindow : Window, new() 
            where TViewModel : AddViewModelBase, new()
        {
            var window = new TWindow();
            viewModel = new TViewModel();
            
            window.DataContext = viewModel;
            viewModel.CurrentAddWindow = window;

            return ShowDialog(window, parentWindow);
        }
        
        public static Window? ShowAddPage(Type? windowType, Type? viewModelType, Window? parentWindow)
        {
            if (windowType is null || viewModelType is null || Activator.CreateInstance(windowType) is not Window window)
                return null;
            
            var viewModel = (AddViewModelBase)Activator.CreateInstance(viewModelType)!;
            window.DataContext = viewModel;
            viewModel.CurrentAddWindow = window;
        
            return ShowDialog(window, parentWindow);
        }
        
        public static T? ShowDialog<T>(T window, Window? parentWindow)
            where T : Window
        {
            if (parentWindow is null)
                return null;
            window.ShowDialog(parentWindow);
            CatchClosingWindow(window);
            return window;
        }
        
        public static void CatchClosingWindow(Window window)
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
    }
}