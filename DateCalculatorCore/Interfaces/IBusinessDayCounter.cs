using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateCalculatorCore
{
    public interface IBusinessDayCounter
    {
        int WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate);

        int BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays);
    }
}
