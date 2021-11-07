using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.AddPages;
using GradeManagement.Views.AddPages;

namespace GradeManagement.ViewModels.Lists
{
    public class YearListViewModel : ListViewModelBase, IListViewModel<SchoolYear>
    {
        [Obsolete("Do NOT use this constructor, because it leaves the collection of school years uninitialized " +
                  "and this leads to exceptions and unintended behaviour")]
        public YearListViewModel()
        {
            AddPage = new AddYearWindow();
            AddPageType = typeof(AddYearWindow);
            AddViewModelType = typeof(AddYearViewModel);
        }
        
#pragma warning disable 618
        public YearListViewModel(IEnumerable<SchoolYear> years) : this()
#pragma warning restore 618
        {
            Items = new ObservableCollection<SchoolYear>(years);
            InitializeTopbarElements();
        }

        public ObservableCollection<SchoolYear>? Items { get; set; }
        public bool EmptyCollection => Items?.Count == 0;

        internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            foreach (var control in TopbarTexts!)
                control.IsVisible = false;
        }
    }
}