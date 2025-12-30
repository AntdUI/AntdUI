// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.ComponentModel;
using System.Drawing;

namespace AntdUI
{
    [DefaultProperty("Text")]
    public class ColorPanel : IControl
    {
        #region 属性

        string text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
        public new string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    Invalidate();
                }
            }
        }

        string desc = null;
        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述"), Category("外观"), DefaultValue(null)]
        public string TextDesc
        {
            get => desc;
            set
            {
                if (desc != value)
                {
                    desc = value;
                    Invalidate();
                }
            }
        }

        #endregion

        readonly static FormatFlags s_f = FormatFlags.Center | FormatFlags.EllipsisCharacter;

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);
            var g = e.Canvas;
            var rect = ClientRectangle;
            using (var brush = new SolidBrush(ForeColor))
            {
                if (desc == null) g.String(text, Font, brush, rect, s_f);
                else
                {
                    var size = g.MeasureString(text, Font);
                    using (var font = new Font(Font.FontFamily, Font.Size * 0.7F))
                    {
                        var sizedesc = g.MeasureString(desc, font);
                        int y = (rect.Height - (size.Height + sizedesc.Height)) / 2;
                        g.String(text, Font, brush, new Rectangle(rect.X, rect.Y + y, rect.Width, size.Height), s_f);
                        using (var brush_desc = new SolidBrush(Color.FromArgb(200, ForeColor)))
                        {
                            g.String(desc, font, brush_desc, new Rectangle(rect.X, rect.Y + y + (int)(size.Height * 1.3F), rect.Width, sizedesc.Height), s_f);
                        }
                    }
                }
            }
            this.PaintBadge(g);
        }
    }
}