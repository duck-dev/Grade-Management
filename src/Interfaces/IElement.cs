using GradeManagement.Views.Lists.ElementButtonControls;

namespace GradeManagement.Interfaces
{
    public interface IElement : IGradable
    {
        string Name { get; }
        ButtonStyleBase? ButtonStyle { get; set; }

        T? Duplicate<T>(bool save = true) where T : class, IElement;
        void Save<T>(T? element = null) where T : class, IElement;
    }
}