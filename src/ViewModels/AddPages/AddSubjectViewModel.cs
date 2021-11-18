using System.Globalization;
using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;
using ReactiveUI;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddSubjectViewModel : AddViewModelBase, IAddViewModel<Subject>
    {
        private bool _wishGradeEnabled;
        
        public AddSubjectViewModel()
        {
            BorderBrushes = new SolidColorBrush[] { new(IncompleteColor), new(IncompleteColor) };
            WeightingIndex = 1;
            EditPageText(AddPageAction.Create, "Subject");
        }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName) 
                                                && !float.IsNaN(ElementWeighting)
                                                && DataChanged();
        
        internal bool WishGradeEnabled
        {
            get => _wishGradeEnabled; 
            set => this.RaiseAndSetIfChanged(ref _wishGradeEnabled, value);
        }
        
        private Subject? EditedSubject { get; set; } // TODO: When editing year, overwrite this property with `Subject`

        public void EditElement(Subject subject)
        {
            EditedSubject = subject;
            EditPageText(AddPageAction.Edit, "Subject", subject.Name);

            ElementName = subject.Name;
            ElementWeightingString = subject.Weighting.ToString(CultureInfo.CurrentCulture);
            ElementCounts = subject.Counts;
        }
        
        protected override void EraseData()
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

        private bool DataChanged()
        {
            if (EditedSubject is null)
                return true;

            return ElementName is not null && (!ElementName.Trim().Equals(EditedSubject.Name.Trim()) 
                                               || !ElementWeighting.Equals(EditedSubject.Weighting)
                                               || ElementCounts != EditedSubject.Counts);
        }
    }
}