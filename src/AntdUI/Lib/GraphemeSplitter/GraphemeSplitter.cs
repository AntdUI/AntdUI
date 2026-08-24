// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Globalization;

namespace AntdUI
{
    /// <summary>
    /// 文本方向
    /// </summary>
    public enum TextDirection
    {
        /// <summary>
        /// 从左到右
        /// </summary>
        LTR,
        /// <summary>
        /// 从右到左
        /// </summary>
        RTL,
        /// <summary>
        /// 中性（空格、标点、符号等）
        /// </summary>
        Neutral
    }

    public partial class GraphemeSplitter
    {
        public static int Each(string strText, Action<string, int, int, int> cb) => Each(strText, 0, cb);
        public static int Each(string? strText, int nIndex, Action<string, int, int, int> cb)
        {
            if (strText == null || nIndex >= strText.Length) return 0;
            int nCounter = 0;
            int nIndexCharStart = 0, nCharLen = 0, nLastCharLen = 0;
            int nCodePoint = 0;
            int nLeftBreakType = 0, nRightBreakType = 0;

            while (nIndex < strText.Length && char.IsLowSurrogate(strText, nIndex))
            {
                nIndex++;
                nCharLen++;
            }
            if (nCharLen != 0)
            {
                nCounter++;
                cb(strText, nIndex - nCharLen, nCharLen, nLeftBreakType);
            }
            nIndexCharStart = nIndex;
            nCodePoint = GetCodePoint(strText, nIndex);
            nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;       // >= 0x10000 is double char
            nLeftBreakType = GetBreakProperty(nCodePoint);
            nIndex += nLastCharLen;
            nCharLen = nLastCharLen;
            var lst_history_break_type = new List<int> { nLeftBreakType };
            while (nIndex < strText.Length)
            {
                nCodePoint = GetCodePoint(strText, nIndex);
                nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;   // >= 0x10000 is double char
                nRightBreakType = GetBreakProperty(nCodePoint);
                if (ShouldBreak(nRightBreakType, lst_history_break_type))
                {
                    nCounter++;
                    cb(strText, nIndexCharStart, nCharLen, nLeftBreakType);
                    nIndexCharStart = nIndex;
                    nCharLen = nLastCharLen;
                    lst_history_break_type.Clear();
                }
                else nCharLen += nLastCharLen;
                lst_history_break_type.Add(nRightBreakType);
                nIndex += nLastCharLen;
                nLeftBreakType = nRightBreakType;
            }
            if (nCharLen != 0)
            {
                nCounter++;
                cb(strText, nIndexCharStart, nCharLen, nLeftBreakType);
            }
            return nCounter;
        }

        public static int Each(string strText, Action<string, int> cb) => Each(strText, 0, cb);
        public static int Each(string? strText, int nIndex, Action<string, int> cb)
        {
            if (strText == null || nIndex >= strText.Length) return 0;
            int nCounter = 0;
            int nIndexCharStart = 0, nCharLen = 0, nLastCharLen = 0;
            int nCodePoint = 0;
            int nLeftBreakType = 0, nRightBreakType = 0;

            while (nIndex < strText.Length && char.IsLowSurrogate(strText, nIndex))
            {
                nIndex++;
                nCharLen++;
            }
            if (nCharLen != 0)
            {
                nCounter++;
                cb(strText.Substring(nIndex - nCharLen, nCharLen), nLeftBreakType);
            }
            nIndexCharStart = nIndex;
            nCodePoint = GetCodePoint(strText, nIndex);
            nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;       // >= 0x10000 is double char
            nLeftBreakType = GetBreakProperty(nCodePoint);
            nIndex += nLastCharLen;
            nCharLen = nLastCharLen;
            var lst_history_break_type = new List<int> { nLeftBreakType };
            while (nIndex < strText.Length)
            {
                nCodePoint = GetCodePoint(strText, nIndex);
                nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;   // >= 0x10000 is double char
                nRightBreakType = GetBreakProperty(nCodePoint);
                if (ShouldBreak(nRightBreakType, lst_history_break_type))
                {
                    nCounter++;
                    cb(strText.Substring(nIndexCharStart, nCharLen), nLeftBreakType);
                    nIndexCharStart = nIndex;
                    nCharLen = nLastCharLen;
                    lst_history_break_type.Clear();
                }
                else nCharLen += nLastCharLen;
                lst_history_break_type.Add(nRightBreakType);
                nIndex += nLastCharLen;
                nLeftBreakType = nRightBreakType;
            }
            if (nCharLen != 0)
            {
                nCounter++;
                cb(strText.Substring(nIndexCharStart, nCharLen), nLeftBreakType);
            }
            return nCounter;
        }

        public static int EachT(string? strText, int nIndex, Action<string, STRE_TYPE, int, int, int> cb)
        {
            if (strText == null || nIndex >= strText.Length) return 0;
            int nCounter = 0;
            int nIndexCharStart = 0, nCharLen = 0, nLastCharLen = 0;
            int nCodePoint = 0;
            int nLeftBreakType = 0, nRightBreakType = 0;

            while (nIndex < strText.Length && char.IsLowSurrogate(strText, nIndex))
            {
                nIndex++;
                nCharLen++;
            }
            if (nCharLen != 0)
            {
                nCounter++;
                cb(strText, STRE_TYPE.STR, nIndex - nCharLen, nCharLen, nLeftBreakType);
            }
            nIndexCharStart = nIndex;
            nCodePoint = GetCodePoint(strText, nIndex);
            nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;       // >= 0x10000 is double char
            nLeftBreakType = GetBreakProperty(nCodePoint);
            nIndex += nLastCharLen;
            nCharLen = nLastCharLen;
            var lst_history_break_type = new List<int> { nLeftBreakType };
            while (nIndex < strText.Length)
            {
                nCodePoint = GetCodePoint(strText, nIndex);
                nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;   // >= 0x10000 is double char
                nRightBreakType = GetBreakProperty(nCodePoint);
                int LastLen = nLastCharLen;
                if (ShouldBreak(nRightBreakType, lst_history_break_type))
                {
                    nCounter++;
                    if (ReadSvg(strText, nIndexCharStart, nCharLen, out var endIndexSvg))
                    {
                        cb(strText, STRE_TYPE.SVG, nIndexCharStart, endIndexSvg, 0);
                        nIndexCharStart += endIndexSvg;
                        LastLen = endIndexSvg;
                        nCharLen = nLastCharLen;
                    }
                    else if (ReadBase64Image(strText, nIndexCharStart, nCharLen, out var endIndex))
                    {
                        cb(strText, STRE_TYPE.BASE64IMG, nIndexCharStart, endIndex, 0);
                        nIndexCharStart += endIndex;
                        LastLen = endIndex;
                        nCharLen = nLastCharLen;
                    }
                    else
                    {
                        cb(strText, STRE_TYPE.STR, nIndexCharStart, nCharLen, nLeftBreakType);
                        nIndexCharStart = nIndex;
                        nCharLen = nLastCharLen;
                    }
                    lst_history_break_type.Clear();
                }
                else nCharLen += nLastCharLen;
                lst_history_break_type.Add(nRightBreakType);
                nIndex += LastLen;
                nLeftBreakType = nRightBreakType;
            }
            if (nCharLen != 0 && nIndexCharStart < strText.Length)
            {
                nCounter++;
                cb(strText, STRE_TYPE.STR, nIndexCharStart, nCharLen, nLeftBreakType);
            }
            return nCounter;
        }

        public static int EachCount(string? strText, int nIndex = 0)
        {
            if (strText == null || nIndex >= strText.Length) return 0;
            int nCounter = 0;
            int nIndexCharStart = 0, nCharLen = 0, nLastCharLen = 0;
            int nCodePoint = 0;
            int nLeftBreakType = 0, nRightBreakType = 0;

            while (nIndex < strText.Length && char.IsLowSurrogate(strText, nIndex))
            {
                nIndex++;
                nCharLen++;
            }
            if (nCharLen != 0) nCounter++;

            nIndexCharStart = nIndex;
            nCodePoint = GetCodePoint(strText, nIndex);
            nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;       // >= 0x10000 is double char
            nLeftBreakType = GetBreakProperty(nCodePoint);
            nIndex += nLastCharLen;
            nCharLen = nLastCharLen;
            var lst_history_break_type = new List<int> { nLeftBreakType };
            while (nIndex < strText.Length)
            {
                nCodePoint = GetCodePoint(strText, nIndex);
                nLastCharLen = nCodePoint >= 0x10000 ? 2 : 1;   // >= 0x10000 is double char
                nRightBreakType = GetBreakProperty(nCodePoint);
                if (ShouldBreak(nRightBreakType, lst_history_break_type))
                {
                    nCounter++;
                    nIndexCharStart = nIndex;
                    nCharLen = nLastCharLen;
                    lst_history_break_type.Clear();
                }
                else nCharLen += nLastCharLen;
                lst_history_break_type.Add(nRightBreakType);
                nIndex += nLastCharLen;
                nLeftBreakType = nRightBreakType;
            }
            if (nCharLen != 0) nCounter++;
            return nCounter;
        }

        public static bool IsEmoji(int type, string txt) => type == 18 || (type == 4 && SvgDb.Emoji.ContainsKey(txt));

        public static bool IsArabic(int type, string txt) => type == 0 && IsArabic(GetCodePoint(txt, 0));

        public static bool IsRTL(int type, string txt) => type == 0 && IsRTL(GetCodePoint(txt, 0));

        public static bool IsArabic(int codePoint) =>
            (codePoint >= 0x0600 && codePoint <= 0x06FF) ||   // Basic Arabic
            (codePoint >= 0x0750 && codePoint <= 0x077F) ||   // Arabic Supplement
            (codePoint >= 0x08A0 && codePoint <= 0x08FF) ||   // Arabic Extended-A
            (codePoint >= 0xFB50 && codePoint <= 0xFDFF) ||   // Arabic Presentation Forms-A
            (codePoint >= 0xFE70 && codePoint <= 0xFEFF);     // Arabic Presentation Forms-B

        public static bool IsRTL(int codePoint) =>
            IsArabic(codePoint) ||
            (codePoint >= 0x0590 && codePoint <= 0x05FF) ||   // Hebrew
            (codePoint >= 0x0710 && codePoint <= 0x073F) ||   // Syriac
            (codePoint >= 0x0780 && codePoint <= 0x07FF) ||   // Thaana / NKo
            (codePoint >= 0x0800 && codePoint <= 0x083F) ||   // Samaritan
            (codePoint >= 0x0840 && codePoint <= 0x085F);     // Mandaic

        /// <summary>
        /// 获取字符簇方向
        /// </summary>
        public static TextDirection GetDirection(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return TextDirection.Neutral;
            return GetDirection(GetCodePoint(txt, 0));
        }

        /// <summary>
        /// 获取码点方向（阿拉伯/希伯来等 RTL；字母数字 LTR；其余中性）
        /// </summary>
        public static TextDirection GetDirection(int codePoint)
        {
            if (codePoint < 0) return TextDirection.Neutral;
            if (IsRTL(codePoint)) return TextDirection.RTL;
            if (codePoint >= 0x10000) return TextDirection.Neutral;
            switch (CharUnicodeInfo.GetUnicodeCategory((char)codePoint))
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.LetterNumber:
                case UnicodeCategory.DecimalDigitNumber:
                    return TextDirection.LTR;
                default:
                    return TextDirection.Neutral;
            }
        }
    }
}