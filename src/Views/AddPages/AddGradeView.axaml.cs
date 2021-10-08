using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views
{
    public class AddGradeView : UserControl
    {
        public AddGradeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}