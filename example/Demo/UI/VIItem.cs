// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    internal class VIItem : VirtualItem
    {
        public string Key, Value;
        public VIItem(string key, string value) { Tag = Key = key; Value = value; }

        internal Bitmap bmp = null, bmp_ac = null;
        public override void Paint(Canvas g, VirtualPanelArgs e)
        {
            if (Hover)
            {
                using (var path = e.Rect.RoundPath(e.Radius))
                {
                    g.Fill(Style.Db.Primary, path);
                }
                if (bmp_ac == null) bmp_ac = Value.SvgToBmp(icon_size, icon_size, Style.Db.PrimaryColor);
                g.Image(bmp_ac, rect_icon);
                g.String(Key, e.Panel.Font, Style.Db.PrimaryColor, rect_text, FormatFlags.Default);
            }
            else
            {
                if (bmp == null) bmp = Value.SvgToBmp(icon_size, icon_size, Style.Db.Text);
                g.Image(bmp, rect_icon);
                g.String(Key, e.Panel.Font, Style.Db.Text, rect_text, FormatFlags.Default);

            }
        }

        int icon_size = 36;
        Rectangle rect_icon, rect_text;
        public override Size Size(Canvas g, VirtualPanelArgs e)
        {
            var dpi = g.Dpi;
            int w = (int)(200 * dpi), h = (int)(100 * dpi);
            icon_size = (int)(36 * dpi);
            int text_size = (int)(24 * dpi), y = (h - (icon_size + text_size)) / 2;
            rect_icon = new Rectangle((w - icon_size) / 2, y, icon_size, icon_size);
            rect_text = new Rectangle(0, y + icon_size / 2 + text_size, w, text_size);
            return new Size(w, h);
        }

        public override void Dispose(VirtualPanel sender, bool disposed)
        {
            if (disposed)
            {
                bmp?.Dispose();
                bmp_ac?.Dispose();
                bmp = bmp_ac = null;
            }
            else
            {
                bmp?.Dispose();
                bmp = null;
            }
            base.Dispose(sender, disposed);
        }
    }
}