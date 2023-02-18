using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Models.Elements;
using GradeManagement.ViewModels;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;

namespace GradeManagement.Views
{
    public class MainWindow : Window
    {
        private MainWindowViewModel? _mainWindowModel;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Instance = this;
        }

        internal static MainWindow? Instance { get; private set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnYearPressed(object? sender, PointerPressedEventArgs e)
        {
            SwitchPage<YearListViewModel, SchoolYear>(DataManager.SchoolYears);
            MainWindowViewModel.CurrentYear = null;
        }
        
        private void OnSubjectPressed(object? sender, PointerPressedEventArgs e)
        {
            SwitchPage<SubjectListViewModel, Subject>(MainWindowViewModel.CurrentYear!.Subjects);
        }

        private void OnGradePressed(object? sender, PointerPressedEventArgs e)
        {
            // ReSharper disable once MergeIntoPattern
            if (MainWindowViewModel.Instance?.Content is GradeListViewModel viewModel && viewModel.GradesContainer?.ParentContainer != null)
                SwitchPage<GradeListViewModel, Grade>(viewModel.GradesContainer.ParentContainer.Grades, viewModel.GradesContainer.ParentContainer);
        }

        private void SwitchPage<TViewModel, TElement>(IEnumerable<TElement> elements, IGradesContainer? gradesContainer = null)
            where TViewModel : ListViewModelBase, IListViewModel<TElement> 
            where TElement : Element, IGradable
        {
            _mainWindowModel ??= this.DataContext as MainWindowViewModel;
            _mainWindowModel!.SwitchPage<TViewModel, TElement>(elements, gradesContainer);
        }
    }
}