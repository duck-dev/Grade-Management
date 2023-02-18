using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models.Elements
{
    public class SchoolYear : ColorableElement, ICloneable
    {
        private List<Subject> _subjects = new();

        public SchoolYear(string name, string elementColorHex) : base(elementColorHex, name)
        {
            var isGrid = SettingsManager.Settings?.YearButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
            AdjustTextColors(isGrid);
        }

        [JsonConstructor]
        public SchoolYear(string name, string elementColorHex, List<Subject> subjects) : this(name, elementColorHex)
        {
            this.Subjects = subjects;
        }

        [JsonInclude]
        public List<Subject> Subjects
        {
            get => _subjects;
            private set
            {
                if (value.SequenceEqual(_subjects))
                    return;
                this.RaiseAndSetIfChanged(ref _subjects, value);
                this.RaisePropertyChanged(nameof(GradeValue));
            }
        }

        [JsonIgnore] 
        public override float GradeValue => Utilities.GetAverage(Subjects, true);

        [JsonIgnore]
        public override float Weighting => 1;

        [JsonIgnore] 
        public override bool Counts => true;

        [JsonIgnore] 
        public override int ElementCount => Subjects.Count;

        protected internal override void Save<T>(T? element = null) where T : class
        {
            SchoolYear year = element as SchoolYear ?? this;
            DataManager.SchoolYears.Add(year);
        }

        internal void Edit(string newName, string colorHex)
        {
            this.Name = newName;
            this.ElementColorHex = colorHex;
        }
        
        public override object Clone() => new SchoolYear(Name, ElementColorHex, Subjects.Clone().ToList());
    }
}