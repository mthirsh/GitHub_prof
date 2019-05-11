using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateCalculatorCore
{
    public class AnnualPublicHoliday
    {
        public string Name { get; }
        public bool MoveToMonday { get; }
        public bool FixedDay { get; } = true;
        private int _consecutiveHolidays;
        private int _day;
        private int _month;
        private int _weekOfObservance;
        private DayOfWeek _dayOfWeek;

        //consecutive holidays are for instances such as Christmas and Boxing day where Christmas holiday will be moved to Tuesday if Christmas is on Sunday
        public AnnualPublicHoliday(string name, int day, int month, bool moveToMonday = false, int consecutiveHolidays = 0)
        {
            Name = name;
            MoveToMonday = moveToMonday;
            _day = day;
            _month = month;
            _consecutiveHolidays = consecutiveHolidays;
        }

        public AnnualPublicHoliday(string name, int weekOfObservance, DayOfWeek day, int month)
        {
            Name = name;
            _month = month;
            _weekOfObservance = weekOfObservance;
            _dayOfWeek = day;
            FixedDay = false;
        }

        public DateTime? GetDate (int year)
        {
            var rtn = FixedDay ? new DateTime(year, _month, _day) : new DateTime(year, _month, 1);
            if(FixedDay && MoveToMonday)
            {
                while(DateUtils.IsWeekend(rtn))
                {
                    rtn = rtn.AddDays(1+_consecutiveHolidays);
                }
            } else if(!FixedDay)
            {
                var countDays = 0;
                var nextMonth = rtn.AddMonths(1).Month;
                for(;rtn.Month != nextMonth && countDays < _weekOfObservance; rtn = rtn.AddDays(1))
                {
                    if(rtn.DayOfWeek == _dayOfWeek)
                    {
                        rtn = rtn.AddDays((_weekOfObservance - 1) * 7);
                        break;
                    }
                }
                //Catch if bad data provided. eg: 8th Monday of January
                if(rtn.DayOfWeek != _dayOfWeek || rtn.Month != _month)
                {
                    return null;
                }
            }
            return rtn;
        }

        public bool Match(DateTime date)
        {
            return GetDate(date.Year).Value == date;
        }
    }
}
