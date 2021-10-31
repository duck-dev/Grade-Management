using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Views;

namespace GradeManagement.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class GradeListViewModel : ViewModelBase, IListViewModel<Grade>
    {
        [Obsolete("Do NOT use this constructor, because it leaves the collection of grades uninitialized " +
                         "and this leads to exceptions and unintended behaviour.")]
        public GradeListViewModel() 
        {
            AddPage = new AddGradeWindow();
            AddPageType = typeof(AddGradeWindow);
            AddViewModelType = typeof(AddGradeViewModel);
        }
        
#pragma warning disable 618
        public GradeListViewModel(IEnumerable<Grade> items) : this()
#pragma warning restore 618
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