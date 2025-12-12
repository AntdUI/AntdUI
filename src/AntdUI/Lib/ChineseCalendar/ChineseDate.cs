// THIS FILE IS PART OF ChineseCalendar PROJECT
// THE ChineseCalendar PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) lpz. ALL RIGHTS RESERVED.
// GITEE: https://gitee.com/lipz89/ChineseCalendar

using System;
using System.Globalization;

namespace ChineseCalendar
{
    /// <summary>
    /// 中国农历日期
    /// </summary>
    public class ChineseDate
    {
        private ChineseDate()
        {
        }

        /// <summary>
        /// 转换一个公历日期为农历日期
        /// </summary>
        /// <param name="date">公历日期</param>
        /// <exception cref="ArgumentOutOfRangeException">日期超出范围</exception>
        /// <returns>农历日期</returns>
        public static ChineseDate From(DateTime date)
        {
            try
            {
                var cdate = new ChineseDate
                {
                    Year = chineseCalendar.GetYear(date)
                };
                cdate.MonthIndex = cdate.Month = chineseCalendar.GetMonth(date);
                cdate.Day = chineseCalendar.GetDayOfMonth(date);
                //获取闰月， 0 则表示没有闰月
                int leapMonth = chineseCalendar.GetLeapMonth(cdate.Year);
                cdate.IsLeapMonth = leapMonth == cdate.Month;
                if (cdate.Month >= leapMonth && leapMonth > 0)
                {
                    cdate.Month--;
                }
                cdate.LeapMonthOfYear = Math.Max(0, leapMonth - 1);
                return cdate;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException($"日期超出范围 1901-02-19(公历) -- 2101-01-28(公历)", ex);
            }
        }

        /// <summary>
        /// 指定年月日索引，月/日可以为负数，负数表示倒数
        /// </summary>
        /// <param name="year">年份，范围为1901-2100</param>
        /// <param name="month">月份，允许值：1-12（当年不含闰月），1-13（当年含闰月）</param>
        /// <param name="day">日期，允许值：1-30（大月），1-29（小月）</param>
        /// <exception cref="ArgumentOutOfRangeException">日期超出范围</exception>
        /// <returns>农历日期</returns>
        public static ChineseDate FromIndex(int year, int month, int day)
        {
            if (year < 1901 || year > 2100)
            {
                throw new ArgumentOutOfRangeException($"年份超出范围 1901 -- 2100");
            }
            var months = chineseCalendar.GetMonthsInYear(year);
            if (month == 0 || month > months)
            {
                throw new ArgumentOutOfRangeException($"{year}年，月份允许范围为 1 -- {months}");
            }
            if (month < 0 - months)
            {
                throw new ArgumentOutOfRangeException($"{year}年，月份允许范围为 -1 -- {0 - months}");
            }
            if (month < 0)
            {
                month = months + month + 1;
            }

            var days = chineseCalendar.GetDaysInMonth(year, month);
            if (day == 0 || day > days)
            {
                throw new ArgumentOutOfRangeException($"日期允许范围为 1 -- {days}");
            }
            if (day < 0 - days)
            {
                throw new ArgumentOutOfRangeException($"日期允许范围为 -1 -- {0 - days}");
            }
            if (day < 0)
            {
                day = days + day + 1;
            }

            var cdate = new ChineseDate
            {
                Year = year
            };
            cdate.MonthIndex = cdate.Month = month;
            cdate.Day = day;
            int leapMonth = chineseCalendar.GetLeapMonth(year);
            cdate.IsLeapMonth = leapMonth == cdate.MonthIndex;
            if (cdate.Month >= leapMonth && leapMonth > 0)
            {
                cdate.Month--;
            }
            cdate.LeapMonthOfYear = Math.Max(0, leapMonth - 1);
            return cdate;
        }

        /// <summary>
        /// 指定年月日，月/日可以为负数，负数表示倒数
        /// </summary>
        /// <param name="year">年份，范围为1901-2100</param>
        /// <param name="month">月份，允许值：1-12，正数忽略闰月，负数忽略被闰月</param>
        /// <param name="day">日期，允许值：1-30（大月），1-29（小月）</param>
        /// <exception cref="ArgumentOutOfRangeException">日期超出范围</exception>
        /// <returns>农历日期</returns>
        public static ChineseDate From(int year, int month, int day)
        {
            if (year < 1901 || year > 2100)
            {
                throw new ArgumentOutOfRangeException($"年份超出范围 1901 -- 2100");
            }
            if (month == 0 || month > 12)
            {
                throw new ArgumentOutOfRangeException($"{year}年，月份允许范围为 1 -- 12");
            }
            if (month < 0 - 12)
            {
                throw new ArgumentOutOfRangeException($"{year}年，月份允许范围为 -1 -- -12");
            }
            int leapMonth = chineseCalendar.GetLeapMonth(year);
            if (month < 0)
            {
                month = 12 + month + 1;
            }
            var monthIndex = month;
            if (month > 0)
            {
                if (month >= leapMonth && leapMonth > 0)
                {
                    monthIndex++;
                }
            }
            else if (month < 0)
            {
                if (month >= leapMonth - 1 && leapMonth > 0)
                {
                    monthIndex++;
                }
            }

            var days = chineseCalendar.GetDaysInMonth(year, monthIndex);
            if (day == 0 || day > days)
            {
                throw new ArgumentOutOfRangeException($"日期允许范围为 1 -- {days}");
            }
            if (day < 0 - days)
            {
                throw new ArgumentOutOfRangeException($"日期允许范围为 -1 -- {0 - days}");
            }
            if (day < 0)
            {
                day = days + day + 1;
            }

            var cdate = new ChineseDate
            {
                Year = year,
                MonthIndex = monthIndex,
                Month = month,
                Day = day,
                IsLeapMonth = monthIndex == leapMonth,
                LeapMonthOfYear = Math.Max(0, leapMonth - 1)
            };
            return cdate;
        }

        /// <summary> 年份 </summary>
        public int Year { get; internal set; }

        /// <summary> 月份 </summary>
        public int Month { get; internal set; }

        /// <summary> 月份顺序，含闰月 </summary>
        public int MonthIndex { get; internal set; }

        /// <summary> 日期 </summary>
        public int Day { get; internal set; }

        /// <summary> 当前月是闰月 </summary>
        public bool IsLeapMonth { get; internal set; }

        /// <summary> 当年的闰月，0表示无闰月，正常范围 1-12 </summary>
        public int LeapMonthOfYear { get; internal set; }

        /// <summary> 今天 </summary>
        public static ChineseDate Today => From(DateTime.Today);

        /// <summary> 最小值 </summary> 
        public static ChineseDate MinValue => From(chineseCalendar.MinSupportedDateTime);

        /// <summary> 最大值 </summary>
        public static ChineseDate MaxValue => From(chineseCalendar.MaxSupportedDateTime);

        /// <summary>
        /// 返回当前农历日期对应的公历日期
        /// </summary>
        /// <returns>公历日期</returns>
        public DateTime ToDate() => chineseCalendar.ToDateTime(Year, MonthIndex, Day, 0, 0, 0, 0);

        public override bool Equals(object? obj)
        {
            if (obj is ChineseDate cd) return cd.Year == Year && cd.Month == Month && cd.Day == Day;
            return false;
        }

        public override int GetHashCode() => new { Year, MonthIndex, Day }.GetHashCode();

        public override string ToString() => $"{ChineseEra}[{AnimalSign}]年{MonthString}月{DayString}";

        private static readonly ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();

        ///<summary>  农历月 </summary>
        private static readonly string MONTHSTRING = "正二三四五六七八九十冬腊";
        ///<summary> 农历日 </summary>
        private static readonly string DAYSTRING = "初一初二初三初四初五初六初七初八初九初十十一十二十三十四十五十六十七十八十九二十廿一廿二廿三廿四廿五廿六廿七廿八廿九三十";
        ///<summary> 十天干 </summary>
        private static readonly string CELESTIAL_STEMS = "甲乙丙丁戊己庚辛壬癸";
        ///<summary> 十二地支 </summary>
        private static readonly string TERRESTRIAL_BRANCHS = "子丑寅卯辰巳午未申酉戌亥";
        ///<summary> 十二生肖 </summary>
        private static readonly string ANIMAL_SIGNS = "鼠牛虎免龙蛇马羊猴鸡狗猪";
        ///<summary> 数字 </summary>
        private static readonly string DIGITALS = "〇一二三四五六七八九";

        /// <summary> 天干 </summary>
        public string CelestialStem => CELESTIAL_STEMS[(Year - 4) % 10].ToString();

        /// <summary> 地支 </summary>
        public string TerrestrialBranch => TERRESTRIAL_BRANCHS[(Year - 4) % 12].ToString();

        /// <summary> 干支 </summary>
        public string ChineseEra => CelestialStem + TerrestrialBranch;

        /// <summary> 生肖属相 </summary>
        public string AnimalSign => ANIMAL_SIGNS[(Year - 4) % 12].ToString();

        /// <summary> 星期几 </summary>
        public DayOfWeek DayOfWeek => ToDate().DayOfWeek;

        /// <summary> 一年中的第几天，闰年1-384，平年1-354 </summary>
        public int DayOfYear => chineseCalendar.GetDayOfYear(ToDate());

        /// <summary> 当月总天数 </summary>
        public int DayInMonth => chineseCalendar.GetDaysInMonth(Year, MonthIndex);

        /// <summary> 当年总天数 </summary>
        public int DayInYear => chineseCalendar.GetDaysInYear(Year);

        /// <summary> 当年总月份数 </summary>
        public int MonthsInYear => chineseCalendar.GetMonthsInYear(Year);

        /// <summary> 日历名称 </summary>
        public string CalendarName => "农历";

        /// <summary> 年的字符串 </summary>
        public string YearString
        {
            get
            {
                var str = string.Empty;
                var u = 1000;
                var y = Year;
                while (u > 0)
                {
                    var i = y / u;
                    str += DIGITALS[i];
                    y %= u;
                    u /= 10;
                }
                return str;
            }
        }

        /// <summary> 农历月 </summary>
        public string MonthString
        {
            get
            {
                var str = MONTHSTRING[Month - 1].ToString();
                if (IsLeapMonth) str = "闰" + str;
                return str;
            }
        }

        /// <summary> 农历日 </summary>
        public string DayString => DAYSTRING.Substring((Day - 1) * 2, 2);

        /// <summary>
        /// 增加年份数值
        /// 结果总会是非闰月的日期
        /// </summary>
        /// <param name="value">年份数值，可为负数</param>
        /// <returns>增加指定年份后的日期</returns>
        /// <exception cref="ArgumentOutOfRangeException">日期超出范围</exception>
        public ChineseDate AddYears(int value)
        {
            if (value == 0) return this;
            var nyear = Year + value;
            if (nyear < 1901 || nyear > 2100) throw new ArgumentOutOfRangeException($"年份超出范围 1901 -- 2100");

            int leapMonth = chineseCalendar.GetLeapMonth(nyear);
            var nmonthIndex = Month;
            if (nmonthIndex >= leapMonth && leapMonth > 0) nmonthIndex++;
            var days = chineseCalendar.GetDaysInMonth(nyear, nmonthIndex);

            var nday = Math.Min(Day, days);
            return FromIndex(nyear, nmonthIndex, nday);
        }

        /// <summary>
        /// 增加月数
        /// </summary>
        /// <param name="value">月数，可为负数</param>
        /// <returns>增加指定月数后的日期</returns>
        /// <exception cref="ArgumentOutOfRangeException">日期超出范围</exception>
        public ChineseDate AddMonths(int value)
        {
            if (value == 0) return this;
            var nyear = Year;
            var nmonthIndex = MonthIndex + value;
            var months = chineseCalendar.GetMonthsInYear(nyear);
            if (nmonthIndex > months)
            {
                while (nmonthIndex > months)
                {
                    nmonthIndex -= months;
                    nyear += 1;

                    if (nyear < 1901 || nyear > 2100) throw new ArgumentOutOfRangeException($"年份超出范围 1901 -- 2100");
                    else months = chineseCalendar.GetMonthsInYear(nyear);
                }
            }
            else if (nmonthIndex < 1)
            {
                while (nmonthIndex < 1)
                {
                    nyear -= 1;

                    if (nyear < 1901 || nyear > 2100) throw new ArgumentOutOfRangeException($"年份超出范围 1901 -- 2100");
                    else
                    {
                        months = chineseCalendar.GetMonthsInYear(nyear);
                        nmonthIndex += months;
                    }
                }
            }
            var days = chineseCalendar.GetDaysInMonth(nyear, nmonthIndex);

            var nday = Math.Min(Day, days);
            return FromIndex(nyear, nmonthIndex, nday);
        }

        /// <summary>
        /// 增加天数
        /// </summary>
        /// <param name="value">天数，可为负数</param>
        /// <returns>增加指定天数后的日期</returns>
        public ChineseDate AddDays(int value)
        {
            if (value == 0) return this;
            return From(ToDate().AddDays(value));
        }
    }
}