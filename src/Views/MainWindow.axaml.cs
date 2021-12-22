using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using GradeManagement.Models;
using GradeManagement.Models.Elements;
using GradeManagement.ViewModels;
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
            _mainWindowModel ??= this.DataContext as MainWindowViewModel;
            _mainWindowModel!.SwitchPage<YearListViewModel, SchoolYear>(DataManager.SchoolYears);
            MainWindowViewModel.CurrentYear = null;
        }
        
        private void OnSubjectPressed(object? sender, PointerPressedEventArgs e)
        {
            _mainWindowModel ??= this.DataContext as MainWindowViewModel;
            _mainWindowModel!.SwitchPage<SubjectListViewModel, Subject>(MainWindowViewModel.CurrentYear!.Subjects);
            MainWindowViewModel.CurrentSubject = null;
        }
    }
}