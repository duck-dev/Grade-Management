using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class SchoolYear : IElement, ICloneable
    {
        private UserControl? _buttonContentTemplate;
        
        public SchoolYear(string name)
        {
            this.Name = name;
            //this.ButtonControlTemplate = new TEMPLATE(); TODO: Create new Template according to the saved style
        }

        [JsonConstructor]
        public SchoolYear(string name, List<Subject> subjects)
        {
            this.Name = name;
            this.Subjects = subjects;
        }
        
        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude] 
        public List<Subject> Subjects { get; private set; } = new();
        
        internal float Average => Utilities.GetAverage(Subjects, true);

        internal UserControl? ButtonControlTemplate
        {
            get => _buttonContentTemplate;
            set => _buttonContentTemplate = value;
        }

        public IEnumerable<T>? Duplicate<T>() where T : IElement
        {
            if (Clone() is not SchoolYear duplicate)
                return null;

            duplicate.Subjects = duplicate.Subjects.Clone().ToList();
            DataManager.SchoolYears.Add(duplicate);

            return DataManager.SchoolYears as IEnumerable<T>;
        }

        public object Clone() => this.MemberwiseClone();

        internal void Edit(string newName) => this.Name = newName;
    }
}