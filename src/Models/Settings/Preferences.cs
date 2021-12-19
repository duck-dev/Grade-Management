using System;
using GradeManagement.Views.Lists.ElementButtonControls.Grid;

namespace GradeManagement.Models.Settings
{
    public class Preferences
    {
        public Type? YearButtonStyle { get; set; } = typeof(GridYearButton);
        public Type? SubjectButtonStyle { get; set; } = typeof(GridSubjectButton);
        public Type? GradeButtonStyle { get; set; } = typeof(GridGradeButton);
    }
}