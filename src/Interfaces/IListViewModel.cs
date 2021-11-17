using System.Collections.ObjectModel;

namespace GradeManagement.Interfaces
{
    public interface IListViewModel<T>
    {
        ObservableCollection<T>? Items { get; set; }
        bool EmptyCollection { get; }
        
        void Duplicate(IElement element);
    }
}