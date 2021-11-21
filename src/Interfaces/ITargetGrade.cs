using System.Collections.Generic;
using GradeManagement.Models;

namespace GradeManagement.Interfaces
{
    public interface ITargetGrade
    {
        IEnumerable<Grade>? Grades { get; set; }
    }
}