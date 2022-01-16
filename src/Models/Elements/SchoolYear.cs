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

namespace GradeManagement.Models.Elements
{
    public class SchoolYear : ColorableElement, IElement, IGradable
    {
        private const int MaxNameLength = 25;
        
        private string _name = string.Empty;
        private List<Subject> _subjects = new();
        private ButtonStyleBase? _buttonStyle;

        public SchoolYear(string name, string elementColorHex) : base(elementColorHex)
        {
            if (name.Length > MaxNameLength)
                name = name.Substring(0, MaxNameLength);
            this.Name = name;

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
                if (value.SequenceEqual(_subjects))
                    return;
                this.RaiseAndSetIfChanged(ref _subjects, value);
                this.RaisePropertyChanged(nameof(GradeValue));
            }
        }

        [JsonIgnore]
        public ButtonStyleBase? ButtonStyle
        {
            get => _buttonStyle;
            set => this.RaiseAndSetIfChanged(ref _buttonStyle, value);
        }

        [JsonIgnore] 
        public float GradeValue => Utilities.GetAverage(Subjects, true);

        [JsonIgnore]
        public float Weighting => 1;

        [JsonIgnore] 
        public bool Counts => true;

        [JsonIgnore] 
        public int ElementCount => Subjects.Count;

        protected override int GridThresholdAdditionalInfo => 120;
        protected override int ListThresholdAdditionalInfo => 135;

        public T? Duplicate<T>(bool save = true) where T : class, IElement
        {
            var duplicate = this.Clone();
            if(save)
                Save(duplicate);

            return duplicate as T;
        }
        
        public void Save<T>(T? element = null) where T : class, IElement
        {
            SchoolYear year = element as SchoolYear ?? this;
            DataManager.SchoolYears.Add(year);
        }

        internal void Edit(string newName, string colorHex)
        {
            this.Name = newName;
            this.ElementColorHex = colorHex;
        }
        
        private SchoolYear Clone() => new(_name, ElementColorHex, _subjects.Clone().ToList());
    }
}