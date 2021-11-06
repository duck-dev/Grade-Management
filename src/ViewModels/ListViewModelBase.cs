using System;
using Avalonia.Controls;

namespace GradeManagement.ViewModels
{
    public class ListViewModelBase : ViewModelBase
    {
        protected internal Window? AddPage { get; internal set; }
        protected internal Type? AddPageType { get; protected init; }
        protected internal Type? AddViewModelType { get; protected init; }
    }
}