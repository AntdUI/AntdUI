// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;

namespace AntdUI
{
    partial class Markdown
    {
        #region 选择属性

        bool selectable = true;
        /// <summary>
        /// 是否允许鼠标选中内容并复制
        /// </summary>
        [Description("是否允许鼠标选中内容并复制"), Category(nameof(CategoryAttribute.Behavior)), DefaultValue(true)]
        public bool Selectable
        {
            get => selectable;
            set
            {
                if (selectable == value) return;
                selectable = value;
                if (!selectable) ClearSelection();
                OnPropertyChanged(nameof(Selectable));
            }
        }

        Color? selectionColor;
        /// <summary>
        /// 选中区域背景颜色
        /// </summary>
        [Description("选中区域背景颜色"), Category("颜色"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? SelectionColor
        {
            get => selectionColor;
            set
            {
                if (selectionColor == value) return;
                selectionColor = value;
                Invalidate();
                OnPropertyChanged(nameof(SelectionColor));
            }
        }

        /// <summary>
        /// 是否存在选区
        /// </summary>
        [Browsable(false)]
        public bool HasSelection => selActive && SelectionRange(out _, out _, out _, out _, out _, out _);

        /// <summary>
        /// 选中的文本（保留 Markdown 格式）
        /// </summary>
        [Browsable(false)]
        public string SelectedText => GetSelectedText(true);

        #endregion

        #region 选区状态

        bool selActive, selDragging;
        int selALine, selAX, selBLine, selBX;

        /// <summary>
        /// 清除选区
        /// </summary>
        public void ClearSelection()
        {
            if (!selActive && !selDragging) return;
            selActive = false;
            selDragging = false;
            Invalidate();
        }

        /// <summary>
        /// 全选
        /// </summary>
        public void SelectAll()
        {
            if (lines.Count == 0) return;
            selALine = 0;
            selAX = LineLeft(lines[0]);
            selBLine = lines.Count - 1;
            selBX = LineRight(lines[lines.Count - 1]);
            selActive = true;
            selDragging = false;
            Invalidate();
        }

        /// <summary>
        /// 复制选中内容到剪贴板
        /// </summary>
        /// <param name="keepFormat">是否保留 Markdown 格式</param>
        public bool Copy(bool keepFormat = true) => this.ClipboardSetText(GetSelectedText(keepFormat));

        /// <summary>
        /// 获取选中文本
        /// </summary>
        /// <param name="keepFormat">是否保留 Markdown 格式</param>
        public string GetSelectedText(bool keepFormat = true)
        {
            if (!HasSelection) return "";
            string result = "";
            this.GDI(g => result = BuildSelectedText(g, keepFormat));
            return result;
        }

        #endregion

        #region 命中与选区计算

        /// <summary>
        /// 将点击坐标转换为（行索引，行内 X）
        /// </summary>
        bool HitPoint(int mx, int my, out int lineIndex, out int x)
        {
            lineIndex = -1;
            x = 0;
            if (lines.Count == 0) return false;
            int cy = my + ScrollBar.Value;
            int index = FirstVisibleLine(ScrollBar.Value);
            if (index >= lines.Count) index = lines.Count - 1;
            int i = index;
            while (i > 0 && lines[i].Y > cy) i--;
            while (i < lines.Count - 1 && lines[i].Bottom <= cy) i++;
            var line = lines[i];
            lineIndex = i;
            if (cy < line.Y) x = LineLeft(line);
            else if (cy > line.Bottom) x = LineRight(line);
            else x = mx;
            return true;
        }

        static int LineLeft(MDVisualLine line)
        {
            int min = int.MaxValue;
            foreach (var frag in line.Fragments) if (frag.X < min) min = frag.X;
            return min == int.MaxValue ? 0 : min;
        }

        static int LineRight(MDVisualLine line)
        {
            int max = int.MinValue;
            foreach (var frag in line.Fragments) if (frag.X + frag.Width > max) max = frag.X + frag.Width;
            return max == int.MinValue ? 0 : max;
        }

        bool SelectionRange(out int a, out int ax, out int b, out int bx, out int first, out int last)
        {
            a = selALine; ax = selAX; b = selBLine; bx = selBX;
            first = -1; last = -1;
            if (!selActive) return false;
            if (a > b || (a == b && ax > bx))
            {
                int t = a; a = b; b = t;
                int tx = ax; ax = bx; bx = tx;
            }
            if (a < 0) a = 0;
            if (b >= lines.Count) b = lines.Count - 1;
            if (a >= lines.Count || b < 0) return false;
            first = a; last = b;
            return true;
        }

        #endregion

        #region 鼠标

        internal void SelectMouseDown(int mx, int my)
        {
            if (!selectable) return;
            int line, x;
            if (!HitPoint(mx, my, out line, out x)) return;
            selALine = selBLine = line;
            selAX = selBX = x;
            selActive = false;
            selDragging = true;
        }

        internal void SelectMouseMove(int mx, int my)
        {
            if (!selectable || !selDragging) return;
            int line, x;
            if (!HitPoint(mx, my, out line, out x)) return;
            if (line == selBLine && x == selBX) return;
            selBLine = line;
            selBX = x;
            selActive = true;
            Invalidate();
        }

        internal void SelectMouseUp()
        {
            if (!selDragging) return;
            selDragging = false;
            if (selALine == selBLine && selAX == selBX) selActive = false;
            Invalidate();
        }

        /// <summary>
        /// 双击选中词组
        /// </summary>
        internal void SelectWord(int mx, int my)
        {
            if (!selectable) return;
            int line, x;
            if (!HitPoint(mx, my, out line, out x)) return;
            var ln = lines[line];
            foreach (var frag in ln.Fragments)
            {
                if (frag.Kind == MDInlineKind.Image) continue;
                if (x < frag.X || x > frag.X + frag.Width) continue;
                int rel = CharIndexAt(frag, x - frag.X);
                int s = rel, e = rel;
                while (s > 0 && IsWordPart(frag.Text[s - 1])) s--;
                while (e < frag.Text.Length && IsWordPart(frag.Text[e])) e++;
                if (e <= s)
                {
                    s = rel;
                    e = Math.Min(frag.Text.Length, rel + 1);
                }
                selALine = selBLine = line;
                selAX = frag.X + TextWidth(frag, s);
                selBX = frag.X + TextWidth(frag, e);
                selActive = true;
                selDragging = false;
                Invalidate();
                return;
            }
        }

        static bool IsWordPart(char c) => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.';

        int TextWidth(MDFragment frag, int count)
        {
            if (count <= 0) return 0;
            if (count >= frag.Text.Length) return frag.Width;
            string value = frag.Text.Substring(0, count);
            int w = 0;
            this.GDI(g => w = Measure(g, value, frag.Font));
            return w;
        }

        /// <summary>
        /// 根据 X 偏移估算字符索引
        /// </summary>
        int CharIndexAt(MDFragment frag, int offset)
        {
            if (frag.Width <= 0 || frag.Text.Length == 0) return 0;
            double rate = offset / (double)frag.Width;
            int index = (int)Math.Round(frag.Text.Length * rate);
            if (index < 0) index = 0;
            if (index > frag.Text.Length) index = frag.Text.Length;
            return index;
        }

        #endregion

        #region 绘制选区

        void DrawSelection(Canvas g, int sy, int viewH)
        {
            int a, ax, b, bx, first, last;
            if (!SelectionRange(out a, out ax, out b, out bx, out first, out last)) return;
            int bottom = sy + viewH;
            using (var brush = new SolidBrush(ColorSelection))
            {
                for (int i = first; i <= last; i++)
                {
                    var line = lines[i];
                    if (line.Y > bottom) break;
                    if (line.Bottom < sy) continue;
                    int x0 = LineLeft(line), x1 = LineRight(line);
                    if (i == a) x0 = Math.Max(x0, ax);
                    if (i == b) x1 = Math.Min(x1, bx);
                    if (x1 <= x0)
                    {
                        if (i == a && i == b) continue;
                        x1 = x0 + 1;
                    }
                    g.Fill(brush, new Rectangle(x0, line.Y, x1 - x0, line.Height));
                }
            }
        }

        #endregion

        #region 复制

        string BuildSelectedText(Canvas g, bool keepFormat)
        {
            int a, ax, b, bx, first, last;
            if (!SelectionRange(out a, out ax, out b, out bx, out first, out last)) return "";
            var sb = new StringBuilder();
            for (int i = first; i <= last; i++)
            {
                var line = lines[i];
                int x0 = LineLeft(line), x1 = LineRight(line);
                if (i == a) x0 = Math.Max(x0, ax);
                if (i == b) x1 = Math.Min(x1, bx);
                if (x1 <= x0 && !(i == a && i == b)) continue;

                if (line.Raw != null)
                {
                    // 代码块：整行复制原始代码
                    sb.Append(ClipText(g, line, x0, x1, line.Raw, null, false));
                }
                else
                {
                    bool empty = true;
                    int prevCell = -1;
                    foreach (var frag in line.Fragments)
                    {
                        if (prevCell >= 0 && frag.Cell >= 0 && frag.Cell != prevCell) sb.Append('\t');
                        prevCell = frag.Cell;
                        if (frag.Kind == MDInlineKind.Image)
                        {
                            if (frag.X + frag.Width <= x0 || frag.X >= x1) continue;
                            if (frag.X >= x0 && frag.X + frag.Width <= x1)
                            {
                                sb.Append(keepFormat && frag.Md != null ? frag.Md : (frag.Src ?? frag.Text));
                                empty = false;
                            }
                            continue;
                        }
                        if (frag.X + frag.Width <= x0 || frag.X >= x1) continue;
                        if (frag.X >= x0 && frag.X + frag.Width <= x1)
                        {
                            sb.Append(keepFormat && frag.Md != null ? frag.Md : frag.Text);
                        }
                        else
                        {
                            int s = CharIndexAt(frag, Math.Max(0, x0 - frag.X));
                            int e = CharIndexAt(frag, Math.Min(frag.Width, x1 - frag.X));
                            if (e <= s) { int t = s; s = e; e = t; }
                            if (e > frag.Text.Length) e = frag.Text.Length;
                            sb.Append(frag.Text.Substring(s, e - s));
                        }
                        empty = false;
                    }
                    if (empty) continue;
                }
                if (i < last) sb.Append('\n');
            }
            return sb.ToString();
        }

        string ClipText(Canvas g, MDVisualLine line, int x0, int x1, string text, Font? font, bool keepFormat)
        {
            int left = LineLeft(line), right = LineRight(line);
            if (x0 <= left && x1 >= right) return text;
            if (text.Length == 0) return "";
            int total = right - left;
            if (total <= 0) return text;
            int s = (int)Math.Round(text.Length * (x0 - left) / (double)total);
            int e = (int)Math.Round(text.Length * (x1 - left) / (double)total);
            if (s < 0) s = 0;
            if (e > text.Length) e = text.Length;
            if (e <= s) e = Math.Min(text.Length, s + 1);
            return text.Substring(s, e - s);
        }

        #endregion
    }
}