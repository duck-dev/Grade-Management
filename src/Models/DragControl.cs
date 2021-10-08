using Avalonia.Controls;
using Avalonia.Input;

namespace GradeManagement.Models
{
    public abstract class DragControl : UserControl
    {
        internal async void BeginDrag(object sender, PointerPressedEventArgs args)
        {
            System.Diagnostics.Trace.WriteLine("Begin Drag!");
            DataObject data = new();
            await DragDrop.DoDragDrop(args, data, DragDropEffects.Move);
        }
    }
}