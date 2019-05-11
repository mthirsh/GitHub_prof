using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateCalculatorCore;

namespace DateCalculatorLib
{
    public class BusinessDayCounter : IBusinessDayCounter
    {
        public int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            var days = (secondDate.AddDays(-1) - firstDate).Days;
            var weekends = days / 7 * 2;
            if(firstDate.DayOfWeek != DayOfWeek.Saturday && firstDate.DayOfWeek > secondDate.DayOfWeek)
            {
                weekends += 2;
            }
            else if(firstDate.DayOfWeek == DayOfWeek.Saturday && secondDate.DayOfWeek != DayOfWeek.Sunday)
            {
                weekends++;
            }
            return Math.Max(days - weekends, 0);
        }

        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
        {
            var rtn = WeekdaysBetweenTwoDates(firstDate, secondDate);
            rtn = rtn - publicHolidays.Count(date => date > firstDate && date < secondDate);
            return rtn;
        }

        public int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<AnnualPublicHoliday> publicHolidays)
        {
            var rtn = WeekdaysBetweenTwoDates(firstDate, secondDate);
            var maxDate = secondDate.Year == firstDate.Year ? secondDate : new DateTime(firstDate.Year, 12, 31).AddDays(1);
            var minDate = secondDate.Year == firstDate.Year ? firstDate : new DateTime(secondDate.Year, 12, 31).AddDays(-1);
            rtn = rtn - publicHolidays.Count(date => date.GetDate(firstDate.Year) > firstDate && date.GetDate(firstDate.Year) < maxDate);
            rtn = rtn - publicHolidays.Count(date => date.GetDate(secondDate.Year) > minDate && date.GetDate(secondDate.Year) < secondDate);
            rtn = rtn - publicHolidays.Count * (firstDate - secondDate).Days / 365;
            return rtn;
        }
    }
}
