using System.Collections.Generic;
using GradeManagement.Models.Elements;

namespace GradeManagement.Interfaces;

public interface IGradesContainer
{
    List<Grade> Grades { get; }
}