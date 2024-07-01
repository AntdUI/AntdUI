// THIS FILE IS PART OF ChineseCalendar PROJECT
// THE ChineseCalendar PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) lpz. ALL RIGHTS RESERVED.
// GITEE: https://gitee.com/lipz89/ChineseCalendar

using System;

namespace ChineseCalendar
{
    /// <summary>
    /// 固定公历日期节假日
    /// </summary>
    public class FixDateFestival : Festival
    {
        /// <summary> 节日的第一个日期 </summary>
        public DateTime? First { get; protected set; }
        /// <summary> 年份 </summary>
        public int Year { get; private set; }
        /// <summary> 日期 </summary>
        public DateTime Date { get; private set; }
        /// <summary>
        /// 定义一个固定公历日期节假日
        /// </summary>
        /// <param name="name">节日名称</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="day">日期</param>
        /// <param name="description">节日描述</param>
        /// <exception cref="ArgumentNullException">没有名称</exception>
        /// <exception cref="ArgumentOutOfRangeException">年月日超出范围</exception>
        public FixDateFestival(string name, int year, int month, int day, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            try
            {
                this.Name = name;
                this.Year = year;
                this.Month = month;
                this.Day = day;
                this.Description = description;
                this.First = this.Date = new DateTime(year, month, day);
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("日期参数不正确", ex);
            }
        }
        /// <summary>
        /// 定义一个固定公历日期节假日
        /// </summary>
        /// <param name="name">节日名称</param>
        /// <param name="date">节日的公历日期</param>
        /// <param name="description">节日描述</param>
        /// <exception cref="ArgumentNullException">没有名称</exception>
        public FixDateFestival(string name, DateTime date, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            this.Name = name;
            this.Year = date.Year;
            this.Month = date.Month;
            this.Day = date.Day;
            this.Description = description;
            this.Date = date;
        }

        /// <inheritdoc/>
        public override DateTime? GetLastDate(DateTime? date, bool containsThisDate = false)
        {
            DateTime date2 = date.HasValue ? date.Value.Date : DateTime.Today;
            if (containsThisDate && IsThisFestival(date2))
            {
                return date2;
            }
            if (this.Date < date2)
            {
                return this.Date;
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
            if (this.Date > date2)
            {
                return this.Date;
            }
            return null;
        }
        /// <inheritdoc/>
        public override bool IsThisFestival(DateTime date)
        {
            return date.Date == Date.Date;
        }
    }
}
