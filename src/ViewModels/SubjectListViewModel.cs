using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class SubjectListViewModel : ViewModelBase, IListViewModel<Subject>
    {
        private readonly bool[] _elementsVisibilities = { true, false, false }; 
        
        [Obsolete("Do NOT use this constructor, because it leaves the collection of school years uninitialized " +
                  "and this leads to exceptions and unintended behaviour")]
        public SubjectListViewModel() { }

        public SubjectListViewModel(IEnumerable<Subject> items)
        {
            Items = new ObservableCollection<Subject>(items);
            InitializeTopbarElements();
        }
        
        public ObservableCollection<Subject>? Items { get; set; }

        internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            for (int i = 0; i < TopbarTexts!.Count; i++)
                TopbarTexts[i].IsVisible = _elementsVisibilities[i];
        }
    }
}