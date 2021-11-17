using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class ListViewModelBase : ViewModelBase
    {
        protected internal Type? AddPageType { get; protected init; }
        protected internal Type? AddViewModelType { get; protected init; }
        
        protected void DuplicateElement<T>(IElement element) where T : IElement
        {
            var instance = MainWindowViewModel.Instance;
            if (instance is null)
                return;
            
            var content = instance.Content;
            var collection = element.Duplicate<T>();
            var viewModel = content as IListViewModel<T>;
            content.UpdateVisualOnChange(viewModel, collection);
        }

        protected internal virtual void ChangeTopbar()
        {
            if (TopbarTexts is null)
                throw new ArgumentNullException();
        }
    }
}