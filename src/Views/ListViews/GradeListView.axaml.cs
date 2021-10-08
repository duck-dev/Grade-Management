using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views
{
    public class GradeListView : UserControl
    {
        public GradeListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}