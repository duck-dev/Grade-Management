using GradeManagement.Interfaces;
using GradeManagement.Models;

namespace GradeManagement.Views.Lists.ElementButtonControls
{
    public abstract class ButtonStyleBase
    {
        protected ButtonStyleBase(IElement element)
        {
            switch (element)
            {
                case SchoolYear year:
                    YearRef = year;
                    break;
                case Subject subject:
                    SubjectRef = subject;
                    break;
                case Grade grade:
                    GradeRef = grade;
                    break;
            }
        }
        
        protected SchoolYear? YearRef { get; init; }
        protected Subject? SubjectRef { get; init; }
        protected Grade? GradeRef { get; init; }
    }
}