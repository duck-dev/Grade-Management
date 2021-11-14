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
        
        protected void UpdateVisualOnChange<T>(IListViewModel<T>? viewModel, IEnumerable<T> collection) where T : IElement
        {
            if(viewModel is not null)
                viewModel.Items = new ObservableCollection<T>(collection);
            
            DataManager.SaveData();
        }
        
        protected internal virtual void ChangeTopbar()
        {
            if (TopbarTexts is null)
                throw new ArgumentNullException();
        }
    }
}