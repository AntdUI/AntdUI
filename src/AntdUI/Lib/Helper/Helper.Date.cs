// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;

namespace AntdUI
{
    partial class Helper
    {
        /// <summary>
        /// 检查日期是否在指定范围内
        /// </summary>
        /// <param name="date">要检查的日期</param>
        /// <param name="min">最小日期（可选）</param>
        /// <param name="max">最大日期（可选）</param>
        /// <returns>true表示日期在范围内，false表示超出范围</returns>
        public static bool DateExceed(DateTime date, DateTime? min, DateTime? max)
        {
            if (min.HasValue && min.Value >= date) return false;
            if (max.HasValue && max.Value <= date) return false;
            return true;
        }

        /// <summary>
        /// 检查日期加上指定月数后是否在指定范围内
        /// </summary>
        /// <param name="date">原始日期</param>
        /// <param name="num">要添加的月数</param>
        /// <param name="min">最小日期（可选）</param>
        /// <param name="max">最大日期（可选）</param>
        /// <returns>true表示日期在范围内，false表示超出范围</returns>
        public static bool DateExceedMonth(DateTime date, int num, DateTime? min, DateTime? max)
        {
            try
            {
                return DateExceedMonth(date.AddMonths(num), min, max);
            }
            catch { return false; }
        }
        
        /// <summary>
        /// 检查日期月份是否在指定范围内
        /// </summary>
        /// <param name="date">要检查的日期</param>
        /// <param name="min">最小日期（可选）</param>
        /// <param name="max">最大日期（可选）</param>
        /// <returns>true表示月份在范围内，false表示超出范围</returns>
        public static bool DateExceedMonth(DateTime date, DateTime? min, DateTime? max)
        {
            // 检查目标月份是否早于minDate所在的月份
            if (min.HasValue)
            {
                // 如果目标月份比minDate的月份还早，禁用
                if (date.Year < min.Value.Year || (date.Year == min.Value.Year && date.Month < min.Value.Month)) return false;
            }
            // 检查目标月份是否晚于maxDate所在的月份
            if (max.HasValue)
            {
                // 如果目标月份比maxDate的月份还晚，禁用
                if (date.Year > max.Value.Year || (date.Year == max.Value.Year && date.Month > max.Value.Month)) return false;
            }
            // 目标月份在允许范围内
            return true;
        }

        /// <summary>
        /// 检查日期加上指定年数后是否在指定范围内
        /// </summary>
        /// <param name="date">原始日期</param>
        /// <param name="num">要添加的年数</param>
        /// <param name="min">最小日期（可选）</param>
        /// <param name="max">最大日期（可选）</param>
        /// <returns>true表示日期在范围内，false表示超出范围</returns>
        public static bool DateExceedYear(DateTime date, int num, DateTime? min, DateTime? max)
        {
            try
            {
                return DateExceedYear(date.AddYears(num), min, max);
            }
            catch { return false; }
        }
        
        /// <summary>
        /// 检查日期年份是否在指定范围内
        /// </summary>
        /// <param name="date">要检查的日期</param>
        /// <param name="min">最小日期（可选）</param>
        /// <param name="max">最大日期（可选）</param>
        /// <returns>true表示年份在范围内，false表示超出范围</returns>
        public static bool DateExceedYear(DateTime date, DateTime? min, DateTime? max)
        {
            if (min.HasValue && min.Value >= date) return false;
            if (max.HasValue)
            {
                if (max.Value.Year == date.Year) return true;
                if (max.Value <= date) return false;
            }
            return true;
        }

        /// <summary>
        /// 检查日期是否在指定范围内（宽松模式，包含边界）
        /// </summary>
        /// <param name="date">要检查的日期</param>
        /// <param name="min">最小日期（可选）</param>
        /// <param name="max">最大日期（可选）</param>
        /// <returns>true表示日期在范围内，false表示超出范围</returns>
        public static bool DateExceedRelax(DateTime date, DateTime? min, DateTime? max)
        {
            if (min.HasValue && min.Value > date) return false;
            if (max.HasValue && max.Value < date) return false;
            return true;
        }

        /// <summary>
        /// 比较两个日期数组是否相等
        /// </summary>
        /// <param name="array1">第一个日期数组</param>
        /// <param name="array2">第二个日期数组</param>
        /// <returns>true表示两个数组相等，false表示不相等</returns>
        public static bool AreDateTimeArraysEqual(DateTime[]? array1, DateTime[]? array2)
        {
            // 两个都为null，视为相等
            if (array1 == null && array2 == null) return true;

            // 其中一个为null，另一个不为null，视为不相等
            if (array1 == null || array2 == null) return false;

            // 长度不同，视为不相等
            if (array1.Length != array2.Length) return false;

            // 逐个比较元素
            for (int i = 0; i < array1.Length; i++)
            {
                if (!array1[i].Equals(array2[i])) return false;
            }

            // 所有元素都相等
            return true;
        }

        /// <summary>
        /// 初始化语言相关的日期格式和常量
        /// </summary>
        /// <param name="YDR">是否使用英文格式</param>
        /// <param name="YearFormat">年份格式</param>
        /// <param name="MonthFormat">月份格式</param>
        /// <param name="Mon">星期一的显示文本</param>
        /// <param name="Tue">星期二的显示文本</param>
        /// <param name="Wed">星期三的显示文本</param>
        /// <param name="Thu">星期四的显示文本</param>
        /// <param name="Fri">星期五的显示文本</param>
        /// <param name="Sat">星期六的显示文本</param>
        /// <param name="Sun">星期日的显示文本</param>
        /// <param name="s_f_L">左侧对齐格式</param>
        /// <param name="s_f_R">右侧对齐格式</param>
        public static void InitLanguage(bool YDR, out string YearFormat, out string MonthFormat, out string Mon, out string Tue, out string Wed, out string Thu, out string Fri, out string Sat, out string Sun, out FormatFlags s_f_L, out FormatFlags s_f_R)
        {
            if (YDR)
            {
                YearFormat = "yyyy";
                MonthFormat = "MMM";
                Mon = Localization.Get("Mon", "Mon");
                Tue = Localization.Get("Tue", "Tue");
                Wed = Localization.Get("Wed", "Wed");
                Thu = Localization.Get("Thu", "Thu");
                Fri = Localization.Get("Fri", "Fri");
                Sat = Localization.Get("Sat", "Sat");
                Sun = Localization.Get("Sun", "Sun");
                s_f_L = FormatFlags.Left | FormatFlags.VerticalCenter;
                s_f_R = FormatFlags.Right | FormatFlags.VerticalCenter;
            }
            else
            {
                YearFormat = "yyyy年";
                MonthFormat = "MM月";
                Mon = Localization.Get("Mon", "一");
                Tue = Localization.Get("Tue", "二");
                Wed = Localization.Get("Wed", "三");
                Thu = Localization.Get("Thu", "四");
                Fri = Localization.Get("Fri", "五");
                Sat = Localization.Get("Sat", "六");
                Sun = Localization.Get("Sun", "日");
                s_f_L = FormatFlags.Right | FormatFlags.VerticalCenter;
                s_f_R = FormatFlags.Left | FormatFlags.VerticalCenter;
            }
        }
    }
}