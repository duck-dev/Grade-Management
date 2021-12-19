using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels.Lists;

namespace GradeManagement.Models
{
    public class SchoolYear : IElement, ICloneable
    {
        private UserControl? _buttonControlTemplate;
        
        public SchoolYear(string name)
        {
            this.Name = name;

            var type = SettingsManager.Settings?.YearButtonStyle;
            if(type is not null && Activator.CreateInstance(type) is UserControl control)
                ButtonControlTemplate = control;
        }

        [JsonConstructor]
        public SchoolYear(string name, List<Subject> subjects) : this(name)
        {
            this.Subjects = subjects;
        }
        
        [JsonInclude]
        public string Name { get; private set; }

        [JsonInclude] 
        public List<Subject> Subjects { get; private set; } = new();
        
        internal float Average => Utilities.GetAverage(Subjects, true);

        private UserControl? ButtonControlTemplate
        {
            get => _buttonControlTemplate;
            set
            {
                _buttonControlTemplate = value;
                var viewModel = YearListViewModel.Instance;
                viewModel?.UpdateVisualOnChange(viewModel, DataManager.SchoolYears);
            }
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
        
        public void ChangeButtonStyle(Type styleType)
        {
            if (Activator.CreateInstance(styleType) is UserControl control)
                ButtonControlTemplate = control;
        }

        internal void Edit(string newName) => this.Name = newName;
    }
}