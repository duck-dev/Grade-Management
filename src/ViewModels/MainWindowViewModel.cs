using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _content;
        private readonly List<ViewModelBase> _views = new();
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
        
        internal ViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }
        
        internal ViewModelBase[] Views => _views.ToArray();
        
        internal void OpenSubject(Subject subject)
        {
            if (TopbarTexts?[2] is TextBlock textBlock)
            {
                textBlock.Text = subject.Name;
                textBlock.Foreground = new SolidColorBrush(subject.SubjectColor);
            }
            SwitchPage<GradeListViewModel, Grade>(subject.Grades);
        }
        
        internal void OpenYear(SchoolYear year)
        {
            if (TopbarTexts?[0] is TextBlock textBlock)
                textBlock.Text = year.Name;
            SwitchPage<SubjectListViewModel, Subject>(year.Subjects);
            CurrentYear = year;
        }

        internal void OpenAddPage()
        {
            _addButton.IsVisible = false;
            if (_content.AddPage is null && _content.AddPageType is null)
                throw new Exception("_content.AddPageType is null.");
            
            _content.AddPage ??= (Window)Activator.CreateInstance(_content.AddPageType!)!;
            _content.AddPage.DataContext = Activator.CreateInstance(_content.AddViewModelType!);
            _content.AddPage.ShowDialog(MainWindowInstance);

            EventHandler<CancelEventArgs>? closingDel = null;
            closingDel = delegate
            {
                _addButton.IsVisible = true;
                _content.AddPage.Closing -= closingDel;
                _content.AddPage = null;
            };
            _content.AddPage.Closing += closingDel;
        }

        internal void SwitchPage<T, TItems>(IEnumerable<TItems> items) where T : ViewModelBase, IListViewModel<TItems>
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
                        new Grade("First exam Math", 6f, 1.0f, DateTime.Today, true)
                    }, true)
                })
            };
        }
    }
}