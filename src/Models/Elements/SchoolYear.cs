using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Interfaces;
using GradeManagement.Models.Settings;
using GradeManagement.UtilityCollection;
using GradeManagement.ViewModels;
using GradeManagement.Views.Lists.ElementButtonControls;
using ReactiveUI;

namespace GradeManagement.Models.Elements
{
    public class SchoolYear : ReactiveObject, IElement, ISimpleGradable
    {
        private string _name = string.Empty;
        private ObservableCollection<Subject> _subjects = new();
        private ButtonStyleBase? _buttonStyle;

        private readonly MainWindowViewModel? _mainWindowViewModel;

        public SchoolYear(string name)
        {
            this.Name = name;

            var isGrid = SettingsManager.Settings?.YearButtonStyle == SelectedButtonStyle.Grid;
            this.ButtonStyle = isGrid ? new GridButton(this) : new ListButton(this);
            
            _mainWindowViewModel = MainWindowViewModel.Instance;

            _subjects.CollectionChanged += (sender, args) =>
            {
                this.RaisePropertyChanged(nameof(GradeValue));
                if (_mainWindowViewModel is not null)
                    _mainWindowViewModel.CurrentGradables = _subjects;
            };
        }

        [JsonConstructor]
        public SchoolYear(string name, ObservableCollection<Subject> subjects) : this(name)
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
        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            private set
            {
                if (value.SequenceEqual(_subjects))
                    return;
                this.RaiseAndSetIfChanged(ref _subjects, value);
                this.RaisePropertyChanged(nameof(GradeValue));
                
                if (_mainWindowViewModel is not null)
                    _mainWindowViewModel.CurrentGradables = _subjects;
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
        public int ElementCount => Subjects.Count;

        public T? Duplicate<T>() where T : class, IElement
        {
            var duplicate = this.Clone();
            DataManager.SchoolYears.Add(duplicate);

            return duplicate as T;
        }

        internal void Edit(string newName) => this.Name = newName;
        
        private SchoolYear Clone() => new(_name, new ObservableCollection<Subject>(_subjects.Clone()));
    }
}