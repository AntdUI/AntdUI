// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System.Drawing;

namespace AntdUI
{
    partial class CellLink
    {
        public override void PaintBack(Canvas g) { }

        public override void Paint(Canvas g, Font font, bool enable, SolidBrush fore) => Table.PaintLink(g, font, Rect, this, enable, PARENT.PARENT.ColorScheme);

        public override Size GetSize(Canvas g, Font font, TableGaps gap)
        {
            var size = g.MeasureText(Text ?? Config.NullText, font);
            return new Size(size.Width, size.Height);
        }

        public override void SetRect(Canvas g, Font font, Rectangle rect, Size size, int maxwidth, TableGaps gap)
        {
            Rect = new Rectangle(rect.X, rect.Y + (rect.Height - size.Height) / 2, rect.Width, size.Height);
        }

        #region 动画

        internal AnimationTask? ThreadHover, ThreadImageHover;

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

        internal AnimationTask? ThreadClick;
        internal bool AnimationClick = false;
        internal float AnimationClickValue = 0;
        internal virtual void Click()
        { }

        #endregion

        #endregion
    }
}