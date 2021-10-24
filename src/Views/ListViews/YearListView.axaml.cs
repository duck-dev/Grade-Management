using Avalonia.Markup.Xaml;
using GradeManagement.Models;

namespace GradeManagement.Views
{
    public class YearListView : DragControl
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