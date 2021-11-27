using System;
using Avalonia.Controls;
using GradeManagement.Interfaces;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class ListViewModelBase : ViewModelBase
    {
        protected Classes ButtonClasses = new();
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