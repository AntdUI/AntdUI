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
// GITEE: https://gitee.com/AntdUI/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Collections.Generic;
using System.Drawing;

namespace AntdUI
{
    partial class Table
    {
        internal class AutoWidth
        {
            /// <summary>
            /// 自动值
            /// </summary>
            public int value { get; set; }

            /// <summary>
            /// 最小值
            /// </summary>
            public int minvalue { get; set; }
        }

        internal class MoveHeader
        {
            public MoveHeader(Dictionary<int, MoveHeader> dir, Rectangle r, int index, int w, int min)
            {
                rect = r;
                i = index;
                min_width = min;
                if (dir.TryGetValue(index, out var find) && find.MouseDown)
                {
                    x = find.x;
                    MouseDown = find.MouseDown;
                    width = find.width;
                }
                else width = w;
            }

            public Rectangle rect { get; set; }

            public bool MouseDown { get; set; }
            public int x { get; set; }
            public int i { get; set; }
            public int width { get; set; }
            public int min_width { get; set; }
        }

        internal class DragHeader
        {
            public int x { get; set; }
            public int xr { get; set; }
            public int i { get; set; }
            public int im { get; set; } = -1;
            public bool last { get; set; }
            public bool hand { get; set; }
        }
    }
}