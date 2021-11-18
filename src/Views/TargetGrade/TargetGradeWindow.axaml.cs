using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.TargetGrade
{
    public class TargetGradeWindow : Window
    {
        public TargetGradeWindow()
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