﻿using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddSubjectViewModel : AddViewModelBase
    {
        public AddSubjectViewModel()
        {
            BorderBrushes = new SolidColorBrush[] { new(IncompleteColor), new(IncompleteColor) };
            WeightingIndex = 1;
            EditPageText(AddPageAction.Create, this.GetType());
        }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName) 
                                                && !float.IsNaN(ElementWeighting)
                                                && DataChanged();
        
        internal Subject? EditedSubject { get; set; } // TODO: When editing year, overwrite this property with `Subject`
        
        protected internal override void EraseData()
        {
            base.EraseData();
            // TODO: Reset color selection
            EditedSubject = null;
        }
        
        private void CreateElement()
        {
            var currentYear = MainWindowViewModel.CurrentYear;
            if (ElementName is null || currentYear is null)
                return;

            if (EditedSubject is null)
            {
                var subject = new Subject(ElementName, ElementWeighting, "#fcba03", ElementCounts); // TODO: Use selected color
                currentYear.Subjects.SafeAdd(subject);
            }
            else
                EditedSubject.Edit(ElementName, ElementWeighting, "#fcba03", ElementCounts); // TODO: Use selected color
            
            var viewModel = SubjectListViewModel.Instance;
            UpdateVisualOnChange(viewModel, currentYear.Subjects);
            EditedSubject = null;
        }

        private void RemoveElement(Subject subject)
        {
            var currentYear = MainWindowViewModel.CurrentYear;
            if (currentYear is null)
                return;

            currentYear.Subjects.SafeRemove(subject);
            var viewModel = SubjectListViewModel.Instance;
            UpdateVisualOnChange(viewModel, currentYear.Subjects);
        }

        private bool DataChanged()
        {
            if (EditedSubject is null)
                return true;

            return ElementName is not null && (!ElementName.Trim().Equals(EditedSubject.Name.Trim()) 
                                               || !ElementWeighting.Equals(EditedSubject.Weighting));
        }
    }
}