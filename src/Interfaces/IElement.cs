using System.Collections.Generic;
using GradeManagement.Views.Lists.ElementButtonControls;

namespace GradeManagement.Interfaces
{
    public interface IElement
    {
        string Name { get; }
        ButtonStyleBase? ButtonStyle { get; set; }

        IEnumerable<T>? Duplicate<T>() where T : IElement;
    }
}