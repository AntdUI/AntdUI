// THIS FILE IS PART OF ChineseCalendar PROJECT
// THE ChineseCalendar PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) lpz. ALL RIGHTS RESERVED.
// GITEE: https://gitee.com/lipz89/ChineseCalendar

using System;
using System.Collections.Generic;

namespace ChineseCalendar
{
    /// <summary>
    /// 公历节假日
    /// </summary>
    public class GregorianFestival : LoopFestival
    {
        private GregorianFestival() { }
        /// <summary> 节日的第一个日期 </summary>
        public DateTime? First { get; protected set; }

        private static readonly Dictionary<int, int> monthDays = new Dictionary<int, int> {
            { 1,31},{ 2,29},{ 3,31},{ 4,30},{ 5,31},{ 6,30},{ 7,31},{ 8,31},{ 9,30},{ 10,31},{ 11,30},{ 12,31},
        };
        /// <summary>
        /// 定义一个公历节日
        /// </summary>
        /// <param name="name">节日名称</param>
        /// <param name="month">月份</param>
        /// <param name="day">日期</param>
        /// <param name="firstYear">第一个节日的年份，0表示无永恒</param>
        /// <param name="description">节日描述</param>
        /// <exception cref="ArgumentNullException">没有名称</exception>
        /// <exception cref="ArgumentOutOfRangeException">日期超出范围</exception>
        public GregorianFestival(string name, int month, int day, int firstYear = 0, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month), "[1,12]", "月份超出范围");
            }
            var maxDays = monthDays[month];
            if (day < 1 || day > maxDays)
            {
                throw new ArgumentOutOfRangeException(nameof(day), $"[1,{maxDays}]", "日期超出范围");
            }

            Name = name;
            Month = month;
            Day = day;
            FirstYear = firstYear;
            Description = description;
            if (FirstYear.HasValue)
            {
                First = new DateTime(firstYear, month, day);
            }
        }
        /// <summary> 元旦 </summary>
        public static readonly GregorianFestival NewYearsDay = new GregorianFestival
        {
            Name = "元旦",
            Description = "1月1日",
            Month = 1,
            Day = 1,
        };
        /// <summary> 植树节 </summary>
        public static readonly GregorianFestival ArborDay = new GregorianFestival
        {
            Name = "植树节",
            Description = "3月12日",
            Month = 3,
            Day = 12,
            FirstYear = 1979
        };
        /// <summary> 清明 </summary>
        public static readonly GregorianFestival TheTombWeepingDay = new GregorianFestival
        {
            Name = "清明",
            Description = "4月5日",
            Month = 4,
            Day = 5,
        };
        /// <summary> 劳动节 </summary>
        public static readonly GregorianFestival InternationalWorkersDay = new GregorianFestival
        {
            Name = "劳动节",
            Description = "5月1日",
            Month = 5,
            Day = 1,
            FirstYear = 1890
        };
        /// <summary> 国庆节 </summary>
        public static readonly GregorianFestival TheNationalDay = new GregorianFestival
        {
            Name = "国庆节",
            Description = "10月1日",
            Month = 10,
            Day = 1,
            FirstYear = 1949
        };

        /// <inheritdoc/>
        public override bool IsThisFestival(DateTime date)
        {
            return date.Month == Month && date.Day == Day;
        }
    }
}
