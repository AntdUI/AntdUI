// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Collections.Generic;

namespace AntdUI
{
    partial class GraphemeSplitter
    {
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

        static List<RangeInfo> m_lst_code_range = new List<RangeInfo>(1062);

        static GraphemeSplitter()
        {
            // Cc  [10] <control-0000>..<control-0009>
            AddCodeRange(0x00000, 0x00009, Control);
            // Cc   [2] <control-000B>..<control-000C>
            AddCodeRange(0x0000B, 0x0000C, Control);
            // Cc  [18] <control-000E>..<control-001F>
            AddCodeRange(0x0000E, 0x0001F, Control);
            // Cc  [33] <control-007F>..<control-009F>
            AddCodeRange(0x0007F, 0x0009F, Control);
            // Mn [112] COMBINING GRAVE ACCENT..COMBINING LATIN SMALL LETTER X
            AddCodeRange(0x00300, 0x0036F, Extend);
            // Mn   [5] COMBINING CYRILLIC TITLO..COMBINING CYRILLIC POKRYTIE
            AddCodeRange(0x00483, 0x00487, Extend);
            // Me   [2] COMBINING CYRILLIC HUNDRED THOUSANDS SIGN..COMBINING CYRILLIC MILLIONS SIGN
            AddCodeRange(0x00488, 0x00489, Extend);
            // Mn  [45] HEBREW ACCENT ETNAHTA..HEBREW POINT METEG
            AddCodeRange(0x00591, 0x005BD, Extend);
            // Mn   [2] HEBREW POINT SHIN DOT..HEBREW POINT SIN DOT
            AddCodeRange(0x005C1, 0x005C2, Extend);
            // Mn   [2] HEBREW MARK UPPER DOT..HEBREW MARK LOWER DOT
            AddCodeRange(0x005C4, 0x005C5, Extend);
            // Cf   [6] ARABIC NUMBER SIGN..ARABIC NUMBER MARK ABOVE
            AddCodeRange(0x00600, 0x00605, Prepend);
            // Mn  [11] ARABIC SIGN SALLALLAHOU ALAYHE WASSALLAM..ARABIC SMALL KASRA
            AddCodeRange(0x00610, 0x0061A, Extend);
            // Mn  [21] ARABIC FATHATAN..ARABIC WAVY HAMZA BELOW
            AddCodeRange(0x0064B, 0x0065F, Extend);
            // Mn   [7] ARABIC SMALL HIGH LIGATURE SAD WITH LAM WITH ALEF MAKSURA..ARABIC SMALL HIGH SEEN
            AddCodeRange(0x006D6, 0x006DC, Extend);
            // Mn   [6] ARABIC SMALL HIGH ROUNDED ZERO..ARABIC SMALL HIGH MADDA
            AddCodeRange(0x006DF, 0x006E4, Extend);
            // Mn   [2] ARABIC SMALL HIGH YEH..ARABIC SMALL HIGH NOON
            AddCodeRange(0x006E7, 0x006E8, Extend);
            // Mn   [4] ARABIC EMPTY CENTRE LOW STOP..ARABIC SMALL LOW MEEM
            AddCodeRange(0x006EA, 0x006ED, Extend);
            // Mn  [27] SYRIAC PTHAHA ABOVE..SYRIAC BARREKH
            AddCodeRange(0x00730, 0x0074A, Extend);
            // Mn  [11] THAANA ABAFILI..THAANA SUKUN
            AddCodeRange(0x007A6, 0x007B0, Extend);
            // Mn   [9] NKO COMBINING SHORT HIGH TONE..NKO COMBINING DOUBLE DOT ABOVE
            AddCodeRange(0x007EB, 0x007F3, Extend);
            // Mn   [4] SAMARITAN MARK IN..SAMARITAN MARK DAGESH
            AddCodeRange(0x00816, 0x00819, Extend);
            // Mn   [9] SAMARITAN MARK EPENTHETIC YUT..SAMARITAN VOWEL SIGN A
            AddCodeRange(0x0081B, 0x00823, Extend);
            // Mn   [3] SAMARITAN VOWEL SIGN SHORT A..SAMARITAN VOWEL SIGN U
            AddCodeRange(0x00825, 0x00827, Extend);
            // Mn   [5] SAMARITAN VOWEL SIGN LONG I..SAMARITAN MARK NEQUDAA
            AddCodeRange(0x00829, 0x0082D, Extend);
            // Mn   [3] MANDAIC AFFRICATION MARK..MANDAIC GEMINATION MARK
            AddCodeRange(0x00859, 0x0085B, Extend);
            // Cf   [2] ARABIC POUND MARK ABOVE..ARABIC PIASTRE MARK ABOVE
            AddCodeRange(0x00890, 0x00891, Prepend);
            // Mn   [8] ARABIC SMALL HIGH WORD AL-JUZ..ARABIC HALF MADDA OVER MADDA
            AddCodeRange(0x00898, 0x0089F, Extend);
            // Mn  [24] ARABIC SMALL HIGH FARSI YEH..ARABIC SMALL HIGH SIGN SAFHA
            AddCodeRange(0x008CA, 0x008E1, Extend);
            // Mn  [32] ARABIC TURNED DAMMA BELOW..DEVANAGARI SIGN ANUSVARA
            AddCodeRange(0x008E3, 0x00902, Extend);
            // Mc   [3] DEVANAGARI VOWEL SIGN AA..DEVANAGARI VOWEL SIGN II
            AddCodeRange(0x0093E, 0x00940, SpacingMark);
            // Mn   [8] DEVANAGARI VOWEL SIGN U..DEVANAGARI VOWEL SIGN AI
            AddCodeRange(0x00941, 0x00948, Extend);
            // Mc   [4] DEVANAGARI VOWEL SIGN CANDRA O..DEVANAGARI VOWEL SIGN AU
            AddCodeRange(0x00949, 0x0094C, SpacingMark);
            // Mc   [2] DEVANAGARI VOWEL SIGN PRISHTHAMATRA E..DEVANAGARI VOWEL SIGN AW
            AddCodeRange(0x0094E, 0x0094F, SpacingMark);
            // Mn   [7] DEVANAGARI STRESS SIGN UDATTA..DEVANAGARI VOWEL SIGN UUE
            AddCodeRange(0x00951, 0x00957, Extend);
            // Mn   [2] DEVANAGARI VOWEL SIGN VOCALIC L..DEVANAGARI VOWEL SIGN VOCALIC LL
            AddCodeRange(0x00962, 0x00963, Extend);
            // Mc   [2] BENGALI SIGN ANUSVARA..BENGALI SIGN VISARGA
            AddCodeRange(0x00982, 0x00983, SpacingMark);
            // Mc   [2] BENGALI VOWEL SIGN I..BENGALI VOWEL SIGN II
            AddCodeRange(0x009BF, 0x009C0, SpacingMark);
            // Mn   [4] BENGALI VOWEL SIGN U..BENGALI VOWEL SIGN VOCALIC RR
            AddCodeRange(0x009C1, 0x009C4, Extend);
            // Mc   [2] BENGALI VOWEL SIGN E..BENGALI VOWEL SIGN AI
            AddCodeRange(0x009C7, 0x009C8, SpacingMark);
            // Mc   [2] BENGALI VOWEL SIGN O..BENGALI VOWEL SIGN AU
            AddCodeRange(0x009CB, 0x009CC, SpacingMark);
            // Mn   [2] BENGALI VOWEL SIGN VOCALIC L..BENGALI VOWEL SIGN VOCALIC LL
            AddCodeRange(0x009E2, 0x009E3, Extend);
            // Mn   [2] GURMUKHI SIGN ADAK BINDI..GURMUKHI SIGN BINDI
            AddCodeRange(0x00A01, 0x00A02, Extend);
            // Mc   [3] GURMUKHI VOWEL SIGN AA..GURMUKHI VOWEL SIGN II
            AddCodeRange(0x00A3E, 0x00A40, SpacingMark);
            // Mn   [2] GURMUKHI VOWEL SIGN U..GURMUKHI VOWEL SIGN UU
            AddCodeRange(0x00A41, 0x00A42, Extend);
            // Mn   [2] GURMUKHI VOWEL SIGN EE..GURMUKHI VOWEL SIGN AI
            AddCodeRange(0x00A47, 0x00A48, Extend);
            // Mn   [3] GURMUKHI VOWEL SIGN OO..GURMUKHI SIGN VIRAMA
            AddCodeRange(0x00A4B, 0x00A4D, Extend);
            // Mn   [2] GURMUKHI TIPPI..GURMUKHI ADDAK
            AddCodeRange(0x00A70, 0x00A71, Extend);
            // Mn   [2] GUJARATI SIGN CANDRABINDU..GUJARATI SIGN ANUSVARA
            AddCodeRange(0x00A81, 0x00A82, Extend);
            // Mc   [3] GUJARATI VOWEL SIGN AA..GUJARATI VOWEL SIGN II
            AddCodeRange(0x00ABE, 0x00AC0, SpacingMark);
            // Mn   [5] GUJARATI VOWEL SIGN U..GUJARATI VOWEL SIGN CANDRA E
            AddCodeRange(0x00AC1, 0x00AC5, Extend);
            // Mn   [2] GUJARATI VOWEL SIGN E..GUJARATI VOWEL SIGN AI
            AddCodeRange(0x00AC7, 0x00AC8, Extend);
            // Mc   [2] GUJARATI VOWEL SIGN O..GUJARATI VOWEL SIGN AU
            AddCodeRange(0x00ACB, 0x00ACC, SpacingMark);
            // Mn   [2] GUJARATI VOWEL SIGN VOCALIC L..GUJARATI VOWEL SIGN VOCALIC LL
            AddCodeRange(0x00AE2, 0x00AE3, Extend);
            // Mn   [6] GUJARATI SIGN SUKUN..GUJARATI SIGN TWO-CIRCLE NUKTA ABOVE
            AddCodeRange(0x00AFA, 0x00AFF, Extend);
            // Mc   [2] ORIYA SIGN ANUSVARA..ORIYA SIGN VISARGA
            AddCodeRange(0x00B02, 0x00B03, SpacingMark);
            // Mn   [4] ORIYA VOWEL SIGN U..ORIYA VOWEL SIGN VOCALIC RR
            AddCodeRange(0x00B41, 0x00B44, Extend);
            // Mc   [2] ORIYA VOWEL SIGN E..ORIYA VOWEL SIGN AI
            AddCodeRange(0x00B47, 0x00B48, SpacingMark);
            // Mc   [2] ORIYA VOWEL SIGN O..ORIYA VOWEL SIGN AU
            AddCodeRange(0x00B4B, 0x00B4C, SpacingMark);
            // Mn   [2] ORIYA SIGN OVERLINE..ORIYA AI LENGTH MARK
            AddCodeRange(0x00B55, 0x00B56, Extend);
            // Mn   [2] ORIYA VOWEL SIGN VOCALIC L..ORIYA VOWEL SIGN VOCALIC LL
            AddCodeRange(0x00B62, 0x00B63, Extend);
            // Mc   [2] TAMIL VOWEL SIGN U..TAMIL VOWEL SIGN UU
            AddCodeRange(0x00BC1, 0x00BC2, SpacingMark);
            // Mc   [3] TAMIL VOWEL SIGN E..TAMIL VOWEL SIGN AI
            AddCodeRange(0x00BC6, 0x00BC8, SpacingMark);
            // Mc   [3] TAMIL VOWEL SIGN O..TAMIL VOWEL SIGN AU
            AddCodeRange(0x00BCA, 0x00BCC, SpacingMark);
            // Mc   [3] TELUGU SIGN CANDRABINDU..TELUGU SIGN VISARGA
            AddCodeRange(0x00C01, 0x00C03, SpacingMark);
            // Mn   [3] TELUGU VOWEL SIGN AA..TELUGU VOWEL SIGN II
            AddCodeRange(0x00C3E, 0x00C40, Extend);
            // Mc   [4] TELUGU VOWEL SIGN U..TELUGU VOWEL SIGN VOCALIC RR
            AddCodeRange(0x00C41, 0x00C44, SpacingMark);
            // Mn   [3] TELUGU VOWEL SIGN E..TELUGU VOWEL SIGN AI
            AddCodeRange(0x00C46, 0x00C48, Extend);
            // Mn   [4] TELUGU VOWEL SIGN O..TELUGU SIGN VIRAMA
            AddCodeRange(0x00C4A, 0x00C4D, Extend);
            // Mn   [2] TELUGU LENGTH MARK..TELUGU AI LENGTH MARK
            AddCodeRange(0x00C55, 0x00C56, Extend);
            // Mn   [2] TELUGU VOWEL SIGN VOCALIC L..TELUGU VOWEL SIGN VOCALIC LL
            AddCodeRange(0x00C62, 0x00C63, Extend);
            // Mc   [2] KANNADA SIGN ANUSVARA..KANNADA SIGN VISARGA
            AddCodeRange(0x00C82, 0x00C83, SpacingMark);
            // Mc   [2] KANNADA VOWEL SIGN II..KANNADA VOWEL SIGN U
            AddCodeRange(0x00CC0, 0x00CC1, SpacingMark);
            // Mc   [2] KANNADA VOWEL SIGN VOCALIC R..KANNADA VOWEL SIGN VOCALIC RR
            AddCodeRange(0x00CC3, 0x00CC4, SpacingMark);
            // Mc   [2] KANNADA VOWEL SIGN EE..KANNADA VOWEL SIGN AI
            AddCodeRange(0x00CC7, 0x00CC8, SpacingMark);
            // Mc   [2] KANNADA VOWEL SIGN O..KANNADA VOWEL SIGN OO
            AddCodeRange(0x00CCA, 0x00CCB, SpacingMark);
            // Mn   [2] KANNADA VOWEL SIGN AU..KANNADA SIGN VIRAMA
            AddCodeRange(0x00CCC, 0x00CCD, Extend);
            // Mc   [2] KANNADA LENGTH MARK..KANNADA AI LENGTH MARK
            AddCodeRange(0x00CD5, 0x00CD6, Extend);
            // Mn   [2] KANNADA VOWEL SIGN VOCALIC L..KANNADA VOWEL SIGN VOCALIC LL
            AddCodeRange(0x00CE2, 0x00CE3, Extend);
            // Mn   [2] MALAYALAM SIGN COMBINING ANUSVARA ABOVE..MALAYALAM SIGN CANDRABINDU
            AddCodeRange(0x00D00, 0x00D01, Extend);
            // Mc   [2] MALAYALAM SIGN ANUSVARA..MALAYALAM SIGN VISARGA
            AddCodeRange(0x00D02, 0x00D03, SpacingMark);
            // Mn   [2] MALAYALAM SIGN VERTICAL BAR VIRAMA..MALAYALAM SIGN CIRCULAR VIRAMA
            AddCodeRange(0x00D3B, 0x00D3C, Extend);
            // Mc   [2] MALAYALAM VOWEL SIGN I..MALAYALAM VOWEL SIGN II
            AddCodeRange(0x00D3F, 0x00D40, SpacingMark);
            // Mn   [4] MALAYALAM VOWEL SIGN U..MALAYALAM VOWEL SIGN VOCALIC RR
            AddCodeRange(0x00D41, 0x00D44, Extend);
            // Mc   [3] MALAYALAM VOWEL SIGN E..MALAYALAM VOWEL SIGN AI
            AddCodeRange(0x00D46, 0x00D48, SpacingMark);
            // Mc   [3] MALAYALAM VOWEL SIGN O..MALAYALAM VOWEL SIGN AU
            AddCodeRange(0x00D4A, 0x00D4C, SpacingMark);
            // Mn   [2] MALAYALAM VOWEL SIGN VOCALIC L..MALAYALAM VOWEL SIGN VOCALIC LL
            AddCodeRange(0x00D62, 0x00D63, Extend);
            // Mc   [2] SINHALA SIGN ANUSVARAYA..SINHALA SIGN VISARGAYA
            AddCodeRange(0x00D82, 0x00D83, SpacingMark);
            // Mc   [2] SINHALA VOWEL SIGN KETTI AEDA-PILLA..SINHALA VOWEL SIGN DIGA AEDA-PILLA
            AddCodeRange(0x00DD0, 0x00DD1, SpacingMark);
            // Mn   [3] SINHALA VOWEL SIGN KETTI IS-PILLA..SINHALA VOWEL SIGN KETTI PAA-PILLA
            AddCodeRange(0x00DD2, 0x00DD4, Extend);
            // Mc   [7] SINHALA VOWEL SIGN GAETTA-PILLA..SINHALA VOWEL SIGN KOMBUVA HAA GAYANUKITTA
            AddCodeRange(0x00DD8, 0x00DDE, SpacingMark);
            // Mc   [2] SINHALA VOWEL SIGN DIGA GAETTA-PILLA..SINHALA VOWEL SIGN DIGA GAYANUKITTA
            AddCodeRange(0x00DF2, 0x00DF3, SpacingMark);
            // Mn   [7] THAI CHARACTER SARA I..THAI CHARACTER PHINTHU
            AddCodeRange(0x00E34, 0x00E3A, Extend);
            // Mn   [8] THAI CHARACTER MAITAIKHU..THAI CHARACTER YAMAKKAN
            AddCodeRange(0x00E47, 0x00E4E, Extend);
            // Mn   [9] LAO VOWEL SIGN I..LAO SEMIVOWEL SIGN LO
            AddCodeRange(0x00EB4, 0x00EBC, Extend);
            // Mn   [6] LAO TONE MAI EK..LAO NIGGAHITA
            AddCodeRange(0x00EC8, 0x00ECD, Extend);
            // Mn   [2] TIBETAN ASTROLOGICAL SIGN -KHYUD PA..TIBETAN ASTROLOGICAL SIGN SDONG TSHUGS
            AddCodeRange(0x00F18, 0x00F19, Extend);
            // Mc   [2] TIBETAN SIGN YAR TSHES..TIBETAN SIGN MAR TSHES
            AddCodeRange(0x00F3E, 0x00F3F, SpacingMark);
            // Mn  [14] TIBETAN VOWEL SIGN AA..TIBETAN SIGN RJES SU NGA RO
            AddCodeRange(0x00F71, 0x00F7E, Extend);
            // Mn   [5] TIBETAN VOWEL SIGN REVERSED I..TIBETAN MARK HALANTA
            AddCodeRange(0x00F80, 0x00F84, Extend);
            // Mn   [2] TIBETAN SIGN LCI RTAGS..TIBETAN SIGN YANG RTAGS
            AddCodeRange(0x00F86, 0x00F87, Extend);
            // Mn  [11] TIBETAN SUBJOINED SIGN LCE TSA CAN..TIBETAN SUBJOINED LETTER JA
            AddCodeRange(0x00F8D, 0x00F97, Extend);
            // Mn  [36] TIBETAN SUBJOINED LETTER NYA..TIBETAN SUBJOINED LETTER FIXED-FORM RA
            AddCodeRange(0x00F99, 0x00FBC, Extend);
            // Mn   [4] MYANMAR VOWEL SIGN I..MYANMAR VOWEL SIGN UU
            AddCodeRange(0x0102D, 0x01030, Extend);
            // Mn   [6] MYANMAR VOWEL SIGN AI..MYANMAR SIGN DOT BELOW
            AddCodeRange(0x01032, 0x01037, Extend);
            // Mn   [2] MYANMAR SIGN VIRAMA..MYANMAR SIGN ASAT
            AddCodeRange(0x01039, 0x0103A, Extend);
            // Mc   [2] MYANMAR CONSONANT SIGN MEDIAL YA..MYANMAR CONSONANT SIGN MEDIAL RA
            AddCodeRange(0x0103B, 0x0103C, SpacingMark);
            // Mn   [2] MYANMAR CONSONANT SIGN MEDIAL WA..MYANMAR CONSONANT SIGN MEDIAL HA
            AddCodeRange(0x0103D, 0x0103E, Extend);
            // Mc   [2] MYANMAR VOWEL SIGN VOCALIC R..MYANMAR VOWEL SIGN VOCALIC RR
            AddCodeRange(0x01056, 0x01057, SpacingMark);
            // Mn   [2] MYANMAR VOWEL SIGN VOCALIC L..MYANMAR VOWEL SIGN VOCALIC LL
            AddCodeRange(0x01058, 0x01059, Extend);
            // Mn   [3] MYANMAR CONSONANT SIGN MON MEDIAL NA..MYANMAR CONSONANT SIGN MON MEDIAL LA
            AddCodeRange(0x0105E, 0x01060, Extend);
            // Mn   [4] MYANMAR VOWEL SIGN GEBA KAREN I..MYANMAR VOWEL SIGN KAYAH EE
            AddCodeRange(0x01071, 0x01074, Extend);
            // Mn   [2] MYANMAR VOWEL SIGN SHAN E ABOVE..MYANMAR VOWEL SIGN SHAN FINAL Y
            AddCodeRange(0x01085, 0x01086, Extend);
            // Lo  [96] HANGUL CHOSEONG KIYEOK..HANGUL CHOSEONG FILLER
            AddCodeRange(0x01100, 0x0115F, L);
            // Lo  [72] HANGUL JUNGSEONG FILLER..HANGUL JUNGSEONG O-YAE
            AddCodeRange(0x01160, 0x011A7, V);
            // Lo  [88] HANGUL JONGSEONG KIYEOK..HANGUL JONGSEONG SSANGNIEUN
            AddCodeRange(0x011A8, 0x011FF, T);
            // Mn   [3] ETHIOPIC COMBINING GEMINATION AND VOWEL LENGTH MARK..ETHIOPIC COMBINING GEMINATION MARK
            AddCodeRange(0x0135D, 0x0135F, Extend);
            // Mn   [3] TAGALOG VOWEL SIGN I..TAGALOG SIGN VIRAMA
            AddCodeRange(0x01712, 0x01714, Extend);
            // Mn   [2] HANUNOO VOWEL SIGN I..HANUNOO VOWEL SIGN U
            AddCodeRange(0x01732, 0x01733, Extend);
            // Mn   [2] BUHID VOWEL SIGN I..BUHID VOWEL SIGN U
            AddCodeRange(0x01752, 0x01753, Extend);
            // Mn   [2] TAGBANWA VOWEL SIGN I..TAGBANWA VOWEL SIGN U
            AddCodeRange(0x01772, 0x01773, Extend);
            // Mn   [2] KHMER VOWEL INHERENT AQ..KHMER VOWEL INHERENT AA
            AddCodeRange(0x017B4, 0x017B5, Extend);
            // Mn   [7] KHMER VOWEL SIGN I..KHMER VOWEL SIGN UA
            AddCodeRange(0x017B7, 0x017BD, Extend);
            // Mc   [8] KHMER VOWEL SIGN OE..KHMER VOWEL SIGN AU
            AddCodeRange(0x017BE, 0x017C5, SpacingMark);
            // Mc   [2] KHMER SIGN REAHMUK..KHMER SIGN YUUKALEAPINTU
            AddCodeRange(0x017C7, 0x017C8, SpacingMark);
            // Mn  [11] KHMER SIGN MUUSIKATOAN..KHMER SIGN BATHAMASAT
            AddCodeRange(0x017C9, 0x017D3, Extend);
            // Mn   [3] MONGOLIAN FREE VARIATION SELECTOR ONE..MONGOLIAN FREE VARIATION SELECTOR THREE
            AddCodeRange(0x0180B, 0x0180D, Extend);
            // Mn   [2] MONGOLIAN LETTER ALI GALI BALUDA..MONGOLIAN LETTER ALI GALI THREE BALUDA
            AddCodeRange(0x01885, 0x01886, Extend);
            // Mn   [3] LIMBU VOWEL SIGN A..LIMBU VOWEL SIGN U
            AddCodeRange(0x01920, 0x01922, Extend);
            // Mc   [4] LIMBU VOWEL SIGN EE..LIMBU VOWEL SIGN AU
            AddCodeRange(0x01923, 0x01926, SpacingMark);
            // Mn   [2] LIMBU VOWEL SIGN E..LIMBU VOWEL SIGN O
            AddCodeRange(0x01927, 0x01928, Extend);
            // Mc   [3] LIMBU SUBJOINED LETTER YA..LIMBU SUBJOINED LETTER WA
            AddCodeRange(0x01929, 0x0192B, SpacingMark);
            // Mc   [2] LIMBU SMALL LETTER KA..LIMBU SMALL LETTER NGA
            AddCodeRange(0x01930, 0x01931, SpacingMark);
            // Mc   [6] LIMBU SMALL LETTER TA..LIMBU SMALL LETTER LA
            AddCodeRange(0x01933, 0x01938, SpacingMark);
            // Mn   [3] LIMBU SIGN MUKPHRENG..LIMBU SIGN SA-I
            AddCodeRange(0x01939, 0x0193B, Extend);
            // Mn   [2] BUGINESE VOWEL SIGN I..BUGINESE VOWEL SIGN U
            AddCodeRange(0x01A17, 0x01A18, Extend);
            // Mc   [2] BUGINESE VOWEL SIGN E..BUGINESE VOWEL SIGN O
            AddCodeRange(0x01A19, 0x01A1A, SpacingMark);
            // Mn   [7] TAI THAM SIGN MAI KANG LAI..TAI THAM CONSONANT SIGN SA
            AddCodeRange(0x01A58, 0x01A5E, Extend);
            // Mn   [8] TAI THAM VOWEL SIGN I..TAI THAM VOWEL SIGN OA BELOW
            AddCodeRange(0x01A65, 0x01A6C, Extend);
            // Mc   [6] TAI THAM VOWEL SIGN OY..TAI THAM VOWEL SIGN THAM AI
            AddCodeRange(0x01A6D, 0x01A72, SpacingMark);
            // Mn  [10] TAI THAM VOWEL SIGN OA ABOVE..TAI THAM SIGN KHUEN-LUE KARAN
            AddCodeRange(0x01A73, 0x01A7C, Extend);
            // Mn  [14] COMBINING DOUBLED CIRCUMFLEX ACCENT..COMBINING PARENTHESES BELOW
            AddCodeRange(0x01AB0, 0x01ABD, Extend);
            // Mn  [16] COMBINING LATIN SMALL LETTER W BELOW..COMBINING LATIN SMALL LETTER INSULAR T
            AddCodeRange(0x01ABF, 0x01ACE, Extend);
            // Mn   [4] BALINESE SIGN ULU RICEM..BALINESE SIGN SURANG
            AddCodeRange(0x01B00, 0x01B03, Extend);
            // Mn   [5] BALINESE VOWEL SIGN ULU..BALINESE VOWEL SIGN RA REPA
            AddCodeRange(0x01B36, 0x01B3A, Extend);
            // Mc   [5] BALINESE VOWEL SIGN LA LENGA TEDUNG..BALINESE VOWEL SIGN TALING REPA TEDUNG
            AddCodeRange(0x01B3D, 0x01B41, SpacingMark);
            // Mc   [2] BALINESE VOWEL SIGN PEPET TEDUNG..BALINESE ADEG ADEG
            AddCodeRange(0x01B43, 0x01B44, SpacingMark);
            // Mn   [9] BALINESE MUSICAL SYMBOL COMBINING TEGEH..BALINESE MUSICAL SYMBOL COMBINING GONG
            AddCodeRange(0x01B6B, 0x01B73, Extend);
            // Mn   [2] SUNDANESE SIGN PANYECEK..SUNDANESE SIGN PANGLAYAR
            AddCodeRange(0x01B80, 0x01B81, Extend);
            // Mn   [4] SUNDANESE CONSONANT SIGN PANYAKRA..SUNDANESE VOWEL SIGN PANYUKU
            AddCodeRange(0x01BA2, 0x01BA5, Extend);
            // Mc   [2] SUNDANESE VOWEL SIGN PANAELAENG..SUNDANESE VOWEL SIGN PANOLONG
            AddCodeRange(0x01BA6, 0x01BA7, SpacingMark);
            // Mn   [2] SUNDANESE VOWEL SIGN PAMEPET..SUNDANESE VOWEL SIGN PANEULEUNG
            AddCodeRange(0x01BA8, 0x01BA9, Extend);
            // Mn   [3] SUNDANESE SIGN VIRAMA..SUNDANESE CONSONANT SIGN PASANGAN WA
            AddCodeRange(0x01BAB, 0x01BAD, Extend);
            // Mn   [2] BATAK VOWEL SIGN PAKPAK E..BATAK VOWEL SIGN EE
            AddCodeRange(0x01BE8, 0x01BE9, Extend);
            // Mc   [3] BATAK VOWEL SIGN I..BATAK VOWEL SIGN O
            AddCodeRange(0x01BEA, 0x01BEC, SpacingMark);
            // Mn   [3] BATAK VOWEL SIGN U FOR SIMALUNGUN SA..BATAK CONSONANT SIGN H
            AddCodeRange(0x01BEF, 0x01BF1, Extend);
            // Mc   [2] BATAK PANGOLAT..BATAK PANONGONAN
            AddCodeRange(0x01BF2, 0x01BF3, SpacingMark);
            // Mc   [8] LEPCHA SUBJOINED LETTER YA..LEPCHA VOWEL SIGN UU
            AddCodeRange(0x01C24, 0x01C2B, SpacingMark);
            // Mn   [8] LEPCHA VOWEL SIGN E..LEPCHA CONSONANT SIGN T
            AddCodeRange(0x01C2C, 0x01C33, Extend);
            // Mc   [2] LEPCHA CONSONANT SIGN NYIN-DO..LEPCHA CONSONANT SIGN KANG
            AddCodeRange(0x01C34, 0x01C35, SpacingMark);
            // Mn   [2] LEPCHA SIGN RAN..LEPCHA SIGN NUKTA
            AddCodeRange(0x01C36, 0x01C37, Extend);
            // Mn   [3] VEDIC TONE KARSHANA..VEDIC TONE PRENKHA
            AddCodeRange(0x01CD0, 0x01CD2, Extend);
            // Mn  [13] VEDIC SIGN YAJURVEDIC MIDLINE SVARITA..VEDIC TONE RIGVEDIC KASHMIRI INDEPENDENT SVARITA
            AddCodeRange(0x01CD4, 0x01CE0, Extend);
            // Mn   [7] VEDIC SIGN VISARGA SVARITA..VEDIC SIGN VISARGA ANUDATTA WITH TAIL
            AddCodeRange(0x01CE2, 0x01CE8, Extend);
            // Mn   [2] VEDIC TONE RING ABOVE..VEDIC TONE DOUBLE RING ABOVE
            AddCodeRange(0x01CF8, 0x01CF9, Extend);
            // Mn  [64] COMBINING DOTTED GRAVE ACCENT..COMBINING RIGHT ARROWHEAD AND DOWN ARROWHEAD BELOW
            AddCodeRange(0x01DC0, 0x01DFF, Extend);
            // Cf   [2] LEFT-TO-RIGHT MARK..RIGHT-TO-LEFT MARK
            AddCodeRange(0x0200E, 0x0200F, Control);
            // Cf   [5] LEFT-TO-RIGHT EMBEDDING..RIGHT-TO-LEFT OVERRIDE
            AddCodeRange(0x0202A, 0x0202E, Control);
            // Cf   [5] WORD JOINER..INVISIBLE PLUS
            AddCodeRange(0x02060, 0x02064, Control);
            // Cf  [10] LEFT-TO-RIGHT ISOLATE..NOMINAL DIGIT SHAPES
            AddCodeRange(0x02066, 0x0206F, Control);
            // Mn  [13] COMBINING LEFT HARPOON ABOVE..COMBINING FOUR DOTS ABOVE
            AddCodeRange(0x020D0, 0x020DC, Extend);
            // Me   [4] COMBINING ENCLOSING CIRCLE..COMBINING ENCLOSING CIRCLE BACKSLASH
            AddCodeRange(0x020DD, 0x020E0, Extend);
            // Me   [3] COMBINING ENCLOSING SCREEN..COMBINING ENCLOSING UPWARD POINTING TRIANGLE
            AddCodeRange(0x020E2, 0x020E4, Extend);
            // Mn  [12] COMBINING REVERSE SOLIDUS OVERLAY..COMBINING ASTERISK ABOVE
            AddCodeRange(0x020E5, 0x020F0, Extend);
            // E0.6   [6] (↔️..↙️)    left-right arrow..down-left arrow
            AddCodeRange(0x02194, 0x02199, Extended_Pictographic);
            // E0.6   [2] (↩️..↪️)    right arrow curving left..left arrow curving right
            AddCodeRange(0x021A9, 0x021AA, Extended_Pictographic);
            // E0.6   [2] (⌚..⌛)    watch..hourglass done
            AddCodeRange(0x0231A, 0x0231B, Extended_Pictographic);
            // E0.6   [4] (⏩..⏬)    fast-forward button..fast down button
            AddCodeRange(0x023E9, 0x023EC, Extended_Pictographic);
            // E0.7   [2] (⏭️..⏮️)    next track button..last track button
            AddCodeRange(0x023ED, 0x023EE, Extended_Pictographic);
            // E1.0   [2] (⏱️..⏲️)    stopwatch..timer clock
            AddCodeRange(0x023F1, 0x023F2, Extended_Pictographic);
            // E0.7   [3] (⏸️..⏺️)    pause button..record button
            AddCodeRange(0x023F8, 0x023FA, Extended_Pictographic);
            // E0.6   [2] (▪️..▫️)    black small square..white small square
            AddCodeRange(0x025AA, 0x025AB, Extended_Pictographic);
            // E0.6   [4] (◻️..◾)    white medium square..black medium-small square
            AddCodeRange(0x025FB, 0x025FE, Extended_Pictographic);
            // E0.6   [2] (☀️..☁️)    sun..cloud
            AddCodeRange(0x02600, 0x02601, Extended_Pictographic);
            // E0.7   [2] (☂️..☃️)    umbrella..snowman
            AddCodeRange(0x02602, 0x02603, Extended_Pictographic);
            // E0.0   [7] (☇..☍)    LIGHTNING..OPPOSITION
            AddCodeRange(0x02607, 0x0260D, Extended_Pictographic);
            // E0.0   [2] (☏..☐)    WHITE TELEPHONE..BALLOT BOX
            AddCodeRange(0x0260F, 0x02610, Extended_Pictographic);
            // E0.6   [2] (☔..☕)    umbrella with rain drops..hot beverage
            AddCodeRange(0x02614, 0x02615, Extended_Pictographic);
            // E0.0   [2] (☖..☗)    WHITE SHOGI PIECE..BLACK SHOGI PIECE
            AddCodeRange(0x02616, 0x02617, Extended_Pictographic);
            // E0.0   [4] (☙..☜)    REVERSED ROTATED FLORAL HEART BULLET..WHITE LEFT POINTING INDEX
            AddCodeRange(0x02619, 0x0261C, Extended_Pictographic);
            // E0.0   [2] (☞..☟)    WHITE RIGHT POINTING INDEX..WHITE DOWN POINTING INDEX
            AddCodeRange(0x0261E, 0x0261F, Extended_Pictographic);
            // E1.0   [2] (☢️..☣️)    radioactive..biohazard
            AddCodeRange(0x02622, 0x02623, Extended_Pictographic);
            // E0.0   [2] (☤..☥)    CADUCEUS..ANKH
            AddCodeRange(0x02624, 0x02625, Extended_Pictographic);
            // E0.0   [3] (☧..☩)    CHI RHO..CROSS OF JERUSALEM
            AddCodeRange(0x02627, 0x02629, Extended_Pictographic);
            // E0.0   [3] (☫..☭)    FARSI SYMBOL..HAMMER AND SICKLE
            AddCodeRange(0x0262B, 0x0262D, Extended_Pictographic);
            // E0.0   [8] (☰..☷)    TRIGRAM FOR HEAVEN..TRIGRAM FOR EARTH
            AddCodeRange(0x02630, 0x02637, Extended_Pictographic);
            // E0.7   [2] (☸️..☹️)    wheel of dharma..frowning face
            AddCodeRange(0x02638, 0x02639, Extended_Pictographic);
            // E0.0   [5] (☻..☿)    BLACK SMILING FACE..MERCURY
            AddCodeRange(0x0263B, 0x0263F, Extended_Pictographic);
            // E0.0   [5] (♃..♇)    JUPITER..PLUTO
            AddCodeRange(0x02643, 0x02647, Extended_Pictographic);
            // E0.6  [12] (♈..♓)    Aries..Pisces
            AddCodeRange(0x02648, 0x02653, Extended_Pictographic);
            // E0.0  [11] (♔..♞)    WHITE CHESS KING..BLACK CHESS KNIGHT
            AddCodeRange(0x02654, 0x0265E, Extended_Pictographic);
            // E0.0   [2] (♡..♢)    WHITE HEART SUIT..WHITE DIAMOND SUIT
            AddCodeRange(0x02661, 0x02662, Extended_Pictographic);
            // E0.6   [2] (♥️..♦️)    heart suit..diamond suit
            AddCodeRange(0x02665, 0x02666, Extended_Pictographic);
            // E0.0  [18] (♩..♺)    QUARTER NOTE..RECYCLING SYMBOL FOR GENERIC MATERIALS
            AddCodeRange(0x02669, 0x0267A, Extended_Pictographic);
            // E0.0   [2] (♼..♽)    RECYCLED PAPER SYMBOL..PARTIALLY-RECYCLED PAPER SYMBOL
            AddCodeRange(0x0267C, 0x0267D, Extended_Pictographic);
            // E0.0   [6] (⚀..⚅)    DIE FACE-1..DIE FACE-6
            AddCodeRange(0x02680, 0x02685, Extended_Pictographic);
            // E0.0   [2] (⚐..⚑)    WHITE FLAG..BLACK FLAG
            AddCodeRange(0x02690, 0x02691, Extended_Pictographic);
            // E1.0   [2] (⚖️..⚗️)    balance scale..alembic
            AddCodeRange(0x02696, 0x02697, Extended_Pictographic);
            // E1.0   [2] (⚛️..⚜️)    atom symbol..fleur-de-lis
            AddCodeRange(0x0269B, 0x0269C, Extended_Pictographic);
            // E0.0   [3] (⚝..⚟)    OUTLINED WHITE STAR..THREE LINES CONVERGING LEFT
            AddCodeRange(0x0269D, 0x0269F, Extended_Pictographic);
            // E0.6   [2] (⚠️..⚡)    warning..high voltage
            AddCodeRange(0x026A0, 0x026A1, Extended_Pictographic);
            // E0.0   [5] (⚢..⚦)    DOUBLED FEMALE SIGN..MALE WITH STROKE SIGN
            AddCodeRange(0x026A2, 0x026A6, Extended_Pictographic);
            // E0.0   [2] (⚨..⚩)    VERTICAL MALE WITH STROKE SIGN..HORIZONTAL MALE WITH STROKE SIGN
            AddCodeRange(0x026A8, 0x026A9, Extended_Pictographic);
            // E0.6   [2] (⚪..⚫)    white circle..black circle
            AddCodeRange(0x026AA, 0x026AB, Extended_Pictographic);
            // E0.0   [4] (⚬..⚯)    MEDIUM SMALL WHITE CIRCLE..UNMARRIED PARTNERSHIP SYMBOL
            AddCodeRange(0x026AC, 0x026AF, Extended_Pictographic);
            // E1.0   [2] (⚰️..⚱️)    coffin..funeral urn
            AddCodeRange(0x026B0, 0x026B1, Extended_Pictographic);
            // E0.0  [11] (⚲..⚼)    NEUTER..SESQUIQUADRATE
            AddCodeRange(0x026B2, 0x026BC, Extended_Pictographic);
            // E0.6   [2] (⚽..⚾)    soccer ball..baseball
            AddCodeRange(0x026BD, 0x026BE, Extended_Pictographic);
            // E0.0   [5] (⚿..⛃)    SQUARED KEY..BLACK DRAUGHTS KING
            AddCodeRange(0x026BF, 0x026C3, Extended_Pictographic);
            // E0.6   [2] (⛄..⛅)    snowman without snow..sun behind cloud
            AddCodeRange(0x026C4, 0x026C5, Extended_Pictographic);
            // E0.0   [2] (⛆..⛇)    RAIN..BLACK SNOWMAN
            AddCodeRange(0x026C6, 0x026C7, Extended_Pictographic);
            // E0.0   [5] (⛉..⛍)    TURNED WHITE SHOGI PIECE..DISABLED CAR
            AddCodeRange(0x026C9, 0x026CD, Extended_Pictographic);
            // E0.0  [20] (⛕..⛨)    ALTERNATE ONE-WAY LEFT WAY TRAFFIC..BLACK CROSS ON SHIELD
            AddCodeRange(0x026D5, 0x026E8, Extended_Pictographic);
            // E0.0   [5] (⛫..⛯)    CASTLE..MAP SYMBOL FOR LIGHTHOUSE
            AddCodeRange(0x026EB, 0x026EF, Extended_Pictographic);
            // E0.7   [2] (⛰️..⛱️)    mountain..umbrella on ground
            AddCodeRange(0x026F0, 0x026F1, Extended_Pictographic);
            // E0.6   [2] (⛲..⛳)    fountain..flag in hole
            AddCodeRange(0x026F2, 0x026F3, Extended_Pictographic);
            // E0.7   [3] (⛷️..⛹️)    skier..person bouncing ball
            AddCodeRange(0x026F7, 0x026F9, Extended_Pictographic);
            // E0.0   [2] (⛻..⛼)    JAPANESE BANK SYMBOL..HEADSTONE GRAVEYARD SYMBOL
            AddCodeRange(0x026FB, 0x026FC, Extended_Pictographic);
            // E0.0   [4] (⛾..✁)    CUP ON BLACK SQUARE..UPPER BLADE SCISSORS
            AddCodeRange(0x026FE, 0x02701, Extended_Pictographic);
            // E0.0   [2] (✃..✄)    LOWER BLADE SCISSORS..WHITE SCISSORS
            AddCodeRange(0x02703, 0x02704, Extended_Pictographic);
            // E0.6   [5] (✈️..✌️)    airplane..victory hand
            AddCodeRange(0x02708, 0x0270C, Extended_Pictographic);
            // E0.0   [2] (✐..✑)    UPPER RIGHT PENCIL..WHITE NIB
            AddCodeRange(0x02710, 0x02711, Extended_Pictographic);
            // E0.6   [2] (✳️..✴️)    eight-spoked asterisk..eight-pointed star
            AddCodeRange(0x02733, 0x02734, Extended_Pictographic);
            // E0.6   [3] (❓..❕)    red question mark..white exclamation mark
            AddCodeRange(0x02753, 0x02755, Extended_Pictographic);
            // E0.0   [3] (❥..❧)    ROTATED HEAVY BLACK HEART BULLET..ROTATED FLORAL HEART BULLET
            AddCodeRange(0x02765, 0x02767, Extended_Pictographic);
            // E0.6   [3] (➕..➗)    plus..divide
            AddCodeRange(0x02795, 0x02797, Extended_Pictographic);
            // E0.6   [2] (⤴️..⤵️)    right arrow curving up..right arrow curving down
            AddCodeRange(0x02934, 0x02935, Extended_Pictographic);
            // E0.6   [3] (⬅️..⬇️)    left arrow..down arrow
            AddCodeRange(0x02B05, 0x02B07, Extended_Pictographic);
            // E0.6   [2] (⬛..⬜)    black large square..white large square
            AddCodeRange(0x02B1B, 0x02B1C, Extended_Pictographic);
            // Mn   [3] COPTIC COMBINING NI ABOVE..COPTIC COMBINING SPIRITUS LENIS
            AddCodeRange(0x02CEF, 0x02CF1, Extend);
            // Mn  [32] COMBINING CYRILLIC LETTER BE..COMBINING CYRILLIC LETTER IOTIFIED BIG YUS
            AddCodeRange(0x02DE0, 0x02DFF, Extend);
            // Mn   [4] IDEOGRAPHIC LEVEL TONE MARK..IDEOGRAPHIC ENTERING TONE MARK
            AddCodeRange(0x0302A, 0x0302D, Extend);
            // Mc   [2] HANGUL SINGLE DOT TONE MARK..HANGUL DOUBLE DOT TONE MARK
            AddCodeRange(0x0302E, 0x0302F, Extend);
            // Mn   [2] COMBINING KATAKANA-HIRAGANA VOICED SOUND MARK..COMBINING KATAKANA-HIRAGANA SEMI-VOICED SOUND MARK
            AddCodeRange(0x03099, 0x0309A, Extend);
            // Me   [3] COMBINING CYRILLIC TEN MILLIONS SIGN..COMBINING CYRILLIC THOUSAND MILLIONS SIGN
            AddCodeRange(0x0A670, 0x0A672, Extend);
            // Mn  [10] COMBINING CYRILLIC LETTER UKRAINIAN IE..COMBINING CYRILLIC PAYEROK
            AddCodeRange(0x0A674, 0x0A67D, Extend);
            // Mn   [2] COMBINING CYRILLIC LETTER EF..COMBINING CYRILLIC LETTER IOTIFIED E
            AddCodeRange(0x0A69E, 0x0A69F, Extend);
            // Mn   [2] BAMUM COMBINING MARK KOQNDON..BAMUM COMBINING MARK TUKWENTIS
            AddCodeRange(0x0A6F0, 0x0A6F1, Extend);
            // Mc   [2] SYLOTI NAGRI VOWEL SIGN A..SYLOTI NAGRI VOWEL SIGN I
            AddCodeRange(0x0A823, 0x0A824, SpacingMark);
            // Mn   [2] SYLOTI NAGRI VOWEL SIGN U..SYLOTI NAGRI VOWEL SIGN E
            AddCodeRange(0x0A825, 0x0A826, Extend);
            // Mc   [2] SAURASHTRA SIGN ANUSVARA..SAURASHTRA SIGN VISARGA
            AddCodeRange(0x0A880, 0x0A881, SpacingMark);
            // Mc  [16] SAURASHTRA CONSONANT SIGN HAARU..SAURASHTRA VOWEL SIGN AU
            AddCodeRange(0x0A8B4, 0x0A8C3, SpacingMark);
            // Mn   [2] SAURASHTRA SIGN VIRAMA..SAURASHTRA SIGN CANDRABINDU
            AddCodeRange(0x0A8C4, 0x0A8C5, Extend);
            // Mn  [18] COMBINING DEVANAGARI DIGIT ZERO..COMBINING DEVANAGARI SIGN AVAGRAHA
            AddCodeRange(0x0A8E0, 0x0A8F1, Extend);
            // Mn   [8] KAYAH LI VOWEL UE..KAYAH LI TONE CALYA PLOPHU
            AddCodeRange(0x0A926, 0x0A92D, Extend);
            // Mn  [11] REJANG VOWEL SIGN I..REJANG CONSONANT SIGN R
            AddCodeRange(0x0A947, 0x0A951, Extend);
            // Mc   [2] REJANG CONSONANT SIGN H..REJANG VIRAMA
            AddCodeRange(0x0A952, 0x0A953, SpacingMark);
            // Lo  [29] HANGUL CHOSEONG TIKEUT-MIEUM..HANGUL CHOSEONG SSANGYEORINHIEUH
            AddCodeRange(0x0A960, 0x0A97C, L);
            // Mn   [3] JAVANESE SIGN PANYANGGA..JAVANESE SIGN LAYAR
            AddCodeRange(0x0A980, 0x0A982, Extend);
            // Mc   [2] JAVANESE VOWEL SIGN TARUNG..JAVANESE VOWEL SIGN TOLONG
            AddCodeRange(0x0A9B4, 0x0A9B5, SpacingMark);
            // Mn   [4] JAVANESE VOWEL SIGN WULU..JAVANESE VOWEL SIGN SUKU MENDUT
            AddCodeRange(0x0A9B6, 0x0A9B9, Extend);
            // Mc   [2] JAVANESE VOWEL SIGN TALING..JAVANESE VOWEL SIGN DIRGA MURE
            AddCodeRange(0x0A9BA, 0x0A9BB, SpacingMark);
            // Mn   [2] JAVANESE VOWEL SIGN PEPET..JAVANESE CONSONANT SIGN KERET
            AddCodeRange(0x0A9BC, 0x0A9BD, Extend);
            // Mc   [3] JAVANESE CONSONANT SIGN PENGKAL..JAVANESE PANGKON
            AddCodeRange(0x0A9BE, 0x0A9C0, SpacingMark);
            // Mn   [6] CHAM VOWEL SIGN AA..CHAM VOWEL SIGN OE
            AddCodeRange(0x0AA29, 0x0AA2E, Extend);
            // Mc   [2] CHAM VOWEL SIGN O..CHAM VOWEL SIGN AI
            AddCodeRange(0x0AA2F, 0x0AA30, SpacingMark);
            // Mn   [2] CHAM VOWEL SIGN AU..CHAM VOWEL SIGN UE
            AddCodeRange(0x0AA31, 0x0AA32, Extend);
            // Mc   [2] CHAM CONSONANT SIGN YA..CHAM CONSONANT SIGN RA
            AddCodeRange(0x0AA33, 0x0AA34, SpacingMark);
            // Mn   [2] CHAM CONSONANT SIGN LA..CHAM CONSONANT SIGN WA
            AddCodeRange(0x0AA35, 0x0AA36, Extend);
            // Mn   [3] TAI VIET VOWEL I..TAI VIET VOWEL U
            AddCodeRange(0x0AAB2, 0x0AAB4, Extend);
            // Mn   [2] TAI VIET MAI KHIT..TAI VIET VOWEL IA
            AddCodeRange(0x0AAB7, 0x0AAB8, Extend);
            // Mn   [2] TAI VIET VOWEL AM..TAI VIET TONE MAI EK
            AddCodeRange(0x0AABE, 0x0AABF, Extend);
            // Mn   [2] MEETEI MAYEK VOWEL SIGN UU..MEETEI MAYEK VOWEL SIGN AAI
            AddCodeRange(0x0AAEC, 0x0AAED, Extend);
            // Mc   [2] MEETEI MAYEK VOWEL SIGN AU..MEETEI MAYEK VOWEL SIGN AAU
            AddCodeRange(0x0AAEE, 0x0AAEF, SpacingMark);
            // Mc   [2] MEETEI MAYEK VOWEL SIGN ONAP..MEETEI MAYEK VOWEL SIGN INAP
            AddCodeRange(0x0ABE3, 0x0ABE4, SpacingMark);
            // Mc   [2] MEETEI MAYEK VOWEL SIGN YENAP..MEETEI MAYEK VOWEL SIGN SOUNAP
            AddCodeRange(0x0ABE6, 0x0ABE7, SpacingMark);
            // Mc   [2] MEETEI MAYEK VOWEL SIGN CHEINAP..MEETEI MAYEK VOWEL SIGN NUNG
            AddCodeRange(0x0ABE9, 0x0ABEA, SpacingMark);
            // Lo  [27] HANGUL SYLLABLE GAG..HANGUL SYLLABLE GAH
            AddCodeRange(0x0AC01, 0x0AC1B, LVT);
            // Lo  [27] HANGUL SYLLABLE GAEG..HANGUL SYLLABLE GAEH
            AddCodeRange(0x0AC1D, 0x0AC37, LVT);
            // Lo  [27] HANGUL SYLLABLE GYAG..HANGUL SYLLABLE GYAH
            AddCodeRange(0x0AC39, 0x0AC53, LVT);
            // Lo  [27] HANGUL SYLLABLE GYAEG..HANGUL SYLLABLE GYAEH
            AddCodeRange(0x0AC55, 0x0AC6F, LVT);
            // Lo  [27] HANGUL SYLLABLE GEOG..HANGUL SYLLABLE GEOH
            AddCodeRange(0x0AC71, 0x0AC8B, LVT);
            // Lo  [27] HANGUL SYLLABLE GEG..HANGUL SYLLABLE GEH
            AddCodeRange(0x0AC8D, 0x0ACA7, LVT);
            // Lo  [27] HANGUL SYLLABLE GYEOG..HANGUL SYLLABLE GYEOH
            AddCodeRange(0x0ACA9, 0x0ACC3, LVT);
            // Lo  [27] HANGUL SYLLABLE GYEG..HANGUL SYLLABLE GYEH
            AddCodeRange(0x0ACC5, 0x0ACDF, LVT);
            // Lo  [27] HANGUL SYLLABLE GOG..HANGUL SYLLABLE GOH
            AddCodeRange(0x0ACE1, 0x0ACFB, LVT);
            // Lo  [27] HANGUL SYLLABLE GWAG..HANGUL SYLLABLE GWAH
            AddCodeRange(0x0ACFD, 0x0AD17, LVT);
            // Lo  [27] HANGUL SYLLABLE GWAEG..HANGUL SYLLABLE GWAEH
            AddCodeRange(0x0AD19, 0x0AD33, LVT);
            // Lo  [27] HANGUL SYLLABLE GOEG..HANGUL SYLLABLE GOEH
            AddCodeRange(0x0AD35, 0x0AD4F, LVT);
            // Lo  [27] HANGUL SYLLABLE GYOG..HANGUL SYLLABLE GYOH
            AddCodeRange(0x0AD51, 0x0AD6B, LVT);
            // Lo  [27] HANGUL SYLLABLE GUG..HANGUL SYLLABLE GUH
            AddCodeRange(0x0AD6D, 0x0AD87, LVT);
            // Lo  [27] HANGUL SYLLABLE GWEOG..HANGUL SYLLABLE GWEOH
            AddCodeRange(0x0AD89, 0x0ADA3, LVT);
            // Lo  [27] HANGUL SYLLABLE GWEG..HANGUL SYLLABLE GWEH
            AddCodeRange(0x0ADA5, 0x0ADBF, LVT);
            // Lo  [27] HANGUL SYLLABLE GWIG..HANGUL SYLLABLE GWIH
            AddCodeRange(0x0ADC1, 0x0ADDB, LVT);
            // Lo  [27] HANGUL SYLLABLE GYUG..HANGUL SYLLABLE GYUH
            AddCodeRange(0x0ADDD, 0x0ADF7, LVT);
            // Lo  [27] HANGUL SYLLABLE GEUG..HANGUL SYLLABLE GEUH
            AddCodeRange(0x0ADF9, 0x0AE13, LVT);
            // Lo  [27] HANGUL SYLLABLE GYIG..HANGUL SYLLABLE GYIH
            AddCodeRange(0x0AE15, 0x0AE2F, LVT);
            // Lo  [27] HANGUL SYLLABLE GIG..HANGUL SYLLABLE GIH
            AddCodeRange(0x0AE31, 0x0AE4B, LVT);
            // Lo  [27] HANGUL SYLLABLE GGAG..HANGUL SYLLABLE GGAH
            AddCodeRange(0x0AE4D, 0x0AE67, LVT);
            // Lo  [27] HANGUL SYLLABLE GGAEG..HANGUL SYLLABLE GGAEH
            AddCodeRange(0x0AE69, 0x0AE83, LVT);
            // Lo  [27] HANGUL SYLLABLE GGYAG..HANGUL SYLLABLE GGYAH
            AddCodeRange(0x0AE85, 0x0AE9F, LVT);
            // Lo  [27] HANGUL SYLLABLE GGYAEG..HANGUL SYLLABLE GGYAEH
            AddCodeRange(0x0AEA1, 0x0AEBB, LVT);
            // Lo  [27] HANGUL SYLLABLE GGEOG..HANGUL SYLLABLE GGEOH
            AddCodeRange(0x0AEBD, 0x0AED7, LVT);
            // Lo  [27] HANGUL SYLLABLE GGEG..HANGUL SYLLABLE GGEH
            AddCodeRange(0x0AED9, 0x0AEF3, LVT);
            // Lo  [27] HANGUL SYLLABLE GGYEOG..HANGUL SYLLABLE GGYEOH
            AddCodeRange(0x0AEF5, 0x0AF0F, LVT);
            // Lo  [27] HANGUL SYLLABLE GGYEG..HANGUL SYLLABLE GGYEH
            AddCodeRange(0x0AF11, 0x0AF2B, LVT);
            // Lo  [27] HANGUL SYLLABLE GGOG..HANGUL SYLLABLE GGOH
            AddCodeRange(0x0AF2D, 0x0AF47, LVT);
            // Lo  [27] HANGUL SYLLABLE GGWAG..HANGUL SYLLABLE GGWAH
            AddCodeRange(0x0AF49, 0x0AF63, LVT);
            // Lo  [27] HANGUL SYLLABLE GGWAEG..HANGUL SYLLABLE GGWAEH
            AddCodeRange(0x0AF65, 0x0AF7F, LVT);
            // Lo  [27] HANGUL SYLLABLE GGOEG..HANGUL SYLLABLE GGOEH
            AddCodeRange(0x0AF81, 0x0AF9B, LVT);
            // Lo  [27] HANGUL SYLLABLE GGYOG..HANGUL SYLLABLE GGYOH
            AddCodeRange(0x0AF9D, 0x0AFB7, LVT);
            // Lo  [27] HANGUL SYLLABLE GGUG..HANGUL SYLLABLE GGUH
            AddCodeRange(0x0AFB9, 0x0AFD3, LVT);
            // Lo  [27] HANGUL SYLLABLE GGWEOG..HANGUL SYLLABLE GGWEOH
            AddCodeRange(0x0AFD5, 0x0AFEF, LVT);
            // Lo  [27] HANGUL SYLLABLE GGWEG..HANGUL SYLLABLE GGWEH
            AddCodeRange(0x0AFF1, 0x0B00B, LVT);
            // Lo  [27] HANGUL SYLLABLE GGWIG..HANGUL SYLLABLE GGWIH
            AddCodeRange(0x0B00D, 0x0B027, LVT);
            // Lo  [27] HANGUL SYLLABLE GGYUG..HANGUL SYLLABLE GGYUH
            AddCodeRange(0x0B029, 0x0B043, LVT);
            // Lo  [27] HANGUL SYLLABLE GGEUG..HANGUL SYLLABLE GGEUH
            AddCodeRange(0x0B045, 0x0B05F, LVT);
            // Lo  [27] HANGUL SYLLABLE GGYIG..HANGUL SYLLABLE GGYIH
            AddCodeRange(0x0B061, 0x0B07B, LVT);
            // Lo  [27] HANGUL SYLLABLE GGIG..HANGUL SYLLABLE GGIH
            AddCodeRange(0x0B07D, 0x0B097, LVT);
            // Lo  [27] HANGUL SYLLABLE NAG..HANGUL SYLLABLE NAH
            AddCodeRange(0x0B099, 0x0B0B3, LVT);
            // Lo  [27] HANGUL SYLLABLE NAEG..HANGUL SYLLABLE NAEH
            AddCodeRange(0x0B0B5, 0x0B0CF, LVT);
            // Lo  [27] HANGUL SYLLABLE NYAG..HANGUL SYLLABLE NYAH
            AddCodeRange(0x0B0D1, 0x0B0EB, LVT);
            // Lo  [27] HANGUL SYLLABLE NYAEG..HANGUL SYLLABLE NYAEH
            AddCodeRange(0x0B0ED, 0x0B107, LVT);
            // Lo  [27] HANGUL SYLLABLE NEOG..HANGUL SYLLABLE NEOH
            AddCodeRange(0x0B109, 0x0B123, LVT);
            // Lo  [27] HANGUL SYLLABLE NEG..HANGUL SYLLABLE NEH
            AddCodeRange(0x0B125, 0x0B13F, LVT);
            // Lo  [27] HANGUL SYLLABLE NYEOG..HANGUL SYLLABLE NYEOH
            AddCodeRange(0x0B141, 0x0B15B, LVT);
            // Lo  [27] HANGUL SYLLABLE NYEG..HANGUL SYLLABLE NYEH
            AddCodeRange(0x0B15D, 0x0B177, LVT);
            // Lo  [27] HANGUL SYLLABLE NOG..HANGUL SYLLABLE NOH
            AddCodeRange(0x0B179, 0x0B193, LVT);
            // Lo  [27] HANGUL SYLLABLE NWAG..HANGUL SYLLABLE NWAH
            AddCodeRange(0x0B195, 0x0B1AF, LVT);
            // Lo  [27] HANGUL SYLLABLE NWAEG..HANGUL SYLLABLE NWAEH
            AddCodeRange(0x0B1B1, 0x0B1CB, LVT);
            // Lo  [27] HANGUL SYLLABLE NOEG..HANGUL SYLLABLE NOEH
            AddCodeRange(0x0B1CD, 0x0B1E7, LVT);
            // Lo  [27] HANGUL SYLLABLE NYOG..HANGUL SYLLABLE NYOH
            AddCodeRange(0x0B1E9, 0x0B203, LVT);
            // Lo  [27] HANGUL SYLLABLE NUG..HANGUL SYLLABLE NUH
            AddCodeRange(0x0B205, 0x0B21F, LVT);
            // Lo  [27] HANGUL SYLLABLE NWEOG..HANGUL SYLLABLE NWEOH
            AddCodeRange(0x0B221, 0x0B23B, LVT);
            // Lo  [27] HANGUL SYLLABLE NWEG..HANGUL SYLLABLE NWEH
            AddCodeRange(0x0B23D, 0x0B257, LVT);
            // Lo  [27] HANGUL SYLLABLE NWIG..HANGUL SYLLABLE NWIH
            AddCodeRange(0x0B259, 0x0B273, LVT);
            // Lo  [27] HANGUL SYLLABLE NYUG..HANGUL SYLLABLE NYUH
            AddCodeRange(0x0B275, 0x0B28F, LVT);
            // Lo  [27] HANGUL SYLLABLE NEUG..HANGUL SYLLABLE NEUH
            AddCodeRange(0x0B291, 0x0B2AB, LVT);
            // Lo  [27] HANGUL SYLLABLE NYIG..HANGUL SYLLABLE NYIH
            AddCodeRange(0x0B2AD, 0x0B2C7, LVT);
            // Lo  [27] HANGUL SYLLABLE NIG..HANGUL SYLLABLE NIH
            AddCodeRange(0x0B2C9, 0x0B2E3, LVT);
            // Lo  [27] HANGUL SYLLABLE DAG..HANGUL SYLLABLE DAH
            AddCodeRange(0x0B2E5, 0x0B2FF, LVT);
            // Lo  [27] HANGUL SYLLABLE DAEG..HANGUL SYLLABLE DAEH
            AddCodeRange(0x0B301, 0x0B31B, LVT);
            // Lo  [27] HANGUL SYLLABLE DYAG..HANGUL SYLLABLE DYAH
            AddCodeRange(0x0B31D, 0x0B337, LVT);
            // Lo  [27] HANGUL SYLLABLE DYAEG..HANGUL SYLLABLE DYAEH
            AddCodeRange(0x0B339, 0x0B353, LVT);
            // Lo  [27] HANGUL SYLLABLE DEOG..HANGUL SYLLABLE DEOH
            AddCodeRange(0x0B355, 0x0B36F, LVT);
            // Lo  [27] HANGUL SYLLABLE DEG..HANGUL SYLLABLE DEH
            AddCodeRange(0x0B371, 0x0B38B, LVT);
            // Lo  [27] HANGUL SYLLABLE DYEOG..HANGUL SYLLABLE DYEOH
            AddCodeRange(0x0B38D, 0x0B3A7, LVT);
            // Lo  [27] HANGUL SYLLABLE DYEG..HANGUL SYLLABLE DYEH
            AddCodeRange(0x0B3A9, 0x0B3C3, LVT);
            // Lo  [27] HANGUL SYLLABLE DOG..HANGUL SYLLABLE DOH
            AddCodeRange(0x0B3C5, 0x0B3DF, LVT);
            // Lo  [27] HANGUL SYLLABLE DWAG..HANGUL SYLLABLE DWAH
            AddCodeRange(0x0B3E1, 0x0B3FB, LVT);
            // Lo  [27] HANGUL SYLLABLE DWAEG..HANGUL SYLLABLE DWAEH
            AddCodeRange(0x0B3FD, 0x0B417, LVT);
            // Lo  [27] HANGUL SYLLABLE DOEG..HANGUL SYLLABLE DOEH
            AddCodeRange(0x0B419, 0x0B433, LVT);
            // Lo  [27] HANGUL SYLLABLE DYOG..HANGUL SYLLABLE DYOH
            AddCodeRange(0x0B435, 0x0B44F, LVT);
            // Lo  [27] HANGUL SYLLABLE DUG..HANGUL SYLLABLE DUH
            AddCodeRange(0x0B451, 0x0B46B, LVT);
            // Lo  [27] HANGUL SYLLABLE DWEOG..HANGUL SYLLABLE DWEOH
            AddCodeRange(0x0B46D, 0x0B487, LVT);
            // Lo  [27] HANGUL SYLLABLE DWEG..HANGUL SYLLABLE DWEH
            AddCodeRange(0x0B489, 0x0B4A3, LVT);
            // Lo  [27] HANGUL SYLLABLE DWIG..HANGUL SYLLABLE DWIH
            AddCodeRange(0x0B4A5, 0x0B4BF, LVT);
            // Lo  [27] HANGUL SYLLABLE DYUG..HANGUL SYLLABLE DYUH
            AddCodeRange(0x0B4C1, 0x0B4DB, LVT);
            // Lo  [27] HANGUL SYLLABLE DEUG..HANGUL SYLLABLE DEUH
            AddCodeRange(0x0B4DD, 0x0B4F7, LVT);
            // Lo  [27] HANGUL SYLLABLE DYIG..HANGUL SYLLABLE DYIH
            AddCodeRange(0x0B4F9, 0x0B513, LVT);
            // Lo  [27] HANGUL SYLLABLE DIG..HANGUL SYLLABLE DIH
            AddCodeRange(0x0B515, 0x0B52F, LVT);
            // Lo  [27] HANGUL SYLLABLE DDAG..HANGUL SYLLABLE DDAH
            AddCodeRange(0x0B531, 0x0B54B, LVT);
            // Lo  [27] HANGUL SYLLABLE DDAEG..HANGUL SYLLABLE DDAEH
            AddCodeRange(0x0B54D, 0x0B567, LVT);
            // Lo  [27] HANGUL SYLLABLE DDYAG..HANGUL SYLLABLE DDYAH
            AddCodeRange(0x0B569, 0x0B583, LVT);
            // Lo  [27] HANGUL SYLLABLE DDYAEG..HANGUL SYLLABLE DDYAEH
            AddCodeRange(0x0B585, 0x0B59F, LVT);
            // Lo  [27] HANGUL SYLLABLE DDEOG..HANGUL SYLLABLE DDEOH
            AddCodeRange(0x0B5A1, 0x0B5BB, LVT);
            // Lo  [27] HANGUL SYLLABLE DDEG..HANGUL SYLLABLE DDEH
            AddCodeRange(0x0B5BD, 0x0B5D7, LVT);
            // Lo  [27] HANGUL SYLLABLE DDYEOG..HANGUL SYLLABLE DDYEOH
            AddCodeRange(0x0B5D9, 0x0B5F3, LVT);
            // Lo  [27] HANGUL SYLLABLE DDYEG..HANGUL SYLLABLE DDYEH
            AddCodeRange(0x0B5F5, 0x0B60F, LVT);
            // Lo  [27] HANGUL SYLLABLE DDOG..HANGUL SYLLABLE DDOH
            AddCodeRange(0x0B611, 0x0B62B, LVT);
            // Lo  [27] HANGUL SYLLABLE DDWAG..HANGUL SYLLABLE DDWAH
            AddCodeRange(0x0B62D, 0x0B647, LVT);
            // Lo  [27] HANGUL SYLLABLE DDWAEG..HANGUL SYLLABLE DDWAEH
            AddCodeRange(0x0B649, 0x0B663, LVT);
            // Lo  [27] HANGUL SYLLABLE DDOEG..HANGUL SYLLABLE DDOEH
            AddCodeRange(0x0B665, 0x0B67F, LVT);
            // Lo  [27] HANGUL SYLLABLE DDYOG..HANGUL SYLLABLE DDYOH
            AddCodeRange(0x0B681, 0x0B69B, LVT);
            // Lo  [27] HANGUL SYLLABLE DDUG..HANGUL SYLLABLE DDUH
            AddCodeRange(0x0B69D, 0x0B6B7, LVT);
            // Lo  [27] HANGUL SYLLABLE DDWEOG..HANGUL SYLLABLE DDWEOH
            AddCodeRange(0x0B6B9, 0x0B6D3, LVT);
            // Lo  [27] HANGUL SYLLABLE DDWEG..HANGUL SYLLABLE DDWEH
            AddCodeRange(0x0B6D5, 0x0B6EF, LVT);
            // Lo  [27] HANGUL SYLLABLE DDWIG..HANGUL SYLLABLE DDWIH
            AddCodeRange(0x0B6F1, 0x0B70B, LVT);
            // Lo  [27] HANGUL SYLLABLE DDYUG..HANGUL SYLLABLE DDYUH
            AddCodeRange(0x0B70D, 0x0B727, LVT);
            // Lo  [27] HANGUL SYLLABLE DDEUG..HANGUL SYLLABLE DDEUH
            AddCodeRange(0x0B729, 0x0B743, LVT);
            // Lo  [27] HANGUL SYLLABLE DDYIG..HANGUL SYLLABLE DDYIH
            AddCodeRange(0x0B745, 0x0B75F, LVT);
            // Lo  [27] HANGUL SYLLABLE DDIG..HANGUL SYLLABLE DDIH
            AddCodeRange(0x0B761, 0x0B77B, LVT);
            // Lo  [27] HANGUL SYLLABLE RAG..HANGUL SYLLABLE RAH
            AddCodeRange(0x0B77D, 0x0B797, LVT);
            // Lo  [27] HANGUL SYLLABLE RAEG..HANGUL SYLLABLE RAEH
            AddCodeRange(0x0B799, 0x0B7B3, LVT);
            // Lo  [27] HANGUL SYLLABLE RYAG..HANGUL SYLLABLE RYAH
            AddCodeRange(0x0B7B5, 0x0B7CF, LVT);
            // Lo  [27] HANGUL SYLLABLE RYAEG..HANGUL SYLLABLE RYAEH
            AddCodeRange(0x0B7D1, 0x0B7EB, LVT);
            // Lo  [27] HANGUL SYLLABLE REOG..HANGUL SYLLABLE REOH
            AddCodeRange(0x0B7ED, 0x0B807, LVT);
            // Lo  [27] HANGUL SYLLABLE REG..HANGUL SYLLABLE REH
            AddCodeRange(0x0B809, 0x0B823, LVT);
            // Lo  [27] HANGUL SYLLABLE RYEOG..HANGUL SYLLABLE RYEOH
            AddCodeRange(0x0B825, 0x0B83F, LVT);
            // Lo  [27] HANGUL SYLLABLE RYEG..HANGUL SYLLABLE RYEH
            AddCodeRange(0x0B841, 0x0B85B, LVT);
            // Lo  [27] HANGUL SYLLABLE ROG..HANGUL SYLLABLE ROH
            AddCodeRange(0x0B85D, 0x0B877, LVT);
            // Lo  [27] HANGUL SYLLABLE RWAG..HANGUL SYLLABLE RWAH
            AddCodeRange(0x0B879, 0x0B893, LVT);
            // Lo  [27] HANGUL SYLLABLE RWAEG..HANGUL SYLLABLE RWAEH
            AddCodeRange(0x0B895, 0x0B8AF, LVT);
            // Lo  [27] HANGUL SYLLABLE ROEG..HANGUL SYLLABLE ROEH
            AddCodeRange(0x0B8B1, 0x0B8CB, LVT);
            // Lo  [27] HANGUL SYLLABLE RYOG..HANGUL SYLLABLE RYOH
            AddCodeRange(0x0B8CD, 0x0B8E7, LVT);
            // Lo  [27] HANGUL SYLLABLE RUG..HANGUL SYLLABLE RUH
            AddCodeRange(0x0B8E9, 0x0B903, LVT);
            // Lo  [27] HANGUL SYLLABLE RWEOG..HANGUL SYLLABLE RWEOH
            AddCodeRange(0x0B905, 0x0B91F, LVT);
            // Lo  [27] HANGUL SYLLABLE RWEG..HANGUL SYLLABLE RWEH
            AddCodeRange(0x0B921, 0x0B93B, LVT);
            // Lo  [27] HANGUL SYLLABLE RWIG..HANGUL SYLLABLE RWIH
            AddCodeRange(0x0B93D, 0x0B957, LVT);
            // Lo  [27] HANGUL SYLLABLE RYUG..HANGUL SYLLABLE RYUH
            AddCodeRange(0x0B959, 0x0B973, LVT);
            // Lo  [27] HANGUL SYLLABLE REUG..HANGUL SYLLABLE REUH
            AddCodeRange(0x0B975, 0x0B98F, LVT);
            // Lo  [27] HANGUL SYLLABLE RYIG..HANGUL SYLLABLE RYIH
            AddCodeRange(0x0B991, 0x0B9AB, LVT);
            // Lo  [27] HANGUL SYLLABLE RIG..HANGUL SYLLABLE RIH
            AddCodeRange(0x0B9AD, 0x0B9C7, LVT);
            // Lo  [27] HANGUL SYLLABLE MAG..HANGUL SYLLABLE MAH
            AddCodeRange(0x0B9C9, 0x0B9E3, LVT);
            // Lo  [27] HANGUL SYLLABLE MAEG..HANGUL SYLLABLE MAEH
            AddCodeRange(0x0B9E5, 0x0B9FF, LVT);
            // Lo  [27] HANGUL SYLLABLE MYAG..HANGUL SYLLABLE MYAH
            AddCodeRange(0x0BA01, 0x0BA1B, LVT);
            // Lo  [27] HANGUL SYLLABLE MYAEG..HANGUL SYLLABLE MYAEH
            AddCodeRange(0x0BA1D, 0x0BA37, LVT);
            // Lo  [27] HANGUL SYLLABLE MEOG..HANGUL SYLLABLE MEOH
            AddCodeRange(0x0BA39, 0x0BA53, LVT);
            // Lo  [27] HANGUL SYLLABLE MEG..HANGUL SYLLABLE MEH
            AddCodeRange(0x0BA55, 0x0BA6F, LVT);
            // Lo  [27] HANGUL SYLLABLE MYEOG..HANGUL SYLLABLE MYEOH
            AddCodeRange(0x0BA71, 0x0BA8B, LVT);
            // Lo  [27] HANGUL SYLLABLE MYEG..HANGUL SYLLABLE MYEH
            AddCodeRange(0x0BA8D, 0x0BAA7, LVT);
            // Lo  [27] HANGUL SYLLABLE MOG..HANGUL SYLLABLE MOH
            AddCodeRange(0x0BAA9, 0x0BAC3, LVT);
            // Lo  [27] HANGUL SYLLABLE MWAG..HANGUL SYLLABLE MWAH
            AddCodeRange(0x0BAC5, 0x0BADF, LVT);
            // Lo  [27] HANGUL SYLLABLE MWAEG..HANGUL SYLLABLE MWAEH
            AddCodeRange(0x0BAE1, 0x0BAFB, LVT);
            // Lo  [27] HANGUL SYLLABLE MOEG..HANGUL SYLLABLE MOEH
            AddCodeRange(0x0BAFD, 0x0BB17, LVT);
            // Lo  [27] HANGUL SYLLABLE MYOG..HANGUL SYLLABLE MYOH
            AddCodeRange(0x0BB19, 0x0BB33, LVT);
            // Lo  [27] HANGUL SYLLABLE MUG..HANGUL SYLLABLE MUH
            AddCodeRange(0x0BB35, 0x0BB4F, LVT);
            // Lo  [27] HANGUL SYLLABLE MWEOG..HANGUL SYLLABLE MWEOH
            AddCodeRange(0x0BB51, 0x0BB6B, LVT);
            // Lo  [27] HANGUL SYLLABLE MWEG..HANGUL SYLLABLE MWEH
            AddCodeRange(0x0BB6D, 0x0BB87, LVT);
            // Lo  [27] HANGUL SYLLABLE MWIG..HANGUL SYLLABLE MWIH
            AddCodeRange(0x0BB89, 0x0BBA3, LVT);
            // Lo  [27] HANGUL SYLLABLE MYUG..HANGUL SYLLABLE MYUH
            AddCodeRange(0x0BBA5, 0x0BBBF, LVT);
            // Lo  [27] HANGUL SYLLABLE MEUG..HANGUL SYLLABLE MEUH
            AddCodeRange(0x0BBC1, 0x0BBDB, LVT);
            // Lo  [27] HANGUL SYLLABLE MYIG..HANGUL SYLLABLE MYIH
            AddCodeRange(0x0BBDD, 0x0BBF7, LVT);
            // Lo  [27] HANGUL SYLLABLE MIG..HANGUL SYLLABLE MIH
            AddCodeRange(0x0BBF9, 0x0BC13, LVT);
            // Lo  [27] HANGUL SYLLABLE BAG..HANGUL SYLLABLE BAH
            AddCodeRange(0x0BC15, 0x0BC2F, LVT);
            // Lo  [27] HANGUL SYLLABLE BAEG..HANGUL SYLLABLE BAEH
            AddCodeRange(0x0BC31, 0x0BC4B, LVT);
            // Lo  [27] HANGUL SYLLABLE BYAG..HANGUL SYLLABLE BYAH
            AddCodeRange(0x0BC4D, 0x0BC67, LVT);
            // Lo  [27] HANGUL SYLLABLE BYAEG..HANGUL SYLLABLE BYAEH
            AddCodeRange(0x0BC69, 0x0BC83, LVT);
            // Lo  [27] HANGUL SYLLABLE BEOG..HANGUL SYLLABLE BEOH
            AddCodeRange(0x0BC85, 0x0BC9F, LVT);
            // Lo  [27] HANGUL SYLLABLE BEG..HANGUL SYLLABLE BEH
            AddCodeRange(0x0BCA1, 0x0BCBB, LVT);
            // Lo  [27] HANGUL SYLLABLE BYEOG..HANGUL SYLLABLE BYEOH
            AddCodeRange(0x0BCBD, 0x0BCD7, LVT);
            // Lo  [27] HANGUL SYLLABLE BYEG..HANGUL SYLLABLE BYEH
            AddCodeRange(0x0BCD9, 0x0BCF3, LVT);
            // Lo  [27] HANGUL SYLLABLE BOG..HANGUL SYLLABLE BOH
            AddCodeRange(0x0BCF5, 0x0BD0F, LVT);
            // Lo  [27] HANGUL SYLLABLE BWAG..HANGUL SYLLABLE BWAH
            AddCodeRange(0x0BD11, 0x0BD2B, LVT);
            // Lo  [27] HANGUL SYLLABLE BWAEG..HANGUL SYLLABLE BWAEH
            AddCodeRange(0x0BD2D, 0x0BD47, LVT);
            // Lo  [27] HANGUL SYLLABLE BOEG..HANGUL SYLLABLE BOEH
            AddCodeRange(0x0BD49, 0x0BD63, LVT);
            // Lo  [27] HANGUL SYLLABLE BYOG..HANGUL SYLLABLE BYOH
            AddCodeRange(0x0BD65, 0x0BD7F, LVT);
            // Lo  [27] HANGUL SYLLABLE BUG..HANGUL SYLLABLE BUH
            AddCodeRange(0x0BD81, 0x0BD9B, LVT);
            // Lo  [27] HANGUL SYLLABLE BWEOG..HANGUL SYLLABLE BWEOH
            AddCodeRange(0x0BD9D, 0x0BDB7, LVT);
            // Lo  [27] HANGUL SYLLABLE BWEG..HANGUL SYLLABLE BWEH
            AddCodeRange(0x0BDB9, 0x0BDD3, LVT);
            // Lo  [27] HANGUL SYLLABLE BWIG..HANGUL SYLLABLE BWIH
            AddCodeRange(0x0BDD5, 0x0BDEF, LVT);
            // Lo  [27] HANGUL SYLLABLE BYUG..HANGUL SYLLABLE BYUH
            AddCodeRange(0x0BDF1, 0x0BE0B, LVT);
            // Lo  [27] HANGUL SYLLABLE BEUG..HANGUL SYLLABLE BEUH
            AddCodeRange(0x0BE0D, 0x0BE27, LVT);
            // Lo  [27] HANGUL SYLLABLE BYIG..HANGUL SYLLABLE BYIH
            AddCodeRange(0x0BE29, 0x0BE43, LVT);
            // Lo  [27] HANGUL SYLLABLE BIG..HANGUL SYLLABLE BIH
            AddCodeRange(0x0BE45, 0x0BE5F, LVT);
            // Lo  [27] HANGUL SYLLABLE BBAG..HANGUL SYLLABLE BBAH
            AddCodeRange(0x0BE61, 0x0BE7B, LVT);
            // Lo  [27] HANGUL SYLLABLE BBAEG..HANGUL SYLLABLE BBAEH
            AddCodeRange(0x0BE7D, 0x0BE97, LVT);
            // Lo  [27] HANGUL SYLLABLE BBYAG..HANGUL SYLLABLE BBYAH
            AddCodeRange(0x0BE99, 0x0BEB3, LVT);
            // Lo  [27] HANGUL SYLLABLE BBYAEG..HANGUL SYLLABLE BBYAEH
            AddCodeRange(0x0BEB5, 0x0BECF, LVT);
            // Lo  [27] HANGUL SYLLABLE BBEOG..HANGUL SYLLABLE BBEOH
            AddCodeRange(0x0BED1, 0x0BEEB, LVT);
            // Lo  [27] HANGUL SYLLABLE BBEG..HANGUL SYLLABLE BBEH
            AddCodeRange(0x0BEED, 0x0BF07, LVT);
            // Lo  [27] HANGUL SYLLABLE BBYEOG..HANGUL SYLLABLE BBYEOH
            AddCodeRange(0x0BF09, 0x0BF23, LVT);
            // Lo  [27] HANGUL SYLLABLE BBYEG..HANGUL SYLLABLE BBYEH
            AddCodeRange(0x0BF25, 0x0BF3F, LVT);
            // Lo  [27] HANGUL SYLLABLE BBOG..HANGUL SYLLABLE BBOH
            AddCodeRange(0x0BF41, 0x0BF5B, LVT);
            // Lo  [27] HANGUL SYLLABLE BBWAG..HANGUL SYLLABLE BBWAH
            AddCodeRange(0x0BF5D, 0x0BF77, LVT);
            // Lo  [27] HANGUL SYLLABLE BBWAEG..HANGUL SYLLABLE BBWAEH
            AddCodeRange(0x0BF79, 0x0BF93, LVT);
            // Lo  [27] HANGUL SYLLABLE BBOEG..HANGUL SYLLABLE BBOEH
            AddCodeRange(0x0BF95, 0x0BFAF, LVT);
            // Lo  [27] HANGUL SYLLABLE BBYOG..HANGUL SYLLABLE BBYOH
            AddCodeRange(0x0BFB1, 0x0BFCB, LVT);
            // Lo  [27] HANGUL SYLLABLE BBUG..HANGUL SYLLABLE BBUH
            AddCodeRange(0x0BFCD, 0x0BFE7, LVT);
            // Lo  [27] HANGUL SYLLABLE BBWEOG..HANGUL SYLLABLE BBWEOH
            AddCodeRange(0x0BFE9, 0x0C003, LVT);
            // Lo  [27] HANGUL SYLLABLE BBWEG..HANGUL SYLLABLE BBWEH
            AddCodeRange(0x0C005, 0x0C01F, LVT);
            // Lo  [27] HANGUL SYLLABLE BBWIG..HANGUL SYLLABLE BBWIH
            AddCodeRange(0x0C021, 0x0C03B, LVT);
            // Lo  [27] HANGUL SYLLABLE BBYUG..HANGUL SYLLABLE BBYUH
            AddCodeRange(0x0C03D, 0x0C057, LVT);
            // Lo  [27] HANGUL SYLLABLE BBEUG..HANGUL SYLLABLE BBEUH
            AddCodeRange(0x0C059, 0x0C073, LVT);
            // Lo  [27] HANGUL SYLLABLE BBYIG..HANGUL SYLLABLE BBYIH
            AddCodeRange(0x0C075, 0x0C08F, LVT);
            // Lo  [27] HANGUL SYLLABLE BBIG..HANGUL SYLLABLE BBIH
            AddCodeRange(0x0C091, 0x0C0AB, LVT);
            // Lo  [27] HANGUL SYLLABLE SAG..HANGUL SYLLABLE SAH
            AddCodeRange(0x0C0AD, 0x0C0C7, LVT);
            // Lo  [27] HANGUL SYLLABLE SAEG..HANGUL SYLLABLE SAEH
            AddCodeRange(0x0C0C9, 0x0C0E3, LVT);
            // Lo  [27] HANGUL SYLLABLE SYAG..HANGUL SYLLABLE SYAH
            AddCodeRange(0x0C0E5, 0x0C0FF, LVT);
            // Lo  [27] HANGUL SYLLABLE SYAEG..HANGUL SYLLABLE SYAEH
            AddCodeRange(0x0C101, 0x0C11B, LVT);
            // Lo  [27] HANGUL SYLLABLE SEOG..HANGUL SYLLABLE SEOH
            AddCodeRange(0x0C11D, 0x0C137, LVT);
            // Lo  [27] HANGUL SYLLABLE SEG..HANGUL SYLLABLE SEH
            AddCodeRange(0x0C139, 0x0C153, LVT);
            // Lo  [27] HANGUL SYLLABLE SYEOG..HANGUL SYLLABLE SYEOH
            AddCodeRange(0x0C155, 0x0C16F, LVT);
            // Lo  [27] HANGUL SYLLABLE SYEG..HANGUL SYLLABLE SYEH
            AddCodeRange(0x0C171, 0x0C18B, LVT);
            // Lo  [27] HANGUL SYLLABLE SOG..HANGUL SYLLABLE SOH
            AddCodeRange(0x0C18D, 0x0C1A7, LVT);
            // Lo  [27] HANGUL SYLLABLE SWAG..HANGUL SYLLABLE SWAH
            AddCodeRange(0x0C1A9, 0x0C1C3, LVT);
            // Lo  [27] HANGUL SYLLABLE SWAEG..HANGUL SYLLABLE SWAEH
            AddCodeRange(0x0C1C5, 0x0C1DF, LVT);
            // Lo  [27] HANGUL SYLLABLE SOEG..HANGUL SYLLABLE SOEH
            AddCodeRange(0x0C1E1, 0x0C1FB, LVT);
            // Lo  [27] HANGUL SYLLABLE SYOG..HANGUL SYLLABLE SYOH
            AddCodeRange(0x0C1FD, 0x0C217, LVT);
            // Lo  [27] HANGUL SYLLABLE SUG..HANGUL SYLLABLE SUH
            AddCodeRange(0x0C219, 0x0C233, LVT);
            // Lo  [27] HANGUL SYLLABLE SWEOG..HANGUL SYLLABLE SWEOH
            AddCodeRange(0x0C235, 0x0C24F, LVT);
            // Lo  [27] HANGUL SYLLABLE SWEG..HANGUL SYLLABLE SWEH
            AddCodeRange(0x0C251, 0x0C26B, LVT);
            // Lo  [27] HANGUL SYLLABLE SWIG..HANGUL SYLLABLE SWIH
            AddCodeRange(0x0C26D, 0x0C287, LVT);
            // Lo  [27] HANGUL SYLLABLE SYUG..HANGUL SYLLABLE SYUH
            AddCodeRange(0x0C289, 0x0C2A3, LVT);
            // Lo  [27] HANGUL SYLLABLE SEUG..HANGUL SYLLABLE SEUH
            AddCodeRange(0x0C2A5, 0x0C2BF, LVT);
            // Lo  [27] HANGUL SYLLABLE SYIG..HANGUL SYLLABLE SYIH
            AddCodeRange(0x0C2C1, 0x0C2DB, LVT);
            // Lo  [27] HANGUL SYLLABLE SIG..HANGUL SYLLABLE SIH
            AddCodeRange(0x0C2DD, 0x0C2F7, LVT);
            // Lo  [27] HANGUL SYLLABLE SSAG..HANGUL SYLLABLE SSAH
            AddCodeRange(0x0C2F9, 0x0C313, LVT);
            // Lo  [27] HANGUL SYLLABLE SSAEG..HANGUL SYLLABLE SSAEH
            AddCodeRange(0x0C315, 0x0C32F, LVT);
            // Lo  [27] HANGUL SYLLABLE SSYAG..HANGUL SYLLABLE SSYAH
            AddCodeRange(0x0C331, 0x0C34B, LVT);
            // Lo  [27] HANGUL SYLLABLE SSYAEG..HANGUL SYLLABLE SSYAEH
            AddCodeRange(0x0C34D, 0x0C367, LVT);
            // Lo  [27] HANGUL SYLLABLE SSEOG..HANGUL SYLLABLE SSEOH
            AddCodeRange(0x0C369, 0x0C383, LVT);
            // Lo  [27] HANGUL SYLLABLE SSEG..HANGUL SYLLABLE SSEH
            AddCodeRange(0x0C385, 0x0C39F, LVT);
            // Lo  [27] HANGUL SYLLABLE SSYEOG..HANGUL SYLLABLE SSYEOH
            AddCodeRange(0x0C3A1, 0x0C3BB, LVT);
            // Lo  [27] HANGUL SYLLABLE SSYEG..HANGUL SYLLABLE SSYEH
            AddCodeRange(0x0C3BD, 0x0C3D7, LVT);
            // Lo  [27] HANGUL SYLLABLE SSOG..HANGUL SYLLABLE SSOH
            AddCodeRange(0x0C3D9, 0x0C3F3, LVT);
            // Lo  [27] HANGUL SYLLABLE SSWAG..HANGUL SYLLABLE SSWAH
            AddCodeRange(0x0C3F5, 0x0C40F, LVT);
            // Lo  [27] HANGUL SYLLABLE SSWAEG..HANGUL SYLLABLE SSWAEH
            AddCodeRange(0x0C411, 0x0C42B, LVT);
            // Lo  [27] HANGUL SYLLABLE SSOEG..HANGUL SYLLABLE SSOEH
            AddCodeRange(0x0C42D, 0x0C447, LVT);
            // Lo  [27] HANGUL SYLLABLE SSYOG..HANGUL SYLLABLE SSYOH
            AddCodeRange(0x0C449, 0x0C463, LVT);
            // Lo  [27] HANGUL SYLLABLE SSUG..HANGUL SYLLABLE SSUH
            AddCodeRange(0x0C465, 0x0C47F, LVT);
            // Lo  [27] HANGUL SYLLABLE SSWEOG..HANGUL SYLLABLE SSWEOH
            AddCodeRange(0x0C481, 0x0C49B, LVT);
            // Lo  [27] HANGUL SYLLABLE SSWEG..HANGUL SYLLABLE SSWEH
            AddCodeRange(0x0C49D, 0x0C4B7, LVT);
            // Lo  [27] HANGUL SYLLABLE SSWIG..HANGUL SYLLABLE SSWIH
            AddCodeRange(0x0C4B9, 0x0C4D3, LVT);
            // Lo  [27] HANGUL SYLLABLE SSYUG..HANGUL SYLLABLE SSYUH
            AddCodeRange(0x0C4D5, 0x0C4EF, LVT);
            // Lo  [27] HANGUL SYLLABLE SSEUG..HANGUL SYLLABLE SSEUH
            AddCodeRange(0x0C4F1, 0x0C50B, LVT);
            // Lo  [27] HANGUL SYLLABLE SSYIG..HANGUL SYLLABLE SSYIH
            AddCodeRange(0x0C50D, 0x0C527, LVT);
            // Lo  [27] HANGUL SYLLABLE SSIG..HANGUL SYLLABLE SSIH
            AddCodeRange(0x0C529, 0x0C543, LVT);
            // Lo  [27] HANGUL SYLLABLE AG..HANGUL SYLLABLE AH
            AddCodeRange(0x0C545, 0x0C55F, LVT);
            // Lo  [27] HANGUL SYLLABLE AEG..HANGUL SYLLABLE AEH
            AddCodeRange(0x0C561, 0x0C57B, LVT);
            // Lo  [27] HANGUL SYLLABLE YAG..HANGUL SYLLABLE YAH
            AddCodeRange(0x0C57D, 0x0C597, LVT);
            // Lo  [27] HANGUL SYLLABLE YAEG..HANGUL SYLLABLE YAEH
            AddCodeRange(0x0C599, 0x0C5B3, LVT);
            // Lo  [27] HANGUL SYLLABLE EOG..HANGUL SYLLABLE EOH
            AddCodeRange(0x0C5B5, 0x0C5CF, LVT);
            // Lo  [27] HANGUL SYLLABLE EG..HANGUL SYLLABLE EH
            AddCodeRange(0x0C5D1, 0x0C5EB, LVT);
            // Lo  [27] HANGUL SYLLABLE YEOG..HANGUL SYLLABLE YEOH
            AddCodeRange(0x0C5ED, 0x0C607, LVT);
            // Lo  [27] HANGUL SYLLABLE YEG..HANGUL SYLLABLE YEH
            AddCodeRange(0x0C609, 0x0C623, LVT);
            // Lo  [27] HANGUL SYLLABLE OG..HANGUL SYLLABLE OH
            AddCodeRange(0x0C625, 0x0C63F, LVT);
            // Lo  [27] HANGUL SYLLABLE WAG..HANGUL SYLLABLE WAH
            AddCodeRange(0x0C641, 0x0C65B, LVT);
            // Lo  [27] HANGUL SYLLABLE WAEG..HANGUL SYLLABLE WAEH
            AddCodeRange(0x0C65D, 0x0C677, LVT);
            // Lo  [27] HANGUL SYLLABLE OEG..HANGUL SYLLABLE OEH
            AddCodeRange(0x0C679, 0x0C693, LVT);
            // Lo  [27] HANGUL SYLLABLE YOG..HANGUL SYLLABLE YOH
            AddCodeRange(0x0C695, 0x0C6AF, LVT);
            // Lo  [27] HANGUL SYLLABLE UG..HANGUL SYLLABLE UH
            AddCodeRange(0x0C6B1, 0x0C6CB, LVT);
            // Lo  [27] HANGUL SYLLABLE WEOG..HANGUL SYLLABLE WEOH
            AddCodeRange(0x0C6CD, 0x0C6E7, LVT);
            // Lo  [27] HANGUL SYLLABLE WEG..HANGUL SYLLABLE WEH
            AddCodeRange(0x0C6E9, 0x0C703, LVT);
            // Lo  [27] HANGUL SYLLABLE WIG..HANGUL SYLLABLE WIH
            AddCodeRange(0x0C705, 0x0C71F, LVT);
            // Lo  [27] HANGUL SYLLABLE YUG..HANGUL SYLLABLE YUH
            AddCodeRange(0x0C721, 0x0C73B, LVT);
            // Lo  [27] HANGUL SYLLABLE EUG..HANGUL SYLLABLE EUH
            AddCodeRange(0x0C73D, 0x0C757, LVT);
            // Lo  [27] HANGUL SYLLABLE YIG..HANGUL SYLLABLE YIH
            AddCodeRange(0x0C759, 0x0C773, LVT);
            // Lo  [27] HANGUL SYLLABLE IG..HANGUL SYLLABLE IH
            AddCodeRange(0x0C775, 0x0C78F, LVT);
            // Lo  [27] HANGUL SYLLABLE JAG..HANGUL SYLLABLE JAH
            AddCodeRange(0x0C791, 0x0C7AB, LVT);
            // Lo  [27] HANGUL SYLLABLE JAEG..HANGUL SYLLABLE JAEH
            AddCodeRange(0x0C7AD, 0x0C7C7, LVT);
            // Lo  [27] HANGUL SYLLABLE JYAG..HANGUL SYLLABLE JYAH
            AddCodeRange(0x0C7C9, 0x0C7E3, LVT);
            // Lo  [27] HANGUL SYLLABLE JYAEG..HANGUL SYLLABLE JYAEH
            AddCodeRange(0x0C7E5, 0x0C7FF, LVT);
            // Lo  [27] HANGUL SYLLABLE JEOG..HANGUL SYLLABLE JEOH
            AddCodeRange(0x0C801, 0x0C81B, LVT);
            // Lo  [27] HANGUL SYLLABLE JEG..HANGUL SYLLABLE JEH
            AddCodeRange(0x0C81D, 0x0C837, LVT);
            // Lo  [27] HANGUL SYLLABLE JYEOG..HANGUL SYLLABLE JYEOH
            AddCodeRange(0x0C839, 0x0C853, LVT);
            // Lo  [27] HANGUL SYLLABLE JYEG..HANGUL SYLLABLE JYEH
            AddCodeRange(0x0C855, 0x0C86F, LVT);
            // Lo  [27] HANGUL SYLLABLE JOG..HANGUL SYLLABLE JOH
            AddCodeRange(0x0C871, 0x0C88B, LVT);
            // Lo  [27] HANGUL SYLLABLE JWAG..HANGUL SYLLABLE JWAH
            AddCodeRange(0x0C88D, 0x0C8A7, LVT);
            // Lo  [27] HANGUL SYLLABLE JWAEG..HANGUL SYLLABLE JWAEH
            AddCodeRange(0x0C8A9, 0x0C8C3, LVT);
            // Lo  [27] HANGUL SYLLABLE JOEG..HANGUL SYLLABLE JOEH
            AddCodeRange(0x0C8C5, 0x0C8DF, LVT);
            // Lo  [27] HANGUL SYLLABLE JYOG..HANGUL SYLLABLE JYOH
            AddCodeRange(0x0C8E1, 0x0C8FB, LVT);
            // Lo  [27] HANGUL SYLLABLE JUG..HANGUL SYLLABLE JUH
            AddCodeRange(0x0C8FD, 0x0C917, LVT);
            // Lo  [27] HANGUL SYLLABLE JWEOG..HANGUL SYLLABLE JWEOH
            AddCodeRange(0x0C919, 0x0C933, LVT);
            // Lo  [27] HANGUL SYLLABLE JWEG..HANGUL SYLLABLE JWEH
            AddCodeRange(0x0C935, 0x0C94F, LVT);
            // Lo  [27] HANGUL SYLLABLE JWIG..HANGUL SYLLABLE JWIH
            AddCodeRange(0x0C951, 0x0C96B, LVT);
            // Lo  [27] HANGUL SYLLABLE JYUG..HANGUL SYLLABLE JYUH
            AddCodeRange(0x0C96D, 0x0C987, LVT);
            // Lo  [27] HANGUL SYLLABLE JEUG..HANGUL SYLLABLE JEUH
            AddCodeRange(0x0C989, 0x0C9A3, LVT);
            // Lo  [27] HANGUL SYLLABLE JYIG..HANGUL SYLLABLE JYIH
            AddCodeRange(0x0C9A5, 0x0C9BF, LVT);
            // Lo  [27] HANGUL SYLLABLE JIG..HANGUL SYLLABLE JIH
            AddCodeRange(0x0C9C1, 0x0C9DB, LVT);
            // Lo  [27] HANGUL SYLLABLE JJAG..HANGUL SYLLABLE JJAH
            AddCodeRange(0x0C9DD, 0x0C9F7, LVT);
            // Lo  [27] HANGUL SYLLABLE JJAEG..HANGUL SYLLABLE JJAEH
            AddCodeRange(0x0C9F9, 0x0CA13, LVT);
            // Lo  [27] HANGUL SYLLABLE JJYAG..HANGUL SYLLABLE JJYAH
            AddCodeRange(0x0CA15, 0x0CA2F, LVT);
            // Lo  [27] HANGUL SYLLABLE JJYAEG..HANGUL SYLLABLE JJYAEH
            AddCodeRange(0x0CA31, 0x0CA4B, LVT);
            // Lo  [27] HANGUL SYLLABLE JJEOG..HANGUL SYLLABLE JJEOH
            AddCodeRange(0x0CA4D, 0x0CA67, LVT);
            // Lo  [27] HANGUL SYLLABLE JJEG..HANGUL SYLLABLE JJEH
            AddCodeRange(0x0CA69, 0x0CA83, LVT);
            // Lo  [27] HANGUL SYLLABLE JJYEOG..HANGUL SYLLABLE JJYEOH
            AddCodeRange(0x0CA85, 0x0CA9F, LVT);
            // Lo  [27] HANGUL SYLLABLE JJYEG..HANGUL SYLLABLE JJYEH
            AddCodeRange(0x0CAA1, 0x0CABB, LVT);
            // Lo  [27] HANGUL SYLLABLE JJOG..HANGUL SYLLABLE JJOH
            AddCodeRange(0x0CABD, 0x0CAD7, LVT);
            // Lo  [27] HANGUL SYLLABLE JJWAG..HANGUL SYLLABLE JJWAH
            AddCodeRange(0x0CAD9, 0x0CAF3, LVT);
            // Lo  [27] HANGUL SYLLABLE JJWAEG..HANGUL SYLLABLE JJWAEH
            AddCodeRange(0x0CAF5, 0x0CB0F, LVT);
            // Lo  [27] HANGUL SYLLABLE JJOEG..HANGUL SYLLABLE JJOEH
            AddCodeRange(0x0CB11, 0x0CB2B, LVT);
            // Lo  [27] HANGUL SYLLABLE JJYOG..HANGUL SYLLABLE JJYOH
            AddCodeRange(0x0CB2D, 0x0CB47, LVT);
            // Lo  [27] HANGUL SYLLABLE JJUG..HANGUL SYLLABLE JJUH
            AddCodeRange(0x0CB49, 0x0CB63, LVT);
            // Lo  [27] HANGUL SYLLABLE JJWEOG..HANGUL SYLLABLE JJWEOH
            AddCodeRange(0x0CB65, 0x0CB7F, LVT);
            // Lo  [27] HANGUL SYLLABLE JJWEG..HANGUL SYLLABLE JJWEH
            AddCodeRange(0x0CB81, 0x0CB9B, LVT);
            // Lo  [27] HANGUL SYLLABLE JJWIG..HANGUL SYLLABLE JJWIH
            AddCodeRange(0x0CB9D, 0x0CBB7, LVT);
            // Lo  [27] HANGUL SYLLABLE JJYUG..HANGUL SYLLABLE JJYUH
            AddCodeRange(0x0CBB9, 0x0CBD3, LVT);
            // Lo  [27] HANGUL SYLLABLE JJEUG..HANGUL SYLLABLE JJEUH
            AddCodeRange(0x0CBD5, 0x0CBEF, LVT);
            // Lo  [27] HANGUL SYLLABLE JJYIG..HANGUL SYLLABLE JJYIH
            AddCodeRange(0x0CBF1, 0x0CC0B, LVT);
            // Lo  [27] HANGUL SYLLABLE JJIG..HANGUL SYLLABLE JJIH
            AddCodeRange(0x0CC0D, 0x0CC27, LVT);
            // Lo  [27] HANGUL SYLLABLE CAG..HANGUL SYLLABLE CAH
            AddCodeRange(0x0CC29, 0x0CC43, LVT);
            // Lo  [27] HANGUL SYLLABLE CAEG..HANGUL SYLLABLE CAEH
            AddCodeRange(0x0CC45, 0x0CC5F, LVT);
            // Lo  [27] HANGUL SYLLABLE CYAG..HANGUL SYLLABLE CYAH
            AddCodeRange(0x0CC61, 0x0CC7B, LVT);
            // Lo  [27] HANGUL SYLLABLE CYAEG..HANGUL SYLLABLE CYAEH
            AddCodeRange(0x0CC7D, 0x0CC97, LVT);
            // Lo  [27] HANGUL SYLLABLE CEOG..HANGUL SYLLABLE CEOH
            AddCodeRange(0x0CC99, 0x0CCB3, LVT);
            // Lo  [27] HANGUL SYLLABLE CEG..HANGUL SYLLABLE CEH
            AddCodeRange(0x0CCB5, 0x0CCCF, LVT);
            // Lo  [27] HANGUL SYLLABLE CYEOG..HANGUL SYLLABLE CYEOH
            AddCodeRange(0x0CCD1, 0x0CCEB, LVT);
            // Lo  [27] HANGUL SYLLABLE CYEG..HANGUL SYLLABLE CYEH
            AddCodeRange(0x0CCED, 0x0CD07, LVT);
            // Lo  [27] HANGUL SYLLABLE COG..HANGUL SYLLABLE COH
            AddCodeRange(0x0CD09, 0x0CD23, LVT);
            // Lo  [27] HANGUL SYLLABLE CWAG..HANGUL SYLLABLE CWAH
            AddCodeRange(0x0CD25, 0x0CD3F, LVT);
            // Lo  [27] HANGUL SYLLABLE CWAEG..HANGUL SYLLABLE CWAEH
            AddCodeRange(0x0CD41, 0x0CD5B, LVT);
            // Lo  [27] HANGUL SYLLABLE COEG..HANGUL SYLLABLE COEH
            AddCodeRange(0x0CD5D, 0x0CD77, LVT);
            // Lo  [27] HANGUL SYLLABLE CYOG..HANGUL SYLLABLE CYOH
            AddCodeRange(0x0CD79, 0x0CD93, LVT);
            // Lo  [27] HANGUL SYLLABLE CUG..HANGUL SYLLABLE CUH
            AddCodeRange(0x0CD95, 0x0CDAF, LVT);
            // Lo  [27] HANGUL SYLLABLE CWEOG..HANGUL SYLLABLE CWEOH
            AddCodeRange(0x0CDB1, 0x0CDCB, LVT);
            // Lo  [27] HANGUL SYLLABLE CWEG..HANGUL SYLLABLE CWEH
            AddCodeRange(0x0CDCD, 0x0CDE7, LVT);
            // Lo  [27] HANGUL SYLLABLE CWIG..HANGUL SYLLABLE CWIH
            AddCodeRange(0x0CDE9, 0x0CE03, LVT);
            // Lo  [27] HANGUL SYLLABLE CYUG..HANGUL SYLLABLE CYUH
            AddCodeRange(0x0CE05, 0x0CE1F, LVT);
            // Lo  [27] HANGUL SYLLABLE CEUG..HANGUL SYLLABLE CEUH
            AddCodeRange(0x0CE21, 0x0CE3B, LVT);
            // Lo  [27] HANGUL SYLLABLE CYIG..HANGUL SYLLABLE CYIH
            AddCodeRange(0x0CE3D, 0x0CE57, LVT);
            // Lo  [27] HANGUL SYLLABLE CIG..HANGUL SYLLABLE CIH
            AddCodeRange(0x0CE59, 0x0CE73, LVT);
            // Lo  [27] HANGUL SYLLABLE KAG..HANGUL SYLLABLE KAH
            AddCodeRange(0x0CE75, 0x0CE8F, LVT);
            // Lo  [27] HANGUL SYLLABLE KAEG..HANGUL SYLLABLE KAEH
            AddCodeRange(0x0CE91, 0x0CEAB, LVT);
            // Lo  [27] HANGUL SYLLABLE KYAG..HANGUL SYLLABLE KYAH
            AddCodeRange(0x0CEAD, 0x0CEC7, LVT);
            // Lo  [27] HANGUL SYLLABLE KYAEG..HANGUL SYLLABLE KYAEH
            AddCodeRange(0x0CEC9, 0x0CEE3, LVT);
            // Lo  [27] HANGUL SYLLABLE KEOG..HANGUL SYLLABLE KEOH
            AddCodeRange(0x0CEE5, 0x0CEFF, LVT);
            // Lo  [27] HANGUL SYLLABLE KEG..HANGUL SYLLABLE KEH
            AddCodeRange(0x0CF01, 0x0CF1B, LVT);
            // Lo  [27] HANGUL SYLLABLE KYEOG..HANGUL SYLLABLE KYEOH
            AddCodeRange(0x0CF1D, 0x0CF37, LVT);
            // Lo  [27] HANGUL SYLLABLE KYEG..HANGUL SYLLABLE KYEH
            AddCodeRange(0x0CF39, 0x0CF53, LVT);
            // Lo  [27] HANGUL SYLLABLE KOG..HANGUL SYLLABLE KOH
            AddCodeRange(0x0CF55, 0x0CF6F, LVT);
            // Lo  [27] HANGUL SYLLABLE KWAG..HANGUL SYLLABLE KWAH
            AddCodeRange(0x0CF71, 0x0CF8B, LVT);
            // Lo  [27] HANGUL SYLLABLE KWAEG..HANGUL SYLLABLE KWAEH
            AddCodeRange(0x0CF8D, 0x0CFA7, LVT);
            // Lo  [27] HANGUL SYLLABLE KOEG..HANGUL SYLLABLE KOEH
            AddCodeRange(0x0CFA9, 0x0CFC3, LVT);
            // Lo  [27] HANGUL SYLLABLE KYOG..HANGUL SYLLABLE KYOH
            AddCodeRange(0x0CFC5, 0x0CFDF, LVT);
            // Lo  [27] HANGUL SYLLABLE KUG..HANGUL SYLLABLE KUH
            AddCodeRange(0x0CFE1, 0x0CFFB, LVT);
            // Lo  [27] HANGUL SYLLABLE KWEOG..HANGUL SYLLABLE KWEOH
            AddCodeRange(0x0CFFD, 0x0D017, LVT);
            // Lo  [27] HANGUL SYLLABLE KWEG..HANGUL SYLLABLE KWEH
            AddCodeRange(0x0D019, 0x0D033, LVT);
            // Lo  [27] HANGUL SYLLABLE KWIG..HANGUL SYLLABLE KWIH
            AddCodeRange(0x0D035, 0x0D04F, LVT);
            // Lo  [27] HANGUL SYLLABLE KYUG..HANGUL SYLLABLE KYUH
            AddCodeRange(0x0D051, 0x0D06B, LVT);
            // Lo  [27] HANGUL SYLLABLE KEUG..HANGUL SYLLABLE KEUH
            AddCodeRange(0x0D06D, 0x0D087, LVT);
            // Lo  [27] HANGUL SYLLABLE KYIG..HANGUL SYLLABLE KYIH
            AddCodeRange(0x0D089, 0x0D0A3, LVT);
            // Lo  [27] HANGUL SYLLABLE KIG..HANGUL SYLLABLE KIH
            AddCodeRange(0x0D0A5, 0x0D0BF, LVT);
            // Lo  [27] HANGUL SYLLABLE TAG..HANGUL SYLLABLE TAH
            AddCodeRange(0x0D0C1, 0x0D0DB, LVT);
            // Lo  [27] HANGUL SYLLABLE TAEG..HANGUL SYLLABLE TAEH
            AddCodeRange(0x0D0DD, 0x0D0F7, LVT);
            // Lo  [27] HANGUL SYLLABLE TYAG..HANGUL SYLLABLE TYAH
            AddCodeRange(0x0D0F9, 0x0D113, LVT);
            // Lo  [27] HANGUL SYLLABLE TYAEG..HANGUL SYLLABLE TYAEH
            AddCodeRange(0x0D115, 0x0D12F, LVT);
            // Lo  [27] HANGUL SYLLABLE TEOG..HANGUL SYLLABLE TEOH
            AddCodeRange(0x0D131, 0x0D14B, LVT);
            // Lo  [27] HANGUL SYLLABLE TEG..HANGUL SYLLABLE TEH
            AddCodeRange(0x0D14D, 0x0D167, LVT);
            // Lo  [27] HANGUL SYLLABLE TYEOG..HANGUL SYLLABLE TYEOH
            AddCodeRange(0x0D169, 0x0D183, LVT);
            // Lo  [27] HANGUL SYLLABLE TYEG..HANGUL SYLLABLE TYEH
            AddCodeRange(0x0D185, 0x0D19F, LVT);
            // Lo  [27] HANGUL SYLLABLE TOG..HANGUL SYLLABLE TOH
            AddCodeRange(0x0D1A1, 0x0D1BB, LVT);
            // Lo  [27] HANGUL SYLLABLE TWAG..HANGUL SYLLABLE TWAH
            AddCodeRange(0x0D1BD, 0x0D1D7, LVT);
            // Lo  [27] HANGUL SYLLABLE TWAEG..HANGUL SYLLABLE TWAEH
            AddCodeRange(0x0D1D9, 0x0D1F3, LVT);
            // Lo  [27] HANGUL SYLLABLE TOEG..HANGUL SYLLABLE TOEH
            AddCodeRange(0x0D1F5, 0x0D20F, LVT);
            // Lo  [27] HANGUL SYLLABLE TYOG..HANGUL SYLLABLE TYOH
            AddCodeRange(0x0D211, 0x0D22B, LVT);
            // Lo  [27] HANGUL SYLLABLE TUG..HANGUL SYLLABLE TUH
            AddCodeRange(0x0D22D, 0x0D247, LVT);
            // Lo  [27] HANGUL SYLLABLE TWEOG..HANGUL SYLLABLE TWEOH
            AddCodeRange(0x0D249, 0x0D263, LVT);
            // Lo  [27] HANGUL SYLLABLE TWEG..HANGUL SYLLABLE TWEH
            AddCodeRange(0x0D265, 0x0D27F, LVT);
            // Lo  [27] HANGUL SYLLABLE TWIG..HANGUL SYLLABLE TWIH
            AddCodeRange(0x0D281, 0x0D29B, LVT);
            // Lo  [27] HANGUL SYLLABLE TYUG..HANGUL SYLLABLE TYUH
            AddCodeRange(0x0D29D, 0x0D2B7, LVT);
            // Lo  [27] HANGUL SYLLABLE TEUG..HANGUL SYLLABLE TEUH
            AddCodeRange(0x0D2B9, 0x0D2D3, LVT);
            // Lo  [27] HANGUL SYLLABLE TYIG..HANGUL SYLLABLE TYIH
            AddCodeRange(0x0D2D5, 0x0D2EF, LVT);
            // Lo  [27] HANGUL SYLLABLE TIG..HANGUL SYLLABLE TIH
            AddCodeRange(0x0D2F1, 0x0D30B, LVT);
            // Lo  [27] HANGUL SYLLABLE PAG..HANGUL SYLLABLE PAH
            AddCodeRange(0x0D30D, 0x0D327, LVT);
            // Lo  [27] HANGUL SYLLABLE PAEG..HANGUL SYLLABLE PAEH
            AddCodeRange(0x0D329, 0x0D343, LVT);
            // Lo  [27] HANGUL SYLLABLE PYAG..HANGUL SYLLABLE PYAH
            AddCodeRange(0x0D345, 0x0D35F, LVT);
            // Lo  [27] HANGUL SYLLABLE PYAEG..HANGUL SYLLABLE PYAEH
            AddCodeRange(0x0D361, 0x0D37B, LVT);
            // Lo  [27] HANGUL SYLLABLE PEOG..HANGUL SYLLABLE PEOH
            AddCodeRange(0x0D37D, 0x0D397, LVT);
            // Lo  [27] HANGUL SYLLABLE PEG..HANGUL SYLLABLE PEH
            AddCodeRange(0x0D399, 0x0D3B3, LVT);
            // Lo  [27] HANGUL SYLLABLE PYEOG..HANGUL SYLLABLE PYEOH
            AddCodeRange(0x0D3B5, 0x0D3CF, LVT);
            // Lo  [27] HANGUL SYLLABLE PYEG..HANGUL SYLLABLE PYEH
            AddCodeRange(0x0D3D1, 0x0D3EB, LVT);
            // Lo  [27] HANGUL SYLLABLE POG..HANGUL SYLLABLE POH
            AddCodeRange(0x0D3ED, 0x0D407, LVT);
            // Lo  [27] HANGUL SYLLABLE PWAG..HANGUL SYLLABLE PWAH
            AddCodeRange(0x0D409, 0x0D423, LVT);
            // Lo  [27] HANGUL SYLLABLE PWAEG..HANGUL SYLLABLE PWAEH
            AddCodeRange(0x0D425, 0x0D43F, LVT);
            // Lo  [27] HANGUL SYLLABLE POEG..HANGUL SYLLABLE POEH
            AddCodeRange(0x0D441, 0x0D45B, LVT);
            // Lo  [27] HANGUL SYLLABLE PYOG..HANGUL SYLLABLE PYOH
            AddCodeRange(0x0D45D, 0x0D477, LVT);
            // Lo  [27] HANGUL SYLLABLE PUG..HANGUL SYLLABLE PUH
            AddCodeRange(0x0D479, 0x0D493, LVT);
            // Lo  [27] HANGUL SYLLABLE PWEOG..HANGUL SYLLABLE PWEOH
            AddCodeRange(0x0D495, 0x0D4AF, LVT);
            // Lo  [27] HANGUL SYLLABLE PWEG..HANGUL SYLLABLE PWEH
            AddCodeRange(0x0D4B1, 0x0D4CB, LVT);
            // Lo  [27] HANGUL SYLLABLE PWIG..HANGUL SYLLABLE PWIH
            AddCodeRange(0x0D4CD, 0x0D4E7, LVT);
            // Lo  [27] HANGUL SYLLABLE PYUG..HANGUL SYLLABLE PYUH
            AddCodeRange(0x0D4E9, 0x0D503, LVT);
            // Lo  [27] HANGUL SYLLABLE PEUG..HANGUL SYLLABLE PEUH
            AddCodeRange(0x0D505, 0x0D51F, LVT);
            // Lo  [27] HANGUL SYLLABLE PYIG..HANGUL SYLLABLE PYIH
            AddCodeRange(0x0D521, 0x0D53B, LVT);
            // Lo  [27] HANGUL SYLLABLE PIG..HANGUL SYLLABLE PIH
            AddCodeRange(0x0D53D, 0x0D557, LVT);
            // Lo  [27] HANGUL SYLLABLE HAG..HANGUL SYLLABLE HAH
            AddCodeRange(0x0D559, 0x0D573, LVT);
            // Lo  [27] HANGUL SYLLABLE HAEG..HANGUL SYLLABLE HAEH
            AddCodeRange(0x0D575, 0x0D58F, LVT);
            // Lo  [27] HANGUL SYLLABLE HYAG..HANGUL SYLLABLE HYAH
            AddCodeRange(0x0D591, 0x0D5AB, LVT);
            // Lo  [27] HANGUL SYLLABLE HYAEG..HANGUL SYLLABLE HYAEH
            AddCodeRange(0x0D5AD, 0x0D5C7, LVT);
            // Lo  [27] HANGUL SYLLABLE HEOG..HANGUL SYLLABLE HEOH
            AddCodeRange(0x0D5C9, 0x0D5E3, LVT);
            // Lo  [27] HANGUL SYLLABLE HEG..HANGUL SYLLABLE HEH
            AddCodeRange(0x0D5E5, 0x0D5FF, LVT);
            // Lo  [27] HANGUL SYLLABLE HYEOG..HANGUL SYLLABLE HYEOH
            AddCodeRange(0x0D601, 0x0D61B, LVT);
            // Lo  [27] HANGUL SYLLABLE HYEG..HANGUL SYLLABLE HYEH
            AddCodeRange(0x0D61D, 0x0D637, LVT);
            // Lo  [27] HANGUL SYLLABLE HOG..HANGUL SYLLABLE HOH
            AddCodeRange(0x0D639, 0x0D653, LVT);
            // Lo  [27] HANGUL SYLLABLE HWAG..HANGUL SYLLABLE HWAH
            AddCodeRange(0x0D655, 0x0D66F, LVT);
            // Lo  [27] HANGUL SYLLABLE HWAEG..HANGUL SYLLABLE HWAEH
            AddCodeRange(0x0D671, 0x0D68B, LVT);
            // Lo  [27] HANGUL SYLLABLE HOEG..HANGUL SYLLABLE HOEH
            AddCodeRange(0x0D68D, 0x0D6A7, LVT);
            // Lo  [27] HANGUL SYLLABLE HYOG..HANGUL SYLLABLE HYOH
            AddCodeRange(0x0D6A9, 0x0D6C3, LVT);
            // Lo  [27] HANGUL SYLLABLE HUG..HANGUL SYLLABLE HUH
            AddCodeRange(0x0D6C5, 0x0D6DF, LVT);
            // Lo  [27] HANGUL SYLLABLE HWEOG..HANGUL SYLLABLE HWEOH
            AddCodeRange(0x0D6E1, 0x0D6FB, LVT);
            // Lo  [27] HANGUL SYLLABLE HWEG..HANGUL SYLLABLE HWEH
            AddCodeRange(0x0D6FD, 0x0D717, LVT);
            // Lo  [27] HANGUL SYLLABLE HWIG..HANGUL SYLLABLE HWIH
            AddCodeRange(0x0D719, 0x0D733, LVT);
            // Lo  [27] HANGUL SYLLABLE HYUG..HANGUL SYLLABLE HYUH
            AddCodeRange(0x0D735, 0x0D74F, LVT);
            // Lo  [27] HANGUL SYLLABLE HEUG..HANGUL SYLLABLE HEUH
            AddCodeRange(0x0D751, 0x0D76B, LVT);
            // Lo  [27] HANGUL SYLLABLE HYIG..HANGUL SYLLABLE HYIH
            AddCodeRange(0x0D76D, 0x0D787, LVT);
            // Lo  [27] HANGUL SYLLABLE HIG..HANGUL SYLLABLE HIH
            AddCodeRange(0x0D789, 0x0D7A3, LVT);
            // Lo  [23] HANGUL JUNGSEONG O-YEO..HANGUL JUNGSEONG ARAEA-E
            AddCodeRange(0x0D7B0, 0x0D7C6, V);
            // Lo  [49] HANGUL JONGSEONG NIEUN-RIEUL..HANGUL JONGSEONG PHIEUPH-THIEUTH
            AddCodeRange(0x0D7CB, 0x0D7FB, T);
            // Mn  [16] VARIATION SELECTOR-1..VARIATION SELECTOR-16
            AddCodeRange(0x0FE00, 0x0FE0F, Extend);
            // Mn  [16] COMBINING LIGATURE LEFT HALF..COMBINING CYRILLIC TITLO RIGHT HALF
            AddCodeRange(0x0FE20, 0x0FE2F, Extend);
            // Lm   [2] HALFWIDTH KATAKANA VOICED SOUND MARK..HALFWIDTH KATAKANA SEMI-VOICED SOUND MARK
            AddCodeRange(0x0FF9E, 0x0FF9F, Extend);
            // Cn   [9] <reserved-FFF0>..<reserved-FFF8>
            AddCodeRange(0x0FFF0, 0x0FFF8, Control);
            // Cf   [3] INTERLINEAR ANNOTATION ANCHOR..INTERLINEAR ANNOTATION TERMINATOR
            AddCodeRange(0x0FFF9, 0x0FFFB, Control);
            // Mn   [5] COMBINING OLD PERMIC LETTER AN..COMBINING OLD PERMIC LETTER SII
            AddCodeRange(0x10376, 0x1037A, Extend);
            // Mn   [3] KHAROSHTHI VOWEL SIGN I..KHAROSHTHI VOWEL SIGN VOCALIC R
            AddCodeRange(0x10A01, 0x10A03, Extend);
            // Mn   [2] KHAROSHTHI VOWEL SIGN E..KHAROSHTHI VOWEL SIGN O
            AddCodeRange(0x10A05, 0x10A06, Extend);
            // Mn   [4] KHAROSHTHI VOWEL LENGTH MARK..KHAROSHTHI SIGN VISARGA
            AddCodeRange(0x10A0C, 0x10A0F, Extend);
            // Mn   [3] KHAROSHTHI SIGN BAR ABOVE..KHAROSHTHI SIGN DOT BELOW
            AddCodeRange(0x10A38, 0x10A3A, Extend);
            // Mn   [2] MANICHAEAN ABBREVIATION MARK ABOVE..MANICHAEAN ABBREVIATION MARK BELOW
            AddCodeRange(0x10AE5, 0x10AE6, Extend);
            // Mn   [4] HANIFI ROHINGYA SIGN HARBAHAY..HANIFI ROHINGYA SIGN TASSI
            AddCodeRange(0x10D24, 0x10D27, Extend);
            // Mn   [2] YEZIDI COMBINING HAMZA MARK..YEZIDI COMBINING MADDA MARK
            AddCodeRange(0x10EAB, 0x10EAC, Extend);
            // Mn  [11] SOGDIAN COMBINING DOT BELOW..SOGDIAN COMBINING STROKE BELOW
            AddCodeRange(0x10F46, 0x10F50, Extend);
            // Mn   [4] OLD UYGHUR COMBINING DOT ABOVE..OLD UYGHUR COMBINING TWO DOTS BELOW
            AddCodeRange(0x10F82, 0x10F85, Extend);
            // Mn  [15] BRAHMI VOWEL SIGN AA..BRAHMI VIRAMA
            AddCodeRange(0x11038, 0x11046, Extend);
            // Mn   [2] BRAHMI VOWEL SIGN OLD TAMIL SHORT E..BRAHMI VOWEL SIGN OLD TAMIL SHORT O
            AddCodeRange(0x11073, 0x11074, Extend);
            // Mn   [3] BRAHMI NUMBER JOINER..KAITHI SIGN ANUSVARA
            AddCodeRange(0x1107F, 0x11081, Extend);
            // Mc   [3] KAITHI VOWEL SIGN AA..KAITHI VOWEL SIGN II
            AddCodeRange(0x110B0, 0x110B2, SpacingMark);
            // Mn   [4] KAITHI VOWEL SIGN U..KAITHI VOWEL SIGN AI
            AddCodeRange(0x110B3, 0x110B6, Extend);
            // Mc   [2] KAITHI VOWEL SIGN O..KAITHI VOWEL SIGN AU
            AddCodeRange(0x110B7, 0x110B8, SpacingMark);
            // Mn   [2] KAITHI SIGN VIRAMA..KAITHI SIGN NUKTA
            AddCodeRange(0x110B9, 0x110BA, Extend);
            // Mn   [3] CHAKMA SIGN CANDRABINDU..CHAKMA SIGN VISARGA
            AddCodeRange(0x11100, 0x11102, Extend);
            // Mn   [5] CHAKMA VOWEL SIGN A..CHAKMA VOWEL SIGN UU
            AddCodeRange(0x11127, 0x1112B, Extend);
            // Mn   [8] CHAKMA VOWEL SIGN AI..CHAKMA MAAYYAA
            AddCodeRange(0x1112D, 0x11134, Extend);
            // Mc   [2] CHAKMA VOWEL SIGN AA..CHAKMA VOWEL SIGN EI
            AddCodeRange(0x11145, 0x11146, SpacingMark);
            // Mn   [2] SHARADA SIGN CANDRABINDU..SHARADA SIGN ANUSVARA
            AddCodeRange(0x11180, 0x11181, Extend);
            // Mc   [3] SHARADA VOWEL SIGN AA..SHARADA VOWEL SIGN II
            AddCodeRange(0x111B3, 0x111B5, SpacingMark);
            // Mn   [9] SHARADA VOWEL SIGN U..SHARADA VOWEL SIGN O
            AddCodeRange(0x111B6, 0x111BE, Extend);
            // Mc   [2] SHARADA VOWEL SIGN AU..SHARADA SIGN VIRAMA
            AddCodeRange(0x111BF, 0x111C0, SpacingMark);
            // Lo   [2] SHARADA SIGN JIHVAMULIYA..SHARADA SIGN UPADHMANIYA
            AddCodeRange(0x111C2, 0x111C3, Prepend);
            // Mn   [4] SHARADA SANDHI MARK..SHARADA EXTRA SHORT VOWEL MARK
            AddCodeRange(0x111C9, 0x111CC, Extend);
            // Mc   [3] KHOJKI VOWEL SIGN AA..KHOJKI VOWEL SIGN II
            AddCodeRange(0x1122C, 0x1122E, SpacingMark);
            // Mn   [3] KHOJKI VOWEL SIGN U..KHOJKI VOWEL SIGN AI
            AddCodeRange(0x1122F, 0x11231, Extend);
            // Mc   [2] KHOJKI VOWEL SIGN O..KHOJKI VOWEL SIGN AU
            AddCodeRange(0x11232, 0x11233, SpacingMark);
            // Mn   [2] KHOJKI SIGN NUKTA..KHOJKI SIGN SHADDA
            AddCodeRange(0x11236, 0x11237, Extend);
            // Mc   [3] KHUDAWADI VOWEL SIGN AA..KHUDAWADI VOWEL SIGN II
            AddCodeRange(0x112E0, 0x112E2, SpacingMark);
            // Mn   [8] KHUDAWADI VOWEL SIGN U..KHUDAWADI SIGN VIRAMA
            AddCodeRange(0x112E3, 0x112EA, Extend);
            // Mn   [2] GRANTHA SIGN COMBINING ANUSVARA ABOVE..GRANTHA SIGN CANDRABINDU
            AddCodeRange(0x11300, 0x11301, Extend);
            // Mc   [2] GRANTHA SIGN ANUSVARA..GRANTHA SIGN VISARGA
            AddCodeRange(0x11302, 0x11303, SpacingMark);
            // Mn   [2] COMBINING BINDU BELOW..GRANTHA SIGN NUKTA
            AddCodeRange(0x1133B, 0x1133C, Extend);
            // Mc   [4] GRANTHA VOWEL SIGN U..GRANTHA VOWEL SIGN VOCALIC RR
            AddCodeRange(0x11341, 0x11344, SpacingMark);
            // Mc   [2] GRANTHA VOWEL SIGN EE..GRANTHA VOWEL SIGN AI
            AddCodeRange(0x11347, 0x11348, SpacingMark);
            // Mc   [3] GRANTHA VOWEL SIGN OO..GRANTHA SIGN VIRAMA
            AddCodeRange(0x1134B, 0x1134D, SpacingMark);
            // Mc   [2] GRANTHA VOWEL SIGN VOCALIC L..GRANTHA VOWEL SIGN VOCALIC LL
            AddCodeRange(0x11362, 0x11363, SpacingMark);
            // Mn   [7] COMBINING GRANTHA DIGIT ZERO..COMBINING GRANTHA DIGIT SIX
            AddCodeRange(0x11366, 0x1136C, Extend);
            // Mn   [5] COMBINING GRANTHA LETTER A..COMBINING GRANTHA LETTER PA
            AddCodeRange(0x11370, 0x11374, Extend);
            // Mc   [3] NEWA VOWEL SIGN AA..NEWA VOWEL SIGN II
            AddCodeRange(0x11435, 0x11437, SpacingMark);
            // Mn   [8] NEWA VOWEL SIGN U..NEWA VOWEL SIGN AI
            AddCodeRange(0x11438, 0x1143F, Extend);
            // Mc   [2] NEWA VOWEL SIGN O..NEWA VOWEL SIGN AU
            AddCodeRange(0x11440, 0x11441, SpacingMark);
            // Mn   [3] NEWA SIGN VIRAMA..NEWA SIGN ANUSVARA
            AddCodeRange(0x11442, 0x11444, Extend);
            // Mc   [2] TIRHUTA VOWEL SIGN I..TIRHUTA VOWEL SIGN II
            AddCodeRange(0x114B1, 0x114B2, SpacingMark);
            // Mn   [6] TIRHUTA VOWEL SIGN U..TIRHUTA VOWEL SIGN VOCALIC LL
            AddCodeRange(0x114B3, 0x114B8, Extend);
            // Mc   [2] TIRHUTA VOWEL SIGN AI..TIRHUTA VOWEL SIGN O
            AddCodeRange(0x114BB, 0x114BC, SpacingMark);
            // Mn   [2] TIRHUTA SIGN CANDRABINDU..TIRHUTA SIGN ANUSVARA
            AddCodeRange(0x114BF, 0x114C0, Extend);
            // Mn   [2] TIRHUTA SIGN VIRAMA..TIRHUTA SIGN NUKTA
            AddCodeRange(0x114C2, 0x114C3, Extend);
            // Mc   [2] SIDDHAM VOWEL SIGN I..SIDDHAM VOWEL SIGN II
            AddCodeRange(0x115B0, 0x115B1, SpacingMark);
            // Mn   [4] SIDDHAM VOWEL SIGN U..SIDDHAM VOWEL SIGN VOCALIC RR
            AddCodeRange(0x115B2, 0x115B5, Extend);
            // Mc   [4] SIDDHAM VOWEL SIGN E..SIDDHAM VOWEL SIGN AU
            AddCodeRange(0x115B8, 0x115BB, SpacingMark);
            // Mn   [2] SIDDHAM SIGN CANDRABINDU..SIDDHAM SIGN ANUSVARA
            AddCodeRange(0x115BC, 0x115BD, Extend);
            // Mn   [2] SIDDHAM SIGN VIRAMA..SIDDHAM SIGN NUKTA
            AddCodeRange(0x115BF, 0x115C0, Extend);
            // Mn   [2] SIDDHAM VOWEL SIGN ALTERNATE U..SIDDHAM VOWEL SIGN ALTERNATE UU
            AddCodeRange(0x115DC, 0x115DD, Extend);
            // Mc   [3] MODI VOWEL SIGN AA..MODI VOWEL SIGN II
            AddCodeRange(0x11630, 0x11632, SpacingMark);
            // Mn   [8] MODI VOWEL SIGN U..MODI VOWEL SIGN AI
            AddCodeRange(0x11633, 0x1163A, Extend);
            // Mc   [2] MODI VOWEL SIGN O..MODI VOWEL SIGN AU
            AddCodeRange(0x1163B, 0x1163C, SpacingMark);
            // Mn   [2] MODI SIGN VIRAMA..MODI SIGN ARDHACANDRA
            AddCodeRange(0x1163F, 0x11640, Extend);
            // Mc   [2] TAKRI VOWEL SIGN I..TAKRI VOWEL SIGN II
            AddCodeRange(0x116AE, 0x116AF, SpacingMark);
            // Mn   [6] TAKRI VOWEL SIGN U..TAKRI VOWEL SIGN AU
            AddCodeRange(0x116B0, 0x116B5, Extend);
            // Mn   [3] AHOM CONSONANT SIGN MEDIAL LA..AHOM CONSONANT SIGN MEDIAL LIGATING RA
            AddCodeRange(0x1171D, 0x1171F, Extend);
            // Mn   [4] AHOM VOWEL SIGN I..AHOM VOWEL SIGN UU
            AddCodeRange(0x11722, 0x11725, Extend);
            // Mn   [5] AHOM VOWEL SIGN AW..AHOM SIGN KILLER
            AddCodeRange(0x11727, 0x1172B, Extend);
            // Mc   [3] DOGRA VOWEL SIGN AA..DOGRA VOWEL SIGN II
            AddCodeRange(0x1182C, 0x1182E, SpacingMark);
            // Mn   [9] DOGRA VOWEL SIGN U..DOGRA SIGN ANUSVARA
            AddCodeRange(0x1182F, 0x11837, Extend);
            // Mn   [2] DOGRA SIGN VIRAMA..DOGRA SIGN NUKTA
            AddCodeRange(0x11839, 0x1183A, Extend);
            // Mc   [5] DIVES AKURU VOWEL SIGN I..DIVES AKURU VOWEL SIGN E
            AddCodeRange(0x11931, 0x11935, SpacingMark);
            // Mc   [2] DIVES AKURU VOWEL SIGN AI..DIVES AKURU VOWEL SIGN O
            AddCodeRange(0x11937, 0x11938, SpacingMark);
            // Mn   [2] DIVES AKURU SIGN ANUSVARA..DIVES AKURU SIGN CANDRABINDU
            AddCodeRange(0x1193B, 0x1193C, Extend);
            // Mc   [3] NANDINAGARI VOWEL SIGN AA..NANDINAGARI VOWEL SIGN II
            AddCodeRange(0x119D1, 0x119D3, SpacingMark);
            // Mn   [4] NANDINAGARI VOWEL SIGN U..NANDINAGARI VOWEL SIGN VOCALIC RR
            AddCodeRange(0x119D4, 0x119D7, Extend);
            // Mn   [2] NANDINAGARI VOWEL SIGN E..NANDINAGARI VOWEL SIGN AI
            AddCodeRange(0x119DA, 0x119DB, Extend);
            // Mc   [4] NANDINAGARI VOWEL SIGN O..NANDINAGARI SIGN VISARGA
            AddCodeRange(0x119DC, 0x119DF, SpacingMark);
            // Mn  [10] ZANABAZAR SQUARE VOWEL SIGN I..ZANABAZAR SQUARE VOWEL LENGTH MARK
            AddCodeRange(0x11A01, 0x11A0A, Extend);
            // Mn   [6] ZANABAZAR SQUARE FINAL CONSONANT MARK..ZANABAZAR SQUARE SIGN ANUSVARA
            AddCodeRange(0x11A33, 0x11A38, Extend);
            // Mn   [4] ZANABAZAR SQUARE CLUSTER-FINAL LETTER YA..ZANABAZAR SQUARE CLUSTER-FINAL LETTER VA
            AddCodeRange(0x11A3B, 0x11A3E, Extend);
            // Mn   [6] SOYOMBO VOWEL SIGN I..SOYOMBO VOWEL SIGN OE
            AddCodeRange(0x11A51, 0x11A56, Extend);
            // Mc   [2] SOYOMBO VOWEL SIGN AI..SOYOMBO VOWEL SIGN AU
            AddCodeRange(0x11A57, 0x11A58, SpacingMark);
            // Mn   [3] SOYOMBO VOWEL SIGN VOCALIC R..SOYOMBO VOWEL LENGTH MARK
            AddCodeRange(0x11A59, 0x11A5B, Extend);
            // Lo   [6] SOYOMBO SIGN JIHVAMULIYA..SOYOMBO CLUSTER-INITIAL LETTER SA
            AddCodeRange(0x11A84, 0x11A89, Prepend);
            // Mn  [13] SOYOMBO FINAL CONSONANT SIGN G..SOYOMBO SIGN ANUSVARA
            AddCodeRange(0x11A8A, 0x11A96, Extend);
            // Mn   [2] SOYOMBO GEMINATION MARK..SOYOMBO SUBJOINER
            AddCodeRange(0x11A98, 0x11A99, Extend);
            // Mn   [7] BHAIKSUKI VOWEL SIGN I..BHAIKSUKI VOWEL SIGN VOCALIC L
            AddCodeRange(0x11C30, 0x11C36, Extend);
            // Mn   [6] BHAIKSUKI VOWEL SIGN E..BHAIKSUKI SIGN ANUSVARA
            AddCodeRange(0x11C38, 0x11C3D, Extend);
            // Mn  [22] MARCHEN SUBJOINED LETTER KA..MARCHEN SUBJOINED LETTER ZA
            AddCodeRange(0x11C92, 0x11CA7, Extend);
            // Mn   [7] MARCHEN SUBJOINED LETTER RA..MARCHEN VOWEL SIGN AA
            AddCodeRange(0x11CAA, 0x11CB0, Extend);
            // Mn   [2] MARCHEN VOWEL SIGN U..MARCHEN VOWEL SIGN E
            AddCodeRange(0x11CB2, 0x11CB3, Extend);
            // Mn   [2] MARCHEN SIGN ANUSVARA..MARCHEN SIGN CANDRABINDU
            AddCodeRange(0x11CB5, 0x11CB6, Extend);
            // Mn   [6] MASARAM GONDI VOWEL SIGN AA..MASARAM GONDI VOWEL SIGN VOCALIC R
            AddCodeRange(0x11D31, 0x11D36, Extend);
            // Mn   [2] MASARAM GONDI VOWEL SIGN AI..MASARAM GONDI VOWEL SIGN O
            AddCodeRange(0x11D3C, 0x11D3D, Extend);
            // Mn   [7] MASARAM GONDI VOWEL SIGN AU..MASARAM GONDI VIRAMA
            AddCodeRange(0x11D3F, 0x11D45, Extend);
            // Mc   [5] GUNJALA GONDI VOWEL SIGN AA..GUNJALA GONDI VOWEL SIGN UU
            AddCodeRange(0x11D8A, 0x11D8E, SpacingMark);
            // Mn   [2] GUNJALA GONDI VOWEL SIGN EE..GUNJALA GONDI VOWEL SIGN AI
            AddCodeRange(0x11D90, 0x11D91, Extend);
            // Mc   [2] GUNJALA GONDI VOWEL SIGN OO..GUNJALA GONDI VOWEL SIGN AU
            AddCodeRange(0x11D93, 0x11D94, SpacingMark);
            // Mn   [2] MAKASAR VOWEL SIGN I..MAKASAR VOWEL SIGN U
            AddCodeRange(0x11EF3, 0x11EF4, Extend);
            // Mc   [2] MAKASAR VOWEL SIGN E..MAKASAR VOWEL SIGN O
            AddCodeRange(0x11EF5, 0x11EF6, SpacingMark);
            // Cf   [9] EGYPTIAN HIEROGLYPH VERTICAL JOINER..EGYPTIAN HIEROGLYPH END SEGMENT
            AddCodeRange(0x13430, 0x13438, Control);
            // Mn   [5] BASSA VAH COMBINING HIGH TONE..BASSA VAH COMBINING HIGH-LOW TONE
            AddCodeRange(0x16AF0, 0x16AF4, Extend);
            // Mn   [7] PAHAWH HMONG MARK CIM TUB..PAHAWH HMONG MARK CIM TAUM
            AddCodeRange(0x16B30, 0x16B36, Extend);
            // Mc  [55] MIAO SIGN ASPIRATION..MIAO VOWEL SIGN UI
            AddCodeRange(0x16F51, 0x16F87, SpacingMark);
            // Mn   [4] MIAO TONE RIGHT..MIAO TONE BELOW
            AddCodeRange(0x16F8F, 0x16F92, Extend);
            // Mc   [2] VIETNAMESE ALTERNATE READING MARK CA..VIETNAMESE ALTERNATE READING MARK NHAY
            AddCodeRange(0x16FF0, 0x16FF1, SpacingMark);
            // Mn   [2] DUPLOYAN THICK LETTER SELECTOR..DUPLOYAN DOUBLE MARK
            AddCodeRange(0x1BC9D, 0x1BC9E, Extend);
            // Cf   [4] SHORTHAND FORMAT LETTER OVERLAP..SHORTHAND FORMAT UP STEP
            AddCodeRange(0x1BCA0, 0x1BCA3, Control);
            // Mn  [46] ZNAMENNY COMBINING MARK GORAZDO NIZKO S KRYZHEM ON LEFT..ZNAMENNY COMBINING MARK KRYZH ON LEFT
            AddCodeRange(0x1CF00, 0x1CF2D, Extend);
            // Mn  [23] ZNAMENNY COMBINING TONAL RANGE MARK MRACHNO..ZNAMENNY PRIZNAK MODIFIER ROG
            AddCodeRange(0x1CF30, 0x1CF46, Extend);
            // Mn   [3] MUSICAL SYMBOL COMBINING TREMOLO-1..MUSICAL SYMBOL COMBINING TREMOLO-3
            AddCodeRange(0x1D167, 0x1D169, Extend);
            // Mc   [5] MUSICAL SYMBOL COMBINING FLAG-1..MUSICAL SYMBOL COMBINING FLAG-5
            AddCodeRange(0x1D16E, 0x1D172, Extend);
            // Cf   [8] MUSICAL SYMBOL BEGIN BEAM..MUSICAL SYMBOL END PHRASE
            AddCodeRange(0x1D173, 0x1D17A, Control);
            // Mn   [8] MUSICAL SYMBOL COMBINING ACCENT..MUSICAL SYMBOL COMBINING LOURE
            AddCodeRange(0x1D17B, 0x1D182, Extend);
            // Mn   [7] MUSICAL SYMBOL COMBINING DOIT..MUSICAL SYMBOL COMBINING TRIPLE TONGUE
            AddCodeRange(0x1D185, 0x1D18B, Extend);
            // Mn   [4] MUSICAL SYMBOL COMBINING DOWN BOW..MUSICAL SYMBOL COMBINING SNAP PIZZICATO
            AddCodeRange(0x1D1AA, 0x1D1AD, Extend);
            // Mn   [3] COMBINING GREEK MUSICAL TRISEME..COMBINING GREEK MUSICAL PENTASEME
            AddCodeRange(0x1D242, 0x1D244, Extend);
            // Mn  [55] SIGNWRITING HEAD RIM..SIGNWRITING AIR SUCKING IN
            AddCodeRange(0x1DA00, 0x1DA36, Extend);
            // Mn  [50] SIGNWRITING MOUTH CLOSED NEUTRAL..SIGNWRITING EXCITEMENT
            AddCodeRange(0x1DA3B, 0x1DA6C, Extend);
            // Mn   [5] SIGNWRITING FILL MODIFIER-2..SIGNWRITING FILL MODIFIER-6
            AddCodeRange(0x1DA9B, 0x1DA9F, Extend);
            // Mn  [15] SIGNWRITING ROTATION MODIFIER-2..SIGNWRITING ROTATION MODIFIER-16
            AddCodeRange(0x1DAA1, 0x1DAAF, Extend);
            // Mn   [7] COMBINING GLAGOLITIC LETTER AZU..COMBINING GLAGOLITIC LETTER ZHIVETE
            AddCodeRange(0x1E000, 0x1E006, Extend);
            // Mn  [17] COMBINING GLAGOLITIC LETTER ZEMLJA..COMBINING GLAGOLITIC LETTER HERU
            AddCodeRange(0x1E008, 0x1E018, Extend);
            // Mn   [7] COMBINING GLAGOLITIC LETTER SHTA..COMBINING GLAGOLITIC LETTER YATI
            AddCodeRange(0x1E01B, 0x1E021, Extend);
            // Mn   [2] COMBINING GLAGOLITIC LETTER YU..COMBINING GLAGOLITIC LETTER SMALL YUS
            AddCodeRange(0x1E023, 0x1E024, Extend);
            // Mn   [5] COMBINING GLAGOLITIC LETTER YO..COMBINING GLAGOLITIC LETTER FITA
            AddCodeRange(0x1E026, 0x1E02A, Extend);
            // Mn   [7] NYIAKENG PUACHUE HMONG TONE-B..NYIAKENG PUACHUE HMONG TONE-D
            AddCodeRange(0x1E130, 0x1E136, Extend);
            // Mn   [4] WANCHO TONE TUP..WANCHO TONE KOINI
            AddCodeRange(0x1E2EC, 0x1E2EF, Extend);
            // Mn   [7] MENDE KIKAKUI COMBINING NUMBER TEENS..MENDE KIKAKUI COMBINING NUMBER MILLIONS
            AddCodeRange(0x1E8D0, 0x1E8D6, Extend);
            // Mn   [7] ADLAM ALIF LENGTHENER..ADLAM NUKTA
            AddCodeRange(0x1E944, 0x1E94A, Extend);
            // E0.0   [4] (🀀..🀃)    MAHJONG TILE EAST WIND..MAHJONG TILE NORTH WIND
            AddCodeRange(0x1F000, 0x1F003, Extended_Pictographic);
            // E0.0 [202] (🀅..🃎)    MAHJONG TILE GREEN DRAGON..PLAYING CARD KING OF DIAMONDS
            AddCodeRange(0x1F005, 0x1F0CE, Extended_Pictographic);
            // E0.0  [48] (🃐..🃿)    <reserved-1F0D0>..<reserved-1F0FF>
            AddCodeRange(0x1F0D0, 0x1F0FF, Extended_Pictographic);
            // E0.0   [3] (🄍..🄏)    CIRCLED ZERO WITH SLASH..CIRCLED DOLLAR SIGN WITH OVERLAID BACKSLASH
            AddCodeRange(0x1F10D, 0x1F10F, Extended_Pictographic);
            // E0.0   [4] (🅬..🅯)    RAISED MR SIGN..CIRCLED HUMAN FIGURE
            AddCodeRange(0x1F16C, 0x1F16F, Extended_Pictographic);
            // E0.6   [2] (🅰️..🅱️)    A button (blood type)..B button (blood type)
            AddCodeRange(0x1F170, 0x1F171, Extended_Pictographic);
            // E0.6   [2] (🅾️..🅿️)    O button (blood type)..P button
            AddCodeRange(0x1F17E, 0x1F17F, Extended_Pictographic);
            // E0.6  [10] (🆑..🆚)    CL button..VS button
            AddCodeRange(0x1F191, 0x1F19A, Extended_Pictographic);
            // E0.0  [57] (🆭..🇥)    MASK WORK SYMBOL..<reserved-1F1E5>
            AddCodeRange(0x1F1AD, 0x1F1E5, Extended_Pictographic);
            // So  [26] REGIONAL INDICATOR SYMBOL LETTER A..REGIONAL INDICATOR SYMBOL LETTER Z
            AddCodeRange(0x1F1E6, 0x1F1FF, Regional_Indicator);
            // E0.6   [2] (🈁..🈂️)    Japanese “here” button..Japanese “service charge” button
            AddCodeRange(0x1F201, 0x1F202, Extended_Pictographic);
            // E0.0  [13] (🈃..🈏)    <reserved-1F203>..<reserved-1F20F>
            AddCodeRange(0x1F203, 0x1F20F, Extended_Pictographic);
            // E0.6   [9] (🈲..🈺)    Japanese “prohibited” button..Japanese “open for business” button
            AddCodeRange(0x1F232, 0x1F23A, Extended_Pictographic);
            // E0.0   [4] (🈼..🈿)    <reserved-1F23C>..<reserved-1F23F>
            AddCodeRange(0x1F23C, 0x1F23F, Extended_Pictographic);
            // E0.0   [7] (🉉..🉏)    <reserved-1F249>..<reserved-1F24F>
            AddCodeRange(0x1F249, 0x1F24F, Extended_Pictographic);
            // E0.6   [2] (🉐..🉑)    Japanese “bargain” button..Japanese “acceptable” button
            AddCodeRange(0x1F250, 0x1F251, Extended_Pictographic);
            // E0.0 [174] (🉒..🋿)    <reserved-1F252>..<reserved-1F2FF>
            AddCodeRange(0x1F252, 0x1F2FF, Extended_Pictographic);
            // E0.6  [13] (🌀..🌌)    cyclone..milky way
            AddCodeRange(0x1F300, 0x1F30C, Extended_Pictographic);
            // E0.7   [2] (🌍..🌎)    globe showing Europe-Africa..globe showing Americas
            AddCodeRange(0x1F30D, 0x1F30E, Extended_Pictographic);
            // E0.6   [3] (🌓..🌕)    first quarter moon..full moon
            AddCodeRange(0x1F313, 0x1F315, Extended_Pictographic);
            // E1.0   [3] (🌖..🌘)    waning gibbous moon..waning crescent moon
            AddCodeRange(0x1F316, 0x1F318, Extended_Pictographic);
            // E1.0   [2] (🌝..🌞)    full moon face..sun with face
            AddCodeRange(0x1F31D, 0x1F31E, Extended_Pictographic);
            // E0.6   [2] (🌟..🌠)    glowing star..shooting star
            AddCodeRange(0x1F31F, 0x1F320, Extended_Pictographic);
            // E0.0   [2] (🌢..🌣)    BLACK DROPLET..WHITE SUN
            AddCodeRange(0x1F322, 0x1F323, Extended_Pictographic);
            // E0.7   [9] (🌤️..🌬️)    sun behind small cloud..wind face
            AddCodeRange(0x1F324, 0x1F32C, Extended_Pictographic);
            // E1.0   [3] (🌭..🌯)    hot dog..burrito
            AddCodeRange(0x1F32D, 0x1F32F, Extended_Pictographic);
            // E0.6   [2] (🌰..🌱)    chestnut..seedling
            AddCodeRange(0x1F330, 0x1F331, Extended_Pictographic);
            // E1.0   [2] (🌲..🌳)    evergreen tree..deciduous tree
            AddCodeRange(0x1F332, 0x1F333, Extended_Pictographic);
            // E0.6   [2] (🌴..🌵)    palm tree..cactus
            AddCodeRange(0x1F334, 0x1F335, Extended_Pictographic);
            // E0.6  [20] (🌷..🍊)    tulip..tangerine
            AddCodeRange(0x1F337, 0x1F34A, Extended_Pictographic);
            // E0.6   [4] (🍌..🍏)    banana..green apple
            AddCodeRange(0x1F34C, 0x1F34F, Extended_Pictographic);
            // E0.6  [43] (🍑..🍻)    peach..clinking beer mugs
            AddCodeRange(0x1F351, 0x1F37B, Extended_Pictographic);
            // E1.0   [2] (🍾..🍿)    bottle with popping cork..popcorn
            AddCodeRange(0x1F37E, 0x1F37F, Extended_Pictographic);
            // E0.6  [20] (🎀..🎓)    ribbon..graduation cap
            AddCodeRange(0x1F380, 0x1F393, Extended_Pictographic);
            // E0.0   [2] (🎔..🎕)    HEART WITH TIP ON THE LEFT..BOUQUET OF FLOWERS
            AddCodeRange(0x1F394, 0x1F395, Extended_Pictographic);
            // E0.7   [2] (🎖️..🎗️)    military medal..reminder ribbon
            AddCodeRange(0x1F396, 0x1F397, Extended_Pictographic);
            // E0.7   [3] (🎙️..🎛️)    studio microphone..control knobs
            AddCodeRange(0x1F399, 0x1F39B, Extended_Pictographic);
            // E0.0   [2] (🎜..🎝)    BEAMED ASCENDING MUSICAL NOTES..BEAMED DESCENDING MUSICAL NOTES
            AddCodeRange(0x1F39C, 0x1F39D, Extended_Pictographic);
            // E0.7   [2] (🎞️..🎟️)    film frames..admission tickets
            AddCodeRange(0x1F39E, 0x1F39F, Extended_Pictographic);
            // E0.6  [37] (🎠..🏄)    carousel horse..person surfing
            AddCodeRange(0x1F3A0, 0x1F3C4, Extended_Pictographic);
            // E0.7   [4] (🏋️..🏎️)    person lifting weights..racing car
            AddCodeRange(0x1F3CB, 0x1F3CE, Extended_Pictographic);
            // E1.0   [5] (🏏..🏓)    cricket game..ping pong
            AddCodeRange(0x1F3CF, 0x1F3D3, Extended_Pictographic);
            // E0.7  [12] (🏔️..🏟️)    snow-capped mountain..stadium
            AddCodeRange(0x1F3D4, 0x1F3DF, Extended_Pictographic);
            // E0.6   [4] (🏠..🏣)    house..Japanese post office
            AddCodeRange(0x1F3E0, 0x1F3E3, Extended_Pictographic);
            // E0.6  [12] (🏥..🏰)    hospital..castle
            AddCodeRange(0x1F3E5, 0x1F3F0, Extended_Pictographic);
            // E0.0   [2] (🏱..🏲)    WHITE PENNANT..BLACK PENNANT
            AddCodeRange(0x1F3F1, 0x1F3F2, Extended_Pictographic);
            // E1.0   [3] (🏸..🏺)    badminton..amphora
            AddCodeRange(0x1F3F8, 0x1F3FA, Extended_Pictographic);
            // Sk   [5] EMOJI MODIFIER FITZPATRICK TYPE-1-2..EMOJI MODIFIER FITZPATRICK TYPE-6
            AddCodeRange(0x1F3FB, 0x1F3FF, Extend);
            // E1.0   [8] (🐀..🐇)    rat..rabbit
            AddCodeRange(0x1F400, 0x1F407, Extended_Pictographic);
            // E1.0   [3] (🐉..🐋)    dragon..whale
            AddCodeRange(0x1F409, 0x1F40B, Extended_Pictographic);
            // E0.6   [3] (🐌..🐎)    snail..horse
            AddCodeRange(0x1F40C, 0x1F40E, Extended_Pictographic);
            // E1.0   [2] (🐏..🐐)    ram..goat
            AddCodeRange(0x1F40F, 0x1F410, Extended_Pictographic);
            // E0.6   [2] (🐑..🐒)    ewe..monkey
            AddCodeRange(0x1F411, 0x1F412, Extended_Pictographic);
            // E0.6  [19] (🐗..🐩)    boar..poodle
            AddCodeRange(0x1F417, 0x1F429, Extended_Pictographic);
            // E0.6  [20] (🐫..🐾)    two-hump camel..paw prints
            AddCodeRange(0x1F42B, 0x1F43E, Extended_Pictographic);
            // E0.6  [35] (👂..👤)    ear..bust in silhouette
            AddCodeRange(0x1F442, 0x1F464, Extended_Pictographic);
            // E0.6   [6] (👦..👫)    boy..woman and man holding hands
            AddCodeRange(0x1F466, 0x1F46B, Extended_Pictographic);
            // E1.0   [2] (👬..👭)    men holding hands..women holding hands
            AddCodeRange(0x1F46C, 0x1F46D, Extended_Pictographic);
            // E0.6  [63] (👮..💬)    police officer..speech balloon
            AddCodeRange(0x1F46E, 0x1F4AC, Extended_Pictographic);
            // E0.6   [8] (💮..💵)    white flower..dollar banknote
            AddCodeRange(0x1F4AE, 0x1F4B5, Extended_Pictographic);
            // E1.0   [2] (💶..💷)    euro banknote..pound banknote
            AddCodeRange(0x1F4B6, 0x1F4B7, Extended_Pictographic);
            // E0.6  [52] (💸..📫)    money with wings..closed mailbox with raised flag
            AddCodeRange(0x1F4B8, 0x1F4EB, Extended_Pictographic);
            // E0.7   [2] (📬..📭)    open mailbox with raised flag..open mailbox with lowered flag
            AddCodeRange(0x1F4EC, 0x1F4ED, Extended_Pictographic);
            // E0.6   [5] (📰..📴)    newspaper..mobile phone off
            AddCodeRange(0x1F4F0, 0x1F4F4, Extended_Pictographic);
            // E0.6   [2] (📶..📷)    antenna bars..camera
            AddCodeRange(0x1F4F6, 0x1F4F7, Extended_Pictographic);
            // E0.6   [4] (📹..📼)    video camera..videocassette
            AddCodeRange(0x1F4F9, 0x1F4FC, Extended_Pictographic);
            // E1.0   [4] (📿..🔂)    prayer beads..repeat single button
            AddCodeRange(0x1F4FF, 0x1F502, Extended_Pictographic);
            // E1.0   [4] (🔄..🔇)    counterclockwise arrows button..muted speaker
            AddCodeRange(0x1F504, 0x1F507, Extended_Pictographic);
            // E0.6  [11] (🔊..🔔)    speaker high volume..bell
            AddCodeRange(0x1F50A, 0x1F514, Extended_Pictographic);
            // E0.6  [22] (🔖..🔫)    bookmark..water pistol
            AddCodeRange(0x1F516, 0x1F52B, Extended_Pictographic);
            // E1.0   [2] (🔬..🔭)    microscope..telescope
            AddCodeRange(0x1F52C, 0x1F52D, Extended_Pictographic);
            // E0.6  [16] (🔮..🔽)    crystal ball..downwards button
            AddCodeRange(0x1F52E, 0x1F53D, Extended_Pictographic);
            // E0.0   [3] (🕆..🕈)    WHITE LATIN CROSS..CELTIC CROSS
            AddCodeRange(0x1F546, 0x1F548, Extended_Pictographic);
            // E0.7   [2] (🕉️..🕊️)    om..dove
            AddCodeRange(0x1F549, 0x1F54A, Extended_Pictographic);
            // E1.0   [4] (🕋..🕎)    kaaba..menorah
            AddCodeRange(0x1F54B, 0x1F54E, Extended_Pictographic);
            // E0.6  [12] (🕐..🕛)    one o’clock..twelve o’clock
            AddCodeRange(0x1F550, 0x1F55B, Extended_Pictographic);
            // E0.7  [12] (🕜..🕧)    one-thirty..twelve-thirty
            AddCodeRange(0x1F55C, 0x1F567, Extended_Pictographic);
            // E0.0   [7] (🕨..🕮)    RIGHT SPEAKER..BOOK
            AddCodeRange(0x1F568, 0x1F56E, Extended_Pictographic);
            // E0.7   [2] (🕯️..🕰️)    candle..mantelpiece clock
            AddCodeRange(0x1F56F, 0x1F570, Extended_Pictographic);
            // E0.0   [2] (🕱..🕲)    BLACK SKULL AND CROSSBONES..NO PIRACY
            AddCodeRange(0x1F571, 0x1F572, Extended_Pictographic);
            // E0.7   [7] (🕳️..🕹️)    hole..joystick
            AddCodeRange(0x1F573, 0x1F579, Extended_Pictographic);
            // E0.0  [12] (🕻..🖆)    LEFT HAND TELEPHONE RECEIVER..PEN OVER STAMPED ENVELOPE
            AddCodeRange(0x1F57B, 0x1F586, Extended_Pictographic);
            // E0.0   [2] (🖈..🖉)    BLACK PUSHPIN..LOWER LEFT PENCIL
            AddCodeRange(0x1F588, 0x1F589, Extended_Pictographic);
            // E0.7   [4] (🖊️..🖍️)    pen..crayon
            AddCodeRange(0x1F58A, 0x1F58D, Extended_Pictographic);
            // E0.0   [2] (🖎..🖏)    LEFT WRITING HAND..TURNED OK HAND SIGN
            AddCodeRange(0x1F58E, 0x1F58F, Extended_Pictographic);
            // E0.0   [4] (🖑..🖔)    REVERSED RAISED HAND WITH FINGERS SPLAYED..REVERSED VICTORY HAND
            AddCodeRange(0x1F591, 0x1F594, Extended_Pictographic);
            // E1.0   [2] (🖕..🖖)    middle finger..vulcan salute
            AddCodeRange(0x1F595, 0x1F596, Extended_Pictographic);
            // E0.0  [13] (🖗..🖣)    WHITE DOWN POINTING LEFT HAND INDEX..BLACK DOWN POINTING BACKHAND INDEX
            AddCodeRange(0x1F597, 0x1F5A3, Extended_Pictographic);
            // E0.0   [2] (🖦..🖧)    KEYBOARD AND MOUSE..THREE NETWORKED COMPUTERS
            AddCodeRange(0x1F5A6, 0x1F5A7, Extended_Pictographic);
            // E0.0   [8] (🖩..🖰)    POCKET CALCULATOR..TWO BUTTON MOUSE
            AddCodeRange(0x1F5A9, 0x1F5B0, Extended_Pictographic);
            // E0.7   [2] (🖱️..🖲️)    computer mouse..trackball
            AddCodeRange(0x1F5B1, 0x1F5B2, Extended_Pictographic);
            // E0.0   [9] (🖳..🖻)    OLD PERSONAL COMPUTER..DOCUMENT WITH PICTURE
            AddCodeRange(0x1F5B3, 0x1F5BB, Extended_Pictographic);
            // E0.0   [5] (🖽..🗁)    FRAME WITH TILES..OPEN FOLDER
            AddCodeRange(0x1F5BD, 0x1F5C1, Extended_Pictographic);
            // E0.7   [3] (🗂️..🗄️)    card index dividers..file cabinet
            AddCodeRange(0x1F5C2, 0x1F5C4, Extended_Pictographic);
            // E0.0  [12] (🗅..🗐)    EMPTY NOTE..PAGES
            AddCodeRange(0x1F5C5, 0x1F5D0, Extended_Pictographic);
            // E0.7   [3] (🗑️..🗓️)    wastebasket..spiral calendar
            AddCodeRange(0x1F5D1, 0x1F5D3, Extended_Pictographic);
            // E0.0   [8] (🗔..🗛)    DESKTOP WINDOW..DECREASE FONT SIZE SYMBOL
            AddCodeRange(0x1F5D4, 0x1F5DB, Extended_Pictographic);
            // E0.7   [3] (🗜️..🗞️)    clamp..rolled-up newspaper
            AddCodeRange(0x1F5DC, 0x1F5DE, Extended_Pictographic);
            // E0.0   [2] (🗟..🗠)    PAGE WITH CIRCLED TEXT..STOCK CHART
            AddCodeRange(0x1F5DF, 0x1F5E0, Extended_Pictographic);
            // E0.0   [4] (🗤..🗧)    THREE RAYS ABOVE..THREE RAYS RIGHT
            AddCodeRange(0x1F5E4, 0x1F5E7, Extended_Pictographic);
            // E0.0   [6] (🗩..🗮)    RIGHT SPEECH BUBBLE..LEFT ANGER BUBBLE
            AddCodeRange(0x1F5E9, 0x1F5EE, Extended_Pictographic);
            // E0.0   [3] (🗰..🗲)    MOOD BUBBLE..LIGHTNING MOOD
            AddCodeRange(0x1F5F0, 0x1F5F2, Extended_Pictographic);
            // E0.0   [6] (🗴..🗹)    BALLOT SCRIPT X..BALLOT BOX WITH BOLD CHECK
            AddCodeRange(0x1F5F4, 0x1F5F9, Extended_Pictographic);
            // E0.6   [5] (🗻..🗿)    mount fuji..moai
            AddCodeRange(0x1F5FB, 0x1F5FF, Extended_Pictographic);
            // E0.6   [6] (😁..😆)    beaming face with smiling eyes..grinning squinting face
            AddCodeRange(0x1F601, 0x1F606, Extended_Pictographic);
            // E1.0   [2] (😇..😈)    smiling face with halo..smiling face with horns
            AddCodeRange(0x1F607, 0x1F608, Extended_Pictographic);
            // E0.6   [5] (😉..😍)    winking face..smiling face with heart-eyes
            AddCodeRange(0x1F609, 0x1F60D, Extended_Pictographic);
            // E0.6   [3] (😒..😔)    unamused face..pensive face
            AddCodeRange(0x1F612, 0x1F614, Extended_Pictographic);
            // E0.6   [3] (😜..😞)    winking face with tongue..disappointed face
            AddCodeRange(0x1F61C, 0x1F61E, Extended_Pictographic);
            // E0.6   [6] (😠..😥)    angry face..sad but relieved face
            AddCodeRange(0x1F620, 0x1F625, Extended_Pictographic);
            // E1.0   [2] (😦..😧)    frowning face with open mouth..anguished face
            AddCodeRange(0x1F626, 0x1F627, Extended_Pictographic);
            // E0.6   [4] (😨..😫)    fearful face..tired face
            AddCodeRange(0x1F628, 0x1F62B, Extended_Pictographic);
            // E1.0   [2] (😮..😯)    face with open mouth..hushed face
            AddCodeRange(0x1F62E, 0x1F62F, Extended_Pictographic);
            // E0.6   [4] (😰..😳)    anxious face with sweat..flushed face
            AddCodeRange(0x1F630, 0x1F633, Extended_Pictographic);
            // E0.6  [10] (😷..🙀)    face with medical mask..weary cat
            AddCodeRange(0x1F637, 0x1F640, Extended_Pictographic);
            // E1.0   [4] (🙁..🙄)    slightly frowning face..face with rolling eyes
            AddCodeRange(0x1F641, 0x1F644, Extended_Pictographic);
            // E0.6  [11] (🙅..🙏)    person gesturing NO..folded hands
            AddCodeRange(0x1F645, 0x1F64F, Extended_Pictographic);
            // E1.0   [2] (🚁..🚂)    helicopter..locomotive
            AddCodeRange(0x1F681, 0x1F682, Extended_Pictographic);
            // E0.6   [3] (🚃..🚅)    railway car..bullet train
            AddCodeRange(0x1F683, 0x1F685, Extended_Pictographic);
            // E1.0   [2] (🚊..🚋)    tram..tram car
            AddCodeRange(0x1F68A, 0x1F68B, Extended_Pictographic);
            // E0.6   [3] (🚑..🚓)    ambulance..police car
            AddCodeRange(0x1F691, 0x1F693, Extended_Pictographic);
            // E0.6   [2] (🚙..🚚)    sport utility vehicle..delivery truck
            AddCodeRange(0x1F699, 0x1F69A, Extended_Pictographic);
            // E1.0   [7] (🚛..🚡)    articulated lorry..aerial tramway
            AddCodeRange(0x1F69B, 0x1F6A1, Extended_Pictographic);
            // E0.6   [2] (🚤..🚥)    speedboat..horizontal traffic light
            AddCodeRange(0x1F6A4, 0x1F6A5, Extended_Pictographic);
            // E0.6   [7] (🚧..🚭)    construction..no smoking
            AddCodeRange(0x1F6A7, 0x1F6AD, Extended_Pictographic);
            // E1.0   [4] (🚮..🚱)    litter in bin sign..non-potable water
            AddCodeRange(0x1F6AE, 0x1F6B1, Extended_Pictographic);
            // E1.0   [3] (🚳..🚵)    no bicycles..person mountain biking
            AddCodeRange(0x1F6B3, 0x1F6B5, Extended_Pictographic);
            // E1.0   [2] (🚷..🚸)    no pedestrians..children crossing
            AddCodeRange(0x1F6B7, 0x1F6B8, Extended_Pictographic);
            // E0.6   [6] (🚹..🚾)    men’s room..water closet
            AddCodeRange(0x1F6B9, 0x1F6BE, Extended_Pictographic);
            // E1.0   [5] (🛁..🛅)    bathtub..left luggage
            AddCodeRange(0x1F6C1, 0x1F6C5, Extended_Pictographic);
            // E0.0   [5] (🛆..🛊)    TRIANGLE WITH ROUNDED CORNERS..GIRLS SYMBOL
            AddCodeRange(0x1F6C6, 0x1F6CA, Extended_Pictographic);
            // E0.7   [3] (🛍️..🛏️)    shopping bags..bed
            AddCodeRange(0x1F6CD, 0x1F6CF, Extended_Pictographic);
            // E3.0   [2] (🛑..🛒)    stop sign..shopping cart
            AddCodeRange(0x1F6D1, 0x1F6D2, Extended_Pictographic);
            // E0.0   [2] (🛓..🛔)    STUPA..PAGODA
            AddCodeRange(0x1F6D3, 0x1F6D4, Extended_Pictographic);
            // E13.0  [2] (🛖..🛗)    hut..elevator
            AddCodeRange(0x1F6D6, 0x1F6D7, Extended_Pictographic);
            // E0.0   [5] (🛘..🛜)    <reserved-1F6D8>..<reserved-1F6DC>
            AddCodeRange(0x1F6D8, 0x1F6DC, Extended_Pictographic);
            // E14.0  [3] (🛝..🛟)    playground slide..ring buoy
            AddCodeRange(0x1F6DD, 0x1F6DF, Extended_Pictographic);
            // E0.7   [6] (🛠️..🛥️)    hammer and wrench..motor boat
            AddCodeRange(0x1F6E0, 0x1F6E5, Extended_Pictographic);
            // E0.0   [3] (🛦..🛨)    UP-POINTING MILITARY AIRPLANE..UP-POINTING SMALL AIRPLANE
            AddCodeRange(0x1F6E6, 0x1F6E8, Extended_Pictographic);
            // E1.0   [2] (🛫..🛬)    airplane departure..airplane arrival
            AddCodeRange(0x1F6EB, 0x1F6EC, Extended_Pictographic);
            // E0.0   [3] (🛭..🛯)    <reserved-1F6ED>..<reserved-1F6EF>
            AddCodeRange(0x1F6ED, 0x1F6EF, Extended_Pictographic);
            // E0.0   [2] (🛱..🛲)    ONCOMING FIRE ENGINE..DIESEL LOCOMOTIVE
            AddCodeRange(0x1F6F1, 0x1F6F2, Extended_Pictographic);
            // E3.0   [3] (🛴..🛶)    kick scooter..canoe
            AddCodeRange(0x1F6F4, 0x1F6F6, Extended_Pictographic);
            // E5.0   [2] (🛷..🛸)    sled..flying saucer
            AddCodeRange(0x1F6F7, 0x1F6F8, Extended_Pictographic);
            // E13.0  [2] (🛻..🛼)    pickup truck..roller skate
            AddCodeRange(0x1F6FB, 0x1F6FC, Extended_Pictographic);
            // E0.0   [3] (🛽..🛿)    <reserved-1F6FD>..<reserved-1F6FF>
            AddCodeRange(0x1F6FD, 0x1F6FF, Extended_Pictographic);
            // E0.0  [12] (🝴..🝿)    <reserved-1F774>..<reserved-1F77F>
            AddCodeRange(0x1F774, 0x1F77F, Extended_Pictographic);
            // E0.0  [11] (🟕..🟟)    CIRCLED TRIANGLE..<reserved-1F7DF>
            AddCodeRange(0x1F7D5, 0x1F7DF, Extended_Pictographic);
            // E12.0 [12] (🟠..🟫)    orange circle..brown square
            AddCodeRange(0x1F7E0, 0x1F7EB, Extended_Pictographic);
            // E0.0   [4] (🟬..🟯)    <reserved-1F7EC>..<reserved-1F7EF>
            AddCodeRange(0x1F7EC, 0x1F7EF, Extended_Pictographic);
            // E0.0  [15] (🟱..🟿)    <reserved-1F7F1>..<reserved-1F7FF>
            AddCodeRange(0x1F7F1, 0x1F7FF, Extended_Pictographic);
            // E0.0   [4] (🠌..🠏)    <reserved-1F80C>..<reserved-1F80F>
            AddCodeRange(0x1F80C, 0x1F80F, Extended_Pictographic);
            // E0.0   [8] (🡈..🡏)    <reserved-1F848>..<reserved-1F84F>
            AddCodeRange(0x1F848, 0x1F84F, Extended_Pictographic);
            // E0.0   [6] (🡚..🡟)    <reserved-1F85A>..<reserved-1F85F>
            AddCodeRange(0x1F85A, 0x1F85F, Extended_Pictographic);
            // E0.0   [8] (🢈..🢏)    <reserved-1F888>..<reserved-1F88F>
            AddCodeRange(0x1F888, 0x1F88F, Extended_Pictographic);
            // E0.0  [82] (🢮..🣿)    <reserved-1F8AE>..<reserved-1F8FF>
            AddCodeRange(0x1F8AE, 0x1F8FF, Extended_Pictographic);
            // E12.0  [3] (🤍..🤏)    white heart..pinching hand
            AddCodeRange(0x1F90D, 0x1F90F, Extended_Pictographic);
            // E1.0   [9] (🤐..🤘)    zipper-mouth face..sign of the horns
            AddCodeRange(0x1F910, 0x1F918, Extended_Pictographic);
            // E3.0   [6] (🤙..🤞)    call me hand..crossed fingers
            AddCodeRange(0x1F919, 0x1F91E, Extended_Pictographic);
            // E3.0   [8] (🤠..🤧)    cowboy hat face..sneezing face
            AddCodeRange(0x1F920, 0x1F927, Extended_Pictographic);
            // E5.0   [8] (🤨..🤯)    face with raised eyebrow..exploding head
            AddCodeRange(0x1F928, 0x1F92F, Extended_Pictographic);
            // E5.0   [2] (🤱..🤲)    breast-feeding..palms up together
            AddCodeRange(0x1F931, 0x1F932, Extended_Pictographic);
            // E3.0   [8] (🤳..🤺)    selfie..person fencing
            AddCodeRange(0x1F933, 0x1F93A, Extended_Pictographic);
            // E3.0   [3] (🤼..🤾)    people wrestling..person playing handball
            AddCodeRange(0x1F93C, 0x1F93E, Extended_Pictographic);
            // E3.0   [6] (🥀..🥅)    wilted flower..goal net
            AddCodeRange(0x1F940, 0x1F945, Extended_Pictographic);
            // E3.0   [5] (🥇..🥋)    1st place medal..martial arts uniform
            AddCodeRange(0x1F947, 0x1F94B, Extended_Pictographic);
            // E11.0  [3] (🥍..🥏)    lacrosse..flying disc
            AddCodeRange(0x1F94D, 0x1F94F, Extended_Pictographic);
            // E3.0  [15] (🥐..🥞)    croissant..pancakes
            AddCodeRange(0x1F950, 0x1F95E, Extended_Pictographic);
            // E5.0  [13] (🥟..🥫)    dumpling..canned food
            AddCodeRange(0x1F95F, 0x1F96B, Extended_Pictographic);
            // E11.0  [5] (🥬..🥰)    leafy green..smiling face with hearts
            AddCodeRange(0x1F96C, 0x1F970, Extended_Pictographic);
            // E11.0  [4] (🥳..🥶)    partying face..cold face
            AddCodeRange(0x1F973, 0x1F976, Extended_Pictographic);
            // E13.0  [2] (🥷..🥸)    ninja..disguised face
            AddCodeRange(0x1F977, 0x1F978, Extended_Pictographic);
            // E11.0  [4] (🥼..🥿)    lab coat..flat shoe
            AddCodeRange(0x1F97C, 0x1F97F, Extended_Pictographic);
            // E1.0   [5] (🦀..🦄)    crab..unicorn
            AddCodeRange(0x1F980, 0x1F984, Extended_Pictographic);
            // E3.0  [13] (🦅..🦑)    eagle..squid
            AddCodeRange(0x1F985, 0x1F991, Extended_Pictographic);
            // E5.0   [6] (🦒..🦗)    giraffe..cricket
            AddCodeRange(0x1F992, 0x1F997, Extended_Pictographic);
            // E11.0 [11] (🦘..🦢)    kangaroo..swan
            AddCodeRange(0x1F998, 0x1F9A2, Extended_Pictographic);
            // E13.0  [2] (🦣..🦤)    mammoth..dodo
            AddCodeRange(0x1F9A3, 0x1F9A4, Extended_Pictographic);
            // E12.0  [6] (🦥..🦪)    sloth..oyster
            AddCodeRange(0x1F9A5, 0x1F9AA, Extended_Pictographic);
            // E13.0  [3] (🦫..🦭)    beaver..seal
            AddCodeRange(0x1F9AB, 0x1F9AD, Extended_Pictographic);
            // E12.0  [2] (🦮..🦯)    guide dog..white cane
            AddCodeRange(0x1F9AE, 0x1F9AF, Extended_Pictographic);
            // E11.0 [10] (🦰..🦹)    red hair..supervillain
            AddCodeRange(0x1F9B0, 0x1F9B9, Extended_Pictographic);
            // E12.0  [6] (🦺..🦿)    safety vest..mechanical leg
            AddCodeRange(0x1F9BA, 0x1F9BF, Extended_Pictographic);
            // E11.0  [2] (🧁..🧂)    cupcake..salt
            AddCodeRange(0x1F9C1, 0x1F9C2, Extended_Pictographic);
            // E12.0  [8] (🧃..🧊)    beverage box..ice
            AddCodeRange(0x1F9C3, 0x1F9CA, Extended_Pictographic);
            // E12.0  [3] (🧍..🧏)    person standing..deaf person
            AddCodeRange(0x1F9CD, 0x1F9CF, Extended_Pictographic);
            // E5.0  [23] (🧐..🧦)    face with monocle..socks
            AddCodeRange(0x1F9D0, 0x1F9E6, Extended_Pictographic);
            // E11.0 [25] (🧧..🧿)    red envelope..nazar amulet
            AddCodeRange(0x1F9E7, 0x1F9FF, Extended_Pictographic);
            // E0.0 [112] (🨀..🩯)    NEUTRAL CHESS KING..<reserved-1FA6F>
            AddCodeRange(0x1FA00, 0x1FA6F, Extended_Pictographic);
            // E12.0  [4] (🩰..🩳)    ballet shoes..shorts
            AddCodeRange(0x1FA70, 0x1FA73, Extended_Pictographic);
            // E0.0   [3] (🩵..🩷)    <reserved-1FA75>..<reserved-1FA77>
            AddCodeRange(0x1FA75, 0x1FA77, Extended_Pictographic);
            // E12.0  [3] (🩸..🩺)    drop of blood..stethoscope
            AddCodeRange(0x1FA78, 0x1FA7A, Extended_Pictographic);
            // E14.0  [2] (🩻..🩼)    x-ray..crutch
            AddCodeRange(0x1FA7B, 0x1FA7C, Extended_Pictographic);
            // E0.0   [3] (🩽..🩿)    <reserved-1FA7D>..<reserved-1FA7F>
            AddCodeRange(0x1FA7D, 0x1FA7F, Extended_Pictographic);
            // E12.0  [3] (🪀..🪂)    yo-yo..parachute
            AddCodeRange(0x1FA80, 0x1FA82, Extended_Pictographic);
            // E13.0  [4] (🪃..🪆)    boomerang..nesting dolls
            AddCodeRange(0x1FA83, 0x1FA86, Extended_Pictographic);
            // E0.0   [9] (🪇..🪏)    <reserved-1FA87>..<reserved-1FA8F>
            AddCodeRange(0x1FA87, 0x1FA8F, Extended_Pictographic);
            // E12.0  [6] (🪐..🪕)    ringed planet..banjo
            AddCodeRange(0x1FA90, 0x1FA95, Extended_Pictographic);
            // E13.0 [19] (🪖..🪨)    military helmet..rock
            AddCodeRange(0x1FA96, 0x1FAA8, Extended_Pictographic);
            // E14.0  [4] (🪩..🪬)    mirror ball..hamsa
            AddCodeRange(0x1FAA9, 0x1FAAC, Extended_Pictographic);
            // E0.0   [3] (🪭..🪯)    <reserved-1FAAD>..<reserved-1FAAF>
            AddCodeRange(0x1FAAD, 0x1FAAF, Extended_Pictographic);
            // E13.0  [7] (🪰..🪶)    fly..feather
            AddCodeRange(0x1FAB0, 0x1FAB6, Extended_Pictographic);
            // E14.0  [4] (🪷..🪺)    lotus..nest with eggs
            AddCodeRange(0x1FAB7, 0x1FABA, Extended_Pictographic);
            // E0.0   [5] (🪻..🪿)    <reserved-1FABB>..<reserved-1FABF>
            AddCodeRange(0x1FABB, 0x1FABF, Extended_Pictographic);
            // E13.0  [3] (🫀..🫂)    anatomical heart..people hugging
            AddCodeRange(0x1FAC0, 0x1FAC2, Extended_Pictographic);
            // E14.0  [3] (🫃..🫅)    pregnant man..person with crown
            AddCodeRange(0x1FAC3, 0x1FAC5, Extended_Pictographic);
            // E0.0  [10] (🫆..🫏)    <reserved-1FAC6>..<reserved-1FACF>
            AddCodeRange(0x1FAC6, 0x1FACF, Extended_Pictographic);
            // E13.0  [7] (🫐..🫖)    blueberries..teapot
            AddCodeRange(0x1FAD0, 0x1FAD6, Extended_Pictographic);
            // E14.0  [3] (🫗..🫙)    pouring liquid..jar
            AddCodeRange(0x1FAD7, 0x1FAD9, Extended_Pictographic);
            // E0.0   [6] (🫚..🫟)    <reserved-1FADA>..<reserved-1FADF>
            AddCodeRange(0x1FADA, 0x1FADF, Extended_Pictographic);
            // E14.0  [8] (🫠..🫧)    melting face..bubbles
            AddCodeRange(0x1FAE0, 0x1FAE7, Extended_Pictographic);
            // E0.0   [8] (🫨..🫯)    <reserved-1FAE8>..<reserved-1FAEF>
            AddCodeRange(0x1FAE8, 0x1FAEF, Extended_Pictographic);
            // E14.0  [7] (🫰..🫶)    hand with index finger and thumb crossed..heart hands
            AddCodeRange(0x1FAF0, 0x1FAF6, Extended_Pictographic);
            // E0.0   [9] (🫷..🫿)    <reserved-1FAF7>..<reserved-1FAFF>
            AddCodeRange(0x1FAF7, 0x1FAFF, Extended_Pictographic);
            // E0.0[1022] (🰀..🿽)    <reserved-1FC00>..<reserved-1FFFD>
            AddCodeRange(0x1FC00, 0x1FFFD, Extended_Pictographic);
            // Cn  [30] <reserved-E0002>..<reserved-E001F>
            AddCodeRange(0xE0002, 0xE001F, Control);
            // Cf  [96] TAG SPACE..CANCEL TAG
            AddCodeRange(0xE0020, 0xE007F, Extend);
            // Cn [128] <reserved-E0080>..<reserved-E00FF>
            AddCodeRange(0xE0080, 0xE00FF, Control);
            // Mn [240] VARIATION SELECTOR-17..VARIATION SELECTOR-256
            AddCodeRange(0xE0100, 0xE01EF, Extend);
            // Cn [3600] <reserved-E01F0>..<reserved-E0FFF>
            AddCodeRange(0xE01F0, 0xE0FFF, Control);
            // Unicode 15.0 新增表情
            AddCodeRange(0x01FA70, 0x01FA73, Extended_Pictographic); // 轻微运动表情
            AddCodeRange(0x01FA78, 0x01FA7A, Extended_Pictographic); // 躺着的姿势
            AddCodeRange(0x01FA80, 0x01FA82, Extended_Pictographic); // 头部摆动
            AddCodeRange(0x01FA90, 0x01FA95, Extended_Pictographic); // 工具和物品

            // Unicode 15.1 新增
            AddCodeRange(0x01FAE0, 0x01FAE7, Extended_Pictographic); // 面部表情（揉眼睛、提眉等）

            // Unicode 16.0 新增表情
            AddCodeRange(0x01FAC0, 0x01FAC5, Extended_Pictographic); // 人物姿势与家庭
            AddCodeRange(0x01FAD0, 0x01FAD9, Extended_Pictographic); // 新鲜食物和容器
            AddCodeRange(0x01FAF0, 0x01FAF8, Extended_Pictographic); // 手势姿势

            // Unicode 17.0 新增表情
            AddCodeRange(0x01FB00, 0x01FB22, Extended_Pictographic); // 人物动作、情绪
            AddCodeRange(0x01FBC0, 0x01FBC6, Extended_Pictographic); // 体育活动
            AddCodeRange(0x01FBD0, 0x01FBD3, Extended_Pictographic); // 辅助技术设备

            // 新增箭头符号 (重要: 修复 👩🏿‍🦼➡ 问题的关键)
            AddCodeRange(0x002B05, 0x002B07, Extended_Pictographic); // ←↑↓
            AddCodeRange(0x0027A1, 0x0027A1, Extended_Pictographic); // →
            AddCodeRange(0x01F800, 0x01F86D, Extended_Pictographic); // 各种方向箭头

            // 新增建筑与地点
            AddCodeRange(0x01F6D8, 0x01F6D9, Extended_Pictographic); // 拱门、树屋
            AddCodeRange(0x01F6DD, 0x01F6DF, Extended_Pictographic); // 火山、桥、帐篷

            // 补充符号
            AddCodeRange(0x01FAA8, 0x01FAA9, Extended_Pictographic); // 心脏监测、心碎
            AddCodeRange(0x01FAB0, 0x01FAB6, Extended_Pictographic); // 动物和植物
            AddCodeRange(0x01FAC7, 0x01FACD, Extended_Pictographic); // 头部与身体部位

            // 新增 Unicode 15.0-17.0 的 Extended_Pictographic
            AddCodeRange(0x01FAC3, 0x01FAC5, Extended_Pictographic); // 怀孕人物
            AddCodeRange(0x01F6D8, 0x01F6DD, Extended_Pictographic); // 新增建筑和场所
            AddCodeRange(0x01FA88, 0x01FA90, Extended_Pictographic); // 液体和毛发
            AddCodeRange(0x01F6DC, 0x01F6DF, Extended_Pictographic); // 安全和医疗相关
            AddCodeRange(0x027A1, 0x027A1, Extended_Pictographic); // BLACK RIGHTWARDS ARROW
            AddCodeRange(0x01F8AC, 0x01F8B1, Extended_Pictographic); // 新增方向箭头组合
            AddCodeRange(0x01F7E0, 0x01F7EB, Extended_Pictographic); // 形状符号
            AddCodeRange(0x1CC00, 0x1CC7F, Extended_Pictographic); // 新增符号区块

            // Unicode 17.0.0 的新修饰符
            AddCodeRange(0x01FBC4, 0x01FBC6, Extend); // 新增运动修饰符
            AddCodeRange(0x0E0001, 0x0E007F, Extend); // 标签字符序列
            AddCodeRange(0x01F780, 0x01F78F, Extend); // 新增方向修饰符

            // 扩展区域指示符范围
            AddCodeRange(0x01F3F4, 0x01F3F4, Regional_Indicator); // WAVING BLACK FLAG (used in flag sequences)
        }

        static void AddCodeRange(int Start, int End, int Type) => m_lst_code_range.Add(new RangeInfo { Start = Start, End = End, Type = Type });

        protected struct RangeInfo
        {
            public int Start;
            public int End;
            public int Type;
        }
    }
}