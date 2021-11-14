using Avalonia.Media;
using GradeManagement.Interfaces;
using GradeManagement.Models;
using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddYearViewModel : AddViewModelBase
    {
        public AddYearViewModel()
        {
            BorderBrushes = new SolidColorBrush[] { new(IncompleteColor) };
            EditPageText(AddPageAction.Create, this.GetType());
        }

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName) && DataChanged();

        internal SchoolYear? EditedYear { get; set; } // TODO: When editing year, overwrite this property with `SchoolYear`
        
        protected override void CreateElement(IListViewModel<IElement> viewModel)
        {
            if(ElementName is null)
                return;
            
            if (EditedYear is null)
            {
                if (DataManager.SchoolYears is null)
                    return;
                
                var year = new SchoolYear(ElementName);
                DataManager.SchoolYears.Add(year);
            }
            else
                EditedYear.Edit(ElementName);
            
            base.CreateElement(viewModel);
            EditedYear = null;
        }
        
        protected internal override void StopEditing()
        {
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