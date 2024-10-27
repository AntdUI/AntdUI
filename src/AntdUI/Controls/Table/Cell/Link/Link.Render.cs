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
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Drawing;

namespace AntdUI
{
    partial class CellLink
    {
        internal override void PaintBack(Graphics g) { }

        internal override void Paint(Graphics g, Font font, SolidBrush fore)
        {
            Table.PaintLink(g, font, Rect, this);
        }

        internal override Size GetSize(Graphics g, Font font, int gap, int gap2)
        {
            var size = g.MeasureString(Text ?? Config.NullText, font).Size();
            return new Size(size.Width + gap2 * 2, size.Height + gap);
        }

        internal Rectangle Rect;
        internal override void SetRect(Graphics g, Font font, Rectangle rect, Size size, int gap, int gap2)
        {
            Rect = new Rectangle(rect.X + gap, rect.Y + (rect.Height - size.Height) / 2, rect.Width - gap2, size.Height);
        }

        #region 动画

        internal ITask? ThreadHover = null;
        internal ITask? ThreadImageHover = null;

        internal bool _mouseDown = false;
        internal bool ExtraMouseDown
        {
            get => _mouseDown;
            set
            {
                if (_mouseDown == value) return;
                _mouseDown = value;
                OnPropertyChanged();
            }
        }

        internal int AnimationHoverValue = 0;
        internal bool AnimationHover = false;
        internal bool _mouseHover = false;
        internal virtual bool ExtraMouseHover
        {
            get => _mouseHover;
            set
            {
                if (_mouseHover == value) return;
                _mouseHover = value;
                if (Enabled) OnPropertyChanged();
            }
        }

        internal bool AnimationImageHover = false;
        internal float AnimationImageHoverValue = 0F;

        #region 点击动画

        internal ITask? ThreadClick = null;
        internal bool AnimationClick = false;
        internal float AnimationClickValue = 0;
        internal virtual void Click()
        { }

        #endregion

        #endregion
    }
}