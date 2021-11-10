using Avalonia.Media;
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