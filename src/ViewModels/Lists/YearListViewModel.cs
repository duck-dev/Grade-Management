using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.ViewModels
{
    public class YearListViewModel : ViewModelBase, IListViewModel<SchoolYear>
    {
        [Obsolete("Do NOT use this constructor, because it leaves the collection of school years uninitialized " +
                  "and this leads to exceptions and unintended behaviour")]
        public YearListViewModel()
        {
            AddPage = new AddYearViewModel();
        }
        
#pragma warning disable 618
        public YearListViewModel(IEnumerable<SchoolYear> years) : this()
#pragma warning restore 618
        {
            Items = new ObservableCollection<SchoolYear>(years);
            InitializeTopbarElements();
        }

        public ObservableCollection<SchoolYear>? Items { get; set; }

        internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            foreach (var control in TopbarTexts!)
                control.IsVisible = false;
        }
    }
}