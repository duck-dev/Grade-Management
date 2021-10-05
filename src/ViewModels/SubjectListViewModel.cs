using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Avalonia.Controls;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Views;

namespace GradeManagement.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class SubjectListViewModel : ViewModelBase, IListViewModel<Subject>
    {
        private readonly bool[] _elementsVisibilities = { true, false, false }; 
<<<<<<< HEAD
        
=======
>>>>>>> origin/main
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