using GradeManagement.Enums;
using GradeManagement.ExtensionCollection;
using GradeManagement.Models;
using GradeManagement.UtilityCollection;
using ReactiveUI;

namespace GradeManagement.ViewModels.AddPages
{
    public class AddGradeViewModel : ViewModelBase
    {
        private bool _calendarOpen;
        private int _selectedDay = Utilities.TodaysDay;
        private MonthRepresentation _selectedMonth = new(Utilities.TodaysMonth);
        private int _selectedYear = Utilities.TodaysYear;

        internal static AddGradeViewModel? Instance { get; private set; }

        public AddGradeViewModel()
        {
            Instance = this;
        }

        internal MonthRepresentation SelectedMonth
        {
            get => _selectedMonth;
            set => _selectedMonth = value;
        }
        private bool CalendarOpen
        {
            get => _calendarOpen; 
            set => this.RaiseAndSetIfChanged(ref _calendarOpen, value); // Important to force the UI to update
        }

        private int SelectedDay
        {
            get => _selectedDay;
            set
            {
                if (Utilities.ValidateDate(value, _selectedMonth.Month, _selectedYear, out DateType protocol))
                {
                    _selectedDay = value;
                    return;
                }
                
                // TODO: Red outline/border around corresponding box in the view
                if(protocol.CustomHasFlag(DateType.Day))
                    System.Diagnostics.Trace.WriteLine("Day is invalid!");
            }
        }
        private int SelectedYear
        {
            get => _selectedYear; 
            set => _selectedYear = value;
        }

        private void ToggleCalendar() => CalendarOpen = !CalendarOpen;
    }
}