namespace GradeManagement.Interfaces
{
    public interface IGradable : ISimpleGradable
    {
        float Weighting { get; }
        bool Counts { get; }
    }
}