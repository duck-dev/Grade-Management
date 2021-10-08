using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views
{
    public class AddYearView : UserControl
    {
        public AddYearView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}