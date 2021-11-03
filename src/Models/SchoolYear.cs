using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class SchoolYear
    {
        public SchoolYear(string name, Subject[] subjects)
        {
            this.Name = name;
            this.Subjects = subjects;
        }

        internal float Average => Utilities.GetAverage(Subjects, true);
        internal string Name { get; init; }
        internal Subject[] Subjects { get; init; }
    }
}