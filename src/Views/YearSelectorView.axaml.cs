using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views
{
    public class YearSelectorView : UserControl
    {
        public YearSelectorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}