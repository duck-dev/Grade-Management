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
    public class SchoolYear : ReactiveObject, IElement
    {
        private string _name = string.Empty;
        private List<Subject> _subjects = new();
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
        public string Name
        {
            get => _name; 
            private set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        [JsonInclude]
        public List<Subject> Subjects
        {
            get => _subjects;
            private set
            {
                this.RaiseAndSetIfChanged(ref _subjects, value);
                this.RaisePropertyChanged(nameof(Average));
            }
        }

        [JsonIgnore]
        public ButtonStyleBase? ButtonStyle
        {
            get => _buttonStyle;
            set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
        }

        internal float Average => Utilities.GetAverage(Subjects, true);

        public T? Duplicate<T>() where T : class, IElement
        {
            var duplicate = this.Clone();
            DataManager.SchoolYears.Add(duplicate);

            return duplicate as T;
        }

        internal void Edit(string newName) => this.Name = newName;
        
        private SchoolYear Clone() => new(_name, _subjects.Clone().ToList());
    }
}