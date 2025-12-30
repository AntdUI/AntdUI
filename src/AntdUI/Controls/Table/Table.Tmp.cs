// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

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
            public DragHeader(int ox, int oy, int _i, int _x)
            {
                _ox = ox;
                _oy = oy;
                i = _i;
                x = _x;
            }
            public int _ox { get; set; }
            public int _oy { get; set; }
            public void SetEnable(int x, int y)
            {
                if (x == _ox && y == _oy) return;
                enable = true;
            }

            public bool enable { get; set; }
            public int x { get; set; }
            public int xr { get; set; }
            public int i { get; set; }
            public int im { get; set; } = -1;
            public bool last { get; set; }
            public bool hand { get; set; }
        }
    }
}