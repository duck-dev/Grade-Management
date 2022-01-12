using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GradeManagement.Views.Dialogs
{
    public class ConfirmationDialogView : UserControl
    {
        public ConfirmationDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}