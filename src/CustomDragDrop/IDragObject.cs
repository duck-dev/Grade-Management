namespace GradeManagement.CustomDragDrop;

public interface IDragObject
{
    void OnDragOverTarget(IDropTarget target);
    void OnDragLeaveTarget(IDropTarget target);
}