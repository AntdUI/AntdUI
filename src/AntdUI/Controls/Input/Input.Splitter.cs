// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITCODE: https://gitcode.com/AntdUI/AntdUI
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AntdUI
{
    public class GraphemeSplitter
    {
        #region BASE

        protected struct RangeInfo
        {
            public int Start;
            public int End;
            public int Type;
        }
        public static int Each(string strText, Func<string, int, int, int, bool> cb)
        {
            return Each(strText, 0, cb);
        }
        public static int Each(string strText, int nIndex, Func<string, int, int, int, bool> cb)
        {
            int nCounter = 0;
            if (string.IsNullOrEmpty(strText) || nIndex >= strText.Length) return 0;
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
                if (!CharCompleted(strText, nIndex - nCharLen, nCharLen, nLeftBreakType, cb)) return nCounter;
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
                    if (!CharCompleted(strText, nIndexCharStart, nCharLen, nLeftBreakType, cb)) return nCounter;
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
                CharCompleted(strText, nIndexCharStart, nCharLen, nLeftBreakType, cb);
            }
            return nCounter;
        }

        public static int EachT(string strText, int nIndex, Func<string, STRE_TYPE, int, int, int, bool> cb)
        {
            int nCounter = 0;
            if (string.IsNullOrEmpty(strText) || nIndex >= strText.Length) return 0;
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
                if (!cb(strText, STRE_TYPE.STR, nIndex - nCharLen, nCharLen, nLeftBreakType)) return nCounter;
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
                        if (!cb(strText, STRE_TYPE.SVG, nIndexCharStart, endIndexSvg, 0)) return nCounter;
                        nIndexCharStart += endIndexSvg;
                        LastLen = endIndexSvg;
                        nCharLen = nLastCharLen;
                    }
                    else if (ReadBase64Image(strText, nIndexCharStart, nCharLen, out var endIndex))
                    {
                        if (!cb(strText, STRE_TYPE.BASE64IMG, nIndexCharStart, endIndex, 0)) return nCounter;
                        nIndexCharStart += endIndex;
                        LastLen = endIndex;
                        nCharLen = nLastCharLen;
                    }
                    else
                    {
                        if (!cb(strText, STRE_TYPE.STR, nIndexCharStart, nCharLen, nLeftBreakType)) return nCounter;
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

        static bool ReadSvg(string strText, int start, int len, out int endIndex)
        {
            if (strText.Substring(start, len) == "<" && start + 5 < strText.Length && strText.Substring(start, 4) == "<svg")
            {
                int tmp = strText.IndexOf("</svg>", start + 5, StringComparison.OrdinalIgnoreCase);
                if (tmp > 0)
                {
                    endIndex = tmp + 6 - start;
                    return true;
                }
            }
            endIndex = -1;
            return false;
        }
        static bool ReadBase64Image(string strText, int start, int len, out int endIndex)
        {
            if (start + 20 < strText.Length && strText.Substring(start, 11) == "data:image/")
            {
                int tmp = strText.IndexOf(";base64,", start + 12, StringComparison.OrdinalIgnoreCase);
                if (tmp > 0)
                {
                    var regex = new Regex(@"data:image/(?<type>.+?);base64,(?<data>[A-Za-z0-9+/=]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var match = regex.Match(strText.Substring(start, strText.Length - start));
                    if (match.Success && match.Index == 0)
                    {
                        if (match.Value.EndsWith("data"))
                        {
                            int st = start + match.Value.Length - 4;
                            if (st + 20 < strText.Length && strText.Substring(st, 11) == "data:image/")
                            {
                                endIndex = match.Value.Length - 4;
                                return true;
                            }
                        }
                        endIndex = match.Value.Length;
                        return true;
                    }
                }
            }
            endIndex = -1;
            return false;
        }

        public enum STRE_TYPE : int
        {
            STR,
            SVG,
            BASE64IMG
        }

        public static int GetCodePoint(string strText, int nIndex)
        {
            if (strText[nIndex] < '\uD800' || strText[nIndex] > '\uDFFF') return strText[nIndex];
            if (char.IsHighSurrogate(strText, nIndex))
            {
                if (nIndex + 1 >= strText.Length) return 0;
            }
            else
            {
                if (--nIndex < 0) return 0;
            }
            return ((strText[nIndex] & 0x03FF) << 10) + (strText[nIndex + 1] & 0x03FF) + 0x10000;
        }

        static bool CharCompleted(string strText, int nIndex, int nLen, int nType, Func<string, int, int, int, bool> cb_bool)
        {
            if (!cb_bool(strText, nIndex, nLen, nType)) return false;
            return true;
        }

        #endregion

        public const int Other = 0;
        public const int CR = 1;
        public const int LF = 2;
        public const int Control = 3;
        public const int Extend = 4;
        public const int Regional_Indicator = 5;
        public const int SpacingMark = 6;
        public const int L = 7;
        public const int V = 8;
        public const int T = 9;
        public const int LV = 10;
        public const int LVT = 11;
        public const int Prepend = 12;
        public const int ZWJ = 15;
        public const int Extended_Pictographic = 18;

        static List<RangeInfo> m_lst_code_range = new List<RangeInfo>();

        static GraphemeSplitter()
        {
            // Cc  [10] <control-0000>..<control-0009>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00000, End = 0x00009, Type = Control });
            // Cc   [2] <control-000B>..<control-000C>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0000B, End = 0x0000C, Type = Control });
            // Cc  [18] <control-000E>..<control-001F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0000E, End = 0x0001F, Type = Control });
            // Cc  [33] <control-007F>..<control-009F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0007F, End = 0x0009F, Type = Control });
            // Mn [112] COMBINING GRAVE ACCENT..COMBINING LATIN SMALL LETTER X
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00300, End = 0x0036F, Type = Extend });
            // Mn   [5] COMBINING CYRILLIC TITLO..COMBINING CYRILLIC POKRYTIE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00483, End = 0x00487, Type = Extend });
            // Me   [2] COMBINING CYRILLIC HUNDRED THOUSANDS SIGN..COMBINING CYRILLIC MILLIONS SIGN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00488, End = 0x00489, Type = Extend });
            // Mn  [45] HEBREW ACCENT ETNAHTA..HEBREW POINT METEG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00591, End = 0x005BD, Type = Extend });
            // Mn   [2] HEBREW POINT SHIN DOT..HEBREW POINT SIN DOT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x005C1, End = 0x005C2, Type = Extend });
            // Mn   [2] HEBREW MARK UPPER DOT..HEBREW MARK LOWER DOT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x005C4, End = 0x005C5, Type = Extend });
            // Cf   [6] ARABIC NUMBER SIGN..ARABIC NUMBER MARK ABOVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00600, End = 0x00605, Type = Prepend });
            // Mn  [11] ARABIC SIGN SALLALLAHOU ALAYHE WASSALLAM..ARABIC SMALL KASRA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00610, End = 0x0061A, Type = Extend });
            // Mn  [21] ARABIC FATHATAN..ARABIC WAVY HAMZA BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0064B, End = 0x0065F, Type = Extend });
            // Mn   [7] ARABIC SMALL HIGH LIGATURE SAD WITH LAM WITH ALEF MAKSURA..ARABIC SMALL HIGH SEEN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x006D6, End = 0x006DC, Type = Extend });
            // Mn   [6] ARABIC SMALL HIGH ROUNDED ZERO..ARABIC SMALL HIGH MADDA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x006DF, End = 0x006E4, Type = Extend });
            // Mn   [2] ARABIC SMALL HIGH YEH..ARABIC SMALL HIGH NOON
            m_lst_code_range.Add(new RangeInfo() { Start = 0x006E7, End = 0x006E8, Type = Extend });
            // Mn   [4] ARABIC EMPTY CENTRE LOW STOP..ARABIC SMALL LOW MEEM
            m_lst_code_range.Add(new RangeInfo() { Start = 0x006EA, End = 0x006ED, Type = Extend });
            // Mn  [27] SYRIAC PTHAHA ABOVE..SYRIAC BARREKH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00730, End = 0x0074A, Type = Extend });
            // Mn  [11] THAANA ABAFILI..THAANA SUKUN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x007A6, End = 0x007B0, Type = Extend });
            // Mn   [9] NKO COMBINING SHORT HIGH TONE..NKO COMBINING DOUBLE DOT ABOVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x007EB, End = 0x007F3, Type = Extend });
            // Mn   [4] SAMARITAN MARK IN..SAMARITAN MARK DAGESH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00816, End = 0x00819, Type = Extend });
            // Mn   [9] SAMARITAN MARK EPENTHETIC YUT..SAMARITAN VOWEL SIGN A
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0081B, End = 0x00823, Type = Extend });
            // Mn   [3] SAMARITAN VOWEL SIGN SHORT A..SAMARITAN VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00825, End = 0x00827, Type = Extend });
            // Mn   [5] SAMARITAN VOWEL SIGN LONG I..SAMARITAN MARK NEQUDAA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00829, End = 0x0082D, Type = Extend });
            // Mn   [3] MANDAIC AFFRICATION MARK..MANDAIC GEMINATION MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00859, End = 0x0085B, Type = Extend });
            // Cf   [2] ARABIC POUND MARK ABOVE..ARABIC PIASTRE MARK ABOVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00890, End = 0x00891, Type = Prepend });
            // Mn   [8] ARABIC SMALL HIGH WORD AL-JUZ..ARABIC HALF MADDA OVER MADDA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00898, End = 0x0089F, Type = Extend });
            // Mn  [24] ARABIC SMALL HIGH FARSI YEH..ARABIC SMALL HIGH SIGN SAFHA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x008CA, End = 0x008E1, Type = Extend });
            // Mn  [32] ARABIC TURNED DAMMA BELOW..DEVANAGARI SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x008E3, End = 0x00902, Type = Extend });
            // Mc   [3] DEVANAGARI VOWEL SIGN AA..DEVANAGARI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0093E, End = 0x00940, Type = SpacingMark });
            // Mn   [8] DEVANAGARI VOWEL SIGN U..DEVANAGARI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00941, End = 0x00948, Type = Extend });
            // Mc   [4] DEVANAGARI VOWEL SIGN CANDRA O..DEVANAGARI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00949, End = 0x0094C, Type = SpacingMark });
            // Mc   [2] DEVANAGARI VOWEL SIGN PRISHTHAMATRA E..DEVANAGARI VOWEL SIGN AW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0094E, End = 0x0094F, Type = SpacingMark });
            // Mn   [7] DEVANAGARI STRESS SIGN UDATTA..DEVANAGARI VOWEL SIGN UUE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00951, End = 0x00957, Type = Extend });
            // Mn   [2] DEVANAGARI VOWEL SIGN VOCALIC L..DEVANAGARI VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00962, End = 0x00963, Type = Extend });
            // Mc   [2] BENGALI SIGN ANUSVARA..BENGALI SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00982, End = 0x00983, Type = SpacingMark });
            // Mc   [2] BENGALI VOWEL SIGN I..BENGALI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x009BF, End = 0x009C0, Type = SpacingMark });
            // Mn   [4] BENGALI VOWEL SIGN U..BENGALI VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x009C1, End = 0x009C4, Type = Extend });
            // Mc   [2] BENGALI VOWEL SIGN E..BENGALI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x009C7, End = 0x009C8, Type = SpacingMark });
            // Mc   [2] BENGALI VOWEL SIGN O..BENGALI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x009CB, End = 0x009CC, Type = SpacingMark });
            // Mn   [2] BENGALI VOWEL SIGN VOCALIC L..BENGALI VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x009E2, End = 0x009E3, Type = Extend });
            // Mn   [2] GURMUKHI SIGN ADAK BINDI..GURMUKHI SIGN BINDI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00A01, End = 0x00A02, Type = Extend });
            // Mc   [3] GURMUKHI VOWEL SIGN AA..GURMUKHI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00A3E, End = 0x00A40, Type = SpacingMark });
            // Mn   [2] GURMUKHI VOWEL SIGN U..GURMUKHI VOWEL SIGN UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00A41, End = 0x00A42, Type = Extend });
            // Mn   [2] GURMUKHI VOWEL SIGN EE..GURMUKHI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00A47, End = 0x00A48, Type = Extend });
            // Mn   [3] GURMUKHI VOWEL SIGN OO..GURMUKHI SIGN VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00A4B, End = 0x00A4D, Type = Extend });
            // Mn   [2] GURMUKHI TIPPI..GURMUKHI ADDAK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00A70, End = 0x00A71, Type = Extend });
            // Mn   [2] GUJARATI SIGN CANDRABINDU..GUJARATI SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00A81, End = 0x00A82, Type = Extend });
            // Mc   [3] GUJARATI VOWEL SIGN AA..GUJARATI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00ABE, End = 0x00AC0, Type = SpacingMark });
            // Mn   [5] GUJARATI VOWEL SIGN U..GUJARATI VOWEL SIGN CANDRA E
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00AC1, End = 0x00AC5, Type = Extend });
            // Mn   [2] GUJARATI VOWEL SIGN E..GUJARATI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00AC7, End = 0x00AC8, Type = Extend });
            // Mc   [2] GUJARATI VOWEL SIGN O..GUJARATI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00ACB, End = 0x00ACC, Type = SpacingMark });
            // Mn   [2] GUJARATI VOWEL SIGN VOCALIC L..GUJARATI VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00AE2, End = 0x00AE3, Type = Extend });
            // Mn   [6] GUJARATI SIGN SUKUN..GUJARATI SIGN TWO-CIRCLE NUKTA ABOVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00AFA, End = 0x00AFF, Type = Extend });
            // Mc   [2] ORIYA SIGN ANUSVARA..ORIYA SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00B02, End = 0x00B03, Type = SpacingMark });
            // Mn   [4] ORIYA VOWEL SIGN U..ORIYA VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00B41, End = 0x00B44, Type = Extend });
            // Mc   [2] ORIYA VOWEL SIGN E..ORIYA VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00B47, End = 0x00B48, Type = SpacingMark });
            // Mc   [2] ORIYA VOWEL SIGN O..ORIYA VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00B4B, End = 0x00B4C, Type = SpacingMark });
            // Mn   [2] ORIYA SIGN OVERLINE..ORIYA AI LENGTH MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00B55, End = 0x00B56, Type = Extend });
            // Mn   [2] ORIYA VOWEL SIGN VOCALIC L..ORIYA VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00B62, End = 0x00B63, Type = Extend });
            // Mc   [2] TAMIL VOWEL SIGN U..TAMIL VOWEL SIGN UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00BC1, End = 0x00BC2, Type = SpacingMark });
            // Mc   [3] TAMIL VOWEL SIGN E..TAMIL VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00BC6, End = 0x00BC8, Type = SpacingMark });
            // Mc   [3] TAMIL VOWEL SIGN O..TAMIL VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00BCA, End = 0x00BCC, Type = SpacingMark });
            // Mc   [3] TELUGU SIGN CANDRABINDU..TELUGU SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C01, End = 0x00C03, Type = SpacingMark });
            // Mn   [3] TELUGU VOWEL SIGN AA..TELUGU VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C3E, End = 0x00C40, Type = Extend });
            // Mc   [4] TELUGU VOWEL SIGN U..TELUGU VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C41, End = 0x00C44, Type = SpacingMark });
            // Mn   [3] TELUGU VOWEL SIGN E..TELUGU VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C46, End = 0x00C48, Type = Extend });
            // Mn   [4] TELUGU VOWEL SIGN O..TELUGU SIGN VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C4A, End = 0x00C4D, Type = Extend });
            // Mn   [2] TELUGU LENGTH MARK..TELUGU AI LENGTH MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C55, End = 0x00C56, Type = Extend });
            // Mn   [2] TELUGU VOWEL SIGN VOCALIC L..TELUGU VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C62, End = 0x00C63, Type = Extend });
            // Mc   [2] KANNADA SIGN ANUSVARA..KANNADA SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00C82, End = 0x00C83, Type = SpacingMark });
            // Mc   [2] KANNADA VOWEL SIGN II..KANNADA VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00CC0, End = 0x00CC1, Type = SpacingMark });
            // Mc   [2] KANNADA VOWEL SIGN VOCALIC R..KANNADA VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00CC3, End = 0x00CC4, Type = SpacingMark });
            // Mc   [2] KANNADA VOWEL SIGN EE..KANNADA VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00CC7, End = 0x00CC8, Type = SpacingMark });
            // Mc   [2] KANNADA VOWEL SIGN O..KANNADA VOWEL SIGN OO
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00CCA, End = 0x00CCB, Type = SpacingMark });
            // Mn   [2] KANNADA VOWEL SIGN AU..KANNADA SIGN VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00CCC, End = 0x00CCD, Type = Extend });
            // Mc   [2] KANNADA LENGTH MARK..KANNADA AI LENGTH MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00CD5, End = 0x00CD6, Type = Extend });
            // Mn   [2] KANNADA VOWEL SIGN VOCALIC L..KANNADA VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00CE2, End = 0x00CE3, Type = Extend });
            // Mn   [2] MALAYALAM SIGN COMBINING ANUSVARA ABOVE..MALAYALAM SIGN CANDRABINDU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D00, End = 0x00D01, Type = Extend });
            // Mc   [2] MALAYALAM SIGN ANUSVARA..MALAYALAM SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D02, End = 0x00D03, Type = SpacingMark });
            // Mn   [2] MALAYALAM SIGN VERTICAL BAR VIRAMA..MALAYALAM SIGN CIRCULAR VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D3B, End = 0x00D3C, Type = Extend });
            // Mc   [2] MALAYALAM VOWEL SIGN I..MALAYALAM VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D3F, End = 0x00D40, Type = SpacingMark });
            // Mn   [4] MALAYALAM VOWEL SIGN U..MALAYALAM VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D41, End = 0x00D44, Type = Extend });
            // Mc   [3] MALAYALAM VOWEL SIGN E..MALAYALAM VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D46, End = 0x00D48, Type = SpacingMark });
            // Mc   [3] MALAYALAM VOWEL SIGN O..MALAYALAM VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D4A, End = 0x00D4C, Type = SpacingMark });
            // Mn   [2] MALAYALAM VOWEL SIGN VOCALIC L..MALAYALAM VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D62, End = 0x00D63, Type = Extend });
            // Mc   [2] SINHALA SIGN ANUSVARAYA..SINHALA SIGN VISARGAYA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00D82, End = 0x00D83, Type = SpacingMark });
            // Mc   [2] SINHALA VOWEL SIGN KETTI AEDA-PILLA..SINHALA VOWEL SIGN DIGA AEDA-PILLA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00DD0, End = 0x00DD1, Type = SpacingMark });
            // Mn   [3] SINHALA VOWEL SIGN KETTI IS-PILLA..SINHALA VOWEL SIGN KETTI PAA-PILLA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00DD2, End = 0x00DD4, Type = Extend });
            // Mc   [7] SINHALA VOWEL SIGN GAETTA-PILLA..SINHALA VOWEL SIGN KOMBUVA HAA GAYANUKITTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00DD8, End = 0x00DDE, Type = SpacingMark });
            // Mc   [2] SINHALA VOWEL SIGN DIGA GAETTA-PILLA..SINHALA VOWEL SIGN DIGA GAYANUKITTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00DF2, End = 0x00DF3, Type = SpacingMark });
            // Mn   [7] THAI CHARACTER SARA I..THAI CHARACTER PHINTHU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00E34, End = 0x00E3A, Type = Extend });
            // Mn   [8] THAI CHARACTER MAITAIKHU..THAI CHARACTER YAMAKKAN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00E47, End = 0x00E4E, Type = Extend });
            // Mn   [9] LAO VOWEL SIGN I..LAO SEMIVOWEL SIGN LO
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00EB4, End = 0x00EBC, Type = Extend });
            // Mn   [6] LAO TONE MAI EK..LAO NIGGAHITA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00EC8, End = 0x00ECD, Type = Extend });
            // Mn   [2] TIBETAN ASTROLOGICAL SIGN -KHYUD PA..TIBETAN ASTROLOGICAL SIGN SDONG TSHUGS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00F18, End = 0x00F19, Type = Extend });
            // Mc   [2] TIBETAN SIGN YAR TSHES..TIBETAN SIGN MAR TSHES
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00F3E, End = 0x00F3F, Type = SpacingMark });
            // Mn  [14] TIBETAN VOWEL SIGN AA..TIBETAN SIGN RJES SU NGA RO
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00F71, End = 0x00F7E, Type = Extend });
            // Mn   [5] TIBETAN VOWEL SIGN REVERSED I..TIBETAN MARK HALANTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00F80, End = 0x00F84, Type = Extend });
            // Mn   [2] TIBETAN SIGN LCI RTAGS..TIBETAN SIGN YANG RTAGS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00F86, End = 0x00F87, Type = Extend });
            // Mn  [11] TIBETAN SUBJOINED SIGN LCE TSA CAN..TIBETAN SUBJOINED LETTER JA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00F8D, End = 0x00F97, Type = Extend });
            // Mn  [36] TIBETAN SUBJOINED LETTER NYA..TIBETAN SUBJOINED LETTER FIXED-FORM RA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x00F99, End = 0x00FBC, Type = Extend });
            // Mn   [4] MYANMAR VOWEL SIGN I..MYANMAR VOWEL SIGN UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0102D, End = 0x01030, Type = Extend });
            // Mn   [6] MYANMAR VOWEL SIGN AI..MYANMAR SIGN DOT BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01032, End = 0x01037, Type = Extend });
            // Mn   [2] MYANMAR SIGN VIRAMA..MYANMAR SIGN ASAT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01039, End = 0x0103A, Type = Extend });
            // Mc   [2] MYANMAR CONSONANT SIGN MEDIAL YA..MYANMAR CONSONANT SIGN MEDIAL RA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0103B, End = 0x0103C, Type = SpacingMark });
            // Mn   [2] MYANMAR CONSONANT SIGN MEDIAL WA..MYANMAR CONSONANT SIGN MEDIAL HA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0103D, End = 0x0103E, Type = Extend });
            // Mc   [2] MYANMAR VOWEL SIGN VOCALIC R..MYANMAR VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01056, End = 0x01057, Type = SpacingMark });
            // Mn   [2] MYANMAR VOWEL SIGN VOCALIC L..MYANMAR VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01058, End = 0x01059, Type = Extend });
            // Mn   [3] MYANMAR CONSONANT SIGN MON MEDIAL NA..MYANMAR CONSONANT SIGN MON MEDIAL LA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0105E, End = 0x01060, Type = Extend });
            // Mn   [4] MYANMAR VOWEL SIGN GEBA KAREN I..MYANMAR VOWEL SIGN KAYAH EE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01071, End = 0x01074, Type = Extend });
            // Mn   [2] MYANMAR VOWEL SIGN SHAN E ABOVE..MYANMAR VOWEL SIGN SHAN FINAL Y
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01085, End = 0x01086, Type = Extend });
            // Lo  [96] HANGUL CHOSEONG KIYEOK..HANGUL CHOSEONG FILLER
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01100, End = 0x0115F, Type = L });
            // Lo  [72] HANGUL JUNGSEONG FILLER..HANGUL JUNGSEONG O-YAE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01160, End = 0x011A7, Type = V });
            // Lo  [88] HANGUL JONGSEONG KIYEOK..HANGUL JONGSEONG SSANGNIEUN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x011A8, End = 0x011FF, Type = T });
            // Mn   [3] ETHIOPIC COMBINING GEMINATION AND VOWEL LENGTH MARK..ETHIOPIC COMBINING GEMINATION MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0135D, End = 0x0135F, Type = Extend });
            // Mn   [3] TAGALOG VOWEL SIGN I..TAGALOG SIGN VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01712, End = 0x01714, Type = Extend });
            // Mn   [2] HANUNOO VOWEL SIGN I..HANUNOO VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01732, End = 0x01733, Type = Extend });
            // Mn   [2] BUHID VOWEL SIGN I..BUHID VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01752, End = 0x01753, Type = Extend });
            // Mn   [2] TAGBANWA VOWEL SIGN I..TAGBANWA VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01772, End = 0x01773, Type = Extend });
            // Mn   [2] KHMER VOWEL INHERENT AQ..KHMER VOWEL INHERENT AA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x017B4, End = 0x017B5, Type = Extend });
            // Mn   [7] KHMER VOWEL SIGN I..KHMER VOWEL SIGN UA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x017B7, End = 0x017BD, Type = Extend });
            // Mc   [8] KHMER VOWEL SIGN OE..KHMER VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x017BE, End = 0x017C5, Type = SpacingMark });
            // Mc   [2] KHMER SIGN REAHMUK..KHMER SIGN YUUKALEAPINTU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x017C7, End = 0x017C8, Type = SpacingMark });
            // Mn  [11] KHMER SIGN MUUSIKATOAN..KHMER SIGN BATHAMASAT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x017C9, End = 0x017D3, Type = Extend });
            // Mn   [3] MONGOLIAN FREE VARIATION SELECTOR ONE..MONGOLIAN FREE VARIATION SELECTOR THREE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0180B, End = 0x0180D, Type = Extend });
            // Mn   [2] MONGOLIAN LETTER ALI GALI BALUDA..MONGOLIAN LETTER ALI GALI THREE BALUDA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01885, End = 0x01886, Type = Extend });
            // Mn   [3] LIMBU VOWEL SIGN A..LIMBU VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01920, End = 0x01922, Type = Extend });
            // Mc   [4] LIMBU VOWEL SIGN EE..LIMBU VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01923, End = 0x01926, Type = SpacingMark });
            // Mn   [2] LIMBU VOWEL SIGN E..LIMBU VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01927, End = 0x01928, Type = Extend });
            // Mc   [3] LIMBU SUBJOINED LETTER YA..LIMBU SUBJOINED LETTER WA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01929, End = 0x0192B, Type = SpacingMark });
            // Mc   [2] LIMBU SMALL LETTER KA..LIMBU SMALL LETTER NGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01930, End = 0x01931, Type = SpacingMark });
            // Mc   [6] LIMBU SMALL LETTER TA..LIMBU SMALL LETTER LA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01933, End = 0x01938, Type = SpacingMark });
            // Mn   [3] LIMBU SIGN MUKPHRENG..LIMBU SIGN SA-I
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01939, End = 0x0193B, Type = Extend });
            // Mn   [2] BUGINESE VOWEL SIGN I..BUGINESE VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01A17, End = 0x01A18, Type = Extend });
            // Mc   [2] BUGINESE VOWEL SIGN E..BUGINESE VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01A19, End = 0x01A1A, Type = SpacingMark });
            // Mn   [7] TAI THAM SIGN MAI KANG LAI..TAI THAM CONSONANT SIGN SA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01A58, End = 0x01A5E, Type = Extend });
            // Mn   [8] TAI THAM VOWEL SIGN I..TAI THAM VOWEL SIGN OA BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01A65, End = 0x01A6C, Type = Extend });
            // Mc   [6] TAI THAM VOWEL SIGN OY..TAI THAM VOWEL SIGN THAM AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01A6D, End = 0x01A72, Type = SpacingMark });
            // Mn  [10] TAI THAM VOWEL SIGN OA ABOVE..TAI THAM SIGN KHUEN-LUE KARAN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01A73, End = 0x01A7C, Type = Extend });
            // Mn  [14] COMBINING DOUBLED CIRCUMFLEX ACCENT..COMBINING PARENTHESES BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01AB0, End = 0x01ABD, Type = Extend });
            // Mn  [16] COMBINING LATIN SMALL LETTER W BELOW..COMBINING LATIN SMALL LETTER INSULAR T
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01ABF, End = 0x01ACE, Type = Extend });
            // Mn   [4] BALINESE SIGN ULU RICEM..BALINESE SIGN SURANG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01B00, End = 0x01B03, Type = Extend });
            // Mn   [5] BALINESE VOWEL SIGN ULU..BALINESE VOWEL SIGN RA REPA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01B36, End = 0x01B3A, Type = Extend });
            // Mc   [5] BALINESE VOWEL SIGN LA LENGA TEDUNG..BALINESE VOWEL SIGN TALING REPA TEDUNG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01B3D, End = 0x01B41, Type = SpacingMark });
            // Mc   [2] BALINESE VOWEL SIGN PEPET TEDUNG..BALINESE ADEG ADEG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01B43, End = 0x01B44, Type = SpacingMark });
            // Mn   [9] BALINESE MUSICAL SYMBOL COMBINING TEGEH..BALINESE MUSICAL SYMBOL COMBINING GONG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01B6B, End = 0x01B73, Type = Extend });
            // Mn   [2] SUNDANESE SIGN PANYECEK..SUNDANESE SIGN PANGLAYAR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01B80, End = 0x01B81, Type = Extend });
            // Mn   [4] SUNDANESE CONSONANT SIGN PANYAKRA..SUNDANESE VOWEL SIGN PANYUKU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BA2, End = 0x01BA5, Type = Extend });
            // Mc   [2] SUNDANESE VOWEL SIGN PANAELAENG..SUNDANESE VOWEL SIGN PANOLONG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BA6, End = 0x01BA7, Type = SpacingMark });
            // Mn   [2] SUNDANESE VOWEL SIGN PAMEPET..SUNDANESE VOWEL SIGN PANEULEUNG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BA8, End = 0x01BA9, Type = Extend });
            // Mn   [3] SUNDANESE SIGN VIRAMA..SUNDANESE CONSONANT SIGN PASANGAN WA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BAB, End = 0x01BAD, Type = Extend });
            // Mn   [2] BATAK VOWEL SIGN PAKPAK E..BATAK VOWEL SIGN EE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BE8, End = 0x01BE9, Type = Extend });
            // Mc   [3] BATAK VOWEL SIGN I..BATAK VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BEA, End = 0x01BEC, Type = SpacingMark });
            // Mn   [3] BATAK VOWEL SIGN U FOR SIMALUNGUN SA..BATAK CONSONANT SIGN H
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BEF, End = 0x01BF1, Type = Extend });
            // Mc   [2] BATAK PANGOLAT..BATAK PANONGONAN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01BF2, End = 0x01BF3, Type = SpacingMark });
            // Mc   [8] LEPCHA SUBJOINED LETTER YA..LEPCHA VOWEL SIGN UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01C24, End = 0x01C2B, Type = SpacingMark });
            // Mn   [8] LEPCHA VOWEL SIGN E..LEPCHA CONSONANT SIGN T
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01C2C, End = 0x01C33, Type = Extend });
            // Mc   [2] LEPCHA CONSONANT SIGN NYIN-DO..LEPCHA CONSONANT SIGN KANG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01C34, End = 0x01C35, Type = SpacingMark });
            // Mn   [2] LEPCHA SIGN RAN..LEPCHA SIGN NUKTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01C36, End = 0x01C37, Type = Extend });
            // Mn   [3] VEDIC TONE KARSHANA..VEDIC TONE PRENKHA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01CD0, End = 0x01CD2, Type = Extend });
            // Mn  [13] VEDIC SIGN YAJURVEDIC MIDLINE SVARITA..VEDIC TONE RIGVEDIC KASHMIRI INDEPENDENT SVARITA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01CD4, End = 0x01CE0, Type = Extend });
            // Mn   [7] VEDIC SIGN VISARGA SVARITA..VEDIC SIGN VISARGA ANUDATTA WITH TAIL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01CE2, End = 0x01CE8, Type = Extend });
            // Mn   [2] VEDIC TONE RING ABOVE..VEDIC TONE DOUBLE RING ABOVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01CF8, End = 0x01CF9, Type = Extend });
            // Mn  [64] COMBINING DOTTED GRAVE ACCENT..COMBINING RIGHT ARROWHEAD AND DOWN ARROWHEAD BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x01DC0, End = 0x01DFF, Type = Extend });
            // Cf   [2] LEFT-TO-RIGHT MARK..RIGHT-TO-LEFT MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0200E, End = 0x0200F, Type = Control });
            // Cf   [5] LEFT-TO-RIGHT EMBEDDING..RIGHT-TO-LEFT OVERRIDE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0202A, End = 0x0202E, Type = Control });
            // Cf   [5] WORD JOINER..INVISIBLE PLUS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02060, End = 0x02064, Type = Control });
            // Cf  [10] LEFT-TO-RIGHT ISOLATE..NOMINAL DIGIT SHAPES
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02066, End = 0x0206F, Type = Control });
            // Mn  [13] COMBINING LEFT HARPOON ABOVE..COMBINING FOUR DOTS ABOVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x020D0, End = 0x020DC, Type = Extend });
            // Me   [4] COMBINING ENCLOSING CIRCLE..COMBINING ENCLOSING CIRCLE BACKSLASH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x020DD, End = 0x020E0, Type = Extend });
            // Me   [3] COMBINING ENCLOSING SCREEN..COMBINING ENCLOSING UPWARD POINTING TRIANGLE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x020E2, End = 0x020E4, Type = Extend });
            // Mn  [12] COMBINING REVERSE SOLIDUS OVERLAY..COMBINING ASTERISK ABOVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x020E5, End = 0x020F0, Type = Extend });
            // E0.6   [6] (↔️..↙️)    left-right arrow..down-left arrow
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02194, End = 0x02199, Type = Extended_Pictographic });
            // E0.6   [2] (↩️..↪️)    right arrow curving left..left arrow curving right
            m_lst_code_range.Add(new RangeInfo() { Start = 0x021A9, End = 0x021AA, Type = Extended_Pictographic });
            // E0.6   [2] (⌚..⌛)    watch..hourglass done
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0231A, End = 0x0231B, Type = Extended_Pictographic });
            // E0.6   [4] (⏩..⏬)    fast-forward button..fast down button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x023E9, End = 0x023EC, Type = Extended_Pictographic });
            // E0.7   [2] (⏭️..⏮️)    next track button..last track button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x023ED, End = 0x023EE, Type = Extended_Pictographic });
            // E1.0   [2] (⏱️..⏲️)    stopwatch..timer clock
            m_lst_code_range.Add(new RangeInfo() { Start = 0x023F1, End = 0x023F2, Type = Extended_Pictographic });
            // E0.7   [3] (⏸️..⏺️)    pause button..record button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x023F8, End = 0x023FA, Type = Extended_Pictographic });
            // E0.6   [2] (▪️..▫️)    black small square..white small square
            m_lst_code_range.Add(new RangeInfo() { Start = 0x025AA, End = 0x025AB, Type = Extended_Pictographic });
            // E0.6   [4] (◻️..◾)    white medium square..black medium-small square
            m_lst_code_range.Add(new RangeInfo() { Start = 0x025FB, End = 0x025FE, Type = Extended_Pictographic });
            // E0.6   [2] (☀️..☁️)    sun..cloud
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02600, End = 0x02601, Type = Extended_Pictographic });
            // E0.7   [2] (☂️..☃️)    umbrella..snowman
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02602, End = 0x02603, Type = Extended_Pictographic });
            // E0.0   [7] (☇..☍)    LIGHTNING..OPPOSITION
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02607, End = 0x0260D, Type = Extended_Pictographic });
            // E0.0   [2] (☏..☐)    WHITE TELEPHONE..BALLOT BOX
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0260F, End = 0x02610, Type = Extended_Pictographic });
            // E0.6   [2] (☔..☕)    umbrella with rain drops..hot beverage
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02614, End = 0x02615, Type = Extended_Pictographic });
            // E0.0   [2] (☖..☗)    WHITE SHOGI PIECE..BLACK SHOGI PIECE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02616, End = 0x02617, Type = Extended_Pictographic });
            // E0.0   [4] (☙..☜)    REVERSED ROTATED FLORAL HEART BULLET..WHITE LEFT POINTING INDEX
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02619, End = 0x0261C, Type = Extended_Pictographic });
            // E0.0   [2] (☞..☟)    WHITE RIGHT POINTING INDEX..WHITE DOWN POINTING INDEX
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0261E, End = 0x0261F, Type = Extended_Pictographic });
            // E1.0   [2] (☢️..☣️)    radioactive..biohazard
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02622, End = 0x02623, Type = Extended_Pictographic });
            // E0.0   [2] (☤..☥)    CADUCEUS..ANKH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02624, End = 0x02625, Type = Extended_Pictographic });
            // E0.0   [3] (☧..☩)    CHI RHO..CROSS OF JERUSALEM
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02627, End = 0x02629, Type = Extended_Pictographic });
            // E0.0   [3] (☫..☭)    FARSI SYMBOL..HAMMER AND SICKLE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0262B, End = 0x0262D, Type = Extended_Pictographic });
            // E0.0   [8] (☰..☷)    TRIGRAM FOR HEAVEN..TRIGRAM FOR EARTH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02630, End = 0x02637, Type = Extended_Pictographic });
            // E0.7   [2] (☸️..☹️)    wheel of dharma..frowning face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02638, End = 0x02639, Type = Extended_Pictographic });
            // E0.0   [5] (☻..☿)    BLACK SMILING FACE..MERCURY
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0263B, End = 0x0263F, Type = Extended_Pictographic });
            // E0.0   [5] (♃..♇)    JUPITER..PLUTO
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02643, End = 0x02647, Type = Extended_Pictographic });
            // E0.6  [12] (♈..♓)    Aries..Pisces
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02648, End = 0x02653, Type = Extended_Pictographic });
            // E0.0  [11] (♔..♞)    WHITE CHESS KING..BLACK CHESS KNIGHT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02654, End = 0x0265E, Type = Extended_Pictographic });
            // E0.0   [2] (♡..♢)    WHITE HEART SUIT..WHITE DIAMOND SUIT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02661, End = 0x02662, Type = Extended_Pictographic });
            // E0.6   [2] (♥️..♦️)    heart suit..diamond suit
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02665, End = 0x02666, Type = Extended_Pictographic });
            // E0.0  [18] (♩..♺)    QUARTER NOTE..RECYCLING SYMBOL FOR GENERIC MATERIALS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02669, End = 0x0267A, Type = Extended_Pictographic });
            // E0.0   [2] (♼..♽)    RECYCLED PAPER SYMBOL..PARTIALLY-RECYCLED PAPER SYMBOL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0267C, End = 0x0267D, Type = Extended_Pictographic });
            // E0.0   [6] (⚀..⚅)    DIE FACE-1..DIE FACE-6
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02680, End = 0x02685, Type = Extended_Pictographic });
            // E0.0   [2] (⚐..⚑)    WHITE FLAG..BLACK FLAG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02690, End = 0x02691, Type = Extended_Pictographic });
            // E1.0   [2] (⚖️..⚗️)    balance scale..alembic
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02696, End = 0x02697, Type = Extended_Pictographic });
            // E1.0   [2] (⚛️..⚜️)    atom symbol..fleur-de-lis
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0269B, End = 0x0269C, Type = Extended_Pictographic });
            // E0.0   [3] (⚝..⚟)    OUTLINED WHITE STAR..THREE LINES CONVERGING LEFT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0269D, End = 0x0269F, Type = Extended_Pictographic });
            // E0.6   [2] (⚠️..⚡)    warning..high voltage
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026A0, End = 0x026A1, Type = Extended_Pictographic });
            // E0.0   [5] (⚢..⚦)    DOUBLED FEMALE SIGN..MALE WITH STROKE SIGN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026A2, End = 0x026A6, Type = Extended_Pictographic });
            // E0.0   [2] (⚨..⚩)    VERTICAL MALE WITH STROKE SIGN..HORIZONTAL MALE WITH STROKE SIGN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026A8, End = 0x026A9, Type = Extended_Pictographic });
            // E0.6   [2] (⚪..⚫)    white circle..black circle
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026AA, End = 0x026AB, Type = Extended_Pictographic });
            // E0.0   [4] (⚬..⚯)    MEDIUM SMALL WHITE CIRCLE..UNMARRIED PARTNERSHIP SYMBOL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026AC, End = 0x026AF, Type = Extended_Pictographic });
            // E1.0   [2] (⚰️..⚱️)    coffin..funeral urn
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026B0, End = 0x026B1, Type = Extended_Pictographic });
            // E0.0  [11] (⚲..⚼)    NEUTER..SESQUIQUADRATE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026B2, End = 0x026BC, Type = Extended_Pictographic });
            // E0.6   [2] (⚽..⚾)    soccer ball..baseball
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026BD, End = 0x026BE, Type = Extended_Pictographic });
            // E0.0   [5] (⚿..⛃)    SQUARED KEY..BLACK DRAUGHTS KING
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026BF, End = 0x026C3, Type = Extended_Pictographic });
            // E0.6   [2] (⛄..⛅)    snowman without snow..sun behind cloud
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026C4, End = 0x026C5, Type = Extended_Pictographic });
            // E0.0   [2] (⛆..⛇)    RAIN..BLACK SNOWMAN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026C6, End = 0x026C7, Type = Extended_Pictographic });
            // E0.0   [5] (⛉..⛍)    TURNED WHITE SHOGI PIECE..DISABLED CAR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026C9, End = 0x026CD, Type = Extended_Pictographic });
            // E0.0  [20] (⛕..⛨)    ALTERNATE ONE-WAY LEFT WAY TRAFFIC..BLACK CROSS ON SHIELD
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026D5, End = 0x026E8, Type = Extended_Pictographic });
            // E0.0   [5] (⛫..⛯)    CASTLE..MAP SYMBOL FOR LIGHTHOUSE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026EB, End = 0x026EF, Type = Extended_Pictographic });
            // E0.7   [2] (⛰️..⛱️)    mountain..umbrella on ground
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026F0, End = 0x026F1, Type = Extended_Pictographic });
            // E0.6   [2] (⛲..⛳)    fountain..flag in hole
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026F2, End = 0x026F3, Type = Extended_Pictographic });
            // E0.7   [3] (⛷️..⛹️)    skier..person bouncing ball
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026F7, End = 0x026F9, Type = Extended_Pictographic });
            // E0.0   [2] (⛻..⛼)    JAPANESE BANK SYMBOL..HEADSTONE GRAVEYARD SYMBOL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026FB, End = 0x026FC, Type = Extended_Pictographic });
            // E0.0   [4] (⛾..✁)    CUP ON BLACK SQUARE..UPPER BLADE SCISSORS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x026FE, End = 0x02701, Type = Extended_Pictographic });
            // E0.0   [2] (✃..✄)    LOWER BLADE SCISSORS..WHITE SCISSORS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02703, End = 0x02704, Type = Extended_Pictographic });
            // E0.6   [5] (✈️..✌️)    airplane..victory hand
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02708, End = 0x0270C, Type = Extended_Pictographic });
            // E0.0   [2] (✐..✑)    UPPER RIGHT PENCIL..WHITE NIB
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02710, End = 0x02711, Type = Extended_Pictographic });
            // E0.6   [2] (✳️..✴️)    eight-spoked asterisk..eight-pointed star
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02733, End = 0x02734, Type = Extended_Pictographic });
            // E0.6   [3] (❓..❕)    red question mark..white exclamation mark
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02753, End = 0x02755, Type = Extended_Pictographic });
            // E0.0   [3] (❥..❧)    ROTATED HEAVY BLACK HEART BULLET..ROTATED FLORAL HEART BULLET
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02765, End = 0x02767, Type = Extended_Pictographic });
            // E0.6   [3] (➕..➗)    plus..divide
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02795, End = 0x02797, Type = Extended_Pictographic });
            // E0.6   [2] (⤴️..⤵️)    right arrow curving up..right arrow curving down
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02934, End = 0x02935, Type = Extended_Pictographic });
            // E0.6   [3] (⬅️..⬇️)    left arrow..down arrow
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02B05, End = 0x02B07, Type = Extended_Pictographic });
            // E0.6   [2] (⬛..⬜)    black large square..white large square
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02B1B, End = 0x02B1C, Type = Extended_Pictographic });
            // Mn   [3] COPTIC COMBINING NI ABOVE..COPTIC COMBINING SPIRITUS LENIS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02CEF, End = 0x02CF1, Type = Extend });
            // Mn  [32] COMBINING CYRILLIC LETTER BE..COMBINING CYRILLIC LETTER IOTIFIED BIG YUS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x02DE0, End = 0x02DFF, Type = Extend });
            // Mn   [4] IDEOGRAPHIC LEVEL TONE MARK..IDEOGRAPHIC ENTERING TONE MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0302A, End = 0x0302D, Type = Extend });
            // Mc   [2] HANGUL SINGLE DOT TONE MARK..HANGUL DOUBLE DOT TONE MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0302E, End = 0x0302F, Type = Extend });
            // Mn   [2] COMBINING KATAKANA-HIRAGANA VOICED SOUND MARK..COMBINING KATAKANA-HIRAGANA SEMI-VOICED SOUND MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x03099, End = 0x0309A, Type = Extend });
            // Me   [3] COMBINING CYRILLIC TEN MILLIONS SIGN..COMBINING CYRILLIC THOUSAND MILLIONS SIGN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A670, End = 0x0A672, Type = Extend });
            // Mn  [10] COMBINING CYRILLIC LETTER UKRAINIAN IE..COMBINING CYRILLIC PAYEROK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A674, End = 0x0A67D, Type = Extend });
            // Mn   [2] COMBINING CYRILLIC LETTER EF..COMBINING CYRILLIC LETTER IOTIFIED E
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A69E, End = 0x0A69F, Type = Extend });
            // Mn   [2] BAMUM COMBINING MARK KOQNDON..BAMUM COMBINING MARK TUKWENTIS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A6F0, End = 0x0A6F1, Type = Extend });
            // Mc   [2] SYLOTI NAGRI VOWEL SIGN A..SYLOTI NAGRI VOWEL SIGN I
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A823, End = 0x0A824, Type = SpacingMark });
            // Mn   [2] SYLOTI NAGRI VOWEL SIGN U..SYLOTI NAGRI VOWEL SIGN E
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A825, End = 0x0A826, Type = Extend });
            // Mc   [2] SAURASHTRA SIGN ANUSVARA..SAURASHTRA SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A880, End = 0x0A881, Type = SpacingMark });
            // Mc  [16] SAURASHTRA CONSONANT SIGN HAARU..SAURASHTRA VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A8B4, End = 0x0A8C3, Type = SpacingMark });
            // Mn   [2] SAURASHTRA SIGN VIRAMA..SAURASHTRA SIGN CANDRABINDU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A8C4, End = 0x0A8C5, Type = Extend });
            // Mn  [18] COMBINING DEVANAGARI DIGIT ZERO..COMBINING DEVANAGARI SIGN AVAGRAHA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A8E0, End = 0x0A8F1, Type = Extend });
            // Mn   [8] KAYAH LI VOWEL UE..KAYAH LI TONE CALYA PLOPHU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A926, End = 0x0A92D, Type = Extend });
            // Mn  [11] REJANG VOWEL SIGN I..REJANG CONSONANT SIGN R
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A947, End = 0x0A951, Type = Extend });
            // Mc   [2] REJANG CONSONANT SIGN H..REJANG VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A952, End = 0x0A953, Type = SpacingMark });
            // Lo  [29] HANGUL CHOSEONG TIKEUT-MIEUM..HANGUL CHOSEONG SSANGYEORINHIEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A960, End = 0x0A97C, Type = L });
            // Mn   [3] JAVANESE SIGN PANYANGGA..JAVANESE SIGN LAYAR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A980, End = 0x0A982, Type = Extend });
            // Mc   [2] JAVANESE VOWEL SIGN TARUNG..JAVANESE VOWEL SIGN TOLONG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A9B4, End = 0x0A9B5, Type = SpacingMark });
            // Mn   [4] JAVANESE VOWEL SIGN WULU..JAVANESE VOWEL SIGN SUKU MENDUT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A9B6, End = 0x0A9B9, Type = Extend });
            // Mc   [2] JAVANESE VOWEL SIGN TALING..JAVANESE VOWEL SIGN DIRGA MURE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A9BA, End = 0x0A9BB, Type = SpacingMark });
            // Mn   [2] JAVANESE VOWEL SIGN PEPET..JAVANESE CONSONANT SIGN KERET
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A9BC, End = 0x0A9BD, Type = Extend });
            // Mc   [3] JAVANESE CONSONANT SIGN PENGKAL..JAVANESE PANGKON
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0A9BE, End = 0x0A9C0, Type = SpacingMark });
            // Mn   [6] CHAM VOWEL SIGN AA..CHAM VOWEL SIGN OE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AA29, End = 0x0AA2E, Type = Extend });
            // Mc   [2] CHAM VOWEL SIGN O..CHAM VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AA2F, End = 0x0AA30, Type = SpacingMark });
            // Mn   [2] CHAM VOWEL SIGN AU..CHAM VOWEL SIGN UE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AA31, End = 0x0AA32, Type = Extend });
            // Mc   [2] CHAM CONSONANT SIGN YA..CHAM CONSONANT SIGN RA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AA33, End = 0x0AA34, Type = SpacingMark });
            // Mn   [2] CHAM CONSONANT SIGN LA..CHAM CONSONANT SIGN WA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AA35, End = 0x0AA36, Type = Extend });
            // Mn   [3] TAI VIET VOWEL I..TAI VIET VOWEL U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AAB2, End = 0x0AAB4, Type = Extend });
            // Mn   [2] TAI VIET MAI KHIT..TAI VIET VOWEL IA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AAB7, End = 0x0AAB8, Type = Extend });
            // Mn   [2] TAI VIET VOWEL AM..TAI VIET TONE MAI EK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AABE, End = 0x0AABF, Type = Extend });
            // Mn   [2] MEETEI MAYEK VOWEL SIGN UU..MEETEI MAYEK VOWEL SIGN AAI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AAEC, End = 0x0AAED, Type = Extend });
            // Mc   [2] MEETEI MAYEK VOWEL SIGN AU..MEETEI MAYEK VOWEL SIGN AAU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AAEE, End = 0x0AAEF, Type = SpacingMark });
            // Mc   [2] MEETEI MAYEK VOWEL SIGN ONAP..MEETEI MAYEK VOWEL SIGN INAP
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ABE3, End = 0x0ABE4, Type = SpacingMark });
            // Mc   [2] MEETEI MAYEK VOWEL SIGN YENAP..MEETEI MAYEK VOWEL SIGN SOUNAP
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ABE6, End = 0x0ABE7, Type = SpacingMark });
            // Mc   [2] MEETEI MAYEK VOWEL SIGN CHEINAP..MEETEI MAYEK VOWEL SIGN NUNG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ABE9, End = 0x0ABEA, Type = SpacingMark });
            // Lo  [27] HANGUL SYLLABLE GAG..HANGUL SYLLABLE GAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AC01, End = 0x0AC1B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GAEG..HANGUL SYLLABLE GAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AC1D, End = 0x0AC37, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GYAG..HANGUL SYLLABLE GYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AC39, End = 0x0AC53, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GYAEG..HANGUL SYLLABLE GYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AC55, End = 0x0AC6F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GEOG..HANGUL SYLLABLE GEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AC71, End = 0x0AC8B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GEG..HANGUL SYLLABLE GEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AC8D, End = 0x0ACA7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GYEOG..HANGUL SYLLABLE GYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ACA9, End = 0x0ACC3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GYEG..HANGUL SYLLABLE GYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ACC5, End = 0x0ACDF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GOG..HANGUL SYLLABLE GOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ACE1, End = 0x0ACFB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GWAG..HANGUL SYLLABLE GWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ACFD, End = 0x0AD17, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GWAEG..HANGUL SYLLABLE GWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AD19, End = 0x0AD33, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GOEG..HANGUL SYLLABLE GOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AD35, End = 0x0AD4F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GYOG..HANGUL SYLLABLE GYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AD51, End = 0x0AD6B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GUG..HANGUL SYLLABLE GUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AD6D, End = 0x0AD87, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GWEOG..HANGUL SYLLABLE GWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AD89, End = 0x0ADA3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GWEG..HANGUL SYLLABLE GWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ADA5, End = 0x0ADBF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GWIG..HANGUL SYLLABLE GWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ADC1, End = 0x0ADDB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GYUG..HANGUL SYLLABLE GYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ADDD, End = 0x0ADF7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GEUG..HANGUL SYLLABLE GEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0ADF9, End = 0x0AE13, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GYIG..HANGUL SYLLABLE GYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AE15, End = 0x0AE2F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GIG..HANGUL SYLLABLE GIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AE31, End = 0x0AE4B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGAG..HANGUL SYLLABLE GGAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AE4D, End = 0x0AE67, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGAEG..HANGUL SYLLABLE GGAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AE69, End = 0x0AE83, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGYAG..HANGUL SYLLABLE GGYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AE85, End = 0x0AE9F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGYAEG..HANGUL SYLLABLE GGYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AEA1, End = 0x0AEBB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGEOG..HANGUL SYLLABLE GGEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AEBD, End = 0x0AED7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGEG..HANGUL SYLLABLE GGEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AED9, End = 0x0AEF3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGYEOG..HANGUL SYLLABLE GGYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AEF5, End = 0x0AF0F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGYEG..HANGUL SYLLABLE GGYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AF11, End = 0x0AF2B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGOG..HANGUL SYLLABLE GGOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AF2D, End = 0x0AF47, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGWAG..HANGUL SYLLABLE GGWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AF49, End = 0x0AF63, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGWAEG..HANGUL SYLLABLE GGWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AF65, End = 0x0AF7F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGOEG..HANGUL SYLLABLE GGOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AF81, End = 0x0AF9B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGYOG..HANGUL SYLLABLE GGYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AF9D, End = 0x0AFB7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGUG..HANGUL SYLLABLE GGUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AFB9, End = 0x0AFD3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGWEOG..HANGUL SYLLABLE GGWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AFD5, End = 0x0AFEF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGWEG..HANGUL SYLLABLE GGWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0AFF1, End = 0x0B00B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGWIG..HANGUL SYLLABLE GGWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B00D, End = 0x0B027, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGYUG..HANGUL SYLLABLE GGYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B029, End = 0x0B043, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGEUG..HANGUL SYLLABLE GGEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B045, End = 0x0B05F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGYIG..HANGUL SYLLABLE GGYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B061, End = 0x0B07B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE GGIG..HANGUL SYLLABLE GGIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B07D, End = 0x0B097, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NAG..HANGUL SYLLABLE NAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B099, End = 0x0B0B3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NAEG..HANGUL SYLLABLE NAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B0B5, End = 0x0B0CF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NYAG..HANGUL SYLLABLE NYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B0D1, End = 0x0B0EB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NYAEG..HANGUL SYLLABLE NYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B0ED, End = 0x0B107, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NEOG..HANGUL SYLLABLE NEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B109, End = 0x0B123, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NEG..HANGUL SYLLABLE NEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B125, End = 0x0B13F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NYEOG..HANGUL SYLLABLE NYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B141, End = 0x0B15B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NYEG..HANGUL SYLLABLE NYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B15D, End = 0x0B177, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NOG..HANGUL SYLLABLE NOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B179, End = 0x0B193, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NWAG..HANGUL SYLLABLE NWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B195, End = 0x0B1AF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NWAEG..HANGUL SYLLABLE NWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B1B1, End = 0x0B1CB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NOEG..HANGUL SYLLABLE NOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B1CD, End = 0x0B1E7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NYOG..HANGUL SYLLABLE NYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B1E9, End = 0x0B203, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NUG..HANGUL SYLLABLE NUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B205, End = 0x0B21F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NWEOG..HANGUL SYLLABLE NWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B221, End = 0x0B23B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NWEG..HANGUL SYLLABLE NWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B23D, End = 0x0B257, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NWIG..HANGUL SYLLABLE NWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B259, End = 0x0B273, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NYUG..HANGUL SYLLABLE NYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B275, End = 0x0B28F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NEUG..HANGUL SYLLABLE NEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B291, End = 0x0B2AB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NYIG..HANGUL SYLLABLE NYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B2AD, End = 0x0B2C7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE NIG..HANGUL SYLLABLE NIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B2C9, End = 0x0B2E3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DAG..HANGUL SYLLABLE DAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B2E5, End = 0x0B2FF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DAEG..HANGUL SYLLABLE DAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B301, End = 0x0B31B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DYAG..HANGUL SYLLABLE DYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B31D, End = 0x0B337, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DYAEG..HANGUL SYLLABLE DYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B339, End = 0x0B353, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DEOG..HANGUL SYLLABLE DEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B355, End = 0x0B36F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DEG..HANGUL SYLLABLE DEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B371, End = 0x0B38B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DYEOG..HANGUL SYLLABLE DYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B38D, End = 0x0B3A7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DYEG..HANGUL SYLLABLE DYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B3A9, End = 0x0B3C3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DOG..HANGUL SYLLABLE DOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B3C5, End = 0x0B3DF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DWAG..HANGUL SYLLABLE DWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B3E1, End = 0x0B3FB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DWAEG..HANGUL SYLLABLE DWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B3FD, End = 0x0B417, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DOEG..HANGUL SYLLABLE DOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B419, End = 0x0B433, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DYOG..HANGUL SYLLABLE DYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B435, End = 0x0B44F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DUG..HANGUL SYLLABLE DUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B451, End = 0x0B46B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DWEOG..HANGUL SYLLABLE DWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B46D, End = 0x0B487, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DWEG..HANGUL SYLLABLE DWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B489, End = 0x0B4A3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DWIG..HANGUL SYLLABLE DWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B4A5, End = 0x0B4BF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DYUG..HANGUL SYLLABLE DYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B4C1, End = 0x0B4DB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DEUG..HANGUL SYLLABLE DEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B4DD, End = 0x0B4F7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DYIG..HANGUL SYLLABLE DYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B4F9, End = 0x0B513, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DIG..HANGUL SYLLABLE DIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B515, End = 0x0B52F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDAG..HANGUL SYLLABLE DDAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B531, End = 0x0B54B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDAEG..HANGUL SYLLABLE DDAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B54D, End = 0x0B567, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDYAG..HANGUL SYLLABLE DDYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B569, End = 0x0B583, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDYAEG..HANGUL SYLLABLE DDYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B585, End = 0x0B59F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDEOG..HANGUL SYLLABLE DDEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B5A1, End = 0x0B5BB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDEG..HANGUL SYLLABLE DDEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B5BD, End = 0x0B5D7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDYEOG..HANGUL SYLLABLE DDYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B5D9, End = 0x0B5F3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDYEG..HANGUL SYLLABLE DDYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B5F5, End = 0x0B60F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDOG..HANGUL SYLLABLE DDOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B611, End = 0x0B62B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDWAG..HANGUL SYLLABLE DDWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B62D, End = 0x0B647, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDWAEG..HANGUL SYLLABLE DDWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B649, End = 0x0B663, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDOEG..HANGUL SYLLABLE DDOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B665, End = 0x0B67F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDYOG..HANGUL SYLLABLE DDYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B681, End = 0x0B69B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDUG..HANGUL SYLLABLE DDUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B69D, End = 0x0B6B7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDWEOG..HANGUL SYLLABLE DDWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B6B9, End = 0x0B6D3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDWEG..HANGUL SYLLABLE DDWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B6D5, End = 0x0B6EF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDWIG..HANGUL SYLLABLE DDWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B6F1, End = 0x0B70B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDYUG..HANGUL SYLLABLE DDYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B70D, End = 0x0B727, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDEUG..HANGUL SYLLABLE DDEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B729, End = 0x0B743, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDYIG..HANGUL SYLLABLE DDYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B745, End = 0x0B75F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE DDIG..HANGUL SYLLABLE DDIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B761, End = 0x0B77B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RAG..HANGUL SYLLABLE RAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B77D, End = 0x0B797, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RAEG..HANGUL SYLLABLE RAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B799, End = 0x0B7B3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RYAG..HANGUL SYLLABLE RYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B7B5, End = 0x0B7CF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RYAEG..HANGUL SYLLABLE RYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B7D1, End = 0x0B7EB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE REOG..HANGUL SYLLABLE REOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B7ED, End = 0x0B807, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE REG..HANGUL SYLLABLE REH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B809, End = 0x0B823, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RYEOG..HANGUL SYLLABLE RYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B825, End = 0x0B83F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RYEG..HANGUL SYLLABLE RYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B841, End = 0x0B85B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE ROG..HANGUL SYLLABLE ROH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B85D, End = 0x0B877, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RWAG..HANGUL SYLLABLE RWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B879, End = 0x0B893, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RWAEG..HANGUL SYLLABLE RWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B895, End = 0x0B8AF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE ROEG..HANGUL SYLLABLE ROEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B8B1, End = 0x0B8CB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RYOG..HANGUL SYLLABLE RYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B8CD, End = 0x0B8E7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RUG..HANGUL SYLLABLE RUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B8E9, End = 0x0B903, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RWEOG..HANGUL SYLLABLE RWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B905, End = 0x0B91F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RWEG..HANGUL SYLLABLE RWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B921, End = 0x0B93B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RWIG..HANGUL SYLLABLE RWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B93D, End = 0x0B957, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RYUG..HANGUL SYLLABLE RYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B959, End = 0x0B973, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE REUG..HANGUL SYLLABLE REUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B975, End = 0x0B98F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RYIG..HANGUL SYLLABLE RYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B991, End = 0x0B9AB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE RIG..HANGUL SYLLABLE RIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B9AD, End = 0x0B9C7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MAG..HANGUL SYLLABLE MAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B9C9, End = 0x0B9E3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MAEG..HANGUL SYLLABLE MAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0B9E5, End = 0x0B9FF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MYAG..HANGUL SYLLABLE MYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BA01, End = 0x0BA1B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MYAEG..HANGUL SYLLABLE MYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BA1D, End = 0x0BA37, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MEOG..HANGUL SYLLABLE MEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BA39, End = 0x0BA53, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MEG..HANGUL SYLLABLE MEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BA55, End = 0x0BA6F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MYEOG..HANGUL SYLLABLE MYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BA71, End = 0x0BA8B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MYEG..HANGUL SYLLABLE MYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BA8D, End = 0x0BAA7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MOG..HANGUL SYLLABLE MOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BAA9, End = 0x0BAC3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MWAG..HANGUL SYLLABLE MWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BAC5, End = 0x0BADF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MWAEG..HANGUL SYLLABLE MWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BAE1, End = 0x0BAFB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MOEG..HANGUL SYLLABLE MOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BAFD, End = 0x0BB17, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MYOG..HANGUL SYLLABLE MYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BB19, End = 0x0BB33, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MUG..HANGUL SYLLABLE MUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BB35, End = 0x0BB4F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MWEOG..HANGUL SYLLABLE MWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BB51, End = 0x0BB6B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MWEG..HANGUL SYLLABLE MWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BB6D, End = 0x0BB87, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MWIG..HANGUL SYLLABLE MWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BB89, End = 0x0BBA3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MYUG..HANGUL SYLLABLE MYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BBA5, End = 0x0BBBF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MEUG..HANGUL SYLLABLE MEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BBC1, End = 0x0BBDB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MYIG..HANGUL SYLLABLE MYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BBDD, End = 0x0BBF7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE MIG..HANGUL SYLLABLE MIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BBF9, End = 0x0BC13, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BAG..HANGUL SYLLABLE BAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BC15, End = 0x0BC2F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BAEG..HANGUL SYLLABLE BAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BC31, End = 0x0BC4B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BYAG..HANGUL SYLLABLE BYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BC4D, End = 0x0BC67, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BYAEG..HANGUL SYLLABLE BYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BC69, End = 0x0BC83, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BEOG..HANGUL SYLLABLE BEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BC85, End = 0x0BC9F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BEG..HANGUL SYLLABLE BEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BCA1, End = 0x0BCBB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BYEOG..HANGUL SYLLABLE BYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BCBD, End = 0x0BCD7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BYEG..HANGUL SYLLABLE BYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BCD9, End = 0x0BCF3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BOG..HANGUL SYLLABLE BOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BCF5, End = 0x0BD0F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BWAG..HANGUL SYLLABLE BWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BD11, End = 0x0BD2B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BWAEG..HANGUL SYLLABLE BWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BD2D, End = 0x0BD47, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BOEG..HANGUL SYLLABLE BOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BD49, End = 0x0BD63, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BYOG..HANGUL SYLLABLE BYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BD65, End = 0x0BD7F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BUG..HANGUL SYLLABLE BUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BD81, End = 0x0BD9B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BWEOG..HANGUL SYLLABLE BWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BD9D, End = 0x0BDB7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BWEG..HANGUL SYLLABLE BWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BDB9, End = 0x0BDD3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BWIG..HANGUL SYLLABLE BWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BDD5, End = 0x0BDEF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BYUG..HANGUL SYLLABLE BYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BDF1, End = 0x0BE0B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BEUG..HANGUL SYLLABLE BEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BE0D, End = 0x0BE27, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BYIG..HANGUL SYLLABLE BYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BE29, End = 0x0BE43, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BIG..HANGUL SYLLABLE BIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BE45, End = 0x0BE5F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBAG..HANGUL SYLLABLE BBAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BE61, End = 0x0BE7B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBAEG..HANGUL SYLLABLE BBAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BE7D, End = 0x0BE97, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBYAG..HANGUL SYLLABLE BBYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BE99, End = 0x0BEB3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBYAEG..HANGUL SYLLABLE BBYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BEB5, End = 0x0BECF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBEOG..HANGUL SYLLABLE BBEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BED1, End = 0x0BEEB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBEG..HANGUL SYLLABLE BBEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BEED, End = 0x0BF07, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBYEOG..HANGUL SYLLABLE BBYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BF09, End = 0x0BF23, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBYEG..HANGUL SYLLABLE BBYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BF25, End = 0x0BF3F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBOG..HANGUL SYLLABLE BBOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BF41, End = 0x0BF5B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBWAG..HANGUL SYLLABLE BBWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BF5D, End = 0x0BF77, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBWAEG..HANGUL SYLLABLE BBWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BF79, End = 0x0BF93, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBOEG..HANGUL SYLLABLE BBOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BF95, End = 0x0BFAF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBYOG..HANGUL SYLLABLE BBYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BFB1, End = 0x0BFCB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBUG..HANGUL SYLLABLE BBUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BFCD, End = 0x0BFE7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBWEOG..HANGUL SYLLABLE BBWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0BFE9, End = 0x0C003, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBWEG..HANGUL SYLLABLE BBWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C005, End = 0x0C01F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBWIG..HANGUL SYLLABLE BBWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C021, End = 0x0C03B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBYUG..HANGUL SYLLABLE BBYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C03D, End = 0x0C057, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBEUG..HANGUL SYLLABLE BBEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C059, End = 0x0C073, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBYIG..HANGUL SYLLABLE BBYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C075, End = 0x0C08F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE BBIG..HANGUL SYLLABLE BBIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C091, End = 0x0C0AB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SAG..HANGUL SYLLABLE SAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C0AD, End = 0x0C0C7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SAEG..HANGUL SYLLABLE SAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C0C9, End = 0x0C0E3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SYAG..HANGUL SYLLABLE SYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C0E5, End = 0x0C0FF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SYAEG..HANGUL SYLLABLE SYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C101, End = 0x0C11B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SEOG..HANGUL SYLLABLE SEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C11D, End = 0x0C137, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SEG..HANGUL SYLLABLE SEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C139, End = 0x0C153, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SYEOG..HANGUL SYLLABLE SYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C155, End = 0x0C16F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SYEG..HANGUL SYLLABLE SYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C171, End = 0x0C18B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SOG..HANGUL SYLLABLE SOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C18D, End = 0x0C1A7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SWAG..HANGUL SYLLABLE SWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C1A9, End = 0x0C1C3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SWAEG..HANGUL SYLLABLE SWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C1C5, End = 0x0C1DF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SOEG..HANGUL SYLLABLE SOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C1E1, End = 0x0C1FB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SYOG..HANGUL SYLLABLE SYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C1FD, End = 0x0C217, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SUG..HANGUL SYLLABLE SUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C219, End = 0x0C233, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SWEOG..HANGUL SYLLABLE SWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C235, End = 0x0C24F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SWEG..HANGUL SYLLABLE SWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C251, End = 0x0C26B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SWIG..HANGUL SYLLABLE SWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C26D, End = 0x0C287, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SYUG..HANGUL SYLLABLE SYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C289, End = 0x0C2A3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SEUG..HANGUL SYLLABLE SEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C2A5, End = 0x0C2BF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SYIG..HANGUL SYLLABLE SYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C2C1, End = 0x0C2DB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SIG..HANGUL SYLLABLE SIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C2DD, End = 0x0C2F7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSAG..HANGUL SYLLABLE SSAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C2F9, End = 0x0C313, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSAEG..HANGUL SYLLABLE SSAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C315, End = 0x0C32F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSYAG..HANGUL SYLLABLE SSYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C331, End = 0x0C34B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSYAEG..HANGUL SYLLABLE SSYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C34D, End = 0x0C367, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSEOG..HANGUL SYLLABLE SSEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C369, End = 0x0C383, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSEG..HANGUL SYLLABLE SSEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C385, End = 0x0C39F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSYEOG..HANGUL SYLLABLE SSYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C3A1, End = 0x0C3BB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSYEG..HANGUL SYLLABLE SSYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C3BD, End = 0x0C3D7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSOG..HANGUL SYLLABLE SSOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C3D9, End = 0x0C3F3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSWAG..HANGUL SYLLABLE SSWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C3F5, End = 0x0C40F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSWAEG..HANGUL SYLLABLE SSWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C411, End = 0x0C42B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSOEG..HANGUL SYLLABLE SSOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C42D, End = 0x0C447, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSYOG..HANGUL SYLLABLE SSYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C449, End = 0x0C463, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSUG..HANGUL SYLLABLE SSUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C465, End = 0x0C47F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSWEOG..HANGUL SYLLABLE SSWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C481, End = 0x0C49B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSWEG..HANGUL SYLLABLE SSWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C49D, End = 0x0C4B7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSWIG..HANGUL SYLLABLE SSWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C4B9, End = 0x0C4D3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSYUG..HANGUL SYLLABLE SSYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C4D5, End = 0x0C4EF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSEUG..HANGUL SYLLABLE SSEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C4F1, End = 0x0C50B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSYIG..HANGUL SYLLABLE SSYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C50D, End = 0x0C527, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE SSIG..HANGUL SYLLABLE SSIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C529, End = 0x0C543, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE AG..HANGUL SYLLABLE AH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C545, End = 0x0C55F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE AEG..HANGUL SYLLABLE AEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C561, End = 0x0C57B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE YAG..HANGUL SYLLABLE YAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C57D, End = 0x0C597, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE YAEG..HANGUL SYLLABLE YAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C599, End = 0x0C5B3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE EOG..HANGUL SYLLABLE EOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C5B5, End = 0x0C5CF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE EG..HANGUL SYLLABLE EH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C5D1, End = 0x0C5EB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE YEOG..HANGUL SYLLABLE YEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C5ED, End = 0x0C607, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE YEG..HANGUL SYLLABLE YEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C609, End = 0x0C623, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE OG..HANGUL SYLLABLE OH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C625, End = 0x0C63F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE WAG..HANGUL SYLLABLE WAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C641, End = 0x0C65B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE WAEG..HANGUL SYLLABLE WAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C65D, End = 0x0C677, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE OEG..HANGUL SYLLABLE OEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C679, End = 0x0C693, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE YOG..HANGUL SYLLABLE YOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C695, End = 0x0C6AF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE UG..HANGUL SYLLABLE UH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C6B1, End = 0x0C6CB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE WEOG..HANGUL SYLLABLE WEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C6CD, End = 0x0C6E7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE WEG..HANGUL SYLLABLE WEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C6E9, End = 0x0C703, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE WIG..HANGUL SYLLABLE WIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C705, End = 0x0C71F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE YUG..HANGUL SYLLABLE YUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C721, End = 0x0C73B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE EUG..HANGUL SYLLABLE EUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C73D, End = 0x0C757, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE YIG..HANGUL SYLLABLE YIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C759, End = 0x0C773, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE IG..HANGUL SYLLABLE IH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C775, End = 0x0C78F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JAG..HANGUL SYLLABLE JAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C791, End = 0x0C7AB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JAEG..HANGUL SYLLABLE JAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C7AD, End = 0x0C7C7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JYAG..HANGUL SYLLABLE JYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C7C9, End = 0x0C7E3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JYAEG..HANGUL SYLLABLE JYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C7E5, End = 0x0C7FF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JEOG..HANGUL SYLLABLE JEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C801, End = 0x0C81B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JEG..HANGUL SYLLABLE JEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C81D, End = 0x0C837, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JYEOG..HANGUL SYLLABLE JYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C839, End = 0x0C853, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JYEG..HANGUL SYLLABLE JYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C855, End = 0x0C86F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JOG..HANGUL SYLLABLE JOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C871, End = 0x0C88B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JWAG..HANGUL SYLLABLE JWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C88D, End = 0x0C8A7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JWAEG..HANGUL SYLLABLE JWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C8A9, End = 0x0C8C3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JOEG..HANGUL SYLLABLE JOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C8C5, End = 0x0C8DF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JYOG..HANGUL SYLLABLE JYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C8E1, End = 0x0C8FB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JUG..HANGUL SYLLABLE JUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C8FD, End = 0x0C917, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JWEOG..HANGUL SYLLABLE JWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C919, End = 0x0C933, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JWEG..HANGUL SYLLABLE JWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C935, End = 0x0C94F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JWIG..HANGUL SYLLABLE JWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C951, End = 0x0C96B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JYUG..HANGUL SYLLABLE JYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C96D, End = 0x0C987, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JEUG..HANGUL SYLLABLE JEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C989, End = 0x0C9A3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JYIG..HANGUL SYLLABLE JYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C9A5, End = 0x0C9BF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JIG..HANGUL SYLLABLE JIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C9C1, End = 0x0C9DB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJAG..HANGUL SYLLABLE JJAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C9DD, End = 0x0C9F7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJAEG..HANGUL SYLLABLE JJAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0C9F9, End = 0x0CA13, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJYAG..HANGUL SYLLABLE JJYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CA15, End = 0x0CA2F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJYAEG..HANGUL SYLLABLE JJYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CA31, End = 0x0CA4B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJEOG..HANGUL SYLLABLE JJEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CA4D, End = 0x0CA67, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJEG..HANGUL SYLLABLE JJEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CA69, End = 0x0CA83, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJYEOG..HANGUL SYLLABLE JJYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CA85, End = 0x0CA9F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJYEG..HANGUL SYLLABLE JJYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CAA1, End = 0x0CABB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJOG..HANGUL SYLLABLE JJOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CABD, End = 0x0CAD7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJWAG..HANGUL SYLLABLE JJWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CAD9, End = 0x0CAF3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJWAEG..HANGUL SYLLABLE JJWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CAF5, End = 0x0CB0F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJOEG..HANGUL SYLLABLE JJOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CB11, End = 0x0CB2B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJYOG..HANGUL SYLLABLE JJYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CB2D, End = 0x0CB47, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJUG..HANGUL SYLLABLE JJUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CB49, End = 0x0CB63, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJWEOG..HANGUL SYLLABLE JJWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CB65, End = 0x0CB7F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJWEG..HANGUL SYLLABLE JJWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CB81, End = 0x0CB9B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJWIG..HANGUL SYLLABLE JJWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CB9D, End = 0x0CBB7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJYUG..HANGUL SYLLABLE JJYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CBB9, End = 0x0CBD3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJEUG..HANGUL SYLLABLE JJEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CBD5, End = 0x0CBEF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJYIG..HANGUL SYLLABLE JJYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CBF1, End = 0x0CC0B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE JJIG..HANGUL SYLLABLE JJIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CC0D, End = 0x0CC27, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CAG..HANGUL SYLLABLE CAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CC29, End = 0x0CC43, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CAEG..HANGUL SYLLABLE CAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CC45, End = 0x0CC5F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CYAG..HANGUL SYLLABLE CYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CC61, End = 0x0CC7B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CYAEG..HANGUL SYLLABLE CYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CC7D, End = 0x0CC97, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CEOG..HANGUL SYLLABLE CEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CC99, End = 0x0CCB3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CEG..HANGUL SYLLABLE CEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CCB5, End = 0x0CCCF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CYEOG..HANGUL SYLLABLE CYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CCD1, End = 0x0CCEB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CYEG..HANGUL SYLLABLE CYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CCED, End = 0x0CD07, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE COG..HANGUL SYLLABLE COH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CD09, End = 0x0CD23, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CWAG..HANGUL SYLLABLE CWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CD25, End = 0x0CD3F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CWAEG..HANGUL SYLLABLE CWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CD41, End = 0x0CD5B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE COEG..HANGUL SYLLABLE COEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CD5D, End = 0x0CD77, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CYOG..HANGUL SYLLABLE CYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CD79, End = 0x0CD93, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CUG..HANGUL SYLLABLE CUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CD95, End = 0x0CDAF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CWEOG..HANGUL SYLLABLE CWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CDB1, End = 0x0CDCB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CWEG..HANGUL SYLLABLE CWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CDCD, End = 0x0CDE7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CWIG..HANGUL SYLLABLE CWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CDE9, End = 0x0CE03, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CYUG..HANGUL SYLLABLE CYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CE05, End = 0x0CE1F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CEUG..HANGUL SYLLABLE CEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CE21, End = 0x0CE3B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CYIG..HANGUL SYLLABLE CYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CE3D, End = 0x0CE57, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE CIG..HANGUL SYLLABLE CIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CE59, End = 0x0CE73, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KAG..HANGUL SYLLABLE KAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CE75, End = 0x0CE8F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KAEG..HANGUL SYLLABLE KAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CE91, End = 0x0CEAB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KYAG..HANGUL SYLLABLE KYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CEAD, End = 0x0CEC7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KYAEG..HANGUL SYLLABLE KYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CEC9, End = 0x0CEE3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KEOG..HANGUL SYLLABLE KEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CEE5, End = 0x0CEFF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KEG..HANGUL SYLLABLE KEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CF01, End = 0x0CF1B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KYEOG..HANGUL SYLLABLE KYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CF1D, End = 0x0CF37, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KYEG..HANGUL SYLLABLE KYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CF39, End = 0x0CF53, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KOG..HANGUL SYLLABLE KOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CF55, End = 0x0CF6F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KWAG..HANGUL SYLLABLE KWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CF71, End = 0x0CF8B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KWAEG..HANGUL SYLLABLE KWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CF8D, End = 0x0CFA7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KOEG..HANGUL SYLLABLE KOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CFA9, End = 0x0CFC3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KYOG..HANGUL SYLLABLE KYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CFC5, End = 0x0CFDF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KUG..HANGUL SYLLABLE KUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CFE1, End = 0x0CFFB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KWEOG..HANGUL SYLLABLE KWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0CFFD, End = 0x0D017, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KWEG..HANGUL SYLLABLE KWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D019, End = 0x0D033, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KWIG..HANGUL SYLLABLE KWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D035, End = 0x0D04F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KYUG..HANGUL SYLLABLE KYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D051, End = 0x0D06B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KEUG..HANGUL SYLLABLE KEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D06D, End = 0x0D087, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KYIG..HANGUL SYLLABLE KYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D089, End = 0x0D0A3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE KIG..HANGUL SYLLABLE KIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D0A5, End = 0x0D0BF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TAG..HANGUL SYLLABLE TAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D0C1, End = 0x0D0DB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TAEG..HANGUL SYLLABLE TAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D0DD, End = 0x0D0F7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TYAG..HANGUL SYLLABLE TYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D0F9, End = 0x0D113, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TYAEG..HANGUL SYLLABLE TYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D115, End = 0x0D12F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TEOG..HANGUL SYLLABLE TEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D131, End = 0x0D14B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TEG..HANGUL SYLLABLE TEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D14D, End = 0x0D167, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TYEOG..HANGUL SYLLABLE TYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D169, End = 0x0D183, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TYEG..HANGUL SYLLABLE TYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D185, End = 0x0D19F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TOG..HANGUL SYLLABLE TOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D1A1, End = 0x0D1BB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TWAG..HANGUL SYLLABLE TWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D1BD, End = 0x0D1D7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TWAEG..HANGUL SYLLABLE TWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D1D9, End = 0x0D1F3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TOEG..HANGUL SYLLABLE TOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D1F5, End = 0x0D20F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TYOG..HANGUL SYLLABLE TYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D211, End = 0x0D22B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TUG..HANGUL SYLLABLE TUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D22D, End = 0x0D247, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TWEOG..HANGUL SYLLABLE TWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D249, End = 0x0D263, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TWEG..HANGUL SYLLABLE TWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D265, End = 0x0D27F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TWIG..HANGUL SYLLABLE TWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D281, End = 0x0D29B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TYUG..HANGUL SYLLABLE TYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D29D, End = 0x0D2B7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TEUG..HANGUL SYLLABLE TEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D2B9, End = 0x0D2D3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TYIG..HANGUL SYLLABLE TYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D2D5, End = 0x0D2EF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE TIG..HANGUL SYLLABLE TIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D2F1, End = 0x0D30B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PAG..HANGUL SYLLABLE PAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D30D, End = 0x0D327, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PAEG..HANGUL SYLLABLE PAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D329, End = 0x0D343, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PYAG..HANGUL SYLLABLE PYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D345, End = 0x0D35F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PYAEG..HANGUL SYLLABLE PYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D361, End = 0x0D37B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PEOG..HANGUL SYLLABLE PEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D37D, End = 0x0D397, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PEG..HANGUL SYLLABLE PEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D399, End = 0x0D3B3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PYEOG..HANGUL SYLLABLE PYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D3B5, End = 0x0D3CF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PYEG..HANGUL SYLLABLE PYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D3D1, End = 0x0D3EB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE POG..HANGUL SYLLABLE POH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D3ED, End = 0x0D407, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PWAG..HANGUL SYLLABLE PWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D409, End = 0x0D423, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PWAEG..HANGUL SYLLABLE PWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D425, End = 0x0D43F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE POEG..HANGUL SYLLABLE POEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D441, End = 0x0D45B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PYOG..HANGUL SYLLABLE PYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D45D, End = 0x0D477, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PUG..HANGUL SYLLABLE PUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D479, End = 0x0D493, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PWEOG..HANGUL SYLLABLE PWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D495, End = 0x0D4AF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PWEG..HANGUL SYLLABLE PWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D4B1, End = 0x0D4CB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PWIG..HANGUL SYLLABLE PWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D4CD, End = 0x0D4E7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PYUG..HANGUL SYLLABLE PYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D4E9, End = 0x0D503, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PEUG..HANGUL SYLLABLE PEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D505, End = 0x0D51F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PYIG..HANGUL SYLLABLE PYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D521, End = 0x0D53B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE PIG..HANGUL SYLLABLE PIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D53D, End = 0x0D557, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HAG..HANGUL SYLLABLE HAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D559, End = 0x0D573, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HAEG..HANGUL SYLLABLE HAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D575, End = 0x0D58F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HYAG..HANGUL SYLLABLE HYAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D591, End = 0x0D5AB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HYAEG..HANGUL SYLLABLE HYAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D5AD, End = 0x0D5C7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HEOG..HANGUL SYLLABLE HEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D5C9, End = 0x0D5E3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HEG..HANGUL SYLLABLE HEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D5E5, End = 0x0D5FF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HYEOG..HANGUL SYLLABLE HYEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D601, End = 0x0D61B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HYEG..HANGUL SYLLABLE HYEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D61D, End = 0x0D637, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HOG..HANGUL SYLLABLE HOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D639, End = 0x0D653, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HWAG..HANGUL SYLLABLE HWAH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D655, End = 0x0D66F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HWAEG..HANGUL SYLLABLE HWAEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D671, End = 0x0D68B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HOEG..HANGUL SYLLABLE HOEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D68D, End = 0x0D6A7, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HYOG..HANGUL SYLLABLE HYOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D6A9, End = 0x0D6C3, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HUG..HANGUL SYLLABLE HUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D6C5, End = 0x0D6DF, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HWEOG..HANGUL SYLLABLE HWEOH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D6E1, End = 0x0D6FB, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HWEG..HANGUL SYLLABLE HWEH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D6FD, End = 0x0D717, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HWIG..HANGUL SYLLABLE HWIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D719, End = 0x0D733, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HYUG..HANGUL SYLLABLE HYUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D735, End = 0x0D74F, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HEUG..HANGUL SYLLABLE HEUH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D751, End = 0x0D76B, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HYIG..HANGUL SYLLABLE HYIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D76D, End = 0x0D787, Type = LVT });
            // Lo  [27] HANGUL SYLLABLE HIG..HANGUL SYLLABLE HIH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D789, End = 0x0D7A3, Type = LVT });
            // Lo  [23] HANGUL JUNGSEONG O-YEO..HANGUL JUNGSEONG ARAEA-E
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D7B0, End = 0x0D7C6, Type = V });
            // Lo  [49] HANGUL JONGSEONG NIEUN-RIEUL..HANGUL JONGSEONG PHIEUPH-THIEUTH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0D7CB, End = 0x0D7FB, Type = T });
            // Mn  [16] VARIATION SELECTOR-1..VARIATION SELECTOR-16
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0FE00, End = 0x0FE0F, Type = Extend });
            // Mn  [16] COMBINING LIGATURE LEFT HALF..COMBINING CYRILLIC TITLO RIGHT HALF
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0FE20, End = 0x0FE2F, Type = Extend });
            // Lm   [2] HALFWIDTH KATAKANA VOICED SOUND MARK..HALFWIDTH KATAKANA SEMI-VOICED SOUND MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0FF9E, End = 0x0FF9F, Type = Extend });
            // Cn   [9] <reserved-FFF0>..<reserved-FFF8>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0FFF0, End = 0x0FFF8, Type = Control });
            // Cf   [3] INTERLINEAR ANNOTATION ANCHOR..INTERLINEAR ANNOTATION TERMINATOR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x0FFF9, End = 0x0FFFB, Type = Control });
            // Mn   [5] COMBINING OLD PERMIC LETTER AN..COMBINING OLD PERMIC LETTER SII
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10376, End = 0x1037A, Type = Extend });
            // Mn   [3] KHAROSHTHI VOWEL SIGN I..KHAROSHTHI VOWEL SIGN VOCALIC R
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10A01, End = 0x10A03, Type = Extend });
            // Mn   [2] KHAROSHTHI VOWEL SIGN E..KHAROSHTHI VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10A05, End = 0x10A06, Type = Extend });
            // Mn   [4] KHAROSHTHI VOWEL LENGTH MARK..KHAROSHTHI SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10A0C, End = 0x10A0F, Type = Extend });
            // Mn   [3] KHAROSHTHI SIGN BAR ABOVE..KHAROSHTHI SIGN DOT BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10A38, End = 0x10A3A, Type = Extend });
            // Mn   [2] MANICHAEAN ABBREVIATION MARK ABOVE..MANICHAEAN ABBREVIATION MARK BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10AE5, End = 0x10AE6, Type = Extend });
            // Mn   [4] HANIFI ROHINGYA SIGN HARBAHAY..HANIFI ROHINGYA SIGN TASSI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10D24, End = 0x10D27, Type = Extend });
            // Mn   [2] YEZIDI COMBINING HAMZA MARK..YEZIDI COMBINING MADDA MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10EAB, End = 0x10EAC, Type = Extend });
            // Mn  [11] SOGDIAN COMBINING DOT BELOW..SOGDIAN COMBINING STROKE BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10F46, End = 0x10F50, Type = Extend });
            // Mn   [4] OLD UYGHUR COMBINING DOT ABOVE..OLD UYGHUR COMBINING TWO DOTS BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x10F82, End = 0x10F85, Type = Extend });
            // Mn  [15] BRAHMI VOWEL SIGN AA..BRAHMI VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11038, End = 0x11046, Type = Extend });
            // Mn   [2] BRAHMI VOWEL SIGN OLD TAMIL SHORT E..BRAHMI VOWEL SIGN OLD TAMIL SHORT O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11073, End = 0x11074, Type = Extend });
            // Mn   [3] BRAHMI NUMBER JOINER..KAITHI SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1107F, End = 0x11081, Type = Extend });
            // Mc   [3] KAITHI VOWEL SIGN AA..KAITHI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x110B0, End = 0x110B2, Type = SpacingMark });
            // Mn   [4] KAITHI VOWEL SIGN U..KAITHI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x110B3, End = 0x110B6, Type = Extend });
            // Mc   [2] KAITHI VOWEL SIGN O..KAITHI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x110B7, End = 0x110B8, Type = SpacingMark });
            // Mn   [2] KAITHI SIGN VIRAMA..KAITHI SIGN NUKTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x110B9, End = 0x110BA, Type = Extend });
            // Mn   [3] CHAKMA SIGN CANDRABINDU..CHAKMA SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11100, End = 0x11102, Type = Extend });
            // Mn   [5] CHAKMA VOWEL SIGN A..CHAKMA VOWEL SIGN UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11127, End = 0x1112B, Type = Extend });
            // Mn   [8] CHAKMA VOWEL SIGN AI..CHAKMA MAAYYAA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1112D, End = 0x11134, Type = Extend });
            // Mc   [2] CHAKMA VOWEL SIGN AA..CHAKMA VOWEL SIGN EI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11145, End = 0x11146, Type = SpacingMark });
            // Mn   [2] SHARADA SIGN CANDRABINDU..SHARADA SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11180, End = 0x11181, Type = Extend });
            // Mc   [3] SHARADA VOWEL SIGN AA..SHARADA VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x111B3, End = 0x111B5, Type = SpacingMark });
            // Mn   [9] SHARADA VOWEL SIGN U..SHARADA VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x111B6, End = 0x111BE, Type = Extend });
            // Mc   [2] SHARADA VOWEL SIGN AU..SHARADA SIGN VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x111BF, End = 0x111C0, Type = SpacingMark });
            // Lo   [2] SHARADA SIGN JIHVAMULIYA..SHARADA SIGN UPADHMANIYA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x111C2, End = 0x111C3, Type = Prepend });
            // Mn   [4] SHARADA SANDHI MARK..SHARADA EXTRA SHORT VOWEL MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x111C9, End = 0x111CC, Type = Extend });
            // Mc   [3] KHOJKI VOWEL SIGN AA..KHOJKI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1122C, End = 0x1122E, Type = SpacingMark });
            // Mn   [3] KHOJKI VOWEL SIGN U..KHOJKI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1122F, End = 0x11231, Type = Extend });
            // Mc   [2] KHOJKI VOWEL SIGN O..KHOJKI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11232, End = 0x11233, Type = SpacingMark });
            // Mn   [2] KHOJKI SIGN NUKTA..KHOJKI SIGN SHADDA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11236, End = 0x11237, Type = Extend });
            // Mc   [3] KHUDAWADI VOWEL SIGN AA..KHUDAWADI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x112E0, End = 0x112E2, Type = SpacingMark });
            // Mn   [8] KHUDAWADI VOWEL SIGN U..KHUDAWADI SIGN VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x112E3, End = 0x112EA, Type = Extend });
            // Mn   [2] GRANTHA SIGN COMBINING ANUSVARA ABOVE..GRANTHA SIGN CANDRABINDU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11300, End = 0x11301, Type = Extend });
            // Mc   [2] GRANTHA SIGN ANUSVARA..GRANTHA SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11302, End = 0x11303, Type = SpacingMark });
            // Mn   [2] COMBINING BINDU BELOW..GRANTHA SIGN NUKTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1133B, End = 0x1133C, Type = Extend });
            // Mc   [4] GRANTHA VOWEL SIGN U..GRANTHA VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11341, End = 0x11344, Type = SpacingMark });
            // Mc   [2] GRANTHA VOWEL SIGN EE..GRANTHA VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11347, End = 0x11348, Type = SpacingMark });
            // Mc   [3] GRANTHA VOWEL SIGN OO..GRANTHA SIGN VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1134B, End = 0x1134D, Type = SpacingMark });
            // Mc   [2] GRANTHA VOWEL SIGN VOCALIC L..GRANTHA VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11362, End = 0x11363, Type = SpacingMark });
            // Mn   [7] COMBINING GRANTHA DIGIT ZERO..COMBINING GRANTHA DIGIT SIX
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11366, End = 0x1136C, Type = Extend });
            // Mn   [5] COMBINING GRANTHA LETTER A..COMBINING GRANTHA LETTER PA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11370, End = 0x11374, Type = Extend });
            // Mc   [3] NEWA VOWEL SIGN AA..NEWA VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11435, End = 0x11437, Type = SpacingMark });
            // Mn   [8] NEWA VOWEL SIGN U..NEWA VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11438, End = 0x1143F, Type = Extend });
            // Mc   [2] NEWA VOWEL SIGN O..NEWA VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11440, End = 0x11441, Type = SpacingMark });
            // Mn   [3] NEWA SIGN VIRAMA..NEWA SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11442, End = 0x11444, Type = Extend });
            // Mc   [2] TIRHUTA VOWEL SIGN I..TIRHUTA VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x114B1, End = 0x114B2, Type = SpacingMark });
            // Mn   [6] TIRHUTA VOWEL SIGN U..TIRHUTA VOWEL SIGN VOCALIC LL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x114B3, End = 0x114B8, Type = Extend });
            // Mc   [2] TIRHUTA VOWEL SIGN AI..TIRHUTA VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x114BB, End = 0x114BC, Type = SpacingMark });
            // Mn   [2] TIRHUTA SIGN CANDRABINDU..TIRHUTA SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x114BF, End = 0x114C0, Type = Extend });
            // Mn   [2] TIRHUTA SIGN VIRAMA..TIRHUTA SIGN NUKTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x114C2, End = 0x114C3, Type = Extend });
            // Mc   [2] SIDDHAM VOWEL SIGN I..SIDDHAM VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x115B0, End = 0x115B1, Type = SpacingMark });
            // Mn   [4] SIDDHAM VOWEL SIGN U..SIDDHAM VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x115B2, End = 0x115B5, Type = Extend });
            // Mc   [4] SIDDHAM VOWEL SIGN E..SIDDHAM VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x115B8, End = 0x115BB, Type = SpacingMark });
            // Mn   [2] SIDDHAM SIGN CANDRABINDU..SIDDHAM SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x115BC, End = 0x115BD, Type = Extend });
            // Mn   [2] SIDDHAM SIGN VIRAMA..SIDDHAM SIGN NUKTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x115BF, End = 0x115C0, Type = Extend });
            // Mn   [2] SIDDHAM VOWEL SIGN ALTERNATE U..SIDDHAM VOWEL SIGN ALTERNATE UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x115DC, End = 0x115DD, Type = Extend });
            // Mc   [3] MODI VOWEL SIGN AA..MODI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11630, End = 0x11632, Type = SpacingMark });
            // Mn   [8] MODI VOWEL SIGN U..MODI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11633, End = 0x1163A, Type = Extend });
            // Mc   [2] MODI VOWEL SIGN O..MODI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1163B, End = 0x1163C, Type = SpacingMark });
            // Mn   [2] MODI SIGN VIRAMA..MODI SIGN ARDHACANDRA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1163F, End = 0x11640, Type = Extend });
            // Mc   [2] TAKRI VOWEL SIGN I..TAKRI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x116AE, End = 0x116AF, Type = SpacingMark });
            // Mn   [6] TAKRI VOWEL SIGN U..TAKRI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x116B0, End = 0x116B5, Type = Extend });
            // Mn   [3] AHOM CONSONANT SIGN MEDIAL LA..AHOM CONSONANT SIGN MEDIAL LIGATING RA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1171D, End = 0x1171F, Type = Extend });
            // Mn   [4] AHOM VOWEL SIGN I..AHOM VOWEL SIGN UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11722, End = 0x11725, Type = Extend });
            // Mn   [5] AHOM VOWEL SIGN AW..AHOM SIGN KILLER
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11727, End = 0x1172B, Type = Extend });
            // Mc   [3] DOGRA VOWEL SIGN AA..DOGRA VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1182C, End = 0x1182E, Type = SpacingMark });
            // Mn   [9] DOGRA VOWEL SIGN U..DOGRA SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1182F, End = 0x11837, Type = Extend });
            // Mn   [2] DOGRA SIGN VIRAMA..DOGRA SIGN NUKTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11839, End = 0x1183A, Type = Extend });
            // Mc   [5] DIVES AKURU VOWEL SIGN I..DIVES AKURU VOWEL SIGN E
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11931, End = 0x11935, Type = SpacingMark });
            // Mc   [2] DIVES AKURU VOWEL SIGN AI..DIVES AKURU VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11937, End = 0x11938, Type = SpacingMark });
            // Mn   [2] DIVES AKURU SIGN ANUSVARA..DIVES AKURU SIGN CANDRABINDU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1193B, End = 0x1193C, Type = Extend });
            // Mc   [3] NANDINAGARI VOWEL SIGN AA..NANDINAGARI VOWEL SIGN II
            m_lst_code_range.Add(new RangeInfo() { Start = 0x119D1, End = 0x119D3, Type = SpacingMark });
            // Mn   [4] NANDINAGARI VOWEL SIGN U..NANDINAGARI VOWEL SIGN VOCALIC RR
            m_lst_code_range.Add(new RangeInfo() { Start = 0x119D4, End = 0x119D7, Type = Extend });
            // Mn   [2] NANDINAGARI VOWEL SIGN E..NANDINAGARI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x119DA, End = 0x119DB, Type = Extend });
            // Mc   [4] NANDINAGARI VOWEL SIGN O..NANDINAGARI SIGN VISARGA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x119DC, End = 0x119DF, Type = SpacingMark });
            // Mn  [10] ZANABAZAR SQUARE VOWEL SIGN I..ZANABAZAR SQUARE VOWEL LENGTH MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A01, End = 0x11A0A, Type = Extend });
            // Mn   [6] ZANABAZAR SQUARE FINAL CONSONANT MARK..ZANABAZAR SQUARE SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A33, End = 0x11A38, Type = Extend });
            // Mn   [4] ZANABAZAR SQUARE CLUSTER-FINAL LETTER YA..ZANABAZAR SQUARE CLUSTER-FINAL LETTER VA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A3B, End = 0x11A3E, Type = Extend });
            // Mn   [6] SOYOMBO VOWEL SIGN I..SOYOMBO VOWEL SIGN OE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A51, End = 0x11A56, Type = Extend });
            // Mc   [2] SOYOMBO VOWEL SIGN AI..SOYOMBO VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A57, End = 0x11A58, Type = SpacingMark });
            // Mn   [3] SOYOMBO VOWEL SIGN VOCALIC R..SOYOMBO VOWEL LENGTH MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A59, End = 0x11A5B, Type = Extend });
            // Lo   [6] SOYOMBO SIGN JIHVAMULIYA..SOYOMBO CLUSTER-INITIAL LETTER SA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A84, End = 0x11A89, Type = Prepend });
            // Mn  [13] SOYOMBO FINAL CONSONANT SIGN G..SOYOMBO SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A8A, End = 0x11A96, Type = Extend });
            // Mn   [2] SOYOMBO GEMINATION MARK..SOYOMBO SUBJOINER
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11A98, End = 0x11A99, Type = Extend });
            // Mn   [7] BHAIKSUKI VOWEL SIGN I..BHAIKSUKI VOWEL SIGN VOCALIC L
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11C30, End = 0x11C36, Type = Extend });
            // Mn   [6] BHAIKSUKI VOWEL SIGN E..BHAIKSUKI SIGN ANUSVARA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11C38, End = 0x11C3D, Type = Extend });
            // Mn  [22] MARCHEN SUBJOINED LETTER KA..MARCHEN SUBJOINED LETTER ZA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11C92, End = 0x11CA7, Type = Extend });
            // Mn   [7] MARCHEN SUBJOINED LETTER RA..MARCHEN VOWEL SIGN AA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11CAA, End = 0x11CB0, Type = Extend });
            // Mn   [2] MARCHEN VOWEL SIGN U..MARCHEN VOWEL SIGN E
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11CB2, End = 0x11CB3, Type = Extend });
            // Mn   [2] MARCHEN SIGN ANUSVARA..MARCHEN SIGN CANDRABINDU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11CB5, End = 0x11CB6, Type = Extend });
            // Mn   [6] MASARAM GONDI VOWEL SIGN AA..MASARAM GONDI VOWEL SIGN VOCALIC R
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11D31, End = 0x11D36, Type = Extend });
            // Mn   [2] MASARAM GONDI VOWEL SIGN AI..MASARAM GONDI VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11D3C, End = 0x11D3D, Type = Extend });
            // Mn   [7] MASARAM GONDI VOWEL SIGN AU..MASARAM GONDI VIRAMA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11D3F, End = 0x11D45, Type = Extend });
            // Mc   [5] GUNJALA GONDI VOWEL SIGN AA..GUNJALA GONDI VOWEL SIGN UU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11D8A, End = 0x11D8E, Type = SpacingMark });
            // Mn   [2] GUNJALA GONDI VOWEL SIGN EE..GUNJALA GONDI VOWEL SIGN AI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11D90, End = 0x11D91, Type = Extend });
            // Mc   [2] GUNJALA GONDI VOWEL SIGN OO..GUNJALA GONDI VOWEL SIGN AU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11D93, End = 0x11D94, Type = SpacingMark });
            // Mn   [2] MAKASAR VOWEL SIGN I..MAKASAR VOWEL SIGN U
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11EF3, End = 0x11EF4, Type = Extend });
            // Mc   [2] MAKASAR VOWEL SIGN E..MAKASAR VOWEL SIGN O
            m_lst_code_range.Add(new RangeInfo() { Start = 0x11EF5, End = 0x11EF6, Type = SpacingMark });
            // Cf   [9] EGYPTIAN HIEROGLYPH VERTICAL JOINER..EGYPTIAN HIEROGLYPH END SEGMENT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x13430, End = 0x13438, Type = Control });
            // Mn   [5] BASSA VAH COMBINING HIGH TONE..BASSA VAH COMBINING HIGH-LOW TONE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x16AF0, End = 0x16AF4, Type = Extend });
            // Mn   [7] PAHAWH HMONG MARK CIM TUB..PAHAWH HMONG MARK CIM TAUM
            m_lst_code_range.Add(new RangeInfo() { Start = 0x16B30, End = 0x16B36, Type = Extend });
            // Mc  [55] MIAO SIGN ASPIRATION..MIAO VOWEL SIGN UI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x16F51, End = 0x16F87, Type = SpacingMark });
            // Mn   [4] MIAO TONE RIGHT..MIAO TONE BELOW
            m_lst_code_range.Add(new RangeInfo() { Start = 0x16F8F, End = 0x16F92, Type = Extend });
            // Mc   [2] VIETNAMESE ALTERNATE READING MARK CA..VIETNAMESE ALTERNATE READING MARK NHAY
            m_lst_code_range.Add(new RangeInfo() { Start = 0x16FF0, End = 0x16FF1, Type = SpacingMark });
            // Mn   [2] DUPLOYAN THICK LETTER SELECTOR..DUPLOYAN DOUBLE MARK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1BC9D, End = 0x1BC9E, Type = Extend });
            // Cf   [4] SHORTHAND FORMAT LETTER OVERLAP..SHORTHAND FORMAT UP STEP
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1BCA0, End = 0x1BCA3, Type = Control });
            // Mn  [46] ZNAMENNY COMBINING MARK GORAZDO NIZKO S KRYZHEM ON LEFT..ZNAMENNY COMBINING MARK KRYZH ON LEFT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1CF00, End = 0x1CF2D, Type = Extend });
            // Mn  [23] ZNAMENNY COMBINING TONAL RANGE MARK MRACHNO..ZNAMENNY PRIZNAK MODIFIER ROG
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1CF30, End = 0x1CF46, Type = Extend });
            // Mn   [3] MUSICAL SYMBOL COMBINING TREMOLO-1..MUSICAL SYMBOL COMBINING TREMOLO-3
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1D167, End = 0x1D169, Type = Extend });
            // Mc   [5] MUSICAL SYMBOL COMBINING FLAG-1..MUSICAL SYMBOL COMBINING FLAG-5
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1D16E, End = 0x1D172, Type = Extend });
            // Cf   [8] MUSICAL SYMBOL BEGIN BEAM..MUSICAL SYMBOL END PHRASE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1D173, End = 0x1D17A, Type = Control });
            // Mn   [8] MUSICAL SYMBOL COMBINING ACCENT..MUSICAL SYMBOL COMBINING LOURE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1D17B, End = 0x1D182, Type = Extend });
            // Mn   [7] MUSICAL SYMBOL COMBINING DOIT..MUSICAL SYMBOL COMBINING TRIPLE TONGUE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1D185, End = 0x1D18B, Type = Extend });
            // Mn   [4] MUSICAL SYMBOL COMBINING DOWN BOW..MUSICAL SYMBOL COMBINING SNAP PIZZICATO
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1D1AA, End = 0x1D1AD, Type = Extend });
            // Mn   [3] COMBINING GREEK MUSICAL TRISEME..COMBINING GREEK MUSICAL PENTASEME
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1D242, End = 0x1D244, Type = Extend });
            // Mn  [55] SIGNWRITING HEAD RIM..SIGNWRITING AIR SUCKING IN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1DA00, End = 0x1DA36, Type = Extend });
            // Mn  [50] SIGNWRITING MOUTH CLOSED NEUTRAL..SIGNWRITING EXCITEMENT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1DA3B, End = 0x1DA6C, Type = Extend });
            // Mn   [5] SIGNWRITING FILL MODIFIER-2..SIGNWRITING FILL MODIFIER-6
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1DA9B, End = 0x1DA9F, Type = Extend });
            // Mn  [15] SIGNWRITING ROTATION MODIFIER-2..SIGNWRITING ROTATION MODIFIER-16
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1DAA1, End = 0x1DAAF, Type = Extend });
            // Mn   [7] COMBINING GLAGOLITIC LETTER AZU..COMBINING GLAGOLITIC LETTER ZHIVETE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E000, End = 0x1E006, Type = Extend });
            // Mn  [17] COMBINING GLAGOLITIC LETTER ZEMLJA..COMBINING GLAGOLITIC LETTER HERU
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E008, End = 0x1E018, Type = Extend });
            // Mn   [7] COMBINING GLAGOLITIC LETTER SHTA..COMBINING GLAGOLITIC LETTER YATI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E01B, End = 0x1E021, Type = Extend });
            // Mn   [2] COMBINING GLAGOLITIC LETTER YU..COMBINING GLAGOLITIC LETTER SMALL YUS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E023, End = 0x1E024, Type = Extend });
            // Mn   [5] COMBINING GLAGOLITIC LETTER YO..COMBINING GLAGOLITIC LETTER FITA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E026, End = 0x1E02A, Type = Extend });
            // Mn   [7] NYIAKENG PUACHUE HMONG TONE-B..NYIAKENG PUACHUE HMONG TONE-D
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E130, End = 0x1E136, Type = Extend });
            // Mn   [4] WANCHO TONE TUP..WANCHO TONE KOINI
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E2EC, End = 0x1E2EF, Type = Extend });
            // Mn   [7] MENDE KIKAKUI COMBINING NUMBER TEENS..MENDE KIKAKUI COMBINING NUMBER MILLIONS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E8D0, End = 0x1E8D6, Type = Extend });
            // Mn   [7] ADLAM ALIF LENGTHENER..ADLAM NUKTA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1E944, End = 0x1E94A, Type = Extend });
            // E0.0   [4] (🀀..🀃)    MAHJONG TILE EAST WIND..MAHJONG TILE NORTH WIND
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F000, End = 0x1F003, Type = Extended_Pictographic });
            // E0.0 [202] (🀅..🃎)    MAHJONG TILE GREEN DRAGON..PLAYING CARD KING OF DIAMONDS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F005, End = 0x1F0CE, Type = Extended_Pictographic });
            // E0.0  [48] (🃐..🃿)    <reserved-1F0D0>..<reserved-1F0FF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F0D0, End = 0x1F0FF, Type = Extended_Pictographic });
            // E0.0   [3] (🄍..🄏)    CIRCLED ZERO WITH SLASH..CIRCLED DOLLAR SIGN WITH OVERLAID BACKSLASH
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F10D, End = 0x1F10F, Type = Extended_Pictographic });
            // E0.0   [4] (🅬..🅯)    RAISED MR SIGN..CIRCLED HUMAN FIGURE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F16C, End = 0x1F16F, Type = Extended_Pictographic });
            // E0.6   [2] (🅰️..🅱️)    A button (blood type)..B button (blood type)
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F170, End = 0x1F171, Type = Extended_Pictographic });
            // E0.6   [2] (🅾️..🅿️)    O button (blood type)..P button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F17E, End = 0x1F17F, Type = Extended_Pictographic });
            // E0.6  [10] (🆑..🆚)    CL button..VS button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F191, End = 0x1F19A, Type = Extended_Pictographic });
            // E0.0  [57] (🆭..🇥)    MASK WORK SYMBOL..<reserved-1F1E5>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F1AD, End = 0x1F1E5, Type = Extended_Pictographic });
            // So  [26] REGIONAL INDICATOR SYMBOL LETTER A..REGIONAL INDICATOR SYMBOL LETTER Z
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F1E6, End = 0x1F1FF, Type = Regional_Indicator });
            // E0.6   [2] (🈁..🈂️)    Japanese “here” button..Japanese “service charge” button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F201, End = 0x1F202, Type = Extended_Pictographic });
            // E0.0  [13] (🈃..🈏)    <reserved-1F203>..<reserved-1F20F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F203, End = 0x1F20F, Type = Extended_Pictographic });
            // E0.6   [9] (🈲..🈺)    Japanese “prohibited” button..Japanese “open for business” button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F232, End = 0x1F23A, Type = Extended_Pictographic });
            // E0.0   [4] (🈼..🈿)    <reserved-1F23C>..<reserved-1F23F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F23C, End = 0x1F23F, Type = Extended_Pictographic });
            // E0.0   [7] (🉉..🉏)    <reserved-1F249>..<reserved-1F24F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F249, End = 0x1F24F, Type = Extended_Pictographic });
            // E0.6   [2] (🉐..🉑)    Japanese “bargain” button..Japanese “acceptable” button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F250, End = 0x1F251, Type = Extended_Pictographic });
            // E0.0 [174] (🉒..🋿)    <reserved-1F252>..<reserved-1F2FF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F252, End = 0x1F2FF, Type = Extended_Pictographic });
            // E0.6  [13] (🌀..🌌)    cyclone..milky way
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F300, End = 0x1F30C, Type = Extended_Pictographic });
            // E0.7   [2] (🌍..🌎)    globe showing Europe-Africa..globe showing Americas
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F30D, End = 0x1F30E, Type = Extended_Pictographic });
            // E0.6   [3] (🌓..🌕)    first quarter moon..full moon
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F313, End = 0x1F315, Type = Extended_Pictographic });
            // E1.0   [3] (🌖..🌘)    waning gibbous moon..waning crescent moon
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F316, End = 0x1F318, Type = Extended_Pictographic });
            // E1.0   [2] (🌝..🌞)    full moon face..sun with face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F31D, End = 0x1F31E, Type = Extended_Pictographic });
            // E0.6   [2] (🌟..🌠)    glowing star..shooting star
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F31F, End = 0x1F320, Type = Extended_Pictographic });
            // E0.0   [2] (🌢..🌣)    BLACK DROPLET..WHITE SUN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F322, End = 0x1F323, Type = Extended_Pictographic });
            // E0.7   [9] (🌤️..🌬️)    sun behind small cloud..wind face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F324, End = 0x1F32C, Type = Extended_Pictographic });
            // E1.0   [3] (🌭..🌯)    hot dog..burrito
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F32D, End = 0x1F32F, Type = Extended_Pictographic });
            // E0.6   [2] (🌰..🌱)    chestnut..seedling
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F330, End = 0x1F331, Type = Extended_Pictographic });
            // E1.0   [2] (🌲..🌳)    evergreen tree..deciduous tree
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F332, End = 0x1F333, Type = Extended_Pictographic });
            // E0.6   [2] (🌴..🌵)    palm tree..cactus
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F334, End = 0x1F335, Type = Extended_Pictographic });
            // E0.6  [20] (🌷..🍊)    tulip..tangerine
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F337, End = 0x1F34A, Type = Extended_Pictographic });
            // E0.6   [4] (🍌..🍏)    banana..green apple
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F34C, End = 0x1F34F, Type = Extended_Pictographic });
            // E0.6  [43] (🍑..🍻)    peach..clinking beer mugs
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F351, End = 0x1F37B, Type = Extended_Pictographic });
            // E1.0   [2] (🍾..🍿)    bottle with popping cork..popcorn
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F37E, End = 0x1F37F, Type = Extended_Pictographic });
            // E0.6  [20] (🎀..🎓)    ribbon..graduation cap
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F380, End = 0x1F393, Type = Extended_Pictographic });
            // E0.0   [2] (🎔..🎕)    HEART WITH TIP ON THE LEFT..BOUQUET OF FLOWERS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F394, End = 0x1F395, Type = Extended_Pictographic });
            // E0.7   [2] (🎖️..🎗️)    military medal..reminder ribbon
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F396, End = 0x1F397, Type = Extended_Pictographic });
            // E0.7   [3] (🎙️..🎛️)    studio microphone..control knobs
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F399, End = 0x1F39B, Type = Extended_Pictographic });
            // E0.0   [2] (🎜..🎝)    BEAMED ASCENDING MUSICAL NOTES..BEAMED DESCENDING MUSICAL NOTES
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F39C, End = 0x1F39D, Type = Extended_Pictographic });
            // E0.7   [2] (🎞️..🎟️)    film frames..admission tickets
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F39E, End = 0x1F39F, Type = Extended_Pictographic });
            // E0.6  [37] (🎠..🏄)    carousel horse..person surfing
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3A0, End = 0x1F3C4, Type = Extended_Pictographic });
            // E0.7   [4] (🏋️..🏎️)    person lifting weights..racing car
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3CB, End = 0x1F3CE, Type = Extended_Pictographic });
            // E1.0   [5] (🏏..🏓)    cricket game..ping pong
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3CF, End = 0x1F3D3, Type = Extended_Pictographic });
            // E0.7  [12] (🏔️..🏟️)    snow-capped mountain..stadium
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3D4, End = 0x1F3DF, Type = Extended_Pictographic });
            // E0.6   [4] (🏠..🏣)    house..Japanese post office
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3E0, End = 0x1F3E3, Type = Extended_Pictographic });
            // E0.6  [12] (🏥..🏰)    hospital..castle
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3E5, End = 0x1F3F0, Type = Extended_Pictographic });
            // E0.0   [2] (🏱..🏲)    WHITE PENNANT..BLACK PENNANT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3F1, End = 0x1F3F2, Type = Extended_Pictographic });
            // E1.0   [3] (🏸..🏺)    badminton..amphora
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3F8, End = 0x1F3FA, Type = Extended_Pictographic });
            // Sk   [5] EMOJI MODIFIER FITZPATRICK TYPE-1-2..EMOJI MODIFIER FITZPATRICK TYPE-6
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F3FB, End = 0x1F3FF, Type = Extend });
            // E1.0   [8] (🐀..🐇)    rat..rabbit
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F400, End = 0x1F407, Type = Extended_Pictographic });
            // E1.0   [3] (🐉..🐋)    dragon..whale
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F409, End = 0x1F40B, Type = Extended_Pictographic });
            // E0.6   [3] (🐌..🐎)    snail..horse
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F40C, End = 0x1F40E, Type = Extended_Pictographic });
            // E1.0   [2] (🐏..🐐)    ram..goat
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F40F, End = 0x1F410, Type = Extended_Pictographic });
            // E0.6   [2] (🐑..🐒)    ewe..monkey
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F411, End = 0x1F412, Type = Extended_Pictographic });
            // E0.6  [19] (🐗..🐩)    boar..poodle
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F417, End = 0x1F429, Type = Extended_Pictographic });
            // E0.6  [20] (🐫..🐾)    two-hump camel..paw prints
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F42B, End = 0x1F43E, Type = Extended_Pictographic });
            // E0.6  [35] (👂..👤)    ear..bust in silhouette
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F442, End = 0x1F464, Type = Extended_Pictographic });
            // E0.6   [6] (👦..👫)    boy..woman and man holding hands
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F466, End = 0x1F46B, Type = Extended_Pictographic });
            // E1.0   [2] (👬..👭)    men holding hands..women holding hands
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F46C, End = 0x1F46D, Type = Extended_Pictographic });
            // E0.6  [63] (👮..💬)    police officer..speech balloon
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F46E, End = 0x1F4AC, Type = Extended_Pictographic });
            // E0.6   [8] (💮..💵)    white flower..dollar banknote
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4AE, End = 0x1F4B5, Type = Extended_Pictographic });
            // E1.0   [2] (💶..💷)    euro banknote..pound banknote
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4B6, End = 0x1F4B7, Type = Extended_Pictographic });
            // E0.6  [52] (💸..📫)    money with wings..closed mailbox with raised flag
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4B8, End = 0x1F4EB, Type = Extended_Pictographic });
            // E0.7   [2] (📬..📭)    open mailbox with raised flag..open mailbox with lowered flag
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4EC, End = 0x1F4ED, Type = Extended_Pictographic });
            // E0.6   [5] (📰..📴)    newspaper..mobile phone off
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4F0, End = 0x1F4F4, Type = Extended_Pictographic });
            // E0.6   [2] (📶..📷)    antenna bars..camera
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4F6, End = 0x1F4F7, Type = Extended_Pictographic });
            // E0.6   [4] (📹..📼)    video camera..videocassette
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4F9, End = 0x1F4FC, Type = Extended_Pictographic });
            // E1.0   [4] (📿..🔂)    prayer beads..repeat single button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F4FF, End = 0x1F502, Type = Extended_Pictographic });
            // E1.0   [4] (🔄..🔇)    counterclockwise arrows button..muted speaker
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F504, End = 0x1F507, Type = Extended_Pictographic });
            // E0.6  [11] (🔊..🔔)    speaker high volume..bell
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F50A, End = 0x1F514, Type = Extended_Pictographic });
            // E0.6  [22] (🔖..🔫)    bookmark..water pistol
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F516, End = 0x1F52B, Type = Extended_Pictographic });
            // E1.0   [2] (🔬..🔭)    microscope..telescope
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F52C, End = 0x1F52D, Type = Extended_Pictographic });
            // E0.6  [16] (🔮..🔽)    crystal ball..downwards button
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F52E, End = 0x1F53D, Type = Extended_Pictographic });
            // E0.0   [3] (🕆..🕈)    WHITE LATIN CROSS..CELTIC CROSS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F546, End = 0x1F548, Type = Extended_Pictographic });
            // E0.7   [2] (🕉️..🕊️)    om..dove
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F549, End = 0x1F54A, Type = Extended_Pictographic });
            // E1.0   [4] (🕋..🕎)    kaaba..menorah
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F54B, End = 0x1F54E, Type = Extended_Pictographic });
            // E0.6  [12] (🕐..🕛)    one o’clock..twelve o’clock
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F550, End = 0x1F55B, Type = Extended_Pictographic });
            // E0.7  [12] (🕜..🕧)    one-thirty..twelve-thirty
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F55C, End = 0x1F567, Type = Extended_Pictographic });
            // E0.0   [7] (🕨..🕮)    RIGHT SPEAKER..BOOK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F568, End = 0x1F56E, Type = Extended_Pictographic });
            // E0.7   [2] (🕯️..🕰️)    candle..mantelpiece clock
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F56F, End = 0x1F570, Type = Extended_Pictographic });
            // E0.0   [2] (🕱..🕲)    BLACK SKULL AND CROSSBONES..NO PIRACY
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F571, End = 0x1F572, Type = Extended_Pictographic });
            // E0.7   [7] (🕳️..🕹️)    hole..joystick
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F573, End = 0x1F579, Type = Extended_Pictographic });
            // E0.0  [12] (🕻..🖆)    LEFT HAND TELEPHONE RECEIVER..PEN OVER STAMPED ENVELOPE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F57B, End = 0x1F586, Type = Extended_Pictographic });
            // E0.0   [2] (🖈..🖉)    BLACK PUSHPIN..LOWER LEFT PENCIL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F588, End = 0x1F589, Type = Extended_Pictographic });
            // E0.7   [4] (🖊️..🖍️)    pen..crayon
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F58A, End = 0x1F58D, Type = Extended_Pictographic });
            // E0.0   [2] (🖎..🖏)    LEFT WRITING HAND..TURNED OK HAND SIGN
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F58E, End = 0x1F58F, Type = Extended_Pictographic });
            // E0.0   [4] (🖑..🖔)    REVERSED RAISED HAND WITH FINGERS SPLAYED..REVERSED VICTORY HAND
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F591, End = 0x1F594, Type = Extended_Pictographic });
            // E1.0   [2] (🖕..🖖)    middle finger..vulcan salute
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F595, End = 0x1F596, Type = Extended_Pictographic });
            // E0.0  [13] (🖗..🖣)    WHITE DOWN POINTING LEFT HAND INDEX..BLACK DOWN POINTING BACKHAND INDEX
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F597, End = 0x1F5A3, Type = Extended_Pictographic });
            // E0.0   [2] (🖦..🖧)    KEYBOARD AND MOUSE..THREE NETWORKED COMPUTERS
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5A6, End = 0x1F5A7, Type = Extended_Pictographic });
            // E0.0   [8] (🖩..🖰)    POCKET CALCULATOR..TWO BUTTON MOUSE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5A9, End = 0x1F5B0, Type = Extended_Pictographic });
            // E0.7   [2] (🖱️..🖲️)    computer mouse..trackball
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5B1, End = 0x1F5B2, Type = Extended_Pictographic });
            // E0.0   [9] (🖳..🖻)    OLD PERSONAL COMPUTER..DOCUMENT WITH PICTURE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5B3, End = 0x1F5BB, Type = Extended_Pictographic });
            // E0.0   [5] (🖽..🗁)    FRAME WITH TILES..OPEN FOLDER
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5BD, End = 0x1F5C1, Type = Extended_Pictographic });
            // E0.7   [3] (🗂️..🗄️)    card index dividers..file cabinet
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5C2, End = 0x1F5C4, Type = Extended_Pictographic });
            // E0.0  [12] (🗅..🗐)    EMPTY NOTE..PAGES
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5C5, End = 0x1F5D0, Type = Extended_Pictographic });
            // E0.7   [3] (🗑️..🗓️)    wastebasket..spiral calendar
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5D1, End = 0x1F5D3, Type = Extended_Pictographic });
            // E0.0   [8] (🗔..🗛)    DESKTOP WINDOW..DECREASE FONT SIZE SYMBOL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5D4, End = 0x1F5DB, Type = Extended_Pictographic });
            // E0.7   [3] (🗜️..🗞️)    clamp..rolled-up newspaper
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5DC, End = 0x1F5DE, Type = Extended_Pictographic });
            // E0.0   [2] (🗟..🗠)    PAGE WITH CIRCLED TEXT..STOCK CHART
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5DF, End = 0x1F5E0, Type = Extended_Pictographic });
            // E0.0   [4] (🗤..🗧)    THREE RAYS ABOVE..THREE RAYS RIGHT
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5E4, End = 0x1F5E7, Type = Extended_Pictographic });
            // E0.0   [6] (🗩..🗮)    RIGHT SPEECH BUBBLE..LEFT ANGER BUBBLE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5E9, End = 0x1F5EE, Type = Extended_Pictographic });
            // E0.0   [3] (🗰..🗲)    MOOD BUBBLE..LIGHTNING MOOD
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5F0, End = 0x1F5F2, Type = Extended_Pictographic });
            // E0.0   [6] (🗴..🗹)    BALLOT SCRIPT X..BALLOT BOX WITH BOLD CHECK
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5F4, End = 0x1F5F9, Type = Extended_Pictographic });
            // E0.6   [5] (🗻..🗿)    mount fuji..moai
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F5FB, End = 0x1F5FF, Type = Extended_Pictographic });
            // E0.6   [6] (😁..😆)    beaming face with smiling eyes..grinning squinting face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F601, End = 0x1F606, Type = Extended_Pictographic });
            // E1.0   [2] (😇..😈)    smiling face with halo..smiling face with horns
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F607, End = 0x1F608, Type = Extended_Pictographic });
            // E0.6   [5] (😉..😍)    winking face..smiling face with heart-eyes
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F609, End = 0x1F60D, Type = Extended_Pictographic });
            // E0.6   [3] (😒..😔)    unamused face..pensive face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F612, End = 0x1F614, Type = Extended_Pictographic });
            // E0.6   [3] (😜..😞)    winking face with tongue..disappointed face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F61C, End = 0x1F61E, Type = Extended_Pictographic });
            // E0.6   [6] (😠..😥)    angry face..sad but relieved face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F620, End = 0x1F625, Type = Extended_Pictographic });
            // E1.0   [2] (😦..😧)    frowning face with open mouth..anguished face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F626, End = 0x1F627, Type = Extended_Pictographic });
            // E0.6   [4] (😨..😫)    fearful face..tired face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F628, End = 0x1F62B, Type = Extended_Pictographic });
            // E1.0   [2] (😮..😯)    face with open mouth..hushed face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F62E, End = 0x1F62F, Type = Extended_Pictographic });
            // E0.6   [4] (😰..😳)    anxious face with sweat..flushed face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F630, End = 0x1F633, Type = Extended_Pictographic });
            // E0.6  [10] (😷..🙀)    face with medical mask..weary cat
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F637, End = 0x1F640, Type = Extended_Pictographic });
            // E1.0   [4] (🙁..🙄)    slightly frowning face..face with rolling eyes
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F641, End = 0x1F644, Type = Extended_Pictographic });
            // E0.6  [11] (🙅..🙏)    person gesturing NO..folded hands
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F645, End = 0x1F64F, Type = Extended_Pictographic });
            // E1.0   [2] (🚁..🚂)    helicopter..locomotive
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F681, End = 0x1F682, Type = Extended_Pictographic });
            // E0.6   [3] (🚃..🚅)    railway car..bullet train
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F683, End = 0x1F685, Type = Extended_Pictographic });
            // E1.0   [2] (🚊..🚋)    tram..tram car
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F68A, End = 0x1F68B, Type = Extended_Pictographic });
            // E0.6   [3] (🚑..🚓)    ambulance..police car
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F691, End = 0x1F693, Type = Extended_Pictographic });
            // E0.6   [2] (🚙..🚚)    sport utility vehicle..delivery truck
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F699, End = 0x1F69A, Type = Extended_Pictographic });
            // E1.0   [7] (🚛..🚡)    articulated lorry..aerial tramway
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F69B, End = 0x1F6A1, Type = Extended_Pictographic });
            // E0.6   [2] (🚤..🚥)    speedboat..horizontal traffic light
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6A4, End = 0x1F6A5, Type = Extended_Pictographic });
            // E0.6   [7] (🚧..🚭)    construction..no smoking
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6A7, End = 0x1F6AD, Type = Extended_Pictographic });
            // E1.0   [4] (🚮..🚱)    litter in bin sign..non-potable water
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6AE, End = 0x1F6B1, Type = Extended_Pictographic });
            // E1.0   [3] (🚳..🚵)    no bicycles..person mountain biking
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6B3, End = 0x1F6B5, Type = Extended_Pictographic });
            // E1.0   [2] (🚷..🚸)    no pedestrians..children crossing
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6B7, End = 0x1F6B8, Type = Extended_Pictographic });
            // E0.6   [6] (🚹..🚾)    men’s room..water closet
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6B9, End = 0x1F6BE, Type = Extended_Pictographic });
            // E1.0   [5] (🛁..🛅)    bathtub..left luggage
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6C1, End = 0x1F6C5, Type = Extended_Pictographic });
            // E0.0   [5] (🛆..🛊)    TRIANGLE WITH ROUNDED CORNERS..GIRLS SYMBOL
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6C6, End = 0x1F6CA, Type = Extended_Pictographic });
            // E0.7   [3] (🛍️..🛏️)    shopping bags..bed
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6CD, End = 0x1F6CF, Type = Extended_Pictographic });
            // E3.0   [2] (🛑..🛒)    stop sign..shopping cart
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6D1, End = 0x1F6D2, Type = Extended_Pictographic });
            // E0.0   [2] (🛓..🛔)    STUPA..PAGODA
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6D3, End = 0x1F6D4, Type = Extended_Pictographic });
            // E13.0  [2] (🛖..🛗)    hut..elevator
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6D6, End = 0x1F6D7, Type = Extended_Pictographic });
            // E0.0   [5] (🛘..🛜)    <reserved-1F6D8>..<reserved-1F6DC>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6D8, End = 0x1F6DC, Type = Extended_Pictographic });
            // E14.0  [3] (🛝..🛟)    playground slide..ring buoy
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6DD, End = 0x1F6DF, Type = Extended_Pictographic });
            // E0.7   [6] (🛠️..🛥️)    hammer and wrench..motor boat
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6E0, End = 0x1F6E5, Type = Extended_Pictographic });
            // E0.0   [3] (🛦..🛨)    UP-POINTING MILITARY AIRPLANE..UP-POINTING SMALL AIRPLANE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6E6, End = 0x1F6E8, Type = Extended_Pictographic });
            // E1.0   [2] (🛫..🛬)    airplane departure..airplane arrival
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6EB, End = 0x1F6EC, Type = Extended_Pictographic });
            // E0.0   [3] (🛭..🛯)    <reserved-1F6ED>..<reserved-1F6EF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6ED, End = 0x1F6EF, Type = Extended_Pictographic });
            // E0.0   [2] (🛱..🛲)    ONCOMING FIRE ENGINE..DIESEL LOCOMOTIVE
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6F1, End = 0x1F6F2, Type = Extended_Pictographic });
            // E3.0   [3] (🛴..🛶)    kick scooter..canoe
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6F4, End = 0x1F6F6, Type = Extended_Pictographic });
            // E5.0   [2] (🛷..🛸)    sled..flying saucer
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6F7, End = 0x1F6F8, Type = Extended_Pictographic });
            // E13.0  [2] (🛻..🛼)    pickup truck..roller skate
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6FB, End = 0x1F6FC, Type = Extended_Pictographic });
            // E0.0   [3] (🛽..🛿)    <reserved-1F6FD>..<reserved-1F6FF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F6FD, End = 0x1F6FF, Type = Extended_Pictographic });
            // E0.0  [12] (🝴..🝿)    <reserved-1F774>..<reserved-1F77F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F774, End = 0x1F77F, Type = Extended_Pictographic });
            // E0.0  [11] (🟕..🟟)    CIRCLED TRIANGLE..<reserved-1F7DF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F7D5, End = 0x1F7DF, Type = Extended_Pictographic });
            // E12.0 [12] (🟠..🟫)    orange circle..brown square
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F7E0, End = 0x1F7EB, Type = Extended_Pictographic });
            // E0.0   [4] (🟬..🟯)    <reserved-1F7EC>..<reserved-1F7EF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F7EC, End = 0x1F7EF, Type = Extended_Pictographic });
            // E0.0  [15] (🟱..🟿)    <reserved-1F7F1>..<reserved-1F7FF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F7F1, End = 0x1F7FF, Type = Extended_Pictographic });
            // E0.0   [4] (🠌..🠏)    <reserved-1F80C>..<reserved-1F80F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F80C, End = 0x1F80F, Type = Extended_Pictographic });
            // E0.0   [8] (🡈..🡏)    <reserved-1F848>..<reserved-1F84F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F848, End = 0x1F84F, Type = Extended_Pictographic });
            // E0.0   [6] (🡚..🡟)    <reserved-1F85A>..<reserved-1F85F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F85A, End = 0x1F85F, Type = Extended_Pictographic });
            // E0.0   [8] (🢈..🢏)    <reserved-1F888>..<reserved-1F88F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F888, End = 0x1F88F, Type = Extended_Pictographic });
            // E0.0  [82] (🢮..🣿)    <reserved-1F8AE>..<reserved-1F8FF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F8AE, End = 0x1F8FF, Type = Extended_Pictographic });
            // E12.0  [3] (🤍..🤏)    white heart..pinching hand
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F90D, End = 0x1F90F, Type = Extended_Pictographic });
            // E1.0   [9] (🤐..🤘)    zipper-mouth face..sign of the horns
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F910, End = 0x1F918, Type = Extended_Pictographic });
            // E3.0   [6] (🤙..🤞)    call me hand..crossed fingers
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F919, End = 0x1F91E, Type = Extended_Pictographic });
            // E3.0   [8] (🤠..🤧)    cowboy hat face..sneezing face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F920, End = 0x1F927, Type = Extended_Pictographic });
            // E5.0   [8] (🤨..🤯)    face with raised eyebrow..exploding head
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F928, End = 0x1F92F, Type = Extended_Pictographic });
            // E5.0   [2] (🤱..🤲)    breast-feeding..palms up together
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F931, End = 0x1F932, Type = Extended_Pictographic });
            // E3.0   [8] (🤳..🤺)    selfie..person fencing
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F933, End = 0x1F93A, Type = Extended_Pictographic });
            // E3.0   [3] (🤼..🤾)    people wrestling..person playing handball
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F93C, End = 0x1F93E, Type = Extended_Pictographic });
            // E3.0   [6] (🥀..🥅)    wilted flower..goal net
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F940, End = 0x1F945, Type = Extended_Pictographic });
            // E3.0   [5] (🥇..🥋)    1st place medal..martial arts uniform
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F947, End = 0x1F94B, Type = Extended_Pictographic });
            // E11.0  [3] (🥍..🥏)    lacrosse..flying disc
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F94D, End = 0x1F94F, Type = Extended_Pictographic });
            // E3.0  [15] (🥐..🥞)    croissant..pancakes
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F950, End = 0x1F95E, Type = Extended_Pictographic });
            // E5.0  [13] (🥟..🥫)    dumpling..canned food
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F95F, End = 0x1F96B, Type = Extended_Pictographic });
            // E11.0  [5] (🥬..🥰)    leafy green..smiling face with hearts
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F96C, End = 0x1F970, Type = Extended_Pictographic });
            // E11.0  [4] (🥳..🥶)    partying face..cold face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F973, End = 0x1F976, Type = Extended_Pictographic });
            // E13.0  [2] (🥷..🥸)    ninja..disguised face
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F977, End = 0x1F978, Type = Extended_Pictographic });
            // E11.0  [4] (🥼..🥿)    lab coat..flat shoe
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F97C, End = 0x1F97F, Type = Extended_Pictographic });
            // E1.0   [5] (🦀..🦄)    crab..unicorn
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F980, End = 0x1F984, Type = Extended_Pictographic });
            // E3.0  [13] (🦅..🦑)    eagle..squid
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F985, End = 0x1F991, Type = Extended_Pictographic });
            // E5.0   [6] (🦒..🦗)    giraffe..cricket
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F992, End = 0x1F997, Type = Extended_Pictographic });
            // E11.0 [11] (🦘..🦢)    kangaroo..swan
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F998, End = 0x1F9A2, Type = Extended_Pictographic });
            // E13.0  [2] (🦣..🦤)    mammoth..dodo
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9A3, End = 0x1F9A4, Type = Extended_Pictographic });
            // E12.0  [6] (🦥..🦪)    sloth..oyster
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9A5, End = 0x1F9AA, Type = Extended_Pictographic });
            // E13.0  [3] (🦫..🦭)    beaver..seal
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9AB, End = 0x1F9AD, Type = Extended_Pictographic });
            // E12.0  [2] (🦮..🦯)    guide dog..white cane
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9AE, End = 0x1F9AF, Type = Extended_Pictographic });
            // E11.0 [10] (🦰..🦹)    red hair..supervillain
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9B0, End = 0x1F9B9, Type = Extended_Pictographic });
            // E12.0  [6] (🦺..🦿)    safety vest..mechanical leg
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9BA, End = 0x1F9BF, Type = Extended_Pictographic });
            // E11.0  [2] (🧁..🧂)    cupcake..salt
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9C1, End = 0x1F9C2, Type = Extended_Pictographic });
            // E12.0  [8] (🧃..🧊)    beverage box..ice
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9C3, End = 0x1F9CA, Type = Extended_Pictographic });
            // E12.0  [3] (🧍..🧏)    person standing..deaf person
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9CD, End = 0x1F9CF, Type = Extended_Pictographic });
            // E5.0  [23] (🧐..🧦)    face with monocle..socks
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9D0, End = 0x1F9E6, Type = Extended_Pictographic });
            // E11.0 [25] (🧧..🧿)    red envelope..nazar amulet
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1F9E7, End = 0x1F9FF, Type = Extended_Pictographic });
            // E0.0 [112] (🨀..🩯)    NEUTRAL CHESS KING..<reserved-1FA6F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA00, End = 0x1FA6F, Type = Extended_Pictographic });
            // E12.0  [4] (🩰..🩳)    ballet shoes..shorts
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA70, End = 0x1FA73, Type = Extended_Pictographic });
            // E0.0   [3] (🩵..🩷)    <reserved-1FA75>..<reserved-1FA77>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA75, End = 0x1FA77, Type = Extended_Pictographic });
            // E12.0  [3] (🩸..🩺)    drop of blood..stethoscope
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA78, End = 0x1FA7A, Type = Extended_Pictographic });
            // E14.0  [2] (🩻..🩼)    x-ray..crutch
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA7B, End = 0x1FA7C, Type = Extended_Pictographic });
            // E0.0   [3] (🩽..🩿)    <reserved-1FA7D>..<reserved-1FA7F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA7D, End = 0x1FA7F, Type = Extended_Pictographic });
            // E12.0  [3] (🪀..🪂)    yo-yo..parachute
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA80, End = 0x1FA82, Type = Extended_Pictographic });
            // E13.0  [4] (🪃..🪆)    boomerang..nesting dolls
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA83, End = 0x1FA86, Type = Extended_Pictographic });
            // E0.0   [9] (🪇..🪏)    <reserved-1FA87>..<reserved-1FA8F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA87, End = 0x1FA8F, Type = Extended_Pictographic });
            // E12.0  [6] (🪐..🪕)    ringed planet..banjo
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA90, End = 0x1FA95, Type = Extended_Pictographic });
            // E13.0 [19] (🪖..🪨)    military helmet..rock
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FA96, End = 0x1FAA8, Type = Extended_Pictographic });
            // E14.0  [4] (🪩..🪬)    mirror ball..hamsa
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAA9, End = 0x1FAAC, Type = Extended_Pictographic });
            // E0.0   [3] (🪭..🪯)    <reserved-1FAAD>..<reserved-1FAAF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAAD, End = 0x1FAAF, Type = Extended_Pictographic });
            // E13.0  [7] (🪰..🪶)    fly..feather
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAB0, End = 0x1FAB6, Type = Extended_Pictographic });
            // E14.0  [4] (🪷..🪺)    lotus..nest with eggs
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAB7, End = 0x1FABA, Type = Extended_Pictographic });
            // E0.0   [5] (🪻..🪿)    <reserved-1FABB>..<reserved-1FABF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FABB, End = 0x1FABF, Type = Extended_Pictographic });
            // E13.0  [3] (🫀..🫂)    anatomical heart..people hugging
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAC0, End = 0x1FAC2, Type = Extended_Pictographic });
            // E14.0  [3] (🫃..🫅)    pregnant man..person with crown
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAC3, End = 0x1FAC5, Type = Extended_Pictographic });
            // E0.0  [10] (🫆..🫏)    <reserved-1FAC6>..<reserved-1FACF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAC6, End = 0x1FACF, Type = Extended_Pictographic });
            // E13.0  [7] (🫐..🫖)    blueberries..teapot
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAD0, End = 0x1FAD6, Type = Extended_Pictographic });
            // E14.0  [3] (🫗..🫙)    pouring liquid..jar
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAD7, End = 0x1FAD9, Type = Extended_Pictographic });
            // E0.0   [6] (🫚..🫟)    <reserved-1FADA>..<reserved-1FADF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FADA, End = 0x1FADF, Type = Extended_Pictographic });
            // E14.0  [8] (🫠..🫧)    melting face..bubbles
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAE0, End = 0x1FAE7, Type = Extended_Pictographic });
            // E0.0   [8] (🫨..🫯)    <reserved-1FAE8>..<reserved-1FAEF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAE8, End = 0x1FAEF, Type = Extended_Pictographic });
            // E14.0  [7] (🫰..🫶)    hand with index finger and thumb crossed..heart hands
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAF0, End = 0x1FAF6, Type = Extended_Pictographic });
            // E0.0   [9] (🫷..🫿)    <reserved-1FAF7>..<reserved-1FAFF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FAF7, End = 0x1FAFF, Type = Extended_Pictographic });
            // E0.0[1022] (🰀..🿽)    <reserved-1FC00>..<reserved-1FFFD>
            m_lst_code_range.Add(new RangeInfo() { Start = 0x1FC00, End = 0x1FFFD, Type = Extended_Pictographic });
            // Cn  [30] <reserved-E0002>..<reserved-E001F>
            m_lst_code_range.Add(new RangeInfo() { Start = 0xE0002, End = 0xE001F, Type = Control });
            // Cf  [96] TAG SPACE..CANCEL TAG
            m_lst_code_range.Add(new RangeInfo() { Start = 0xE0020, End = 0xE007F, Type = Extend });
            // Cn [128] <reserved-E0080>..<reserved-E00FF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0xE0080, End = 0xE00FF, Type = Control });
            // Mn [240] VARIATION SELECTOR-17..VARIATION SELECTOR-256
            m_lst_code_range.Add(new RangeInfo() { Start = 0xE0100, End = 0xE01EF, Type = Extend });
            // Cn [3600] <reserved-E01F0>..<reserved-E0FFF>
            m_lst_code_range.Add(new RangeInfo() { Start = 0xE01F0, End = 0xE0FFF, Type = Control });
        }

        static bool ShouldBreak(int nRightType, List<int> lstHistoryBreakType)
        {
            int nLeftType = lstHistoryBreakType[lstHistoryBreakType.Count - 1];
            // The urles from: https://www.unicode.org/reports/tr29/#Grapheme_Cluster_Boundary_Rules
            // ÷    Boundary (allow break here)
            // ×    No boundary(do not allow break here)
            // GB*  LeftChar_Property (÷ | ×) RightChar_Property

            // Break at the start and end of text, unless the text is empty.
            // GB1 	sot     ÷   Any
            // GB2 	Any     ÷   eot
            // for GB1 and GB2 not need code

            // Do not break between a CR and LF. Otherwise, break before and after controls.
            // GB3  CR × LF
            // GB4  (Control | CR | LF) ÷
            // GB5  ÷ (Control | CR | LF)
            if (nLeftType == CR && nRightType == LF) return false;
            if (nLeftType == Control || nLeftType == CR || nLeftType == LF) return true;
            if (nRightType == Control || nRightType == CR || nRightType == LF) return true;

            // Do not break Hangul syllable sequences.
            // GB6  L × (L | V | LV | LVT)
            // GB7  (LV | V) × (V | T)
            // GB8  (LVT | T) × T
            if (nLeftType == L && (nRightType == L || nRightType == V || nRightType == LV || nRightType == LVT)) return false;
            if ((nLeftType == LV || nLeftType == V) && (nRightType == V || nRightType == T)) return false;
            if ((nLeftType == LVT || nLeftType == T) && (nRightType == T)) return false;

            // Do not break before extending characters or ZWJ.
            // GB9  × (Extend | ZWJ)
            if (nRightType == Extend || nRightType == ZWJ) return false;

            // Do not break before SpacingMarks, or after Prepend characters.
            // GB9a × SpacingMark
            // GB9b Prepend ×
            if (nRightType == SpacingMark) return false;
            if (nLeftType == Prepend) return false;

            // Do not break within emoji modifier sequences or emoji zwj sequences.
            // GB11 \p{Extended_Pictographic} Extend* ZWJ × \p{Extended_Pictographic}
            if (nRightType == Extended_Pictographic)
            {
                if (lstHistoryBreakType.Count >= 2 && lstHistoryBreakType[lstHistoryBreakType.Count - 1] == ZWJ)
                {
                    for (int i = lstHistoryBreakType.Count - 2; i >= 0; i--)
                    {
                        switch (lstHistoryBreakType[i])
                        {
                            case Extend: continue;                      // Extend*
                            case Extended_Pictographic: return false;   // \p{Extended_Pictographic}
                            default: i = -1; break;
                        }
                    }
                }
            }
            // Do not break within emoji flag sequences. That is,
            // do not break between regional indicator (RI) symbols if there is an odd number of RI characters before the break point.
            // GB12  sot (RI RI)* RI     ×   RI
            // GB13  [^RI] (RI RI)* RI   ×   RI
            //if (nLeftRICount % 2 == 1 && nRightType == Regional_Indicator) return false;
            if (nRightType == Regional_Indicator)
            {
                int nLeftRICount = 0;
                for (int i = lstHistoryBreakType.Count - 1; i >= 0; i--)
                {
                    if (lstHistoryBreakType[i] != Regional_Indicator)
                    {
                        break;
                    }
                    nLeftRICount++;
                }
                if (nLeftRICount % 2 == 1) return false;
            }
            return true;
        }

        static int GetBreakProperty(int nCodePoint)
        {
            switch (nCodePoint)
            {
                case 0x006DD: return Prepend;              // Cf       ARABIC END OF AYAH
                case 0x0070F: return Prepend;              // Cf       SYRIAC ABBREVIATION MARK
                case 0x008E2: return Prepend;              // Cf       ARABIC DISPUTED END OF AYAH
                case 0x00D4E: return Prepend;              // Lo       MALAYALAM LETTER DOT REPH
                case 0x110BD: return Prepend;              // Cf       KAITHI NUMBER SIGN
                case 0x110CD: return Prepend;              // Cf       KAITHI NUMBER SIGN ABOVE
                case 0x1193F: return Prepend;              // Lo       DIVES AKURU PREFIXED NASAL SIGN
                case 0x11941: return Prepend;              // Lo       DIVES AKURU INITIAL RA
                case 0x11A3A: return Prepend;              // Lo       ZANABAZAR SQUARE CLUSTER-INITIAL LETTER RA
                case 0x11D46: return Prepend;              // Lo       MASARAM GONDI REPHA
                case 0x0000D: return CR;                   // Cc       <control-000D>
                case 0x0000A: return LF;                   // Cc       <control-000A>
                case 0x000AD: return Control;              // Cf       SOFT HYPHEN
                case 0x0061C: return Control;              // Cf       ARABIC LETTER MARK
                case 0x0180E: return Control;              // Cf       MONGOLIAN VOWEL SEPARATOR
                case 0x0200B: return Control;              // Cf       ZERO WIDTH SPACE
                case 0x02028: return Control;              // Zl       LINE SEPARATOR
                case 0x02029: return Control;              // Zp       PARAGRAPH SEPARATOR
                case 0x02065: return Control;              // Cn       <reserved-2065>
                case 0x0FEFF: return Control;              // Cf       ZERO WIDTH NO-BREAK SPACE
                case 0xE0000: return Control;              // Cn       <reserved-E0000>
                case 0xE0001: return Control;              // Cf       LANGUAGE TAG
                case 0x005BF: return Extend;               // Mn       HEBREW POINT RAFE
                case 0x005C7: return Extend;               // Mn       HEBREW POINT QAMATS QATAN
                case 0x00670: return Extend;               // Mn       ARABIC LETTER SUPERSCRIPT ALEF
                case 0x00711: return Extend;               // Mn       SYRIAC LETTER SUPERSCRIPT ALAPH
                case 0x007FD: return Extend;               // Mn       NKO DANTAYALAN
                case 0x0093A: return Extend;               // Mn       DEVANAGARI VOWEL SIGN OE
                case 0x0093C: return Extend;               // Mn       DEVANAGARI SIGN NUKTA
                case 0x0094D: return Extend;               // Mn       DEVANAGARI SIGN VIRAMA
                case 0x00981: return Extend;               // Mn       BENGALI SIGN CANDRABINDU
                case 0x009BC: return Extend;               // Mn       BENGALI SIGN NUKTA
                case 0x009BE: return Extend;               // Mc       BENGALI VOWEL SIGN AA
                case 0x009CD: return Extend;               // Mn       BENGALI SIGN VIRAMA
                case 0x009D7: return Extend;               // Mc       BENGALI AU LENGTH MARK
                case 0x009FE: return Extend;               // Mn       BENGALI SANDHI MARK
                case 0x00A3C: return Extend;               // Mn       GURMUKHI SIGN NUKTA
                case 0x00A51: return Extend;               // Mn       GURMUKHI SIGN UDAAT
                case 0x00A75: return Extend;               // Mn       GURMUKHI SIGN YAKASH
                case 0x00ABC: return Extend;               // Mn       GUJARATI SIGN NUKTA
                case 0x00ACD: return Extend;               // Mn       GUJARATI SIGN VIRAMA
                case 0x00B01: return Extend;               // Mn       ORIYA SIGN CANDRABINDU
                case 0x00B3C: return Extend;               // Mn       ORIYA SIGN NUKTA
                case 0x00B3E: return Extend;               // Mc       ORIYA VOWEL SIGN AA
                case 0x00B3F: return Extend;               // Mn       ORIYA VOWEL SIGN I
                case 0x00B4D: return Extend;               // Mn       ORIYA SIGN VIRAMA
                case 0x00B57: return Extend;               // Mc       ORIYA AU LENGTH MARK
                case 0x00B82: return Extend;               // Mn       TAMIL SIGN ANUSVARA
                case 0x00BBE: return Extend;               // Mc       TAMIL VOWEL SIGN AA
                case 0x00BC0: return Extend;               // Mn       TAMIL VOWEL SIGN II
                case 0x00BCD: return Extend;               // Mn       TAMIL SIGN VIRAMA
                case 0x00BD7: return Extend;               // Mc       TAMIL AU LENGTH MARK
                case 0x00C00: return Extend;               // Mn       TELUGU SIGN COMBINING CANDRABINDU ABOVE
                case 0x00C04: return Extend;               // Mn       TELUGU SIGN COMBINING ANUSVARA ABOVE
                case 0x00C3C: return Extend;               // Mn       TELUGU SIGN NUKTA
                case 0x00C81: return Extend;               // Mn       KANNADA SIGN CANDRABINDU
                case 0x00CBC: return Extend;               // Mn       KANNADA SIGN NUKTA
                case 0x00CBF: return Extend;               // Mn       KANNADA VOWEL SIGN I
                case 0x00CC2: return Extend;               // Mc       KANNADA VOWEL SIGN UU
                case 0x00CC6: return Extend;               // Mn       KANNADA VOWEL SIGN E
                case 0x00D3E: return Extend;               // Mc       MALAYALAM VOWEL SIGN AA
                case 0x00D4D: return Extend;               // Mn       MALAYALAM SIGN VIRAMA
                case 0x00D57: return Extend;               // Mc       MALAYALAM AU LENGTH MARK
                case 0x00D81: return Extend;               // Mn       SINHALA SIGN CANDRABINDU
                case 0x00DCA: return Extend;               // Mn       SINHALA SIGN AL-LAKUNA
                case 0x00DCF: return Extend;               // Mc       SINHALA VOWEL SIGN AELA-PILLA
                case 0x00DD6: return Extend;               // Mn       SINHALA VOWEL SIGN DIGA PAA-PILLA
                case 0x00DDF: return Extend;               // Mc       SINHALA VOWEL SIGN GAYANUKITTA
                case 0x00E31: return Extend;               // Mn       THAI CHARACTER MAI HAN-AKAT
                case 0x00EB1: return Extend;               // Mn       LAO VOWEL SIGN MAI KAN
                case 0x00F35: return Extend;               // Mn       TIBETAN MARK NGAS BZUNG NYI ZLA
                case 0x00F37: return Extend;               // Mn       TIBETAN MARK NGAS BZUNG SGOR RTAGS
                case 0x00F39: return Extend;               // Mn       TIBETAN MARK TSA -PHRU
                case 0x00FC6: return Extend;               // Mn       TIBETAN SYMBOL PADMA GDAN
                case 0x01082: return Extend;               // Mn       MYANMAR CONSONANT SIGN SHAN MEDIAL WA
                case 0x0108D: return Extend;               // Mn       MYANMAR SIGN SHAN COUNCIL EMPHATIC TONE
                case 0x0109D: return Extend;               // Mn       MYANMAR VOWEL SIGN AITON AI
                case 0x017C6: return Extend;               // Mn       KHMER SIGN NIKAHIT
                case 0x017DD: return Extend;               // Mn       KHMER SIGN ATTHACAN
                case 0x0180F: return Extend;               // Mn       MONGOLIAN FREE VARIATION SELECTOR FOUR
                case 0x018A9: return Extend;               // Mn       MONGOLIAN LETTER ALI GALI DAGALGA
                case 0x01932: return Extend;               // Mn       LIMBU SMALL LETTER ANUSVARA
                case 0x01A1B: return Extend;               // Mn       BUGINESE VOWEL SIGN AE
                case 0x01A56: return Extend;               // Mn       TAI THAM CONSONANT SIGN MEDIAL LA
                case 0x01A60: return Extend;               // Mn       TAI THAM SIGN SAKOT
                case 0x01A62: return Extend;               // Mn       TAI THAM VOWEL SIGN MAI SAT
                case 0x01A7F: return Extend;               // Mn       TAI THAM COMBINING CRYPTOGRAMMIC DOT
                case 0x01ABE: return Extend;               // Me       COMBINING PARENTHESES OVERLAY
                case 0x01B34: return Extend;               // Mn       BALINESE SIGN REREKAN
                case 0x01B35: return Extend;               // Mc       BALINESE VOWEL SIGN TEDUNG
                case 0x01B3C: return Extend;               // Mn       BALINESE VOWEL SIGN LA LENGA
                case 0x01B42: return Extend;               // Mn       BALINESE VOWEL SIGN PEPET
                case 0x01BE6: return Extend;               // Mn       BATAK SIGN TOMPI
                case 0x01BED: return Extend;               // Mn       BATAK VOWEL SIGN KARO O
                case 0x01CED: return Extend;               // Mn       VEDIC SIGN TIRYAK
                case 0x01CF4: return Extend;               // Mn       VEDIC TONE CANDRA ABOVE
                case 0x0200C: return Extend;               // Cf       ZERO WIDTH NON-JOINER
                case 0x020E1: return Extend;               // Mn       COMBINING LEFT RIGHT ARROW ABOVE
                case 0x02D7F: return Extend;               // Mn       TIFINAGH CONSONANT JOINER
                case 0x0A66F: return Extend;               // Mn       COMBINING CYRILLIC VZMET
                case 0x0A802: return Extend;               // Mn       SYLOTI NAGRI SIGN DVISVARA
                case 0x0A806: return Extend;               // Mn       SYLOTI NAGRI SIGN HASANTA
                case 0x0A80B: return Extend;               // Mn       SYLOTI NAGRI SIGN ANUSVARA
                case 0x0A82C: return Extend;               // Mn       SYLOTI NAGRI SIGN ALTERNATE HASANTA
                case 0x0A8FF: return Extend;               // Mn       DEVANAGARI VOWEL SIGN AY
                case 0x0A9B3: return Extend;               // Mn       JAVANESE SIGN CECAK TELU
                case 0x0A9E5: return Extend;               // Mn       MYANMAR SIGN SHAN SAW
                case 0x0AA43: return Extend;               // Mn       CHAM CONSONANT SIGN FINAL NG
                case 0x0AA4C: return Extend;               // Mn       CHAM CONSONANT SIGN FINAL M
                case 0x0AA7C: return Extend;               // Mn       MYANMAR SIGN TAI LAING TONE-2
                case 0x0AAB0: return Extend;               // Mn       TAI VIET MAI KANG
                case 0x0AAC1: return Extend;               // Mn       TAI VIET TONE MAI THO
                case 0x0AAF6: return Extend;               // Mn       MEETEI MAYEK VIRAMA
                case 0x0ABE5: return Extend;               // Mn       MEETEI MAYEK VOWEL SIGN ANAP
                case 0x0ABE8: return Extend;               // Mn       MEETEI MAYEK VOWEL SIGN UNAP
                case 0x0ABED: return Extend;               // Mn       MEETEI MAYEK APUN IYEK
                case 0x0FB1E: return Extend;               // Mn       HEBREW POINT JUDEO-SPANISH VARIKA
                case 0x101FD: return Extend;               // Mn       PHAISTOS DISC SIGN COMBINING OBLIQUE STROKE
                case 0x102E0: return Extend;               // Mn       COPTIC EPACT THOUSANDS MARK
                case 0x10A3F: return Extend;               // Mn       KHAROSHTHI VIRAMA
                case 0x11001: return Extend;               // Mn       BRAHMI SIGN ANUSVARA
                case 0x11070: return Extend;               // Mn       BRAHMI SIGN OLD TAMIL VIRAMA
                case 0x110C2: return Extend;               // Mn       KAITHI VOWEL SIGN VOCALIC R
                case 0x11173: return Extend;               // Mn       MAHAJANI SIGN NUKTA
                case 0x111CF: return Extend;               // Mn       SHARADA SIGN INVERTED CANDRABINDU
                case 0x11234: return Extend;               // Mn       KHOJKI SIGN ANUSVARA
                case 0x1123E: return Extend;               // Mn       KHOJKI SIGN SUKUN
                case 0x112DF: return Extend;               // Mn       KHUDAWADI SIGN ANUSVARA
                case 0x1133E: return Extend;               // Mc       GRANTHA VOWEL SIGN AA
                case 0x11340: return Extend;               // Mn       GRANTHA VOWEL SIGN II
                case 0x11357: return Extend;               // Mc       GRANTHA AU LENGTH MARK
                case 0x11446: return Extend;               // Mn       NEWA SIGN NUKTA
                case 0x1145E: return Extend;               // Mn       NEWA SANDHI MARK
                case 0x114B0: return Extend;               // Mc       TIRHUTA VOWEL SIGN AA
                case 0x114BA: return Extend;               // Mn       TIRHUTA VOWEL SIGN SHORT E
                case 0x114BD: return Extend;               // Mc       TIRHUTA VOWEL SIGN SHORT O
                case 0x115AF: return Extend;               // Mc       SIDDHAM VOWEL SIGN AA
                case 0x1163D: return Extend;               // Mn       MODI SIGN ANUSVARA
                case 0x116AB: return Extend;               // Mn       TAKRI SIGN ANUSVARA
                case 0x116AD: return Extend;               // Mn       TAKRI VOWEL SIGN AA
                case 0x116B7: return Extend;               // Mn       TAKRI SIGN NUKTA
                case 0x11930: return Extend;               // Mc       DIVES AKURU VOWEL SIGN AA
                case 0x1193E: return Extend;               // Mn       DIVES AKURU VIRAMA
                case 0x11943: return Extend;               // Mn       DIVES AKURU SIGN NUKTA
                case 0x119E0: return Extend;               // Mn       NANDINAGARI SIGN VIRAMA
                case 0x11A47: return Extend;               // Mn       ZANABAZAR SQUARE SUBJOINER
                case 0x11C3F: return Extend;               // Mn       BHAIKSUKI SIGN VIRAMA
                case 0x11D3A: return Extend;               // Mn       MASARAM GONDI VOWEL SIGN E
                case 0x11D47: return Extend;               // Mn       MASARAM GONDI RA-KARA
                case 0x11D95: return Extend;               // Mn       GUNJALA GONDI SIGN ANUSVARA
                case 0x11D97: return Extend;               // Mn       GUNJALA GONDI VIRAMA
                case 0x16F4F: return Extend;               // Mn       MIAO SIGN CONSONANT MODIFIER BAR
                case 0x16FE4: return Extend;               // Mn       KHITAN SMALL SCRIPT FILLER
                case 0x1D165: return Extend;               // Mc       MUSICAL SYMBOL COMBINING STEM
                case 0x1DA75: return Extend;               // Mn       SIGNWRITING UPPER BODY TILTING FROM HIP JOINTS
                case 0x1DA84: return Extend;               // Mn       SIGNWRITING LOCATION HEAD NECK
                case 0x1E2AE: return Extend;               // Mn       TOTO SIGN RISING TONE
                case 0x00903: return SpacingMark;          // Mc       DEVANAGARI SIGN VISARGA
                case 0x0093B: return SpacingMark;          // Mc       DEVANAGARI VOWEL SIGN OOE
                case 0x00A03: return SpacingMark;          // Mc       GURMUKHI SIGN VISARGA
                case 0x00A83: return SpacingMark;          // Mc       GUJARATI SIGN VISARGA
                case 0x00AC9: return SpacingMark;          // Mc       GUJARATI VOWEL SIGN CANDRA O
                case 0x00B40: return SpacingMark;          // Mc       ORIYA VOWEL SIGN II
                case 0x00BBF: return SpacingMark;          // Mc       TAMIL VOWEL SIGN I
                case 0x00CBE: return SpacingMark;          // Mc       KANNADA VOWEL SIGN AA
                case 0x00E33: return SpacingMark;          // Lo       THAI CHARACTER SARA AM
                case 0x00EB3: return SpacingMark;          // Lo       LAO VOWEL SIGN AM
                case 0x00F7F: return SpacingMark;          // Mc       TIBETAN SIGN RNAM BCAD
                case 0x01031: return SpacingMark;          // Mc       MYANMAR VOWEL SIGN E
                case 0x01084: return SpacingMark;          // Mc       MYANMAR VOWEL SIGN SHAN E
                case 0x01715: return SpacingMark;          // Mc       TAGALOG SIGN PAMUDPOD
                case 0x01734: return SpacingMark;          // Mc       HANUNOO SIGN PAMUDPOD
                case 0x017B6: return SpacingMark;          // Mc       KHMER VOWEL SIGN AA
                case 0x01A55: return SpacingMark;          // Mc       TAI THAM CONSONANT SIGN MEDIAL RA
                case 0x01A57: return SpacingMark;          // Mc       TAI THAM CONSONANT SIGN LA TANG LAI
                case 0x01B04: return SpacingMark;          // Mc       BALINESE SIGN BISAH
                case 0x01B3B: return SpacingMark;          // Mc       BALINESE VOWEL SIGN RA REPA TEDUNG
                case 0x01B82: return SpacingMark;          // Mc       SUNDANESE SIGN PANGWISAD
                case 0x01BA1: return SpacingMark;          // Mc       SUNDANESE CONSONANT SIGN PAMINGKAL
                case 0x01BAA: return SpacingMark;          // Mc       SUNDANESE SIGN PAMAAEH
                case 0x01BE7: return SpacingMark;          // Mc       BATAK VOWEL SIGN E
                case 0x01BEE: return SpacingMark;          // Mc       BATAK VOWEL SIGN U
                case 0x01CE1: return SpacingMark;          // Mc       VEDIC TONE ATHARVAVEDIC INDEPENDENT SVARITA
                case 0x01CF7: return SpacingMark;          // Mc       VEDIC SIGN ATIKRAMA
                case 0x0A827: return SpacingMark;          // Mc       SYLOTI NAGRI VOWEL SIGN OO
                case 0x0A983: return SpacingMark;          // Mc       JAVANESE SIGN WIGNYAN
                case 0x0AA4D: return SpacingMark;          // Mc       CHAM CONSONANT SIGN FINAL H
                case 0x0AAEB: return SpacingMark;          // Mc       MEETEI MAYEK VOWEL SIGN II
                case 0x0AAF5: return SpacingMark;          // Mc       MEETEI MAYEK VOWEL SIGN VISARGA
                case 0x0ABEC: return SpacingMark;          // Mc       MEETEI MAYEK LUM IYEK
                case 0x11000: return SpacingMark;          // Mc       BRAHMI SIGN CANDRABINDU
                case 0x11002: return SpacingMark;          // Mc       BRAHMI SIGN VISARGA
                case 0x11082: return SpacingMark;          // Mc       KAITHI SIGN VISARGA
                case 0x1112C: return SpacingMark;          // Mc       CHAKMA VOWEL SIGN E
                case 0x11182: return SpacingMark;          // Mc       SHARADA SIGN VISARGA
                case 0x111CE: return SpacingMark;          // Mc       SHARADA VOWEL SIGN PRISHTHAMATRA E
                case 0x11235: return SpacingMark;          // Mc       KHOJKI SIGN VIRAMA
                case 0x1133F: return SpacingMark;          // Mc       GRANTHA VOWEL SIGN I
                case 0x11445: return SpacingMark;          // Mc       NEWA SIGN VISARGA
                case 0x114B9: return SpacingMark;          // Mc       TIRHUTA VOWEL SIGN E
                case 0x114BE: return SpacingMark;          // Mc       TIRHUTA VOWEL SIGN AU
                case 0x114C1: return SpacingMark;          // Mc       TIRHUTA SIGN VISARGA
                case 0x115BE: return SpacingMark;          // Mc       SIDDHAM SIGN VISARGA
                case 0x1163E: return SpacingMark;          // Mc       MODI SIGN VISARGA
                case 0x116AC: return SpacingMark;          // Mc       TAKRI SIGN VISARGA
                case 0x116B6: return SpacingMark;          // Mc       TAKRI SIGN VIRAMA
                case 0x11726: return SpacingMark;          // Mc       AHOM VOWEL SIGN E
                case 0x11838: return SpacingMark;          // Mc       DOGRA SIGN VISARGA
                case 0x1193D: return SpacingMark;          // Mc       DIVES AKURU SIGN HALANTA
                case 0x11940: return SpacingMark;          // Mc       DIVES AKURU MEDIAL YA
                case 0x11942: return SpacingMark;          // Mc       DIVES AKURU MEDIAL RA
                case 0x119E4: return SpacingMark;          // Mc       NANDINAGARI VOWEL SIGN PRISHTHAMATRA E
                case 0x11A39: return SpacingMark;          // Mc       ZANABAZAR SQUARE SIGN VISARGA
                case 0x11A97: return SpacingMark;          // Mc       SOYOMBO SIGN VISARGA
                case 0x11C2F: return SpacingMark;          // Mc       BHAIKSUKI VOWEL SIGN AA
                case 0x11C3E: return SpacingMark;          // Mc       BHAIKSUKI SIGN VISARGA
                case 0x11CA9: return SpacingMark;          // Mc       MARCHEN SUBJOINED LETTER YA
                case 0x11CB1: return SpacingMark;          // Mc       MARCHEN VOWEL SIGN I
                case 0x11CB4: return SpacingMark;          // Mc       MARCHEN VOWEL SIGN O
                case 0x11D96: return SpacingMark;          // Mc       GUNJALA GONDI SIGN VISARGA
                case 0x1D166: return SpacingMark;          // Mc       MUSICAL SYMBOL COMBINING SPRECHGESANG STEM
                case 0x1D16D: return SpacingMark;          // Mc       MUSICAL SYMBOL COMBINING AUGMENTATION DOT
                case 0x0AC00: return LV;                   // Lo       HANGUL SYLLABLE GA
                case 0x0AC1C: return LV;                   // Lo       HANGUL SYLLABLE GAE
                case 0x0AC38: return LV;                   // Lo       HANGUL SYLLABLE GYA
                case 0x0AC54: return LV;                   // Lo       HANGUL SYLLABLE GYAE
                case 0x0AC70: return LV;                   // Lo       HANGUL SYLLABLE GEO
                case 0x0AC8C: return LV;                   // Lo       HANGUL SYLLABLE GE
                case 0x0ACA8: return LV;                   // Lo       HANGUL SYLLABLE GYEO
                case 0x0ACC4: return LV;                   // Lo       HANGUL SYLLABLE GYE
                case 0x0ACE0: return LV;                   // Lo       HANGUL SYLLABLE GO
                case 0x0ACFC: return LV;                   // Lo       HANGUL SYLLABLE GWA
                case 0x0AD18: return LV;                   // Lo       HANGUL SYLLABLE GWAE
                case 0x0AD34: return LV;                   // Lo       HANGUL SYLLABLE GOE
                case 0x0AD50: return LV;                   // Lo       HANGUL SYLLABLE GYO
                case 0x0AD6C: return LV;                   // Lo       HANGUL SYLLABLE GU
                case 0x0AD88: return LV;                   // Lo       HANGUL SYLLABLE GWEO
                case 0x0ADA4: return LV;                   // Lo       HANGUL SYLLABLE GWE
                case 0x0ADC0: return LV;                   // Lo       HANGUL SYLLABLE GWI
                case 0x0ADDC: return LV;                   // Lo       HANGUL SYLLABLE GYU
                case 0x0ADF8: return LV;                   // Lo       HANGUL SYLLABLE GEU
                case 0x0AE14: return LV;                   // Lo       HANGUL SYLLABLE GYI
                case 0x0AE30: return LV;                   // Lo       HANGUL SYLLABLE GI
                case 0x0AE4C: return LV;                   // Lo       HANGUL SYLLABLE GGA
                case 0x0AE68: return LV;                   // Lo       HANGUL SYLLABLE GGAE
                case 0x0AE84: return LV;                   // Lo       HANGUL SYLLABLE GGYA
                case 0x0AEA0: return LV;                   // Lo       HANGUL SYLLABLE GGYAE
                case 0x0AEBC: return LV;                   // Lo       HANGUL SYLLABLE GGEO
                case 0x0AED8: return LV;                   // Lo       HANGUL SYLLABLE GGE
                case 0x0AEF4: return LV;                   // Lo       HANGUL SYLLABLE GGYEO
                case 0x0AF10: return LV;                   // Lo       HANGUL SYLLABLE GGYE
                case 0x0AF2C: return LV;                   // Lo       HANGUL SYLLABLE GGO
                case 0x0AF48: return LV;                   // Lo       HANGUL SYLLABLE GGWA
                case 0x0AF64: return LV;                   // Lo       HANGUL SYLLABLE GGWAE
                case 0x0AF80: return LV;                   // Lo       HANGUL SYLLABLE GGOE
                case 0x0AF9C: return LV;                   // Lo       HANGUL SYLLABLE GGYO
                case 0x0AFB8: return LV;                   // Lo       HANGUL SYLLABLE GGU
                case 0x0AFD4: return LV;                   // Lo       HANGUL SYLLABLE GGWEO
                case 0x0AFF0: return LV;                   // Lo       HANGUL SYLLABLE GGWE
                case 0x0B00C: return LV;                   // Lo       HANGUL SYLLABLE GGWI
                case 0x0B028: return LV;                   // Lo       HANGUL SYLLABLE GGYU
                case 0x0B044: return LV;                   // Lo       HANGUL SYLLABLE GGEU
                case 0x0B060: return LV;                   // Lo       HANGUL SYLLABLE GGYI
                case 0x0B07C: return LV;                   // Lo       HANGUL SYLLABLE GGI
                case 0x0B098: return LV;                   // Lo       HANGUL SYLLABLE NA
                case 0x0B0B4: return LV;                   // Lo       HANGUL SYLLABLE NAE
                case 0x0B0D0: return LV;                   // Lo       HANGUL SYLLABLE NYA
                case 0x0B0EC: return LV;                   // Lo       HANGUL SYLLABLE NYAE
                case 0x0B108: return LV;                   // Lo       HANGUL SYLLABLE NEO
                case 0x0B124: return LV;                   // Lo       HANGUL SYLLABLE NE
                case 0x0B140: return LV;                   // Lo       HANGUL SYLLABLE NYEO
                case 0x0B15C: return LV;                   // Lo       HANGUL SYLLABLE NYE
                case 0x0B178: return LV;                   // Lo       HANGUL SYLLABLE NO
                case 0x0B194: return LV;                   // Lo       HANGUL SYLLABLE NWA
                case 0x0B1B0: return LV;                   // Lo       HANGUL SYLLABLE NWAE
                case 0x0B1CC: return LV;                   // Lo       HANGUL SYLLABLE NOE
                case 0x0B1E8: return LV;                   // Lo       HANGUL SYLLABLE NYO
                case 0x0B204: return LV;                   // Lo       HANGUL SYLLABLE NU
                case 0x0B220: return LV;                   // Lo       HANGUL SYLLABLE NWEO
                case 0x0B23C: return LV;                   // Lo       HANGUL SYLLABLE NWE
                case 0x0B258: return LV;                   // Lo       HANGUL SYLLABLE NWI
                case 0x0B274: return LV;                   // Lo       HANGUL SYLLABLE NYU
                case 0x0B290: return LV;                   // Lo       HANGUL SYLLABLE NEU
                case 0x0B2AC: return LV;                   // Lo       HANGUL SYLLABLE NYI
                case 0x0B2C8: return LV;                   // Lo       HANGUL SYLLABLE NI
                case 0x0B2E4: return LV;                   // Lo       HANGUL SYLLABLE DA
                case 0x0B300: return LV;                   // Lo       HANGUL SYLLABLE DAE
                case 0x0B31C: return LV;                   // Lo       HANGUL SYLLABLE DYA
                case 0x0B338: return LV;                   // Lo       HANGUL SYLLABLE DYAE
                case 0x0B354: return LV;                   // Lo       HANGUL SYLLABLE DEO
                case 0x0B370: return LV;                   // Lo       HANGUL SYLLABLE DE
                case 0x0B38C: return LV;                   // Lo       HANGUL SYLLABLE DYEO
                case 0x0B3A8: return LV;                   // Lo       HANGUL SYLLABLE DYE
                case 0x0B3C4: return LV;                   // Lo       HANGUL SYLLABLE DO
                case 0x0B3E0: return LV;                   // Lo       HANGUL SYLLABLE DWA
                case 0x0B3FC: return LV;                   // Lo       HANGUL SYLLABLE DWAE
                case 0x0B418: return LV;                   // Lo       HANGUL SYLLABLE DOE
                case 0x0B434: return LV;                   // Lo       HANGUL SYLLABLE DYO
                case 0x0B450: return LV;                   // Lo       HANGUL SYLLABLE DU
                case 0x0B46C: return LV;                   // Lo       HANGUL SYLLABLE DWEO
                case 0x0B488: return LV;                   // Lo       HANGUL SYLLABLE DWE
                case 0x0B4A4: return LV;                   // Lo       HANGUL SYLLABLE DWI
                case 0x0B4C0: return LV;                   // Lo       HANGUL SYLLABLE DYU
                case 0x0B4DC: return LV;                   // Lo       HANGUL SYLLABLE DEU
                case 0x0B4F8: return LV;                   // Lo       HANGUL SYLLABLE DYI
                case 0x0B514: return LV;                   // Lo       HANGUL SYLLABLE DI
                case 0x0B530: return LV;                   // Lo       HANGUL SYLLABLE DDA
                case 0x0B54C: return LV;                   // Lo       HANGUL SYLLABLE DDAE
                case 0x0B568: return LV;                   // Lo       HANGUL SYLLABLE DDYA
                case 0x0B584: return LV;                   // Lo       HANGUL SYLLABLE DDYAE
                case 0x0B5A0: return LV;                   // Lo       HANGUL SYLLABLE DDEO
                case 0x0B5BC: return LV;                   // Lo       HANGUL SYLLABLE DDE
                case 0x0B5D8: return LV;                   // Lo       HANGUL SYLLABLE DDYEO
                case 0x0B5F4: return LV;                   // Lo       HANGUL SYLLABLE DDYE
                case 0x0B610: return LV;                   // Lo       HANGUL SYLLABLE DDO
                case 0x0B62C: return LV;                   // Lo       HANGUL SYLLABLE DDWA
                case 0x0B648: return LV;                   // Lo       HANGUL SYLLABLE DDWAE
                case 0x0B664: return LV;                   // Lo       HANGUL SYLLABLE DDOE
                case 0x0B680: return LV;                   // Lo       HANGUL SYLLABLE DDYO
                case 0x0B69C: return LV;                   // Lo       HANGUL SYLLABLE DDU
                case 0x0B6B8: return LV;                   // Lo       HANGUL SYLLABLE DDWEO
                case 0x0B6D4: return LV;                   // Lo       HANGUL SYLLABLE DDWE
                case 0x0B6F0: return LV;                   // Lo       HANGUL SYLLABLE DDWI
                case 0x0B70C: return LV;                   // Lo       HANGUL SYLLABLE DDYU
                case 0x0B728: return LV;                   // Lo       HANGUL SYLLABLE DDEU
                case 0x0B744: return LV;                   // Lo       HANGUL SYLLABLE DDYI
                case 0x0B760: return LV;                   // Lo       HANGUL SYLLABLE DDI
                case 0x0B77C: return LV;                   // Lo       HANGUL SYLLABLE RA
                case 0x0B798: return LV;                   // Lo       HANGUL SYLLABLE RAE
                case 0x0B7B4: return LV;                   // Lo       HANGUL SYLLABLE RYA
                case 0x0B7D0: return LV;                   // Lo       HANGUL SYLLABLE RYAE
                case 0x0B7EC: return LV;                   // Lo       HANGUL SYLLABLE REO
                case 0x0B808: return LV;                   // Lo       HANGUL SYLLABLE RE
                case 0x0B824: return LV;                   // Lo       HANGUL SYLLABLE RYEO
                case 0x0B840: return LV;                   // Lo       HANGUL SYLLABLE RYE
                case 0x0B85C: return LV;                   // Lo       HANGUL SYLLABLE RO
                case 0x0B878: return LV;                   // Lo       HANGUL SYLLABLE RWA
                case 0x0B894: return LV;                   // Lo       HANGUL SYLLABLE RWAE
                case 0x0B8B0: return LV;                   // Lo       HANGUL SYLLABLE ROE
                case 0x0B8CC: return LV;                   // Lo       HANGUL SYLLABLE RYO
                case 0x0B8E8: return LV;                   // Lo       HANGUL SYLLABLE RU
                case 0x0B904: return LV;                   // Lo       HANGUL SYLLABLE RWEO
                case 0x0B920: return LV;                   // Lo       HANGUL SYLLABLE RWE
                case 0x0B93C: return LV;                   // Lo       HANGUL SYLLABLE RWI
                case 0x0B958: return LV;                   // Lo       HANGUL SYLLABLE RYU
                case 0x0B974: return LV;                   // Lo       HANGUL SYLLABLE REU
                case 0x0B990: return LV;                   // Lo       HANGUL SYLLABLE RYI
                case 0x0B9AC: return LV;                   // Lo       HANGUL SYLLABLE RI
                case 0x0B9C8: return LV;                   // Lo       HANGUL SYLLABLE MA
                case 0x0B9E4: return LV;                   // Lo       HANGUL SYLLABLE MAE
                case 0x0BA00: return LV;                   // Lo       HANGUL SYLLABLE MYA
                case 0x0BA1C: return LV;                   // Lo       HANGUL SYLLABLE MYAE
                case 0x0BA38: return LV;                   // Lo       HANGUL SYLLABLE MEO
                case 0x0BA54: return LV;                   // Lo       HANGUL SYLLABLE ME
                case 0x0BA70: return LV;                   // Lo       HANGUL SYLLABLE MYEO
                case 0x0BA8C: return LV;                   // Lo       HANGUL SYLLABLE MYE
                case 0x0BAA8: return LV;                   // Lo       HANGUL SYLLABLE MO
                case 0x0BAC4: return LV;                   // Lo       HANGUL SYLLABLE MWA
                case 0x0BAE0: return LV;                   // Lo       HANGUL SYLLABLE MWAE
                case 0x0BAFC: return LV;                   // Lo       HANGUL SYLLABLE MOE
                case 0x0BB18: return LV;                   // Lo       HANGUL SYLLABLE MYO
                case 0x0BB34: return LV;                   // Lo       HANGUL SYLLABLE MU
                case 0x0BB50: return LV;                   // Lo       HANGUL SYLLABLE MWEO
                case 0x0BB6C: return LV;                   // Lo       HANGUL SYLLABLE MWE
                case 0x0BB88: return LV;                   // Lo       HANGUL SYLLABLE MWI
                case 0x0BBA4: return LV;                   // Lo       HANGUL SYLLABLE MYU
                case 0x0BBC0: return LV;                   // Lo       HANGUL SYLLABLE MEU
                case 0x0BBDC: return LV;                   // Lo       HANGUL SYLLABLE MYI
                case 0x0BBF8: return LV;                   // Lo       HANGUL SYLLABLE MI
                case 0x0BC14: return LV;                   // Lo       HANGUL SYLLABLE BA
                case 0x0BC30: return LV;                   // Lo       HANGUL SYLLABLE BAE
                case 0x0BC4C: return LV;                   // Lo       HANGUL SYLLABLE BYA
                case 0x0BC68: return LV;                   // Lo       HANGUL SYLLABLE BYAE
                case 0x0BC84: return LV;                   // Lo       HANGUL SYLLABLE BEO
                case 0x0BCA0: return LV;                   // Lo       HANGUL SYLLABLE BE
                case 0x0BCBC: return LV;                   // Lo       HANGUL SYLLABLE BYEO
                case 0x0BCD8: return LV;                   // Lo       HANGUL SYLLABLE BYE
                case 0x0BCF4: return LV;                   // Lo       HANGUL SYLLABLE BO
                case 0x0BD10: return LV;                   // Lo       HANGUL SYLLABLE BWA
                case 0x0BD2C: return LV;                   // Lo       HANGUL SYLLABLE BWAE
                case 0x0BD48: return LV;                   // Lo       HANGUL SYLLABLE BOE
                case 0x0BD64: return LV;                   // Lo       HANGUL SYLLABLE BYO
                case 0x0BD80: return LV;                   // Lo       HANGUL SYLLABLE BU
                case 0x0BD9C: return LV;                   // Lo       HANGUL SYLLABLE BWEO
                case 0x0BDB8: return LV;                   // Lo       HANGUL SYLLABLE BWE
                case 0x0BDD4: return LV;                   // Lo       HANGUL SYLLABLE BWI
                case 0x0BDF0: return LV;                   // Lo       HANGUL SYLLABLE BYU
                case 0x0BE0C: return LV;                   // Lo       HANGUL SYLLABLE BEU
                case 0x0BE28: return LV;                   // Lo       HANGUL SYLLABLE BYI
                case 0x0BE44: return LV;                   // Lo       HANGUL SYLLABLE BI
                case 0x0BE60: return LV;                   // Lo       HANGUL SYLLABLE BBA
                case 0x0BE7C: return LV;                   // Lo       HANGUL SYLLABLE BBAE
                case 0x0BE98: return LV;                   // Lo       HANGUL SYLLABLE BBYA
                case 0x0BEB4: return LV;                   // Lo       HANGUL SYLLABLE BBYAE
                case 0x0BED0: return LV;                   // Lo       HANGUL SYLLABLE BBEO
                case 0x0BEEC: return LV;                   // Lo       HANGUL SYLLABLE BBE
                case 0x0BF08: return LV;                   // Lo       HANGUL SYLLABLE BBYEO
                case 0x0BF24: return LV;                   // Lo       HANGUL SYLLABLE BBYE
                case 0x0BF40: return LV;                   // Lo       HANGUL SYLLABLE BBO
                case 0x0BF5C: return LV;                   // Lo       HANGUL SYLLABLE BBWA
                case 0x0BF78: return LV;                   // Lo       HANGUL SYLLABLE BBWAE
                case 0x0BF94: return LV;                   // Lo       HANGUL SYLLABLE BBOE
                case 0x0BFB0: return LV;                   // Lo       HANGUL SYLLABLE BBYO
                case 0x0BFCC: return LV;                   // Lo       HANGUL SYLLABLE BBU
                case 0x0BFE8: return LV;                   // Lo       HANGUL SYLLABLE BBWEO
                case 0x0C004: return LV;                   // Lo       HANGUL SYLLABLE BBWE
                case 0x0C020: return LV;                   // Lo       HANGUL SYLLABLE BBWI
                case 0x0C03C: return LV;                   // Lo       HANGUL SYLLABLE BBYU
                case 0x0C058: return LV;                   // Lo       HANGUL SYLLABLE BBEU
                case 0x0C074: return LV;                   // Lo       HANGUL SYLLABLE BBYI
                case 0x0C090: return LV;                   // Lo       HANGUL SYLLABLE BBI
                case 0x0C0AC: return LV;                   // Lo       HANGUL SYLLABLE SA
                case 0x0C0C8: return LV;                   // Lo       HANGUL SYLLABLE SAE
                case 0x0C0E4: return LV;                   // Lo       HANGUL SYLLABLE SYA
                case 0x0C100: return LV;                   // Lo       HANGUL SYLLABLE SYAE
                case 0x0C11C: return LV;                   // Lo       HANGUL SYLLABLE SEO
                case 0x0C138: return LV;                   // Lo       HANGUL SYLLABLE SE
                case 0x0C154: return LV;                   // Lo       HANGUL SYLLABLE SYEO
                case 0x0C170: return LV;                   // Lo       HANGUL SYLLABLE SYE
                case 0x0C18C: return LV;                   // Lo       HANGUL SYLLABLE SO
                case 0x0C1A8: return LV;                   // Lo       HANGUL SYLLABLE SWA
                case 0x0C1C4: return LV;                   // Lo       HANGUL SYLLABLE SWAE
                case 0x0C1E0: return LV;                   // Lo       HANGUL SYLLABLE SOE
                case 0x0C1FC: return LV;                   // Lo       HANGUL SYLLABLE SYO
                case 0x0C218: return LV;                   // Lo       HANGUL SYLLABLE SU
                case 0x0C234: return LV;                   // Lo       HANGUL SYLLABLE SWEO
                case 0x0C250: return LV;                   // Lo       HANGUL SYLLABLE SWE
                case 0x0C26C: return LV;                   // Lo       HANGUL SYLLABLE SWI
                case 0x0C288: return LV;                   // Lo       HANGUL SYLLABLE SYU
                case 0x0C2A4: return LV;                   // Lo       HANGUL SYLLABLE SEU
                case 0x0C2C0: return LV;                   // Lo       HANGUL SYLLABLE SYI
                case 0x0C2DC: return LV;                   // Lo       HANGUL SYLLABLE SI
                case 0x0C2F8: return LV;                   // Lo       HANGUL SYLLABLE SSA
                case 0x0C314: return LV;                   // Lo       HANGUL SYLLABLE SSAE
                case 0x0C330: return LV;                   // Lo       HANGUL SYLLABLE SSYA
                case 0x0C34C: return LV;                   // Lo       HANGUL SYLLABLE SSYAE
                case 0x0C368: return LV;                   // Lo       HANGUL SYLLABLE SSEO
                case 0x0C384: return LV;                   // Lo       HANGUL SYLLABLE SSE
                case 0x0C3A0: return LV;                   // Lo       HANGUL SYLLABLE SSYEO
                case 0x0C3BC: return LV;                   // Lo       HANGUL SYLLABLE SSYE
                case 0x0C3D8: return LV;                   // Lo       HANGUL SYLLABLE SSO
                case 0x0C3F4: return LV;                   // Lo       HANGUL SYLLABLE SSWA
                case 0x0C410: return LV;                   // Lo       HANGUL SYLLABLE SSWAE
                case 0x0C42C: return LV;                   // Lo       HANGUL SYLLABLE SSOE
                case 0x0C448: return LV;                   // Lo       HANGUL SYLLABLE SSYO
                case 0x0C464: return LV;                   // Lo       HANGUL SYLLABLE SSU
                case 0x0C480: return LV;                   // Lo       HANGUL SYLLABLE SSWEO
                case 0x0C49C: return LV;                   // Lo       HANGUL SYLLABLE SSWE
                case 0x0C4B8: return LV;                   // Lo       HANGUL SYLLABLE SSWI
                case 0x0C4D4: return LV;                   // Lo       HANGUL SYLLABLE SSYU
                case 0x0C4F0: return LV;                   // Lo       HANGUL SYLLABLE SSEU
                case 0x0C50C: return LV;                   // Lo       HANGUL SYLLABLE SSYI
                case 0x0C528: return LV;                   // Lo       HANGUL SYLLABLE SSI
                case 0x0C544: return LV;                   // Lo       HANGUL SYLLABLE A
                case 0x0C560: return LV;                   // Lo       HANGUL SYLLABLE AE
                case 0x0C57C: return LV;                   // Lo       HANGUL SYLLABLE YA
                case 0x0C598: return LV;                   // Lo       HANGUL SYLLABLE YAE
                case 0x0C5B4: return LV;                   // Lo       HANGUL SYLLABLE EO
                case 0x0C5D0: return LV;                   // Lo       HANGUL SYLLABLE E
                case 0x0C5EC: return LV;                   // Lo       HANGUL SYLLABLE YEO
                case 0x0C608: return LV;                   // Lo       HANGUL SYLLABLE YE
                case 0x0C624: return LV;                   // Lo       HANGUL SYLLABLE O
                case 0x0C640: return LV;                   // Lo       HANGUL SYLLABLE WA
                case 0x0C65C: return LV;                   // Lo       HANGUL SYLLABLE WAE
                case 0x0C678: return LV;                   // Lo       HANGUL SYLLABLE OE
                case 0x0C694: return LV;                   // Lo       HANGUL SYLLABLE YO
                case 0x0C6B0: return LV;                   // Lo       HANGUL SYLLABLE U
                case 0x0C6CC: return LV;                   // Lo       HANGUL SYLLABLE WEO
                case 0x0C6E8: return LV;                   // Lo       HANGUL SYLLABLE WE
                case 0x0C704: return LV;                   // Lo       HANGUL SYLLABLE WI
                case 0x0C720: return LV;                   // Lo       HANGUL SYLLABLE YU
                case 0x0C73C: return LV;                   // Lo       HANGUL SYLLABLE EU
                case 0x0C758: return LV;                   // Lo       HANGUL SYLLABLE YI
                case 0x0C774: return LV;                   // Lo       HANGUL SYLLABLE I
                case 0x0C790: return LV;                   // Lo       HANGUL SYLLABLE JA
                case 0x0C7AC: return LV;                   // Lo       HANGUL SYLLABLE JAE
                case 0x0C7C8: return LV;                   // Lo       HANGUL SYLLABLE JYA
                case 0x0C7E4: return LV;                   // Lo       HANGUL SYLLABLE JYAE
                case 0x0C800: return LV;                   // Lo       HANGUL SYLLABLE JEO
                case 0x0C81C: return LV;                   // Lo       HANGUL SYLLABLE JE
                case 0x0C838: return LV;                   // Lo       HANGUL SYLLABLE JYEO
                case 0x0C854: return LV;                   // Lo       HANGUL SYLLABLE JYE
                case 0x0C870: return LV;                   // Lo       HANGUL SYLLABLE JO
                case 0x0C88C: return LV;                   // Lo       HANGUL SYLLABLE JWA
                case 0x0C8A8: return LV;                   // Lo       HANGUL SYLLABLE JWAE
                case 0x0C8C4: return LV;                   // Lo       HANGUL SYLLABLE JOE
                case 0x0C8E0: return LV;                   // Lo       HANGUL SYLLABLE JYO
                case 0x0C8FC: return LV;                   // Lo       HANGUL SYLLABLE JU
                case 0x0C918: return LV;                   // Lo       HANGUL SYLLABLE JWEO
                case 0x0C934: return LV;                   // Lo       HANGUL SYLLABLE JWE
                case 0x0C950: return LV;                   // Lo       HANGUL SYLLABLE JWI
                case 0x0C96C: return LV;                   // Lo       HANGUL SYLLABLE JYU
                case 0x0C988: return LV;                   // Lo       HANGUL SYLLABLE JEU
                case 0x0C9A4: return LV;                   // Lo       HANGUL SYLLABLE JYI
                case 0x0C9C0: return LV;                   // Lo       HANGUL SYLLABLE JI
                case 0x0C9DC: return LV;                   // Lo       HANGUL SYLLABLE JJA
                case 0x0C9F8: return LV;                   // Lo       HANGUL SYLLABLE JJAE
                case 0x0CA14: return LV;                   // Lo       HANGUL SYLLABLE JJYA
                case 0x0CA30: return LV;                   // Lo       HANGUL SYLLABLE JJYAE
                case 0x0CA4C: return LV;                   // Lo       HANGUL SYLLABLE JJEO
                case 0x0CA68: return LV;                   // Lo       HANGUL SYLLABLE JJE
                case 0x0CA84: return LV;                   // Lo       HANGUL SYLLABLE JJYEO
                case 0x0CAA0: return LV;                   // Lo       HANGUL SYLLABLE JJYE
                case 0x0CABC: return LV;                   // Lo       HANGUL SYLLABLE JJO
                case 0x0CAD8: return LV;                   // Lo       HANGUL SYLLABLE JJWA
                case 0x0CAF4: return LV;                   // Lo       HANGUL SYLLABLE JJWAE
                case 0x0CB10: return LV;                   // Lo       HANGUL SYLLABLE JJOE
                case 0x0CB2C: return LV;                   // Lo       HANGUL SYLLABLE JJYO
                case 0x0CB48: return LV;                   // Lo       HANGUL SYLLABLE JJU
                case 0x0CB64: return LV;                   // Lo       HANGUL SYLLABLE JJWEO
                case 0x0CB80: return LV;                   // Lo       HANGUL SYLLABLE JJWE
                case 0x0CB9C: return LV;                   // Lo       HANGUL SYLLABLE JJWI
                case 0x0CBB8: return LV;                   // Lo       HANGUL SYLLABLE JJYU
                case 0x0CBD4: return LV;                   // Lo       HANGUL SYLLABLE JJEU
                case 0x0CBF0: return LV;                   // Lo       HANGUL SYLLABLE JJYI
                case 0x0CC0C: return LV;                   // Lo       HANGUL SYLLABLE JJI
                case 0x0CC28: return LV;                   // Lo       HANGUL SYLLABLE CA
                case 0x0CC44: return LV;                   // Lo       HANGUL SYLLABLE CAE
                case 0x0CC60: return LV;                   // Lo       HANGUL SYLLABLE CYA
                case 0x0CC7C: return LV;                   // Lo       HANGUL SYLLABLE CYAE
                case 0x0CC98: return LV;                   // Lo       HANGUL SYLLABLE CEO
                case 0x0CCB4: return LV;                   // Lo       HANGUL SYLLABLE CE
                case 0x0CCD0: return LV;                   // Lo       HANGUL SYLLABLE CYEO
                case 0x0CCEC: return LV;                   // Lo       HANGUL SYLLABLE CYE
                case 0x0CD08: return LV;                   // Lo       HANGUL SYLLABLE CO
                case 0x0CD24: return LV;                   // Lo       HANGUL SYLLABLE CWA
                case 0x0CD40: return LV;                   // Lo       HANGUL SYLLABLE CWAE
                case 0x0CD5C: return LV;                   // Lo       HANGUL SYLLABLE COE
                case 0x0CD78: return LV;                   // Lo       HANGUL SYLLABLE CYO
                case 0x0CD94: return LV;                   // Lo       HANGUL SYLLABLE CU
                case 0x0CDB0: return LV;                   // Lo       HANGUL SYLLABLE CWEO
                case 0x0CDCC: return LV;                   // Lo       HANGUL SYLLABLE CWE
                case 0x0CDE8: return LV;                   // Lo       HANGUL SYLLABLE CWI
                case 0x0CE04: return LV;                   // Lo       HANGUL SYLLABLE CYU
                case 0x0CE20: return LV;                   // Lo       HANGUL SYLLABLE CEU
                case 0x0CE3C: return LV;                   // Lo       HANGUL SYLLABLE CYI
                case 0x0CE58: return LV;                   // Lo       HANGUL SYLLABLE CI
                case 0x0CE74: return LV;                   // Lo       HANGUL SYLLABLE KA
                case 0x0CE90: return LV;                   // Lo       HANGUL SYLLABLE KAE
                case 0x0CEAC: return LV;                   // Lo       HANGUL SYLLABLE KYA
                case 0x0CEC8: return LV;                   // Lo       HANGUL SYLLABLE KYAE
                case 0x0CEE4: return LV;                   // Lo       HANGUL SYLLABLE KEO
                case 0x0CF00: return LV;                   // Lo       HANGUL SYLLABLE KE
                case 0x0CF1C: return LV;                   // Lo       HANGUL SYLLABLE KYEO
                case 0x0CF38: return LV;                   // Lo       HANGUL SYLLABLE KYE
                case 0x0CF54: return LV;                   // Lo       HANGUL SYLLABLE KO
                case 0x0CF70: return LV;                   // Lo       HANGUL SYLLABLE KWA
                case 0x0CF8C: return LV;                   // Lo       HANGUL SYLLABLE KWAE
                case 0x0CFA8: return LV;                   // Lo       HANGUL SYLLABLE KOE
                case 0x0CFC4: return LV;                   // Lo       HANGUL SYLLABLE KYO
                case 0x0CFE0: return LV;                   // Lo       HANGUL SYLLABLE KU
                case 0x0CFFC: return LV;                   // Lo       HANGUL SYLLABLE KWEO
                case 0x0D018: return LV;                   // Lo       HANGUL SYLLABLE KWE
                case 0x0D034: return LV;                   // Lo       HANGUL SYLLABLE KWI
                case 0x0D050: return LV;                   // Lo       HANGUL SYLLABLE KYU
                case 0x0D06C: return LV;                   // Lo       HANGUL SYLLABLE KEU
                case 0x0D088: return LV;                   // Lo       HANGUL SYLLABLE KYI
                case 0x0D0A4: return LV;                   // Lo       HANGUL SYLLABLE KI
                case 0x0D0C0: return LV;                   // Lo       HANGUL SYLLABLE TA
                case 0x0D0DC: return LV;                   // Lo       HANGUL SYLLABLE TAE
                case 0x0D0F8: return LV;                   // Lo       HANGUL SYLLABLE TYA
                case 0x0D114: return LV;                   // Lo       HANGUL SYLLABLE TYAE
                case 0x0D130: return LV;                   // Lo       HANGUL SYLLABLE TEO
                case 0x0D14C: return LV;                   // Lo       HANGUL SYLLABLE TE
                case 0x0D168: return LV;                   // Lo       HANGUL SYLLABLE TYEO
                case 0x0D184: return LV;                   // Lo       HANGUL SYLLABLE TYE
                case 0x0D1A0: return LV;                   // Lo       HANGUL SYLLABLE TO
                case 0x0D1BC: return LV;                   // Lo       HANGUL SYLLABLE TWA
                case 0x0D1D8: return LV;                   // Lo       HANGUL SYLLABLE TWAE
                case 0x0D1F4: return LV;                   // Lo       HANGUL SYLLABLE TOE
                case 0x0D210: return LV;                   // Lo       HANGUL SYLLABLE TYO
                case 0x0D22C: return LV;                   // Lo       HANGUL SYLLABLE TU
                case 0x0D248: return LV;                   // Lo       HANGUL SYLLABLE TWEO
                case 0x0D264: return LV;                   // Lo       HANGUL SYLLABLE TWE
                case 0x0D280: return LV;                   // Lo       HANGUL SYLLABLE TWI
                case 0x0D29C: return LV;                   // Lo       HANGUL SYLLABLE TYU
                case 0x0D2B8: return LV;                   // Lo       HANGUL SYLLABLE TEU
                case 0x0D2D4: return LV;                   // Lo       HANGUL SYLLABLE TYI
                case 0x0D2F0: return LV;                   // Lo       HANGUL SYLLABLE TI
                case 0x0D30C: return LV;                   // Lo       HANGUL SYLLABLE PA
                case 0x0D328: return LV;                   // Lo       HANGUL SYLLABLE PAE
                case 0x0D344: return LV;                   // Lo       HANGUL SYLLABLE PYA
                case 0x0D360: return LV;                   // Lo       HANGUL SYLLABLE PYAE
                case 0x0D37C: return LV;                   // Lo       HANGUL SYLLABLE PEO
                case 0x0D398: return LV;                   // Lo       HANGUL SYLLABLE PE
                case 0x0D3B4: return LV;                   // Lo       HANGUL SYLLABLE PYEO
                case 0x0D3D0: return LV;                   // Lo       HANGUL SYLLABLE PYE
                case 0x0D3EC: return LV;                   // Lo       HANGUL SYLLABLE PO
                case 0x0D408: return LV;                   // Lo       HANGUL SYLLABLE PWA
                case 0x0D424: return LV;                   // Lo       HANGUL SYLLABLE PWAE
                case 0x0D440: return LV;                   // Lo       HANGUL SYLLABLE POE
                case 0x0D45C: return LV;                   // Lo       HANGUL SYLLABLE PYO
                case 0x0D478: return LV;                   // Lo       HANGUL SYLLABLE PU
                case 0x0D494: return LV;                   // Lo       HANGUL SYLLABLE PWEO
                case 0x0D4B0: return LV;                   // Lo       HANGUL SYLLABLE PWE
                case 0x0D4CC: return LV;                   // Lo       HANGUL SYLLABLE PWI
                case 0x0D4E8: return LV;                   // Lo       HANGUL SYLLABLE PYU
                case 0x0D504: return LV;                   // Lo       HANGUL SYLLABLE PEU
                case 0x0D520: return LV;                   // Lo       HANGUL SYLLABLE PYI
                case 0x0D53C: return LV;                   // Lo       HANGUL SYLLABLE PI
                case 0x0D558: return LV;                   // Lo       HANGUL SYLLABLE HA
                case 0x0D574: return LV;                   // Lo       HANGUL SYLLABLE HAE
                case 0x0D590: return LV;                   // Lo       HANGUL SYLLABLE HYA
                case 0x0D5AC: return LV;                   // Lo       HANGUL SYLLABLE HYAE
                case 0x0D5C8: return LV;                   // Lo       HANGUL SYLLABLE HEO
                case 0x0D5E4: return LV;                   // Lo       HANGUL SYLLABLE HE
                case 0x0D600: return LV;                   // Lo       HANGUL SYLLABLE HYEO
                case 0x0D61C: return LV;                   // Lo       HANGUL SYLLABLE HYE
                case 0x0D638: return LV;                   // Lo       HANGUL SYLLABLE HO
                case 0x0D654: return LV;                   // Lo       HANGUL SYLLABLE HWA
                case 0x0D670: return LV;                   // Lo       HANGUL SYLLABLE HWAE
                case 0x0D68C: return LV;                   // Lo       HANGUL SYLLABLE HOE
                case 0x0D6A8: return LV;                   // Lo       HANGUL SYLLABLE HYO
                case 0x0D6C4: return LV;                   // Lo       HANGUL SYLLABLE HU
                case 0x0D6E0: return LV;                   // Lo       HANGUL SYLLABLE HWEO
                case 0x0D6FC: return LV;                   // Lo       HANGUL SYLLABLE HWE
                case 0x0D718: return LV;                   // Lo       HANGUL SYLLABLE HWI
                case 0x0D734: return LV;                   // Lo       HANGUL SYLLABLE HYU
                case 0x0D750: return LV;                   // Lo       HANGUL SYLLABLE HEU
                case 0x0D76C: return LV;                   // Lo       HANGUL SYLLABLE HYI
                case 0x0D788: return LV;                   // Lo       HANGUL SYLLABLE HI
                case 0x0200D: return ZWJ;                  // Cf       ZERO WIDTH JOINER
                case 0x000A9: return Extended_Pictographic; // E0.6   [1] (©️)       copyright
                case 0x000AE: return Extended_Pictographic; // E0.6   [1] (®️)       registered
                case 0x0203C: return Extended_Pictographic; // E0.6   [1] (‼️)       double exclamation mark
                case 0x02049: return Extended_Pictographic; // E0.6   [1] (⁉️)       exclamation question mark
                case 0x02122: return Extended_Pictographic; // E0.6   [1] (™️)       trade mark
                case 0x02139: return Extended_Pictographic; // E0.6   [1] (ℹ️)       information
                case 0x02328: return Extended_Pictographic; // E1.0   [1] (⌨️)       keyboard
                case 0x02388: return Extended_Pictographic; // E0.0   [1] (⎈)       HELM SYMBOL
                case 0x023CF: return Extended_Pictographic; // E1.0   [1] (⏏️)       eject button
                case 0x023EF: return Extended_Pictographic; // E1.0   [1] (⏯️)       play or pause button
                case 0x023F0: return Extended_Pictographic; // E0.6   [1] (⏰)       alarm clock
                case 0x023F3: return Extended_Pictographic; // E0.6   [1] (⏳)       hourglass not done
                case 0x024C2: return Extended_Pictographic; // E0.6   [1] (Ⓜ️)       circled M
                case 0x025B6: return Extended_Pictographic; // E0.6   [1] (▶️)       play button
                case 0x025C0: return Extended_Pictographic; // E0.6   [1] (◀️)       reverse button
                case 0x02604: return Extended_Pictographic; // E1.0   [1] (☄️)       comet
                case 0x02605: return Extended_Pictographic; // E0.0   [1] (★)       BLACK STAR
                case 0x0260E: return Extended_Pictographic; // E0.6   [1] (☎️)       telephone
                case 0x02611: return Extended_Pictographic; // E0.6   [1] (☑️)       check box with check
                case 0x02612: return Extended_Pictographic; // E0.0   [1] (☒)       BALLOT BOX WITH X
                case 0x02618: return Extended_Pictographic; // E1.0   [1] (☘️)       shamrock
                case 0x0261D: return Extended_Pictographic; // E0.6   [1] (☝️)       index pointing up
                case 0x02620: return Extended_Pictographic; // E1.0   [1] (☠️)       skull and crossbones
                case 0x02621: return Extended_Pictographic; // E0.0   [1] (☡)       CAUTION SIGN
                case 0x02626: return Extended_Pictographic; // E1.0   [1] (☦️)       orthodox cross
                case 0x0262A: return Extended_Pictographic; // E0.7   [1] (☪️)       star and crescent
                case 0x0262E: return Extended_Pictographic; // E1.0   [1] (☮️)       peace symbol
                case 0x0262F: return Extended_Pictographic; // E0.7   [1] (☯️)       yin yang
                case 0x0263A: return Extended_Pictographic; // E0.6   [1] (☺️)       smiling face
                case 0x02640: return Extended_Pictographic; // E4.0   [1] (♀️)       female sign
                case 0x02641: return Extended_Pictographic; // E0.0   [1] (♁)       EARTH
                case 0x02642: return Extended_Pictographic; // E4.0   [1] (♂️)       male sign
                case 0x0265F: return Extended_Pictographic; // E11.0  [1] (♟️)       chess pawn
                case 0x02660: return Extended_Pictographic; // E0.6   [1] (♠️)       spade suit
                case 0x02663: return Extended_Pictographic; // E0.6   [1] (♣️)       club suit
                case 0x02664: return Extended_Pictographic; // E0.0   [1] (♤)       WHITE SPADE SUIT
                case 0x02667: return Extended_Pictographic; // E0.0   [1] (♧)       WHITE CLUB SUIT
                case 0x02668: return Extended_Pictographic; // E0.6   [1] (♨️)       hot springs
                case 0x0267B: return Extended_Pictographic; // E0.6   [1] (♻️)       recycling symbol
                case 0x0267E: return Extended_Pictographic; // E11.0  [1] (♾️)       infinity
                case 0x0267F: return Extended_Pictographic; // E0.6   [1] (♿)       wheelchair symbol
                case 0x02692: return Extended_Pictographic; // E1.0   [1] (⚒️)       hammer and pick
                case 0x02693: return Extended_Pictographic; // E0.6   [1] (⚓)       anchor
                case 0x02694: return Extended_Pictographic; // E1.0   [1] (⚔️)       crossed swords
                case 0x02695: return Extended_Pictographic; // E4.0   [1] (⚕️)       medical symbol
                case 0x02698: return Extended_Pictographic; // E0.0   [1] (⚘)       FLOWER
                case 0x02699: return Extended_Pictographic; // E1.0   [1] (⚙️)       gear
                case 0x0269A: return Extended_Pictographic; // E0.0   [1] (⚚)       STAFF OF HERMES
                case 0x026A7: return Extended_Pictographic; // E13.0  [1] (⚧️)       transgender symbol
                case 0x026C8: return Extended_Pictographic; // E0.7   [1] (⛈️)       cloud with lightning and rain
                case 0x026CE: return Extended_Pictographic; // E0.6   [1] (⛎)       Ophiuchus
                case 0x026CF: return Extended_Pictographic; // E0.7   [1] (⛏️)       pick
                case 0x026D0: return Extended_Pictographic; // E0.0   [1] (⛐)       CAR SLIDING
                case 0x026D1: return Extended_Pictographic; // E0.7   [1] (⛑️)       rescue worker’s helmet
                case 0x026D2: return Extended_Pictographic; // E0.0   [1] (⛒)       CIRCLED CROSSING LANES
                case 0x026D3: return Extended_Pictographic; // E0.7   [1] (⛓️)       chains
                case 0x026D4: return Extended_Pictographic; // E0.6   [1] (⛔)       no entry
                case 0x026E9: return Extended_Pictographic; // E0.7   [1] (⛩️)       shinto shrine
                case 0x026EA: return Extended_Pictographic; // E0.6   [1] (⛪)       church
                case 0x026F4: return Extended_Pictographic; // E0.7   [1] (⛴️)       ferry
                case 0x026F5: return Extended_Pictographic; // E0.6   [1] (⛵)       sailboat
                case 0x026F6: return Extended_Pictographic; // E0.0   [1] (⛶)       SQUARE FOUR CORNERS
                case 0x026FA: return Extended_Pictographic; // E0.6   [1] (⛺)       tent
                case 0x026FD: return Extended_Pictographic; // E0.6   [1] (⛽)       fuel pump
                case 0x02702: return Extended_Pictographic; // E0.6   [1] (✂️)       scissors
                case 0x02705: return Extended_Pictographic; // E0.6   [1] (✅)       check mark button
                case 0x0270D: return Extended_Pictographic; // E0.7   [1] (✍️)       writing hand
                case 0x0270E: return Extended_Pictographic; // E0.0   [1] (✎)       LOWER RIGHT PENCIL
                case 0x0270F: return Extended_Pictographic; // E0.6   [1] (✏️)       pencil
                case 0x02712: return Extended_Pictographic; // E0.6   [1] (✒️)       black nib
                case 0x02714: return Extended_Pictographic; // E0.6   [1] (✔️)       check mark
                case 0x02716: return Extended_Pictographic; // E0.6   [1] (✖️)       multiply
                case 0x0271D: return Extended_Pictographic; // E0.7   [1] (✝️)       latin cross
                case 0x02721: return Extended_Pictographic; // E0.7   [1] (✡️)       star of David
                case 0x02728: return Extended_Pictographic; // E0.6   [1] (✨)       sparkles
                case 0x02744: return Extended_Pictographic; // E0.6   [1] (❄️)       snowflake
                case 0x02747: return Extended_Pictographic; // E0.6   [1] (❇️)       sparkle
                case 0x0274C: return Extended_Pictographic; // E0.6   [1] (❌)       cross mark
                case 0x0274E: return Extended_Pictographic; // E0.6   [1] (❎)       cross mark button
                case 0x02757: return Extended_Pictographic; // E0.6   [1] (❗)       red exclamation mark
                case 0x02763: return Extended_Pictographic; // E1.0   [1] (❣️)       heart exclamation
                case 0x02764: return Extended_Pictographic; // E0.6   [1] (❤️)       red heart
                case 0x027A1: return Extended_Pictographic; // E0.6   [1] (➡️)       right arrow
                case 0x027B0: return Extended_Pictographic; // E0.6   [1] (➰)       curly loop
                case 0x027BF: return Extended_Pictographic; // E1.0   [1] (➿)       double curly loop
                case 0x02B50: return Extended_Pictographic; // E0.6   [1] (⭐)       star
                case 0x02B55: return Extended_Pictographic; // E0.6   [1] (⭕)       hollow red circle
                case 0x03030: return Extended_Pictographic; // E0.6   [1] (〰️)       wavy dash
                case 0x0303D: return Extended_Pictographic; // E0.6   [1] (〽️)       part alternation mark
                case 0x03297: return Extended_Pictographic; // E0.6   [1] (㊗️)       Japanese “congratulations” button
                case 0x03299: return Extended_Pictographic; // E0.6   [1] (㊙️)       Japanese “secret” button
                case 0x1F004: return Extended_Pictographic; // E0.6   [1] (🀄)       mahjong red dragon
                case 0x1F0CF: return Extended_Pictographic; // E0.6   [1] (🃏)       joker
                case 0x1F12F: return Extended_Pictographic; // E0.0   [1] (🄯)       COPYLEFT SYMBOL
                case 0x1F18E: return Extended_Pictographic; // E0.6   [1] (🆎)       AB button (blood type)
                case 0x1F21A: return Extended_Pictographic; // E0.6   [1] (🈚)       Japanese “free of charge” button
                case 0x1F22F: return Extended_Pictographic; // E0.6   [1] (🈯)       Japanese “reserved” button
                case 0x1F30F: return Extended_Pictographic; // E0.6   [1] (🌏)       globe showing Asia-Australia
                case 0x1F310: return Extended_Pictographic; // E1.0   [1] (🌐)       globe with meridians
                case 0x1F311: return Extended_Pictographic; // E0.6   [1] (🌑)       new moon
                case 0x1F312: return Extended_Pictographic; // E1.0   [1] (🌒)       waxing crescent moon
                case 0x1F319: return Extended_Pictographic; // E0.6   [1] (🌙)       crescent moon
                case 0x1F31A: return Extended_Pictographic; // E1.0   [1] (🌚)       new moon face
                case 0x1F31B: return Extended_Pictographic; // E0.6   [1] (🌛)       first quarter moon face
                case 0x1F31C: return Extended_Pictographic; // E0.7   [1] (🌜)       last quarter moon face
                case 0x1F321: return Extended_Pictographic; // E0.7   [1] (🌡️)       thermometer
                case 0x1F336: return Extended_Pictographic; // E0.7   [1] (🌶️)       hot pepper
                case 0x1F34B: return Extended_Pictographic; // E1.0   [1] (🍋)       lemon
                case 0x1F350: return Extended_Pictographic; // E1.0   [1] (🍐)       pear
                case 0x1F37C: return Extended_Pictographic; // E1.0   [1] (🍼)       baby bottle
                case 0x1F37D: return Extended_Pictographic; // E0.7   [1] (🍽️)       fork and knife with plate
                case 0x1F398: return Extended_Pictographic; // E0.0   [1] (🎘)       MUSICAL KEYBOARD WITH JACKS
                case 0x1F3C5: return Extended_Pictographic; // E1.0   [1] (🏅)       sports medal
                case 0x1F3C6: return Extended_Pictographic; // E0.6   [1] (🏆)       trophy
                case 0x1F3C7: return Extended_Pictographic; // E1.0   [1] (🏇)       horse racing
                case 0x1F3C8: return Extended_Pictographic; // E0.6   [1] (🏈)       american football
                case 0x1F3C9: return Extended_Pictographic; // E1.0   [1] (🏉)       rugby football
                case 0x1F3CA: return Extended_Pictographic; // E0.6   [1] (🏊)       person swimming
                case 0x1F3E4: return Extended_Pictographic; // E1.0   [1] (🏤)       post office
                case 0x1F3F3: return Extended_Pictographic; // E0.7   [1] (🏳️)       white flag
                case 0x1F3F4: return Extended_Pictographic; // E1.0   [1] (🏴)       black flag
                case 0x1F3F5: return Extended_Pictographic; // E0.7   [1] (🏵️)       rosette
                case 0x1F3F6: return Extended_Pictographic; // E0.0   [1] (🏶)       BLACK ROSETTE
                case 0x1F3F7: return Extended_Pictographic; // E0.7   [1] (🏷️)       label
                case 0x1F408: return Extended_Pictographic; // E0.7   [1] (🐈)       cat
                case 0x1F413: return Extended_Pictographic; // E1.0   [1] (🐓)       rooster
                case 0x1F414: return Extended_Pictographic; // E0.6   [1] (🐔)       chicken
                case 0x1F415: return Extended_Pictographic; // E0.7   [1] (🐕)       dog
                case 0x1F416: return Extended_Pictographic; // E1.0   [1] (🐖)       pig
                case 0x1F42A: return Extended_Pictographic; // E1.0   [1] (🐪)       camel
                case 0x1F43F: return Extended_Pictographic; // E0.7   [1] (🐿️)       chipmunk
                case 0x1F440: return Extended_Pictographic; // E0.6   [1] (👀)       eyes
                case 0x1F441: return Extended_Pictographic; // E0.7   [1] (👁️)       eye
                case 0x1F465: return Extended_Pictographic; // E1.0   [1] (👥)       busts in silhouette
                case 0x1F4AD: return Extended_Pictographic; // E1.0   [1] (💭)       thought balloon
                case 0x1F4EE: return Extended_Pictographic; // E0.6   [1] (📮)       postbox
                case 0x1F4EF: return Extended_Pictographic; // E1.0   [1] (📯)       postal horn
                case 0x1F4F5: return Extended_Pictographic; // E1.0   [1] (📵)       no mobile phones
                case 0x1F4F8: return Extended_Pictographic; // E1.0   [1] (📸)       camera with flash
                case 0x1F4FD: return Extended_Pictographic; // E0.7   [1] (📽️)       film projector
                case 0x1F4FE: return Extended_Pictographic; // E0.0   [1] (📾)       PORTABLE STEREO
                case 0x1F503: return Extended_Pictographic; // E0.6   [1] (🔃)       clockwise vertical arrows
                case 0x1F508: return Extended_Pictographic; // E0.7   [1] (🔈)       speaker low volume
                case 0x1F509: return Extended_Pictographic; // E1.0   [1] (🔉)       speaker medium volume
                case 0x1F515: return Extended_Pictographic; // E1.0   [1] (🔕)       bell with slash
                case 0x1F54F: return Extended_Pictographic; // E0.0   [1] (🕏)       BOWL OF HYGIEIA
                case 0x1F57A: return Extended_Pictographic; // E3.0   [1] (🕺)       man dancing
                case 0x1F587: return Extended_Pictographic; // E0.7   [1] (🖇️)       linked paperclips
                case 0x1F590: return Extended_Pictographic; // E0.7   [1] (🖐️)       hand with fingers splayed
                case 0x1F5A4: return Extended_Pictographic; // E3.0   [1] (🖤)       black heart
                case 0x1F5A5: return Extended_Pictographic; // E0.7   [1] (🖥️)       desktop computer
                case 0x1F5A8: return Extended_Pictographic; // E0.7   [1] (🖨️)       printer
                case 0x1F5BC: return Extended_Pictographic; // E0.7   [1] (🖼️)       framed picture
                case 0x1F5E1: return Extended_Pictographic; // E0.7   [1] (🗡️)       dagger
                case 0x1F5E2: return Extended_Pictographic; // E0.0   [1] (🗢)       LIPS
                case 0x1F5E3: return Extended_Pictographic; // E0.7   [1] (🗣️)       speaking head
                case 0x1F5E8: return Extended_Pictographic; // E2.0   [1] (🗨️)       left speech bubble
                case 0x1F5EF: return Extended_Pictographic; // E0.7   [1] (🗯️)       right anger bubble
                case 0x1F5F3: return Extended_Pictographic; // E0.7   [1] (🗳️)       ballot box with ballot
                case 0x1F5FA: return Extended_Pictographic; // E0.7   [1] (🗺️)       world map
                case 0x1F600: return Extended_Pictographic; // E1.0   [1] (😀)       grinning face
                case 0x1F60E: return Extended_Pictographic; // E1.0   [1] (😎)       smiling face with sunglasses
                case 0x1F60F: return Extended_Pictographic; // E0.6   [1] (😏)       smirking face
                case 0x1F610: return Extended_Pictographic; // E0.7   [1] (😐)       neutral face
                case 0x1F611: return Extended_Pictographic; // E1.0   [1] (😑)       expressionless face
                case 0x1F615: return Extended_Pictographic; // E1.0   [1] (😕)       confused face
                case 0x1F616: return Extended_Pictographic; // E0.6   [1] (😖)       confounded face
                case 0x1F617: return Extended_Pictographic; // E1.0   [1] (😗)       kissing face
                case 0x1F618: return Extended_Pictographic; // E0.6   [1] (😘)       face blowing a kiss
                case 0x1F619: return Extended_Pictographic; // E1.0   [1] (😙)       kissing face with smiling eyes
                case 0x1F61A: return Extended_Pictographic; // E0.6   [1] (😚)       kissing face with closed eyes
                case 0x1F61B: return Extended_Pictographic; // E1.0   [1] (😛)       face with tongue
                case 0x1F61F: return Extended_Pictographic; // E1.0   [1] (😟)       worried face
                case 0x1F62C: return Extended_Pictographic; // E1.0   [1] (😬)       grimacing face
                case 0x1F62D: return Extended_Pictographic; // E0.6   [1] (😭)       loudly crying face
                case 0x1F634: return Extended_Pictographic; // E1.0   [1] (😴)       sleeping face
                case 0x1F635: return Extended_Pictographic; // E0.6   [1] (😵)       face with crossed-out eyes
                case 0x1F636: return Extended_Pictographic; // E1.0   [1] (😶)       face without mouth
                case 0x1F680: return Extended_Pictographic; // E0.6   [1] (🚀)       rocket
                case 0x1F686: return Extended_Pictographic; // E1.0   [1] (🚆)       train
                case 0x1F687: return Extended_Pictographic; // E0.6   [1] (🚇)       metro
                case 0x1F688: return Extended_Pictographic; // E1.0   [1] (🚈)       light rail
                case 0x1F689: return Extended_Pictographic; // E0.6   [1] (🚉)       station
                case 0x1F68C: return Extended_Pictographic; // E0.6   [1] (🚌)       bus
                case 0x1F68D: return Extended_Pictographic; // E0.7   [1] (🚍)       oncoming bus
                case 0x1F68E: return Extended_Pictographic; // E1.0   [1] (🚎)       trolleybus
                case 0x1F68F: return Extended_Pictographic; // E0.6   [1] (🚏)       bus stop
                case 0x1F690: return Extended_Pictographic; // E1.0   [1] (🚐)       minibus
                case 0x1F694: return Extended_Pictographic; // E0.7   [1] (🚔)       oncoming police car
                case 0x1F695: return Extended_Pictographic; // E0.6   [1] (🚕)       taxi
                case 0x1F696: return Extended_Pictographic; // E1.0   [1] (🚖)       oncoming taxi
                case 0x1F697: return Extended_Pictographic; // E0.6   [1] (🚗)       automobile
                case 0x1F698: return Extended_Pictographic; // E0.7   [1] (🚘)       oncoming automobile
                case 0x1F6A2: return Extended_Pictographic; // E0.6   [1] (🚢)       ship
                case 0x1F6A3: return Extended_Pictographic; // E1.0   [1] (🚣)       person rowing boat
                case 0x1F6A6: return Extended_Pictographic; // E1.0   [1] (🚦)       vertical traffic light
                case 0x1F6B2: return Extended_Pictographic; // E0.6   [1] (🚲)       bicycle
                case 0x1F6B6: return Extended_Pictographic; // E0.6   [1] (🚶)       person walking
                case 0x1F6BF: return Extended_Pictographic; // E1.0   [1] (🚿)       shower
                case 0x1F6C0: return Extended_Pictographic; // E0.6   [1] (🛀)       person taking bath
                case 0x1F6CB: return Extended_Pictographic; // E0.7   [1] (🛋️)       couch and lamp
                case 0x1F6CC: return Extended_Pictographic; // E1.0   [1] (🛌)       person in bed
                case 0x1F6D0: return Extended_Pictographic; // E1.0   [1] (🛐)       place of worship
                case 0x1F6D5: return Extended_Pictographic; // E12.0  [1] (🛕)       hindu temple
                case 0x1F6E9: return Extended_Pictographic; // E0.7   [1] (🛩️)       small airplane
                case 0x1F6EA: return Extended_Pictographic; // E0.0   [1] (🛪)       NORTHEAST-POINTING AIRPLANE
                case 0x1F6F0: return Extended_Pictographic; // E0.7   [1] (🛰️)       satellite
                case 0x1F6F3: return Extended_Pictographic; // E0.7   [1] (🛳️)       passenger ship
                case 0x1F6F9: return Extended_Pictographic; // E11.0  [1] (🛹)       skateboard
                case 0x1F6FA: return Extended_Pictographic; // E12.0  [1] (🛺)       auto rickshaw
                case 0x1F7F0: return Extended_Pictographic; // E14.0  [1] (🟰)       heavy equals sign
                case 0x1F90C: return Extended_Pictographic; // E13.0  [1] (🤌)       pinched fingers
                case 0x1F91F: return Extended_Pictographic; // E5.0   [1] (🤟)       love-you gesture
                case 0x1F930: return Extended_Pictographic; // E3.0   [1] (🤰)       pregnant woman
                case 0x1F93F: return Extended_Pictographic; // E12.0  [1] (🤿)       diving mask
                case 0x1F94C: return Extended_Pictographic; // E5.0   [1] (🥌)       curling stone
                case 0x1F971: return Extended_Pictographic; // E12.0  [1] (🥱)       yawning face
                case 0x1F972: return Extended_Pictographic; // E13.0  [1] (🥲)       smiling face with tear
                case 0x1F979: return Extended_Pictographic; // E14.0  [1] (🥹)       face holding back tears
                case 0x1F97A: return Extended_Pictographic; // E11.0  [1] (🥺)       pleading face
                case 0x1F97B: return Extended_Pictographic; // E12.0  [1] (🥻)       sari
                case 0x1F9C0: return Extended_Pictographic; // E1.0   [1] (🧀)       cheese wedge
                case 0x1F9CB: return Extended_Pictographic; // E13.0  [1] (🧋)       bubble tea
                case 0x1F9CC: return Extended_Pictographic; // E14.0  [1] (🧌)       troll
                case 0x1FA74: return Extended_Pictographic; // E13.0  [1] (🩴)       thong sandal
            }
            return BinarySearchRangeFromList(0, m_lst_code_range.Count - 1, nCodePoint, m_lst_code_range);
        }

        static int BinarySearchRangeFromList(int nStart, int nEnd, int nValue, List<RangeInfo> lst)
        {
            if (nEnd < nStart) return 0;
            int nMid = nStart + (nEnd - nStart) / 2;
            if (lst[nMid].Start > nValue) return BinarySearchRangeFromList(nStart, nMid - 1, nValue, lst);
            else if (lst[nMid].End < nValue) return BinarySearchRangeFromList(nMid + 1, nEnd, nValue, lst);
            else return lst[nMid].Type;
        }
    }
}