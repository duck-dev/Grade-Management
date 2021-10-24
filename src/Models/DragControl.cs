using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace GradeManagement.Models
{
    public abstract class DragControl : UserControl
    {
        protected DragControl()
        {
            AddHandler(DragDrop.DropEvent, Drop);
            AddHandler(DragDrop.DragEnterEvent, DragEnter);
            AddHandler(DragDrop.DragLeaveEvent, DragLeave);
        }
        
        internal async void BeginDrag(object? sender, PointerPressedEventArgs args)
        {
            if (sender is not Button button) 
                return;
            
            DataObject data = new();
            data.Set("Button", button);
            await DragDrop.DoDragDrop(args, data, DragDropEffects.Copy);
        }

        private void Drop(object? sender, DragEventArgs args)
        {
            if (sender is not Button button)
                return;
        }
        
        private void DragEnter(object? sender, DragEventArgs args)
        {
            if (sender is not Button button)
                return;
        }

        private void DragLeave(object? sender, RoutedEventArgs args)
        {
            if (sender is not Button button)
                return;
        }
    }
}