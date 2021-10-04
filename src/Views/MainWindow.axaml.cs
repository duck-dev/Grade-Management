using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using GradeManagement.Models;
using GradeManagement.ViewModels;

namespace GradeManagement.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _mainWindowModel;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void OnYearPressed(object? sender, PointerPressedEventArgs e)
        {
            _mainWindowModel ??= this.DataContext as MainWindowViewModel;
            _mainWindowModel!.SwitchPage<YearSelectorViewModel, SchoolYear>(Data.SchoolYears);
            MainWindowViewModel.CurrentYear = null;
        }
        
        private void OnSubjectPressed(object? sender, PointerPressedEventArgs e)
        {
            _mainWindowModel ??= this.DataContext as MainWindowViewModel;
            _mainWindowModel!.SwitchPage<SubjectSelectorViewModel, Subject>(MainWindowViewModel.CurrentYear!.Subjects);
        }
    }
}