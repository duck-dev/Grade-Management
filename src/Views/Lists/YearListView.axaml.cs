using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.Lists
{
    public class YearListView : UserControl
    {
        public YearListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}