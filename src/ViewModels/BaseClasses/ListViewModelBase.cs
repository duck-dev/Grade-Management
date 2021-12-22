using System;
using GradeManagement.Interfaces;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class ListViewModelBase : ViewModelBase
    {
        protected internal Type? AddPageType { get; protected init; }
        protected internal Type? AddViewModelType { get; protected init; }

        protected void DuplicateElement<T>(IElement element) where T : class, IElement
        {
            var instance = MainWindowViewModel.Instance;
            if (instance is null)
                return;
            
            var content = instance.Content;
            var duplicate = element.Duplicate<T>();
            content.UpdateVisualOnChange();

            if (duplicate is null)
                return;
            var viewModel = content as IListViewModel<T>;
            viewModel?.Items?.Add(duplicate);
        }

        protected internal virtual void ChangeTopbar()
        {
            if (TopbarTexts is null)
                throw new ArgumentNullException();
        }
    }
}