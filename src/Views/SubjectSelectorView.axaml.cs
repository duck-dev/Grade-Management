using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Grade_Management.Views
{
    public class SubjectSelectorView : UserControl
    {
        public SubjectSelectorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}