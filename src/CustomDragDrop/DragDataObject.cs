namespace GradeManagement.CustomDragDrop;

public class DragDataObject
{
    public DragDataObject(object data, IDragObject dragObject)
    {
        this.Data = data;
        this.DragObject = dragObject;
    }
    
    public object Data { get; }
    
    public IDragObject DragObject { get; set; }
}