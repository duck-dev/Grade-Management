using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class GradeListViewModel : ViewModelBase, IListViewModel<Grade>
    {
        [System.Obsolete("Do NOT use this constructor, because it leaves the collection of grades uninitialized " +
                  "and this leads to exceptions and unintended behaviour.")]
        public GradeListViewModel() { }
        
        public GradeListViewModel(IEnumerable<Grade> items)
        {
            Items = new ObservableCollection<Grade>(items);
            InitializeTopbarElements();
        }
        
        public ObservableCollection<Grade>? Items { get; set; }

        internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            foreach (var grade in TopbarTexts!)
                grade.IsVisible = true;
        }
    }
}