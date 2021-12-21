using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.Models;
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
        private readonly List<ViewModelBase> _views = new();

        public MainWindowViewModel()
        {
            Instance = this;
            
            SettingsManager.LoadSettings();
            InitializeTopbarElements();
            
            _content = Content = new YearListViewModel(DataManager.SchoolYears!);
            _views.Add(_content);
            _content.ChangeTopbar();
        }

        internal static MainWindowViewModel? Instance { get; private set; }
        internal static SchoolYear? CurrentYear { get; set; }
        internal static Subject? CurrentSubject { get; set; }
        
        internal ListViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        internal void SwitchPage<T, TItems>(IEnumerable<TItems> items) where T : ListViewModelBase, IListViewModel<TItems>
        {
            if (_views.Any(x => x.GetType() == typeof(T)))
            {
                if (_views.Find(x => x.GetType() == typeof(T)) is not ListViewModelBase viewModelBase)
                    return;
                Content = viewModelBase;
                ((IListViewModel<TItems>)Content).Items = new ObservableCollection<TItems>(items);
            }
            else
            {
                Content = (Activator.CreateInstance(typeof(T), items) as T)!;
                _views.Add(Content);
            }
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
            
            if (_views.Any(x => x.GetType() == typeof(TViewModel)))
            {
                var viewModelBase = _views.Find(x => x.GetType() == typeof(TViewModel));
                window.DataContext = viewModelBase;
                viewModel = viewModelBase as TViewModel;
            }
            else
            {
                viewModel = new TViewModel();
                window.DataContext = viewModel;
                _views.Add(viewModel);
            }

            if(viewModel is not null)
                viewModel.CurrentAddWindow = window;

            return ShowDialog(window, MainWindowInstance, this, 0);
        }

        private Window? ShowAddPage(Type? windowType, Type? viewModelType)
        {
            if (windowType is null || viewModelType is null || Activator.CreateInstance(windowType) is not Window window)
                return null;

            AddViewModelBase? viewModel;
            if (_views.Any(x => x.GetType() == viewModelType))
            {
                var viewModelBase = _views.Find(x => x.GetType() == viewModelType);
                viewModel = viewModelBase as AddViewModelBase;
                window.DataContext = viewModelBase;
            }
            else
            {
                viewModel = (AddViewModelBase)Activator.CreateInstance(viewModelType)!;
                window.DataContext = viewModel;
                _views.Add(viewModel);
            }
            
            if(viewModel is not null)
                viewModel.CurrentAddWindow = window;

            return ShowDialog(window, MainWindowInstance, this, 0);
        }

        // TODO: This is horrible, but I'm not really able to use the interface out of the box. I'll have to figure out something...
        private void ChangeView(byte view)
        {
            switch (_content)
            {
                case YearListViewModel yearViewModel:
                    var years = yearViewModel.Items;
                    if (years is null)
                        return;

                    foreach (var year in years)
                        year.ButtonStyle = view == 0 ? new GridButton(year) : new ListButton(year);
                    
                    break;
                case SubjectListViewModel subjectViewModel:
                    var subjects = subjectViewModel.Items;
                    if (subjects is null)
                        return;

                    foreach (var year in subjects)
                        year.ButtonStyle = view == 0 ? new GridButton(year) : new ListButton(year);
                    
                    break;
                case GradeListViewModel gradeViewModel:
                    var grades = gradeViewModel.Items;
                    if (grades is null)
                        return;

                    foreach (var year in grades)
                        year.ButtonStyle = view == 0 ? new GridButton(year) : new ListButton(year);
                    
                    break;
            }
        }
    }
}