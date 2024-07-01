// THIS FILE IS PART OF ChineseCalendar PROJECT
// THE ChineseCalendar PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHT (C) lpz. ALL RIGHTS RESERVED.
// GITEE: https://gitee.com/lipz89/ChineseCalendar

using System;
using System.Collections.Generic;

namespace ChineseCalendar
{
    /// <summary>
    /// 节日
    /// </summary>
    public abstract class Festival
    {
        /// <summary> 节日名称 </summary>
        public string Name { get; protected set; }
        /// <summary> 描述 </summary>
        public string Description { get; protected set; }
        /// <summary> 节日设立年份 </summary>
        public int? FirstYear { get; protected set; }
        /// <summary> 月份 </summary>
        public int Month { get; protected set; }
        /// <summary> 日期 </summary>
        public int Day { get; protected set; }
        /// <summary>
        /// 获取指定公历日期前一个节日
        /// </summary>
        /// <param name="date">指定公历日期</param>
        /// <param name="containsThisDate">是否包含指定日期</param>
        /// <returns>前一个节日，null表示前面没有该节日</returns>
        public abstract DateTime? GetLastDate(DateTime? date = null, bool containsThisDate = false);
        /// <summary>
        /// 获取指定公历日期后一个节日
        /// </summary>
        /// <param name="date">指定公历日期</param>
        /// <param name="containsThisDate">是否包含指定日期</param>
        /// <returns>后一个节日，null表示后面没有该节日</returns>
        public abstract DateTime? GetNextDate(DateTime? date = null, bool containsThisDate = false);
        /// <summary>
        /// 判断指定公历日期是否本节日
        /// </summary>
        /// <param name="date">指定公历日期</param>
        /// <returns>true该日期是当前节日，否则不是</returns>
        public abstract bool IsThisFestival(DateTime date);
        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
        /// <summary>
        /// 返回所有预定义的节日
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Festival> GetAllDefined()
        {
            yield return GregorianFestival.NewYearsDay;
            yield return GregorianFestival.TheTombWeepingDay;
            yield return GregorianFestival.InternationalWorkersDay;
            yield return GregorianFestival.TheNationalDay;

            yield return ChineseFestival.SpringFestival;
            yield return ChineseFestival.LanternFestival;
            yield return ChineseFestival.DragonBoatFestival;
            yield return ChineseFestival.QixiFestival;
            yield return ChineseFestival.MidAutumnFestival;
            yield return ChineseFestival.DoubleNinthFestival;
            yield return ChineseFestival.NewYearsEve;
        }
    }
}
