namespace GradeManagement.ViewModels.BaseClasses
{
    public class AddViewModelBase : ViewModelBase
    {
        protected string? ElementName { get; set; }
        protected float ElementWeighting { get; set; }
        protected string? ElementWeightingString
        {
            get => string.Empty;
            set
            {
                if (float.TryParse(value, out float weighting))
                    ElementWeighting = weighting;
            }
        }
        protected bool ElementCounts { get; set; } = true;

        protected virtual void CreateElement()
        {
            
        }
    }
}