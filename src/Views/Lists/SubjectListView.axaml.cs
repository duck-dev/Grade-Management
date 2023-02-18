using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.Lists
{
    public class SubjectListView : UserControl
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