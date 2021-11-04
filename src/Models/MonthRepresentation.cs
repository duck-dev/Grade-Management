using GradeManagement.Converters;

namespace GradeManagement.Models
{
    public class MonthRepresentation
    {
        private int _month;
        private string? _monthName;
        
        public MonthRepresentation(int month) => this.Month = month;
        public MonthRepresentation(string monthName) => this.MonthName = monthName;

        internal int Month
        {
            get => _month;
            set
            {
                if (_month is <= 0 or > 12) 
                    return;
                _month = value;
                _monthName = MonthConverter.ConvertMonth(value);
            }
        }

        internal string MonthName
        {
            get => _monthName!;
            set
            {
                if (!MonthConverter.TryConvertMonth(value, out int monthNumber)) 
                    return;
                _monthName = value;
                _month = monthNumber;
            }
        }

        internal void Set(string monthName) => _monthName = monthName;
        internal void Set(int monthNum) => _month = monthNum;
        
        public override string? ToString()
        {
            return _monthName;
        }
    }
}