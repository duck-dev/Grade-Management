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
            private init
            {
                if (value is <= 0 or > 12) 
                    return;
                _month = value;
                _monthName = MonthConverter.ConvertMonth(value);
            }
        }

        internal string MonthName
        {
            get => _monthName!;
            private init
            {
                if (!MonthConverter.TryParseMonth(value, out int monthNumber))
                {
                    _monthName = string.Empty;
                    _month = 0;
                    return;
                }
                
                _monthName = value;
                _month = monthNumber;
            }
        }

        public override string? ToString() => _monthName;
        internal void Set(string monthName) => _monthName = monthName;
        internal void Set(int monthNum) => _month = monthNum;
    }
}