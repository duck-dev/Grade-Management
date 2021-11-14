using System.Collections.Generic;
using System.Text.Json.Serialization;
using GradeManagement.Interfaces;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class SchoolYear : IElement
    {
        public SchoolYear(string name) => this.Name = name;

        [JsonConstructor]
        public SchoolYear(string name, List<Subject> subjects)
        {
            this.Name = name;
            this.Subjects = subjects;
        }
        
        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude] 
        public List<Subject> Subjects { get; init; } = new();
        
        internal float Average => Utilities.GetAverage(Subjects, true);

        internal void Edit(string newName) => this.Name = newName;
    }
}