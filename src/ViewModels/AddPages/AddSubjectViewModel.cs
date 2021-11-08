using Avalonia.Media;
using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddSubjectViewModel : AddViewModelBase
    {
        public AddSubjectViewModel() => BorderBrushes = new SolidColorBrush[] { new(IncompleteColor), new(IncompleteColor) };

        protected override bool DataComplete => !string.IsNullOrEmpty(ElementName) 
                                                && !float.IsNaN(ElementWeighting);
    }
}