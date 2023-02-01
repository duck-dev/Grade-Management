﻿using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Models.Elements;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.AddPages;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;
using GradeManagement.Views.AddPages;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ListViewModelBase _content;
        private IEnumerable<IGradable> _currentGradables;
        private IElement? _copiedElement;

        public MainWindowViewModel()
        {
            Instance = this;
            
            SettingsManager.LoadSettings();
            InitializeTopbarElements();
            
            _content = Content = new YearListViewModel(DataManager.SchoolYears);
            _content.ChangeTopbar();
            
            _currentGradables = DataManager.SchoolYears;
        }

        internal static MainWindowViewModel? Instance { get; private set; }
        internal static Subject? CurrentSubject { get; set; }
        internal static SchoolYear? CurrentYear { get; set; }
        
        internal App? AppInstance { get; init; }

        internal ListViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        private float CurrentAverage => Utilities.GetAverage(_currentGradables, true);
        
        private IElement? CopiedElement
        {
            get => _copiedElement;
            set
            {
                _copiedElement = value;
                this.RaisePropertyChanged(nameof(HasCopiedElement));
            }
        }

        private bool HasCopiedElement => CopiedElement is not null 
                                         && CopiedElement.GetType() == Content.ElementType;

        internal T SwitchPage<T, TItems>(IEnumerable<TItems> items) where T : ListViewModelBase, IListViewModel<TItems> 
            where TItems : class, IElement, IGradable
        {
            T content = (Activator.CreateInstance(typeof(T), items) as T)!;
            Content = content;
            Content.ChangeTopbar();

            _currentGradables = items;
            UpdateAverage();
            return content;
        }

        internal void UpdateAverage() => this.RaisePropertyChanged(nameof(CurrentAverage));
        
        private static void EditElement<TElement, TViewModel>(TElement element, TViewModel? viewModel, Window? window) 
            where TElement : IElement where TViewModel : AddViewModelBase, IAddViewModel<TElement>
        {
            if (window is null)
                return;
            
            window.Title = element.Name;
            if (viewModel is null)
            {
                if (window.DataContext is IAddViewModel<TElement> viewModelDataContext)
                    viewModelDataContext.EditElement(element);
                return;
            }
            viewModel.EditElement(element);
        }
        
        private void EditGrade(Grade grade) // I wish I could use a generic method here :(
        {
            var window = Utilities.ShowAddPage<AddGradeWindow, AddGradeViewModel>(out var viewModel, MainWindowInstance);
            EditElement(grade, viewModel, window);
        }

        private void EditSubject(Subject subject) // I wish I could use a generic method here :(
        {
            var window = Utilities.ShowAddPage<AddSubjectWindow, AddSubjectViewModel>(out var viewModel, MainWindowInstance);
            EditElement(subject, viewModel, window);
        }

        private void EditYear(SchoolYear year) // I wish I could use a generic method here :(
        {
            var window = Utilities.ShowAddPage<AddYearWindow, AddYearViewModel>(out var viewModel, MainWindowInstance);
            EditElement(year, viewModel, window);
        }

        private void OpenGrade(Grade grade)
        {
            if (grade is not GradeGroup gradeGroup) // Single grade
            {
                EditGrade(grade);
                return;
            } 
            
            // Group of partial grades
            GradeListViewModel viewModel = SwitchPage<GradeListViewModel, Grade>(gradeGroup.Grades);
            viewModel.GradesContainer = gradeGroup;
        }

        private void OpenSubject(Subject subject)
        {
            AdjustTopbarText(subject, 2);
            GradeListViewModel viewModel = SwitchPage<GradeListViewModel, Grade>(subject.Grades);
            viewModel.GradesContainer = subject;
            CurrentSubject = subject;
        }
        
        private void OpenYear(SchoolYear year)
        {
            AdjustTopbarText(year, 0);
            SwitchPage<SubjectListViewModel, Subject>(year.Subjects);
            CurrentYear = year;
        }

        private void AdjustTopbarText<T>(T element, int index) where T : ColorableElement, IElement
        {
            if (TopbarTexts?[index] is not TextBlock textBlock) 
                return;
            textBlock.Text = element.Name;

            const float darkenFactor = 0.1f;
            var color = element.ElementColor.DarkenColor(darkenFactor);
            textBlock.Foreground = new SolidColorBrush(color);
        }

        private void OpenAddPage()
        {
            Utilities.ShowAddPage(Content.AddPageType, Content.AddViewModelType, MainWindowInstance, out var addViewModel);
            if (addViewModel is AddGradeViewModel addGradeViewModel && Content is GradeListViewModel gradeList)
                addGradeViewModel.GradesContainer = gradeList.GradesContainer;
            addViewModel?.EditPageText(AddPageAction.Create, Content.AddViewModelType!);
        }

        private void ChangeView(bool isGrid)
        {
            var settings = SettingsManager.Settings;
            switch (_content)
            {
                case YearListViewModel:
                    ChangeViewGeneric<SchoolYear>(isGrid);
                    
                    if (settings is null)
                        return;
                    settings.YearButtonStyle = isGrid ? SelectedButtonStyle.Grid : SelectedButtonStyle.List;
                    break;
                case SubjectListViewModel:
                    ChangeViewGeneric<Subject>(isGrid);
                    
                    if (settings is null)
                        return;
                    settings.SubjectButtonStyle = isGrid ? SelectedButtonStyle.Grid : SelectedButtonStyle.List;
                    break;
                case GradeListViewModel:
                    ChangeViewGeneric<Grade>(isGrid);
                    
                    if (settings is null)
                        return;
                    settings.GradeButtonStyle = isGrid ? SelectedButtonStyle.Grid : SelectedButtonStyle.List;
                    break;
            }
            
            SettingsManager.SaveSettings();
        }

        private void ChangeViewGeneric<T>(bool isGrid) where T : class, IElement
        {
            if (_content is not IListViewModel<T> viewModelInterface)
                return;

            var collection = viewModelInterface.Items;
            if (collection is null)
                return;
            
            _content.ChangeButtonView(isGrid);

            foreach (var item in collection)
                item.ButtonStyle = isGrid ? new GridButton(item) : new ListButton(item);
        }

        private void CopyElement(IElement element)
        {
            switch (element)
            {
                case SchoolYear year:
                    CopyElement(year);
                    break;
                case Subject subject:
                    CopyElement(subject);
                    break;
                case Grade grade:
                    CopyElement(grade);
                    break;
            } 
        }
        
        private void CopyElement<T>(T element) where T : class, IElement 
            => CopiedElement = element.Duplicate<T>(false);

        private void PasteCopiedElement()
        {
            switch(Content)
            {
                case YearListViewModel:
                    PasteCopiedElement<SchoolYear>();
                    break;
                case SubjectListViewModel:
                    PasteCopiedElement<Subject>();
                    break;
                case GradeListViewModel:
                    PasteCopiedElement<Grade>();
                    break;
            }
        }

        private void PasteCopiedElement<T>() where T : class, IElement
        {
            if (_content is not IListViewModel<T> viewModel || CopiedElement is not T element) 
                return;
            viewModel.Items?.Add(element);
            element.Save<T>();
            UpdateAverage();
            
            CopiedElement = element.Duplicate<T>(false);
        }
    }
}