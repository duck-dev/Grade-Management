namespace GradeManagement.ViewModels
{
    public class AddGradeViewModel : ViewModelBase
    {
        public AddGradeViewModel() { }

        private bool CalendarOpen { get; set; }

        private void OpenCalendar() => CalendarOpen = !CalendarOpen;
    }
}