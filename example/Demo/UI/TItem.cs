// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    internal class TItem : VirtualItem
    {
        string Title, Count, originalCount;
        string LocalizationTitle;
        public VirtualItem[] data;
        public TItem(string t, string localization, VirtualItem[] d)
        {
            CanClick = false;
            data = d;
            Title = t;
            LocalizationTitle = localization;
            originalCount = Count = d.Length.ToString();
        }

        public void SetCount(int value)
        {
            Count = value.ToString();
            Visible = value > 0;
        }
        public void Restore() => Count = originalCount;

        public override void Paint(Canvas g, VirtualPanelArgs e)
        {
            g.String(Localization.Get(LocalizationTitle, Title), font_title, Style.Db.Text, rect, FormatFlags.Left | FormatFlags.VerticalCenter | FormatFlags.NoWrap);
            using (var path = rect_count.RoundPath(e.Radius))
            {
                g.Fill(Style.Db.TagDefaultBg, path);
                g.Draw(Style.Db.DefaultBorder, sp, path);
            }
            g.String(Count, font_count, Style.Db.Text, rect_count, FormatFlags.Default);
        }

        internal int sp = 1, rl = 0;
        Rectangle rect, rect_count;
        Font font_title, font_count;
        public override Size Size(Canvas g, VirtualPanelArgs e)
        {
            var dpi = g.Dpi;
            sp = (int)(1 * dpi);
            int gap = (int)(8 * dpi), x = (int)(30 * dpi), h = (int)(44 * dpi);
            rect = new Rectangle(x, 0, e.Rect.Width, h);
            font_title?.Dispose();
            font_count?.Dispose();
            font_title = new Font(e.Panel.Font, FontStyle.Bold);
            font_count = new Font(e.Panel.Font.FontFamily, e.Panel.Font.Size * .74F, e.Panel.Font.Style);
            Size size_title = g.MeasureString(Localization.Get(LocalizationTitle, Title), font_title), size_count = g.MeasureString(Count, font_count);
            int rx = x + size_title.Width + gap, ry = (h - size_title.Height) / 2;
            if (size_count.Width > size_count.Height)
            {
                float r = size_title.Height * 1F / size_count.Height;
                rect_count = new Rectangle(rx, ry, (int)(size_count.Width * r), size_title.Height);
            }
            else rect_count = new Rectangle(rx, ry, size_title.Height, size_title.Height);
            rl = rect_count.Right;
            return rect.Size;
        }

        public override void Dispose(VirtualPanel sender, bool disposed)
        {
            font_title?.Dispose();
            font_count?.Dispose();
            base.Dispose(sender, disposed);
        }
    }

    internal class TItem_Skin : TItem
    {
        public TItem_Skin(string t, string localization, VirtualItem[] d) : base(t, localization, d)
        {
            CanClick = true;
        }

        public override void Paint(Canvas g, VirtualPanelArgs e)
        {
            base.Paint(g, e);
            Paint(g, "#ffc83d".ToColor(), hove_default, sel_default, rect_default);
            Paint(g, "#f7d7c4".ToColor(), hove_light, sel_light, rect_light);
            Paint(g, "#d8b094".ToColor(), hove_medium, sel_medium, rect_medium);
            Paint(g, "#bb9167".ToColor(), hove_medium_light, sel_medium_light, rect_medium_light);
            Paint(g, "#8e562e".ToColor(), hove_medium_dark, sel_medium_dark, rect_medium_dark);
            Paint(g, "#613d30".ToColor(), hove_dark, sel_dark, rect_dark);
        }

        void Paint(Canvas g, Color color, bool hove, bool sel, Rectangle rect)
        {
            if (hove) g.FillEllipse(color, GetBig(rect, (int)(20 * g.Dpi)));
            else g.FillEllipse(color, rect);
            if (sel) g.DrawEllipse(color, 4 * g.Dpi, GetBig(rect, (int)(26 * g.Dpi)));
        }

        Rectangle rect_default, rect_light, rect_medium, rect_medium_light, rect_medium_dark, rect_dark;
        public override Size Size(Canvas g, VirtualPanelArgs e)
        {
            var size = base.Size(g, e);
            int gap = (int)(12 * g.Dpi), wh = (int)(18 * g.Dpi);
            int rx = rl + gap, ry = (size.Height - wh) / 2;
            rect_default = new Rectangle(rx, ry, wh, wh);
            rx += gap + wh;
            rect_light = new Rectangle(rx, ry, wh, wh);
            rx += gap + wh;
            rect_medium = new Rectangle(rx, ry, wh, wh);
            rx += gap + wh;
            rect_medium_light = new Rectangle(rx, ry, wh, wh);
            rx += gap + wh;
            rect_medium_dark = new Rectangle(rx, ry, wh, wh);
            rx += gap + wh;
            rect_dark = new Rectangle(rx, ry, wh, wh);
            return size;
        }

        static RectangleF GetBig(Rectangle rect, int wh)
        {
            float xy = (rect.Width - wh) / 2F;
            return new RectangleF(rect.X + xy, rect.Y + xy, wh, wh);
        }

        #region 鼠标

        bool hove_default, hove_light, hove_medium, hove_medium_light, hove_medium_dark, hove_dark;
        bool sel_default = true, sel_light, sel_medium, sel_medium_light, sel_medium_dark, sel_dark;
        public override bool MouseMove(VirtualPanel sender, VirtualPanelMouseArgs e)
        {
            bool old_default = hove_default, old_light = hove_light, old_medium = hove_medium, old_medium_light = hove_medium_light, old_medium_dark = hove_medium_dark, old_dark = hove_dark;
            hove_default = rect_default.Contains(e.X, e.Y);
            hove_light = rect_light.Contains(e.X, e.Y);
            hove_medium = rect_medium.Contains(e.X, e.Y);
            hove_medium_light = rect_medium_light.Contains(e.X, e.Y);
            hove_medium_dark = rect_medium_dark.Contains(e.X, e.Y);
            hove_dark = rect_dark.Contains(e.X, e.Y);

            bool r = hove_default || hove_light || hove_medium || hove_medium_light || hove_medium_dark || hove_dark;
            if (old_default == hove_default && old_light == hove_light && old_medium == hove_medium && old_medium_light == hove_medium_light && old_medium_dark == hove_medium_dark && old_dark == hove_dark) return r;
            Invalidate();
            return r;
        }
        public override void MouseClick(VirtualPanel sender, VirtualPanelMouseArgs e)
        {
            hove_default = rect_default.Contains(e.X, e.Y);
            hove_light = rect_light.Contains(e.X, e.Y);
            hove_medium = rect_medium.Contains(e.X, e.Y);
            hove_medium_light = rect_medium_light.Contains(e.X, e.Y);
            hove_medium_dark = rect_medium_dark.Contains(e.X, e.Y);
            hove_dark = rect_dark.Contains(e.X, e.Y);
            if (hove_default || hove_light || hove_medium || hove_medium_light || hove_medium_dark || hove_dark)
            {
                int code = 0;
                if (hove_default)
                {
                    code = 0;
                    if (sel_default) return;
                    sel_default = sel_light = sel_medium = sel_medium_light = sel_medium_dark = sel_dark = false;
                    sel_default = true;
                }
                else if (hove_light)
                {
                    code = 1;
                    if (sel_light) return;
                    sel_default = sel_light = sel_medium = sel_medium_light = sel_medium_dark = sel_dark = false;
                    sel_light = true;
                }
                else if (hove_medium)
                {
                    code = 2;
                    if (sel_medium) return;
                    sel_default = sel_light = sel_medium = sel_medium_light = sel_medium_dark = sel_dark = false;
                    sel_medium = true;
                }
                else if (hove_medium_light)
                {
                    code = 3;
                    if (sel_medium_light) return;
                    sel_default = sel_light = sel_medium = sel_medium_light = sel_medium_dark = sel_dark = false;
                    sel_medium_light = true;
                }
                else if (hove_medium_dark)
                {
                    code = 4;
                    if (sel_medium_dark) return;
                    sel_default = sel_light = sel_medium = sel_medium_light = sel_medium_dark = sel_dark = false;
                    sel_medium_dark = true;
                }
                else if (hove_dark)
                {
                    code = 5;
                    if (sel_dark) return;
                    sel_default = sel_light = sel_medium = sel_medium_light = sel_medium_dark = sel_dark = false;
                    sel_dark = true;
                }
                sender.PauseLayout = true;
                foreach (var it in data)
                {
                    if (it is EItem eItem) eItem.SetSkin(code);
                }
                sender.PauseLayout = false;
            }
        }

        #endregion
    }
}