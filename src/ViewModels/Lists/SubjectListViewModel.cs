using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.AddPages;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.Views.AddPages;
using ReactiveUI;

namespace GradeManagement.ViewModels.Lists
{
    public class SubjectListViewModel : ListViewModelBase, IListViewModel<Subject>
    {
        private readonly bool[] _elementsVisibilities = { true, false, false };
        private ObservableCollection<Subject>? _items;

        [Obsolete("Do NOT use this constructor, because it leaves the collection of subjects uninitialized " +
                  "and this leads to exceptions and unintended behaviour")]
        public SubjectListViewModel()
        {
            Instance = this;
            AddPageType = typeof(AddSubjectWindow);
            AddViewModelType = typeof(AddSubjectViewModel);
        }

#pragma warning disable 618
        public SubjectListViewModel(IEnumerable<Subject> items) : this()
#pragma warning restore 618
        {
            Items = new ObservableCollection<Subject>(items);
            Items.CollectionChanged += (sender, args) => this.RaisePropertyChanged(nameof(EmptyCollection));
            InitializeTopbarElements();
        }
        
        public ObservableCollection<Subject>? Items
        {
            get => _items;
            set
            {
                this.RaiseAndSetIfChanged(ref _items, value);
                this.RaisePropertyChanged(nameof(EmptyCollection));
            }
        }
        public bool EmptyCollection => Items?.Count == 0;
        
        internal static SubjectListViewModel? Instance { get; private set; }

        protected internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            for (int i = 0; i < TopbarTexts!.Count; i++)
                TopbarTexts[i].IsVisible = _elementsVisibilities[i];
        }
        
        private void RemoveElement(Subject subject)
        {
            var currentYear = MainWindowViewModel.CurrentYear;
            if (currentYear is null)
                return;
            
            currentYear.Subjects.SafeRemove(subject);
            UpdateVisualOnChange(this, currentYear.Subjects);
        }
    }
}