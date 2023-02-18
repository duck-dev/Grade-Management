using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.Lists
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