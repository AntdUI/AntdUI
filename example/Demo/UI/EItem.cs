// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    internal class EItem : VirtualItem
    {
        public string Key, Value;
        public string[] Keywords;
        public string[] Skins = null;
        public EItem(string key, string value, string[] keywords)
        {
            Tag = Key = key;
            Keywords = keywords;
            Value = value;
        }

        public void SetSkin(int i)
        {
            if (Skins == null) return;
            bmp?.Dispose();
            bmp = null;
            Key = Skins[i];
            Value = SvgDb.Emoji[Key];
        }

        Bitmap bmp = null;
        public override void Paint(Canvas g, VirtualPanelArgs e)
        {
            if (bmp == null) bmp = Value.SvgToBmp(rect_icon_hover.Width, rect_icon_hover.Height, Style.Db.Text);
            if (Hover)
            {
                using (var path = e.Rect.RoundPath(e.Radius))
                {
                    g.Draw(Style.Db.Primary, sp, path);
                }
                g.Image(bmp, rect_icon_hover);
            }
            else g.Image(bmp, rect_icon);
        }

        int sp = 4;
        Rectangle rect_icon, rect_icon_hover;
        public override Size Size(Canvas g, VirtualPanelArgs e)
        {
            var dpi = g.Dpi;
            sp = (int)(4 * dpi);
            int size = (int)(100 * dpi);
            int icon_size = (int)(48 * dpi), xy = (size - icon_size) / 2;
            int icon_size_hover = (int)(78 * dpi), xy_hover = (size - icon_size_hover) / 2;
            rect_icon = new Rectangle(xy, xy, icon_size, icon_size);
            rect_icon_hover = new Rectangle(xy_hover, xy_hover, icon_size_hover, icon_size_hover);
            return new Size(size, size);
        }


        public override void Dispose(VirtualPanel sender, bool disposed)
        {
            bmp?.Dispose();
            bmp = null;
            base.Dispose(sender, disposed);
        }
    }
}