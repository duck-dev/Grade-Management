using System;
using Avalonia.Controls;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class ListViewModelBase : ViewModelBase
    {
        protected internal Window? AddPage { get; internal set; }
        protected internal Type? AddPageType { get; protected init; }
        protected internal Type? AddViewModelType { get; protected init; }
        
        protected internal virtual void ChangeTopbar()
        {
            if (TopbarTexts is null)
                throw new ArgumentNullException();
        }
    }
}