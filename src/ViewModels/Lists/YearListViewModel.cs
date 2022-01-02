using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Models.Elements;
using GradeManagement.Models.Settings;
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
            ElementType = typeof(SchoolYear);
            
            bool isGrid = SettingsManager.Settings?.YearButtonStyle == SelectedButtonStyle.Grid;
            ChangeButtonView(isGrid);
        }
        
#pragma warning disable 618
        public YearListViewModel(IEnumerable<SchoolYear> years) : this()
#pragma warning restore 618
        {
            var mainInstance = MainWindowViewModel.Instance;
            
            Items = new ObservableCollection<SchoolYear>(years);
            Items.CollectionChanged += (sender, args) =>
            {
                this.RaisePropertyChanged(nameof(EmptyCollection));
                mainInstance?.UpdateAverage();
            };
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

        public void Duplicate(IElement element) => DuplicateElement<SchoolYear>(element);

        protected internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            foreach (var control in TopbarTexts!)
                control.IsVisible = false;
        }

        private void RemoveElement(SchoolYear year)
        {
            DataManager.SchoolYears.Remove(year);
            Items?.Remove(year);
            UpdateVisualOnChange();
        }
    }
}