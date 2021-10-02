using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Grade_Management.ViewModels;

namespace Grade_Management.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? _mainWindowViewModel;

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
            _mainWindowViewModel ??= this.DataContext as MainWindowViewModel;
            _mainWindowViewModel?.SwitchPage<YearSelectorViewModel>();
        }
        
        private void OnSubjectPressed(object? sender, PointerPressedEventArgs e)
        {
            _mainWindowViewModel ??= this.DataContext as MainWindowViewModel;
            _mainWindowViewModel?.SwitchPage<SubjectSelectorViewModel>();
        }
    }
}