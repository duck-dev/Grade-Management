﻿using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.Models.Elements;
using GradeManagement.Models.Settings;
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

        public MainWindowViewModel()
        {
            Instance = this;
            
            SettingsManager.LoadSettings();
            InitializeTopbarElements();
            
            _content = Content = new YearListViewModel(DataManager.SchoolYears);
            _content.ChangeTopbar();
        }

        internal static MainWindowViewModel? Instance { get; private set; }
        internal static Subject? CurrentSubject { get; set; }
        internal static SchoolYear? CurrentYear { get; set; }

        internal ListViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        internal void SwitchPage<T, TItems>(IEnumerable<TItems> items) where T : ListViewModelBase, IListViewModel<TItems> 
            where TItems : class, IElement
        {
            Content = (Activator.CreateInstance(typeof(T), items) as T)!;
            _content.ChangeTopbar();
        }
        
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
            var window = ShowAddPage<AddGradeWindow, AddGradeViewModel>(out var viewModel);
            EditElement(grade, viewModel, window);
        }

        private void EditSubject(Subject subject) // I wish I could use a generic method here :(
        {
            var window = ShowAddPage<AddSubjectWindow, AddSubjectViewModel>(out var viewModel);
            EditElement(subject, viewModel, window);
        }

        private void EditYear(SchoolYear year) // I wish I could use a generic method here :(
        {
            var window = ShowAddPage<AddYearWindow, AddYearViewModel>(out var viewModel);
            EditElement(year, viewModel, window);
        }

        private void OpenSubject(Subject subject)
        {
            if (TopbarTexts?[2] is TextBlock textBlock)
            {
                textBlock.Text = subject.Name;
                textBlock.Foreground = new SolidColorBrush(subject.SubjectColor);
            }
            SwitchPage<GradeListViewModel, Grade>(subject.Grades);
            CurrentSubject = subject;
        }
        
        private void OpenYear(SchoolYear year)
        {
            if (TopbarTexts?[0] is TextBlock textBlock)
                textBlock.Text = year.Name;
            SwitchPage<SubjectListViewModel, Subject>(year.Subjects);
            CurrentYear = year;
        }

        private void OpenAddPage()
        {
            var window = ShowAddPage(_content.AddPageType, _content.AddViewModelType);
            if (window?.DataContext is AddViewModelBase viewModel)
                viewModel.EditPageText(AddPageAction.Create, _content.AddViewModelType!);
        }

        private TWindow? ShowAddPage<TWindow, TViewModel>(out TViewModel? viewModel) where TWindow : Window, new() 
                                                        where TViewModel : AddViewModelBase, new()
        {
            var window = new TWindow();
            viewModel = new TViewModel();
            
            window.DataContext = viewModel;
            viewModel.CurrentAddWindow = window;

            return ShowDialog(window, MainWindowInstance, this);
        }

        private Window? ShowAddPage(Type? windowType, Type? viewModelType)
        {
            if (windowType is null || viewModelType is null || Activator.CreateInstance(windowType) is not Window window)
                return null;
            
            var viewModel = (AddViewModelBase)Activator.CreateInstance(viewModelType)!;
            window.DataContext = viewModel;
            viewModel.CurrentAddWindow = window;

            return ShowDialog(window, MainWindowInstance, this);
        }

        private void ChangeView(byte view)
        {
            var settings = SettingsManager.Settings;
            switch (_content)
            {
                case YearListViewModel:
                    ChangeViewGeneric<SchoolYear>(view);
                    
                    if (settings is null)
                        return;
                    settings.YearButtonStyle = view == 0 ? SelectedButtonStyle.Grid : SelectedButtonStyle.List;
                    break;
                case SubjectListViewModel:
                    ChangeViewGeneric<Subject>(view);
                    
                    if (settings is null)
                        return;
                    settings.SubjectButtonStyle = view == 0 ? SelectedButtonStyle.Grid : SelectedButtonStyle.List;
                    break;
                case GradeListViewModel:
                    ChangeViewGeneric<Grade>(view);
                    
                    if (settings is null)
                        return;
                    settings.GradeButtonStyle = view == 0 ? SelectedButtonStyle.Grid : SelectedButtonStyle.List;
                    break;
            }
            
            SettingsManager.SaveSettings();
        }

        private void ChangeViewGeneric<T>(byte view) where T : class, IElement
        {
            if (_content is not IListViewModel<T> type)
                return;

            var collection = type.Items;
            if (collection is null)
                return;

            foreach (var item in collection)
                item.ButtonStyle = view == 0 ? new GridButton(item) : new ListButton(item);
        }
    }
}