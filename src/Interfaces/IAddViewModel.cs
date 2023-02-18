using GradeManagement.Models.Elements;

namespace GradeManagement.Interfaces
{
    public interface IAddViewModel<in T> where T : Element
    {
        void EditElement(T element);
    }
}