using System.Collections.Generic;
using GradeManagement.Models.Elements;

namespace GradeManagement.Interfaces
{
    public interface ITargetGrade
    {
        IEnumerable<Grade>? Grades { get; set; }
    }
}