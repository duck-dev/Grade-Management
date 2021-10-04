using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.ViewModels
{
    public class YearSelectorViewModel : ViewModelBase, ISelectorViewModel<SchoolYear>
    {
        [Obsolete("Do NOT use this constructor, because it leaves the collection of school years uninitialized " +
                  "and this leads to exceptions and unintended behaviour")]
        public YearSelectorViewModel() { }
        
        public YearSelectorViewModel(IEnumerable<SchoolYear> years)
        {
            Items = new ObservableCollection<SchoolYear>(years);
        }

        public ObservableCollection<SchoolYear>? Items { get; set; }
    }
}