﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models;
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
        private TargetGradeWindowModel? _targetGradeViewModel;
        
        [Obsolete("Do NOT use this constructor, because it leaves the collection of grades uninitialized " +
                  "and this leads to exceptions and unintended behaviour.")]
        public GradeListViewModel()
        {
            Instance = this;
            AddPageType = typeof(AddGradeWindow);
            AddViewModelType = typeof(AddGradeViewModel);
        }
        
#pragma warning disable 618
        public GradeListViewModel(IEnumerable<Grade> items) : this()
#pragma warning restore 618
        {
            Items = new ObservableCollection<Grade>(items);
            Items.CollectionChanged += (sender, args) => this.RaisePropertyChanged(nameof(EmptyCollection));
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
        
        private void RemoveElement(Grade grade)
        {
            var currentSubject = MainWindowViewModel.CurrentSubject;
            if (currentSubject is null)
                return;
            
            currentSubject.Grades.SafeRemove(grade);
            UpdateVisualOnChange(this, currentSubject.Grades);
        }
        
        private void OpenTargetGradeCalc()
        {
            if (Items is null)
                return;
            var window = new TargetGradeWindow();
            _targetGradeViewModel ??= new TargetGradeWindowModel(Items);
            _targetGradeViewModel.Grades = Items;
            window.DataContext = _targetGradeViewModel;
            
            ShowDialog(window, MainWindowInstance, this, 0);
        }
    }
}