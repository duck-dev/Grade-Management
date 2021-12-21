using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models
{
    public class SchoolYear : ReactiveObject, IElement, ICloneable
    {
        private ButtonStyleBase? _buttonStyle;
        
        public SchoolYear(string name)
        {
            this.Name = name;

            var isGrid = SettingsManager.Settings?.YearButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
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

        [JsonIgnore]
        public ButtonStyleBase? ButtonStyle
        {
            get => _buttonStyle;
            set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
        }

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