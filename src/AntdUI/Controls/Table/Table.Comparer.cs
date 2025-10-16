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

namespace AntdUI
{
    ///<summary>
    ///主要用于文件名的比较。
    ///</summary>
    internal class FilesNameComparerClass
    {
        public static int Compare(string x, string y)
        {
            if (int.TryParse(x, out var int_x) && int.TryParse(y, out var int_y))
            {
                if (int_x == int_y) return 0;
                else if (int_x > int_y) return 1;
                else return -1;
            }
            else if (double.TryParse(x, out var dou_x) && double.TryParse(y, out var dou_y))
            {
                if (dou_x == dou_y) return 0;
                else if (dou_x > dou_y) return 1;
                else return -1;
            }
            string fileA = x, fileB = y;
            char[] arr1 = fileA.ToCharArray();
            char[] arr2 = fileB.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += arr1[i];
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += arr2[j];
                        j++;
                    }
                    if (int.TryParse(s1, out var _s1) && int.TryParse(s2, out var _s2))
                    {
                        if (_s1 > _s2) return 1;
                        else if (_s1 < _s2) return -1;
                    }
                    else if (double.TryParse(s1, out var _sd1) && double.TryParse(s2, out var _sd2))
                    {
                        if (_sd1 > _sd2) return 1;
                        else if (_sd1 < _sd2) return -1;
                    }
                }
                else
                {
                    if (arr1[i] > arr2[j]) return 1;
                    if (arr1[i] < arr2[j]) return -1;
                    i++;
                    j++;
                }
            }
            if (arr1.Length == arr2.Length) return 0;
            else return arr1.Length > arr2.Length ? 1 : -1;
        }
    }

    internal class SortModel
    {
        public SortModel(int _i, string? _v) { i = _i; v = _v ?? ""; }
        public int i { get; set; }
        public string v { get; set; }
    }
}