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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    [DefaultProperty("Text")]
    public class ColorPanelLeft : IControl
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

        readonly static StringFormat stringFormatCenter = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

        protected override void OnDraw(DrawEventArgs e)
        {
            base.OnDraw(e);
            var g = e.Canvas;
            var rect = ClientRectangle.PaddingRect(Padding);
            using (var brush = new SolidBrush(ForeColor))
            {
                if (desc == null) g.String(text, Font, brush, rect, stringFormatCenter);
                else
                {
                    var size = g.MeasureString(text, Font);
                    using (var font = new Font(Font.FontFamily, Font.Size * 0.7F))
                    {
                        var sizedesc = g.MeasureString(desc, font);
                        int y = (rect.Height - (size.Height + sizedesc.Height)) / 2;
                        g.String(text, Font, brush, new Rectangle(rect.X, rect.Y + y, rect.Width, size.Height), stringFormatCenter);
                        using (var brush_desc = new SolidBrush(Color.FromArgb(200, ForeColor)))
                        {
                            g.String(desc, font, brush_desc, new Rectangle(rect.X, rect.Y + y + (int)(size.Height * 1.2F), rect.Width, sizedesc.Height), stringFormatCenter);
                        }
                    }
                }
            }
            this.PaintBadge(g);
        }
    }
}