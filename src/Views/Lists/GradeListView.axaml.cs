using Avalonia.Markup.Xaml;
using GradeManagement.Models;

namespace GradeManagement.Views.Lists
{
    public class GradeListView : DragControl
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