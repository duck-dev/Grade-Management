using System.Collections.Generic;

namespace GradeManagement.Interfaces
{
    public interface IElement
    {
        string Name { get; }
        IEnumerable<T>? Duplicate<T>() where T : IElement;
    }
}