using System.Collections.ObjectModel;
using Avalonia.Media;
using GradeManagement.Models;
using GradeManagement.ViewModels.BaseClasses;
using GradeManagement.ViewModels.Lists;

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
                if (DataManager.SchoolYears is null)
                    return;
                
                var year = new SchoolYear(ElementName);
                DataManager.SchoolYears.Add(year);

                var viewModel = YearListViewModel.Instance;
                if(viewModel is not null)
                    viewModel.Items = new ObservableCollection<SchoolYear>(DataManager.SchoolYears);
            }
            else
                EditedYear.Edit(ElementName);
            
            EditedYear = null;
            CloseAddWindow();
            DataManager.SaveData();
        }

        private bool DataChanged()
        {
            if (EditedYear is null)
                return true;

            return ElementName is not null && !ElementName.Trim().Equals(EditedYear.Name.Trim());
        }
    }
}