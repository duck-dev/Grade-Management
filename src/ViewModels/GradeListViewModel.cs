using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class GradeListViewModel : ViewModelBase, IListViewModel<Grade>
    {
        public ObservableCollection<Grade>? Items { get; set; }
    }
}