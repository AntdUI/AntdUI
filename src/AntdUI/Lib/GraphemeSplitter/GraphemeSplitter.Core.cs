// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AntdUI
{
    partial class GraphemeSplitter
    {
        public static int GetCodePoint(string strText, int nIndex)
        {
            var aa = strText[nIndex];
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

        static bool ShouldBreak(int nRightType, List<int> lstHistoryBreakType)
        {
            int nLeftType = lstHistoryBreakType[lstHistoryBreakType.Count - 1];

            // GB3: CR × LF (不换行)
            if (nLeftType == CR && nRightType == LF) return false;

            // GB4 & GB5: (Control | CR | LF) ÷ and ÷ (Control | CR | LF)
            if (nLeftType == Control || nLeftType == CR || nLeftType == LF || nRightType == Control || nRightType == CR || nRightType == LF) return true;

            // GB6-GB8: Hangul syllable sequences (不换行)
            if (nLeftType == L && (nRightType == L || nRightType == V || nRightType == LV || nRightType == LVT)) return false;
            if ((nLeftType == LV || nLeftType == V) && (nRightType == V || nRightType == T)) return false;
            if ((nLeftType == LVT || nLeftType == T) && nRightType == T) return false;

            // GB9: × (Extend | ZWJ) (不换行)
            if (nRightType == Extend || nRightType == ZWJ) return false;

            // GB9a: × SpacingMark (不换行)
            if (nRightType == SpacingMark) return false;

            // GB9b: Prepend × (不换行)
            if (nLeftType == Prepend) return false;

            // GB11: \p{Extended_Pictographic} Extend* ZWJ × \p{Extended_Pictographic}
            if (nRightType == Extended_Pictographic)
            {
                // 检查前一个字符是否是ZWJ
                if (lstHistoryBreakType.Count >= 2 && lstHistoryBreakType[lstHistoryBreakType.Count - 1] == ZWJ)
                {
                    // 检查ZWJ前面是否有Extended_Pictographic
                    int i = lstHistoryBreakType.Count - 2;
                    while (i >= 0 && (lstHistoryBreakType[i] == Extend || lstHistoryBreakType[i] == ZWJ))
                    {
                        i--;
                    }
                    if (i >= 0 && lstHistoryBreakType[i] == Extended_Pictographic) return false;
                }
            }

            // GB12-GB13: emoji flag sequences (不换行)
            if (nRightType == Regional_Indicator)
            {
                int count = 0;
                for (int i = lstHistoryBreakType.Count - 1; i >= 0; i--)
                {
                    if (lstHistoryBreakType[i] == Regional_Indicator) count++;
                    else break;
                }
                if (count % 2 == 1) return false; // 奇数个区域指示符
            }
            return true; // 默认允许断开
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
                case 0x113D1: return Prepend;              // Lo       TULU-TIGALARI REPHA
                case 0x1193F: return Prepend;              // Lo       DIVES AKURU PREFIXED NASAL SIGN
                case 0x11941: return Prepend;              // Lo       DIVES AKURU INITIAL RA
                case 0x11A3A: return Prepend;              // Lo       ZANABAZAR SQUARE CLUSTER-INITIAL LETTER RA
                case 0x11D46: return Prepend;              // Lo       MASARAM GONDI REPHA
                case 0x11F02: return Prepend;              // Lo       KAWI SIGN REPHA
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
                case 0x2B05: return Extended_Pictographic;
                case 0x2B06: return Extended_Pictographic;
                case 0x2B07: return Extended_Pictographic;
                case 0x1F51C: return Extended_Pictographic;
                case 0x1F51D: return Extended_Pictographic;
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
    }
}