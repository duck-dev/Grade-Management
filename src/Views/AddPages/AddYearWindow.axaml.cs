using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.AddPages
{
    public class AddYearWindow : Window
    {
        public AddYearWindow()
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
    }
}