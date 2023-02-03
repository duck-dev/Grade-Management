using System.Collections.Generic;
using GradeManagement.Models.Elements;

namespace GradeManagement.Interfaces;

public interface IGradesContainer
{
    IGradesContainer? ParentContainer { get; }
    List<Grade> Grades { get; }
}