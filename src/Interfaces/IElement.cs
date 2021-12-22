using System.Collections.Generic;
using GradeManagement.Views.Lists.ElementButtonControls;

namespace GradeManagement.Interfaces
{
    public interface IElement
    {
        string Name { get; }
        ButtonStyleBase? ButtonStyle { get; set; }

        T? Duplicate<T>() where T : class, IElement;
    }
}