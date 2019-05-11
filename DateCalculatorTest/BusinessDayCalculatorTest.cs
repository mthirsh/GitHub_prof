using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections;
using DateCalculatorLib;
using System.Collections.Generic;
using DateCalculatorCore;

namespace DateCalculatorTest
{
    [TestFixture]
    public class BusinessDayCalculatorTest
    {
        [TestCaseSource(typeof(BusinessDayCalculatorTestData), "WeekdaysBetweenTwoDatesValuesData")]
        public int TestWeekdaysBetweenTwoDates(DateTime startDate, DateTime endDate)
        {
            var underTest = new BusinessDayCounter();
            return underTest.WeekdaysBetweenTwoDates(startDate, endDate);
        }

        [TestCaseSource(typeof(BusinessDayCalculatorTestData), "BusinessDaysBetweenTwoDatesData")]
        public int TestBusinessDaysBetweenTwoDates(DateTime startDate, DateTime endDate, IList<DateTime> publicHolidays)
        {
            var underTest = new BusinessDayCounter();
            return underTest.BusinessDaysBetweenTwoDates(startDate, endDate, publicHolidays);
        }

        [TestCaseSource(typeof(BusinessDayCalculatorTestData), "BusinessDaysBetweenTwoDatesDataWithPublicHolidays")]
        public int TestBusinessDaysBetweenTwoDatesWithPublicHolidays(DateTime startDate, DateTime endDate, IList<AnnualPublicHoliday> publicHolidays)
        {
            var underTest = new BusinessDayCounter();
            return underTest.BusinessDaysBetweenTwoDates(startDate, endDate, publicHolidays);
        }
    }

    public class BusinessDayCalculatorTestData
    {
        public static IEnumerable WeekdaysBetweenTwoDatesValuesData
        {
            get
            {
                yield return new TestCaseData(new DateTime(2013, 10, 7), new DateTime(2013, 10, 9)).Returns(1);
                yield return new TestCaseData(new DateTime(2013, 10, 5), new DateTime(2013, 10, 14)).Returns(5);
                yield return new TestCaseData(new DateTime(2019, 5, 4), new DateTime(2019, 5, 12)).Returns(5);
                yield return new TestCaseData(new DateTime(2019, 5, 7), new DateTime(2019, 5, 20)).Returns(8);
                yield return new TestCaseData(new DateTime(2013, 10, 7), new DateTime(2014, 1, 1)).Returns(61);
                yield return new TestCaseData(new DateTime(2013, 10, 7), new DateTime(2013, 10, 5)).Returns(0);
            }
        }

        public static IList<DateTime> PublicHolidayDates => new List<DateTime>
        {
            new DateTime(2013, 12, 25),
            new DateTime(2013, 12, 26),
            new DateTime(2014, 1, 1)
        };

        public static IEnumerable BusinessDaysBetweenTwoDatesData
        {
            get
            {
                yield return new TestCaseData(new DateTime(2013, 10, 7), new DateTime(2013, 10, 9), PublicHolidayDates).Returns(1);
                yield return new TestCaseData(new DateTime(2013, 12, 24), new DateTime(2013, 12, 27), PublicHolidayDates).Returns(0);
                yield return new TestCaseData(new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), PublicHolidayDates).Returns(59);
            }
        }

        public static IList<AnnualPublicHoliday> AnnualPublicHolidays => new List<AnnualPublicHoliday>
        {
            new AnnualPublicHoliday("Christmas",25, 12, true, 1),
            new AnnualPublicHoliday("Boxing Day", 26, 12, true, 1),
            new AnnualPublicHoliday("New Years Day", 1, 1, true),
            new AnnualPublicHoliday("Thanksgiving", 4, DayOfWeek.Thursday, 11)
        };

        public static IEnumerable BusinessDaysBetweenTwoDatesDataWithPublicHolidays
        {
            get
            {
                yield return new TestCaseData(new DateTime(2021, 10, 7), new DateTime(2021, 10, 9), AnnualPublicHolidays).Returns(1);
                yield return new TestCaseData(new DateTime(2021, 12, 24), new DateTime(2021, 12, 27), AnnualPublicHolidays).Returns(0);
                yield return new TestCaseData(new DateTime(2021, 10, 7), new DateTime(2022, 1, 1), AnnualPublicHolidays).Returns(58);
            }
        }
    }
}
