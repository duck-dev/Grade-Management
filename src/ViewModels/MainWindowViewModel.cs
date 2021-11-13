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
        private readonly List<ListViewModelBase> _views = new();
        private readonly Button _addButton;
        
        public MainWindowViewModel()
        {
            GenerateExampleYear(); // TODO: Get rid of this, because it's only temporary to test the behaviour
            InitializeTopbarElements();
            
            _content = Content = new YearListViewModel(Data.SchoolYears!);
            _views.Add(_content);
            _content.ChangeTopbar();

            _addButton = MainWindowInstance.Get<Button>("AddButton");
        }

        internal static SchoolYear? CurrentYear { get; set; }
        
        internal ListViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }
        
        internal ListViewModelBase[] Views => _views.ToArray();

        internal void SwitchPage<T, TItems>(IEnumerable<TItems> items) where T : ListViewModelBase, 
        IListViewModel<TItems>
        {
            if (_views.Any(x => x.GetType() == typeof(T)))
            {
                Content = _views.Find(x => x.GetType() == typeof(T))!;
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
        
        private void OpenGrade(Grade grade)
        {
            var window = ShowAddPage<AddGradeWindow, AddGradeViewModel>();
            window.Title = grade.Name;

            if (window.DataContext is not AddGradeViewModel viewModel) 
                return;

            viewModel.EditedGrade = grade;
            
            viewModel.EditPageText<AddGradeViewModel>(AddPageAction.Edit, grade.Name);

            viewModel.ElementName = grade.Name;
            viewModel.ElementGradeString = grade.GradeValue.ToString(CultureInfo.CurrentCulture); // TODO: Change culture to selected
            viewModel.ElementWeightingString = grade.Weighting.ToString(CultureInfo.CurrentCulture); // TODO: Change culture to selected
            viewModel.ElementCounts = grade.Counts;
            
            viewModel.SelectedDay = grade.Date.Day;
            viewModel.SelectedMonth = new MonthRepresentation(grade.Date.Month);
            viewModel.SelectedYear = grade.Date.Year;
        }

        private void OpenSubject(Subject subject)
        {
            if (TopbarTexts?[2] is TextBlock textBlock)
            {
                textBlock.Text = subject.Name;
                textBlock.Foreground = new SolidColorBrush(subject.SubjectColor);
            }
            SwitchPage<GradeListViewModel, Grade>(subject.Grades);
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
            var window = new TWindow
            {
                DataContext = _views.Any(x => x.GetType() == typeof(TViewModel))
                    ? _views.Find(x => x.GetType() == typeof(TViewModel))
                    : new TViewModel()
            };

            window.ShowDialog(MainWindowInstance);
            CatchClosingWindow(window);
            return window;
        }

        private Window? ShowAddPage(Type? windowType, Type? viewModelType)
        {
            if (windowType is null || viewModelType is null || Activator.CreateInstance(windowType) is not Window window)
                return null;

            window.DataContext = _views.Any(x => x.GetType() == viewModelType) 
                                 ? _views.Find(x => x.GetType() == viewModelType) 
                                 : Activator.CreateInstance(viewModelType);
            
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
                    viewModel.StopEditing();
            };
            window.Closing += closingDel;
        }

        private static void GenerateExampleYear() // TODO: Remove this function, it is only used to test the behaviour
        {
            // Oh yes, this is horribly ugly
            Data.SchoolYears = new SchoolYear[]
            {
                new SchoolYear("2020/21 with some additional text", new Subject[]
                {
                    new Subject("Biology", 1.0f, "#009b72", new Grade[]
                    {
                        new Grade("First exam", 5.5f, 1.0f, DateTime.Today, true),
                        new Grade("Second exam", 6f, 1.0f, DateTime.Today, true)
                    }, true),
                    new Subject("History", 1.0f, "#D64045", new Grade[]
                    {
                        new Grade("First exam History", 6f, 1.0f, DateTime.Today, true)
                    }, true)
                }),
                new SchoolYear("2021/22", new Subject[]
                {
                    new Subject("English", 1.0f, "#009b72", new Grade[]
                    {
                        new Grade("First exam", 5.5f, 1.0f, DateTime.Today, true),
                        new Grade("Second exam", 6f, 1.0f, DateTime.Today, true)
                    }, true),
                    new Subject("Math", 1.0f, "#D64045", new Grade[]
                    {
                        new Grade("First exam Math", 6f, 1.0f, new DateTime(2010,7,28), true)
                    }, true)
                })
            };
        }
    }
}