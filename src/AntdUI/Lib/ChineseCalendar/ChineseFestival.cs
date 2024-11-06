// THIS FILE IS PART OF ChineseCalendar PROJECT
// THE ChineseCalendar PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) lpz. ALL RIGHTS RESERVED.
// GITEE: https://gitee.com/lipz89/ChineseCalendar

using System;

namespace ChineseCalendar
{
    /// <summary>
    /// 农历节假日
    /// </summary>
    public class ChineseFestival : LoopFestival
    {
        /// <summary> 节日的第一个日期 </summary>
        public ChineseDate First { get; protected set; }
        private ChineseFestival() { }
        /// <summary>
        /// 定义一个农历节假日
        /// </summary>
        /// <param name="name">节日名称</param>
        /// <param name="month">月份</param>
        /// <param name="day">日期</param>
        /// <param name="firstYear">第一个节日的年份，0表示无永恒</param>
        /// <param name="description">节日描述</param>
        /// <exception cref="ArgumentNullException">没有名称</exception>
        /// <exception cref="ArgumentOutOfRangeException">日期超出范围</exception>
        public ChineseFestival(string name, int month, int day, int? firstYear = null, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (month == 0 || month > 12 || month < -12)
            {
                throw new ArgumentOutOfRangeException(nameof(month), "[-12,-1],[1,12]", "月份超出范围");
            }
            if (day == 0 || day > 30 || day < -30)
            {
                throw new ArgumentOutOfRangeException(nameof(day), "[-30,-1],[1,30]", "日期超出范围");
            }
            Name = name;
            Month = month;
            Day = day;
            FirstYear = firstYear;
            Description = description;
            if (FirstYear.HasValue)
            {
                First = ChineseDate.From(firstYear.Value, month, day);
            }
        }
        /// <summary> 春节 </summary>
        public static readonly ChineseFestival SpringFestival = new ChineseFestival
        {
            Name = "春节",
            Description = "正月初一",
            Month = 1,
            Day = 1,
        };
        /// <summary> 元宵节 </summary>
        public static readonly ChineseFestival LanternFestival = new ChineseFestival
        {
            Name = "元宵节",
            Description = "正月十五",
            Month = 1,
            Day = 15,
        };
        /// <summary> 龙抬头 </summary>
        public static readonly ChineseFestival DragonHeadraisingDay = new ChineseFestival
        {
            Name = "龙抬头",
            Description = "二月初二",
            Month = 2,
            Day = 2,
        };
        /// <summary> 端午 </summary>
        public static readonly ChineseFestival DragonBoatFestival = new ChineseFestival
        {
            Name = "端午",
            Description = "五月初五",
            Month = 5,
            Day = 5,
        };
        /// <summary> 七夕 </summary>
        public static readonly ChineseFestival QixiFestival = new ChineseFestival
        {
            Name = "七夕",
            Description = "七月初七",
            Month = 7,
            Day = 7,
        };
        /// <summary> 中元节 </summary>
        public static readonly ChineseFestival GhostFestival = new ChineseFestival
        {
            Name = "中元节",
            Description = "七月十五",
            Month = 7,
            Day = 15,
        };
        /// <summary> 中秋 </summary>
        public static readonly ChineseFestival MidAutumnFestival = new ChineseFestival
        {
            Name = "中秋",
            Description = "八月十五",
            Month = 8,
            Day = 15,
        };
        /// <summary> 重阳节 </summary>
        public static readonly ChineseFestival DoubleNinthFestival = new ChineseFestival
        {
            Name = "重阳节",
            Description = "九月初九",
            Month = 9,
            Day = 9,
        };
        /// <summary> 除夕 </summary>
        public static readonly ChineseFestival NewYearsEve = new ChineseFestival
        {
            Name = "除夕",
            Description = "大年三十",
            Month = -1,
            Day = -1,
        };

        /// <inheritdoc/>
        protected override bool TryGetDate(int year, int month, int day, out DateTime date)
        {
            try
            {
                date = ChineseDate.From(year, month, day).ToDate();
                return true;
            }
            catch (Exception)
            {
                date = DateTime.Now;
                return false;
            }
        }
        /// <inheritdoc/>
        public override bool IsThisFestival(DateTime date)
        {
            var cdate = ChineseDate.From(date);
            var festival = ChineseDate.From(cdate.Year, Month, Day);
            return cdate == festival;
        }
    }
}
