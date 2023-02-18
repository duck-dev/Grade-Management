using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models.Elements;
using GradeManagement.Models.Settings;
using GradeManagement.ViewModels.AddPages;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.Views.AddPages;
using ReactiveUI;

namespace GradeManagement.ViewModels.Lists
{
    public class SubjectListViewModel : ListViewModelBase, IListViewModel<Subject>
    {
        private readonly bool[] _elementsVisibilities = { true, false, false, false, false, false, false };
        private ObservableCollection<Subject>? _items;

        public SubjectListViewModel(IEnumerable<Subject> items)
        {
            Instance = this;
            AddPageType = typeof(AddSubjectWindow);
            AddViewModelType = typeof(AddSubjectViewModel);
            ElementType = typeof(Subject);
            
            bool isGrid = SettingsManager.Settings?.SubjectButtonStyle == SelectedButtonStyle.Grid;
            ChangeButtonView(isGrid);
            
            var mainInstance = MainWindowViewModel.Instance;
            Items = new ObservableCollection<Subject>(items);
            Items.CollectionChanged += (sender, args) =>
            {
                this.RaisePropertyChanged(nameof(EmptyCollection));
                mainInstance?.UpdateAverage();
            };
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

        public void Duplicate(Element element) 
            => DuplicateElement<Subject>(element);

        protected internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            for (int i = 0; i < TopbarTexts!.Count; i++)
                TopbarTexts[i].IsVisible = _elementsVisibilities[i];
        }

        private void RemoveElement(Subject subject)
        {
            var currentYear = MainWindowViewModel.CurrentYear;
            Action action = () =>
            {
                currentYear?.Subjects.Remove(subject);
                Items?.Remove(subject);
                UpdateVisualOnChange();
            };
            base.RemoveElement(subject, Enums.ElementType.Subject, action);
        }
    }
}