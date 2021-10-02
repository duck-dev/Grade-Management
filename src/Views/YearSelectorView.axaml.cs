using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Grade_Management.Views
{
    public class YearSelectorView : UserControl
    {
        public YearSelectorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}