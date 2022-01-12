using System;
using Avalonia.Media;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.ViewModels.Dialogs;
using ReactiveUI;

namespace GradeManagement.ViewModels.BaseClasses
{
    public abstract class ListViewModelBase : ViewModelBase
    {
        private bool _isViewGrid;
        private DialogBase? _currentDialog;
        
        protected internal Type? AddPageType { get; protected init; }
        protected internal Type? AddViewModelType { get; protected init; }
        protected internal Type? ElementType { get; protected init; }

        protected bool IsViewGrid
        {
            get => _isViewGrid;
            set => this.RaiseAndSetIfChanged(ref _isViewGrid, value);
        }
        
        internal DialogBase? CurrentDialog
        {
            get => _currentDialog;
            set => this.RaiseAndSetIfChanged(ref _currentDialog, value);
        }

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

        protected void RemoveElement(IElement element, ElementType elementType, Action confirmAction)
        {
            string elementTypeName = elementType.ToString().SplitCamelCase();
            string dialogTitle = $"Do you really want to remove the {elementTypeName} \"{element.Name}\"?";
            CurrentDialog = new ConfirmationDialogViewModel(dialogTitle,
                new [] { Color.Parse("#D64045"), Color.Parse("#808080") },
                new[] { Colors.White, Colors.White },
                new[] { "Remove", "Cancel" },
                confirmAction);
        }

        protected internal virtual void ChangeTopbar()
        {
            if (TopbarTexts is null)
                throw new ArgumentNullException();
        }
        
        protected internal void ChangeButtonView(bool isGrid) => IsViewGrid = isGrid;
    }
}