using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models.Elements;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.AddPages;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.TargetGrade;
using GradeManagement.Views.AddPages;
using GradeManagement.Views.TargetGrade;
using ReactiveUI;

namespace GradeManagement.ViewModels.Lists
{
    public class GradeListViewModel : ListViewModelBase, IListViewModel<Grade>
    {
        private ObservableCollection<Grade>? _items;
        private TargetGradeWindowModel? _targetGradeWindowModel;
        
        public GradeListViewModel(IEnumerable<Grade> items)
        {
            Instance = this;
            AddPageType = typeof(AddGradeWindow);
            AddViewModelType = typeof(AddGradeViewModel);
            ElementType = typeof(Grade);
            
            bool isGrid = SettingsManager.Settings?.GradeButtonStyle == SelectedButtonStyle.Grid;
            ChangeButtonView(isGrid);
            
            var mainInstance = MainWindowViewModel.Instance;
            Items = new ObservableCollection<Grade>(items);
            Items.CollectionChanged += (sender, args) =>
            {
                this.RaisePropertyChanged(nameof(EmptyCollection));
                mainInstance?.UpdateAverage();
            };
            InitializeTopbarElements();
        }

        public ObservableCollection<Grade>? Items
        {
            get => _items;
            set
            {
                this.RaiseAndSetIfChanged(ref _items, value);
                this.RaisePropertyChanged(nameof(EmptyCollection));
            }
        }
        
        public bool EmptyCollection => Items?.Count == 0;
        
        internal IGradesContainer? GradesContainer { get; set; }
        
        internal static GradeListViewModel? Instance { get; private set; }

        public void Duplicate(IElement element) 
            => DuplicateElement<Grade>(element);

        protected internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            foreach (var grade in TopbarTexts!)
                grade.IsVisible = true;
        }

        private void RemoveElement(Grade grade)
        {
            var currentSubject = MainWindowViewModel.CurrentSubject;
            Action action = () =>
            {
                currentSubject?.Grades.Remove(grade);
                Items?.Remove(grade);
                UpdateVisualOnChange();
            };
            base.RemoveElement(grade, Enums.ElementType.Grade, action);
        }

        private void OpenTargetGradeCalc()
        {
            if (Items is null)
                return;
            var window = new TargetGradeWindow();
            _targetGradeWindowModel ??= new TargetGradeWindowModel(Items);
            _targetGradeWindowModel.ClearData();
            _targetGradeWindowModel.ConfigureViewModels(Items);
            window.DataContext = _targetGradeWindowModel;
            
            Utilities.ShowDialog(window, MainWindowInstance);
        }
    }
}