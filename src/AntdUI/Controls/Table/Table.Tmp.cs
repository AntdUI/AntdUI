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

            /// <summary>
            /// Minimum width configured by the column.
            /// </summary>
            public int? MinWidth { get; set; }

            /// <summary>
            /// Maximum width configured by the column.
            /// </summary>
            public int? MaxWidth { get; set; }
        }

        internal class MoveHeader
        {
            public MoveHeader(Dictionary<int, MoveHeader> dir, Rectangle r, int index, int w, int min, AutoWidth? autoWidth)
            {
                rect = r;
                i = index;
                min_width = min;
                if (autoWidth != null)
                {
                    max_width = autoWidth.MaxWidth;
                    if (autoWidth.MinWidth.HasValue && min_width < autoWidth.MinWidth.Value) min_width = autoWidth.MinWidth.Value;
                    if (max_width.HasValue && max_width.Value == min_width) enable = false;
                }
                if (dir.TryGetValue(index, out var find) && find.MouseDown)
                {
                    x = find.x;
                    MouseDown = find.MouseDown;
                    width = find.width;
                }
                else width = w;
            }

            public Rectangle rect { get; set; }

            public bool enable { get; set; } = true;

            public bool MouseDown { get; set; }
            public int x { get; set; }
            public int i { get; set; }
            public int width { get; set; }
            public int min_width { get; set; }
            public int? max_width { get; set; }

            public int GetWidth(int old, int x)
            {
                int value = width + x - old;
                if (value < min_width) return min_width;
                if (max_width.HasValue && value > max_width.Value) return max_width.Value;
                return value;
            }
        }
        internal class DividerList
        {
            public DividerList(int A, int B, int C)
            {
                a = A;
                b = B;
                c = C;
            }

            public int a { get; set; }
            public int b { get; set; }
            public int c { get; set; }
            public bool h { get; set; } = true;
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