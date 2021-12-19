using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;

namespace GradeManagement.Models
{
    public class SchoolYear : ElementBase, IElement, ICloneable
    {
        public SchoolYear(string name)
        {
            this.Name = name;

            var type = SettingsManager.Settings?.YearButtonStyle;
            if(type is not null && Activator.CreateInstance(type) is UserControl control)
                ButtonControlTemplate = control;
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