namespace GradeManagement.CustomDragDrop;

public static class DragDropService
{
    public static void StartDragDrop(DragDataObject data)
    {
        DataObject = data;
    }

    public static void StopDragDrop()
    {
        DataObject = null;
    }
    
    public static DragDataObject? DataObject { get; private set; }
}