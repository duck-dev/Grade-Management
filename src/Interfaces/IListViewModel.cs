using System.Collections.ObjectModel;
using GradeManagement.Models.Elements;

namespace GradeManagement.Interfaces
{
    public interface IListViewModel<T> where T : Element
    {
        ObservableCollection<T>? Items { get; set; }
        bool EmptyCollection { get; }
        
        void Duplicate(Element element);
    }
}