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
    public class YearListViewModel : ListViewModelBase, IListViewModel<SchoolYear>
    {
        private ObservableCollection<SchoolYear>? _items;
        
        [Obsolete("Do NOT use this constructor, because it leaves the collection of school years uninitialized " +
                  "and this leads to exceptions and unintended behaviour")]
        public YearListViewModel()
        {
            Instance = this;
            AddPageType = typeof(AddYearWindow);
            AddViewModelType = typeof(AddYearViewModel);
        }
        
#pragma warning disable 618
        public YearListViewModel(IEnumerable<SchoolYear> years) : this()
#pragma warning restore 618
        {
            Items = new ObservableCollection<SchoolYear>(years);
            Items.CollectionChanged += (sender, args) => this.RaisePropertyChanged(nameof(EmptyCollection));
            InitializeTopbarElements();
        }

        public ObservableCollection<SchoolYear>? Items
        {
            get => _items;
            set
            {
                this.RaiseAndSetIfChanged(ref _items, value);
                this.RaisePropertyChanged(nameof(EmptyCollection));
            }
        }
        public bool EmptyCollection => Items?.Count == 0;
        
        internal static YearListViewModel? Instance { get; private set; }

        protected internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            foreach (var control in TopbarTexts!)
                control.IsVisible = false;
        }
        
        private void RemoveElement(SchoolYear year)
        {
            DataManager.SchoolYears.SafeRemove(year);
            UpdateVisualOnChange(this, DataManager.SchoolYears);
        }
    }
}