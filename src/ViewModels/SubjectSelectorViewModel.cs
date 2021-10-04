using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class SubjectSelectorViewModel : ViewModelBase, ISelectorViewModel<Subject>
    {
        public SubjectSelectorViewModel(IEnumerable<Subject> items)
        {
            Items = new ObservableCollection<Subject>(items);
        }
        
        public ObservableCollection<Subject>? Items { get; set; }
    }
}