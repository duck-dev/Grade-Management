using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.TargetGrade
{
    public class TargetGradeView : UserControl
    {
        public TargetGradeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}