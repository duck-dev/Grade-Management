using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.AddPages;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;
using GradeManagement.Views.AddPages;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ListViewModelBase _content;
        private readonly List<ViewModelBase> _views = new();
        private readonly Button _addButton;
        
        public MainWindowViewModel()
        {
            DataManager.LoadData();
            InitializeTopbarElements();
            
            _content = Content = new YearListViewModel(DataManager.SchoolYears!);
            _views.Add(_content);
            _content.ChangeTopbar();

            _addButton = MainWindowInstance.Get<Button>("AddButton");
        }

        internal static SchoolYear? CurrentYear { get; set; }
        internal static Subject? CurrentSubject { get; set; }
        
        private ListViewModelBase Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
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
            _addButton.IsVisible = true;
        }
        
        private static void EditElement<T>(T element, Window window) where T : IElement
        {
            window.Title = element.Name;
            if (window.DataContext is IAddViewModel<T> viewModel)
                viewModel.EditElement(element);
        }
        
        private void EditGrade(Grade grade) // I wish I could use a generic method here :(
        {
            var window = ShowAddPage<AddGradeWindow, AddGradeViewModel>();
            EditElement(grade, window);
        }

        private void EditSubject(Subject subject) // I wish I could use a generic method here :(
        {
            var window = ShowAddPage<AddSubjectWindow, AddSubjectViewModel>();
            EditElement(subject, window);
        }

        private void EditYear(SchoolYear year) // I wish I could use a generic method here :(
        {
            var window = ShowAddPage<AddYearWindow, AddYearViewModel>();
            EditElement(year, window);
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
            _addButton.IsVisible = false;
            var window = ShowAddPage(_content.AddPageType, _content.AddViewModelType);
            if (window?.DataContext is AddViewModelBase viewModel)
                viewModel.EditPageText(AddPageAction.Create, _content.AddViewModelType!);
        }

        private TWindow ShowAddPage<TWindow, TViewModel>() where TWindow : Window, new() 
                                                        where TViewModel : AddViewModelBase, new()
        {
            var window = new TWindow();
            
            TViewModel? viewModel;
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

            return ShowDialog(window);
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

            return ShowDialog(window);
        }

        private T ShowDialog<T>(T window) where T : Window
        {
            window.ShowDialog(MainWindowInstance);
            CatchClosingWindow(window);
            return window;
        }

        private void CatchClosingWindow(Window window)
        {
            EventHandler<CancelEventArgs>? closingDel = null;
            closingDel = delegate
            {
                _addButton.IsVisible = true;
                window.Closing -= closingDel;
                if (window.DataContext is AddViewModelBase viewModel)
                    viewModel.EraseData();
            };
            window.Closing += closingDel;
        }
    }
}