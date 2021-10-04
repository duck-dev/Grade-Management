using System.Collections.ObjectModel;

namespace GradeManagement.Interfaces
{
    public interface ISelectorViewModel<T>
    {
        ObservableCollection<T>? Items { get; set; }
    }
}