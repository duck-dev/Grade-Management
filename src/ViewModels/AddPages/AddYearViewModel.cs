using Avalonia.Media;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddYearViewModel : AddViewModelBase, IAddViewModel<SchoolYear>
    {
        public AddYearViewModel()
        {
            BorderBrushes = new SolidColorBrush[] { new(IncompleteColor) };
            EditPageText(AddPageAction.Create, "School Year");
        }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName) && DataChanged();

        internal SchoolYear? EditedYear { get; set; } // TODO: When editing year, overwrite this property with `SchoolYear`

        public void EditElement(SchoolYear year)
        {
            EditedYear = year;
            EditPageText(AddPageAction.Edit, "School Year", year.Name);
            ElementName = year.Name;
        }
        
        protected internal override void EraseData()
        {
            base.EraseData();
            EditedYear = null;
        }
        
        private void CreateElement()
        {
            if(ElementName is null)
                return;
            
            if (EditedYear is null)
            {
                var year = new SchoolYear(ElementName);
                DataManager.SchoolYears.Add(year);
            }
            else
                EditedYear.Edit(ElementName);
            
            var viewModel = YearListViewModel.Instance;
            UpdateVisualOnChange(viewModel, DataManager.SchoolYears);
            EditedYear = null;
        }

        private bool DataChanged()
        {
            if (EditedYear is null)
                return true;

            return ElementName is not null && !ElementName.Trim().Equals(EditedYear.Name.Trim());
        }
    }
}