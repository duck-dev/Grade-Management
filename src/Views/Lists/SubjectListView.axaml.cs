using Avalonia.Markup.Xaml;
using GradeManagement.Models;

namespace GradeManagement.Views.Lists
{
    public class SubjectListView : DragControl
    {
        public SubjectListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}