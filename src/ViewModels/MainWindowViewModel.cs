﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _content;
        private readonly List<ViewModelBase> _views = new();
        
        public MainWindowViewModel()
        {
            GenerateExampleYear(); // TODO: Get rid of this, because it's only temporary to test the behaviour
            _content = Content = new YearListViewModel(Data.SchoolYears);
            _views.Add(_content);
            _content.ChangeTopbar();
        }

        internal static SchoolYear? CurrentYear { get; set; }
        
        internal ViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }
        internal ViewModelBase[] Views => _views.ToArray();
        
        public void OpenSubject(Subject subject)
        {
            SwitchPage<GradeListViewModel, Grade>(subject.Grades);
        }
        
        public void OpenYear(SchoolYear year)
        {
            SwitchPage<SubjectListViewModel, Subject>(year.Subjects);
            CurrentYear = year;
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
        }

        void GenerateExampleYear() // TODO: Remove this function, it is only used to test the behaviour
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
                    }),
                    new Subject("History", 1.0f, "#D64045", new Grade[]
                    {
                        new Grade("First exam History", 6f, 1.0f, DateTime.Today, true)
                    })
                }),
                new SchoolYear("2021/22", new Subject[]
                {
                    new Subject("English", 1.0f, "#009b72", new Grade[]
                    {
                        new Grade("First exam", 5.5f, 1.0f, DateTime.Today, true),
                        new Grade("Second exam", 6f, 1.0f, DateTime.Today, true)
                    }),
                    new Subject("Math", 1.0f, "#D64045", new Grade[]
                    {
                        new Grade("First exam Math", 6f, 1.0f, DateTime.Today, true)
                    })
                })
            };
        }
    }
}