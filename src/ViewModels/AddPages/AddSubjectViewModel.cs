using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.BaseClasses;

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
        
        protected override void CreateElement(IListViewModel<IElement> viewModel)
        {
            if (ElementName is null)
                return;
            
            if (EditedSubject is null)
            {
                var currentYear = MainWindowViewModel.CurrentYear;
                if (currentYear is null)
                    return;
                
                var subject = new Subject(ElementName, ElementWeighting, "#fcba03", ElementCounts); // TODO: Use selected color
                currentYear.Subjects.Add(subject);
            }
            else
                EditedSubject.Edit(ElementName, ElementWeighting, "#fcba03", ElementCounts); // TODO: Use selected color

            base.CreateElement(viewModel);
            EditedSubject = null;
        }

        protected internal override void StopEditing()
        {
            EditedSubject = null;
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