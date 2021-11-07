using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddYearViewModel : AddViewModelBase
    {
        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName);
    }
}