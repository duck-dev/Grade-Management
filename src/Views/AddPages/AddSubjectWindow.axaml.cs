using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.AddPages
{
    public class AddSubjectWindow : Window
    {
        public AddSubjectWindow()
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