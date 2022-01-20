using System.Globalization;
using System.Linq;
using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Elements;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddSubjectViewModel : AddViewModelBase, IAddViewModel<Subject>
    {
        public AddSubjectViewModel()
        {
            BorderBrushes = new SolidColorBrush[] { new(IncompleteColor), new(IncompleteColor) };
            WeightingIndex = 1;
            EditPageText(AddPageAction.Create, "Subject");
        }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName)  && !string.IsNullOrWhiteSpace(ElementName)
                                                && !float.IsNaN(ElementWeighting)
                                                && DataChanged();

        private Subject? EditedSubject { get; set; }

        public void EditElement(Subject subject)
        {
            EditedSubject = subject;
            EditPageText(AddPageAction.Edit, "Subject", subject.Name);

            ElementName = subject.Name;
            ElementWeightingString = subject.Weighting.ToString(CultureInfo.CurrentCulture);
            ElementCounts = subject.Counts;
            
            var colorRepresentation = ElementColorsCollection.FirstOrDefault(x => x.ElementColor.Equals(subject.ElementColor));
            if(colorRepresentation is not null)
                ChangeColor(colorRepresentation);
        }
        
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
            
            var colorHex = SelectedColor.ElementColor.ToHexString();
            if (EditedSubject is null)
            {
                var viewModel = SubjectListViewModel.Instance;
                var subject = new Subject(ElementName, ElementWeighting, colorHex, ElementCounts);

                currentYear.Subjects.SafeAdd(subject);
                viewModel?.Items?.Add(subject);
            }
            else
                EditedSubject.Edit(ElementName, ElementWeighting, colorHex, ElementCounts);

            UpdateVisualOnChange();
            EditedSubject = null;
        }

        private bool DataChanged()
        {
            if (EditedSubject is null)
                return true;

            return ElementName is not null && (!ElementName.Trim().Equals(EditedSubject.Name.Trim()) 
                                               || !ElementWeighting.Equals(EditedSubject.Weighting)
                                               || ElementCounts != EditedSubject.Counts
                                               || !SelectedColor.ElementColor.Equals(EditedSubject.ElementColor));
        }
    }
}