using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models.Elements;
using GradeManagement.Models.Settings;
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
        
        [Obsolete("Do NOT use this constructor, because it leaves the collection of grades uninitialized " +
                  "and this leads to exceptions and unintended behaviour.")]
        public GradeListViewModel()
        {
            Instance = this;
            AddPageType = typeof(AddGradeWindow);
            AddViewModelType = typeof(AddGradeViewModel);
            
            bool isGrid = SettingsManager.Settings?.GradeButtonStyle == SelectedButtonStyle.Grid;
            ChangeButtonView(isGrid);
        }
        
#pragma warning disable 618
        public GradeListViewModel(IEnumerable<Grade> items) : this()
#pragma warning restore 618
        {
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
        
        internal static GradeListViewModel? Instance { get; private set; }

        public void Duplicate(IElement element) => DuplicateElement<Grade>(element);

        protected internal override void ChangeTopbar()
        {
            base.ChangeTopbar();
            foreach (var grade in TopbarTexts!)
                grade.IsVisible = true;
        }

        protected override void ChangeButtonView()
        {
            var settings = SettingsManager.Settings;
            if(settings is not null)
                IsViewGrid = settings.GradeButtonStyle == SelectedButtonStyle.Grid;
        }

        private void RemoveElement(Grade grade)
        {
            var currentSubject = MainWindowViewModel.CurrentSubject;

            currentSubject?.Grades.Remove(grade);
            Items?.Remove(grade);
            UpdateVisualOnChange();
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
            
            ShowDialog(window, MainWindowInstance, this);
        }
    }
}