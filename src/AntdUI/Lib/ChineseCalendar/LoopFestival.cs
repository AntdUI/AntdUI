// THIS FILE IS PART OF ChineseCalendar PROJECT
// THE ChineseCalendar PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) lpz. ALL RIGHTS RESERVED.
// GITEE: https://gitee.com/lipz89/ChineseCalendar

using System;

namespace ChineseCalendar
{
    /// <summary>
    /// 循环节假日
    /// </summary>
    public abstract class LoopFestival : Festival
    {
        /// <summary>
        /// 尝试根据年月日获取一个日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="date">返回日期</param>
        /// <returns>是否取到合法日期</returns>
        protected virtual bool TryGetDate(int year, int month, int day, out DateTime date)
        {
            try
            {
                date = new DateTime(year, month, day);
                return true;
            }
            catch (Exception)
            {
                date = DateTime.Now;
                return false;
            }
        }

        /// <inheritdoc/>
        public override DateTime? GetLastDate(DateTime? date, bool containsThisDate = false)
        {
            DateTime date2 = date.HasValue ? date.Value.Date : DateTime.Today;
            if (containsThisDate && IsThisFestival(date2))
            {
                return date2;
            }
            if (TryGetDate(date2.Year, Month, Day, out DateTime date3) && date3 < date2)
            {
                return date3;
            }
            var year = date2.Year - 1;
            while (year >= DateTime.MinValue.Year && (!FirstYear.HasValue || year >= FirstYear.Value))
            {
                if (TryGetDate(year, Month, Day, out DateTime date4))
                {
                    return date4;
                }
                year--;
            }
            return null;
        }
        /// <inheritdoc/>
        public override DateTime? GetNextDate(DateTime? date, bool containsThisDate = false)
        {
            DateTime date2 = date.HasValue ? date.Value.Date : DateTime.Today;
            if (containsThisDate && IsThisFestival(date2))
            {
                return date2;
            }
            if (TryGetDate(date2.Year, Month, Day, out DateTime date3) && date3 > date2)
            {
                return date3;
            }
            var year = date2.Year + 1;
            while (year <= DateTime.MaxValue.Year)
            {
                if (TryGetDate(year, Month, Day, out DateTime date4))
                {
                    return date4;
                }
                year++;
            }
            return null;
        }

        /// <inheritdoc/>
        public override bool IsThisFestival(DateTime date)
        {
            return date.Month == Month && date.Day == Day;
        }
    }
}
