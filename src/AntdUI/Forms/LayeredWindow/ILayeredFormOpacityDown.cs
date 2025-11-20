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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AntdUI
{
    public abstract class ILayeredFormOpacityDown : ILayeredForm
    {
        public PushAnimateConfig animateConfig;
        public ILayeredFormOpacityDown()
        {
            animateConfig = new PushAnimateConfig(this, () =>
            {
                RunAnimation = false;
                LoadOK();
            }, () => RunAnimation = true);
        }

        public override bool MessageEnable => true;

        public bool RunAnimation = true;
        protected override void OnLoad(EventArgs e)
        {
            animateConfig.Start(name);
            base.OnLoad(e);
        }

        public abstract string name { get; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool CanLoadMessage { get; set; }
        public virtual void LoadOK()
        {
            CanLoadMessage = true;
            LoadMessage();
        }

        internal void DisposeTmp() => animateConfig.DisposeBmp();
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (animateConfig.End(name, e.CloseReason)) e.Cancel = true;
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            animateConfig.Dispose();
            base.Dispose(disposing);
        }

        public void CLocation(Point Point, TAlignFrom Placement, bool DropDownArrow, int Padding, int Width, int Height, Rectangle Rect, ref TAlign ArrowAlign, bool Collision = false)
        {
            switch (Placement)
            {
                case TAlignFrom.Top:
                    animateConfig.Inverted = true;
                    if (DropDownArrow) ArrowAlign = TAlign.Top;
                    SetLocation((Point.X + Rect.X) + (Rect.Width - Width) / 2, Point.Y - Height + Rect.Y);
                    break;
                case TAlignFrom.TL:
                    animateConfig.Inverted = true;
                    if (DropDownArrow) ArrowAlign = TAlign.TL;
                    int xTL = Point.X + Rect.X - Padding, yTL = Point.Y - Height + Rect.Y;
                    SetLocation(xTL, yTL);
                    if (Collision)
                    {
                        var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                        if (xTL > (screen.X + screen.Width) - TargetRect.Width)
                        {
                            if (DropDownArrow) ArrowAlign = TAlign.TR;
                            xTL = Point.X + (Rect.X + Rect.Width) - Width + Padding;
                            SetLocation(xTL, yTL);
                        }
                    }
                    break;
                case TAlignFrom.TR:
                    animateConfig.Inverted = true;
                    if (DropDownArrow) ArrowAlign = TAlign.TR;
                    int xTR = Point.X + (Rect.X + Rect.Width) - Width + Padding, yTR = Point.Y - Height + Rect.Y;
                    SetLocation(xTR, yTR);
                    if (Collision)
                    {
                        var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                        if (xTR < 0)
                        {
                            if (DropDownArrow) ArrowAlign = TAlign.TL;
                            xTR = Point.X + Rect.X - Padding;
                            SetLocation(xTR, yTR);
                        }
                    }
                    break;
                case TAlignFrom.Bottom:
                    if (DropDownArrow) ArrowAlign = TAlign.Bottom;
                    SetLocation((Point.X + Rect.X) + (Rect.Width - Width) / 2, Point.Y + Rect.Bottom);
                    break;
                case TAlignFrom.BR:
                    if (DropDownArrow) ArrowAlign = TAlign.BR;
                    int xBR = Point.X + (Rect.X + Rect.Width) - Width + Padding, yBR = Point.Y + Rect.Bottom;
                    SetLocation(xBR, yBR);
                    if (Collision)
                    {
                        var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                        if (xBR < 0)
                        {
                            if (DropDownArrow) ArrowAlign = TAlign.BL;
                            xBR = Point.X + Rect.X - Padding;
                            SetLocation(xBR, yBR);
                        }
                    }
                    break;
                case TAlignFrom.BL:
                default:
                    if (DropDownArrow) ArrowAlign = TAlign.BL;
                    int x = Point.X + Rect.X - Padding, y = Point.Y + Rect.Bottom;
                    SetLocation(x, y);
                    if (Collision)
                    {
                        var screen = Screen.FromPoint(TargetRect.Location).WorkingArea;
                        if (x > (screen.X + screen.Width) - TargetRect.Width)
                        {
                            if (DropDownArrow) ArrowAlign = TAlign.BR;
                            x = Point.X + (Rect.X + Rect.Width) - Width + Padding;
                            SetLocation(x, y);
                        }
                    }
                    break;
            }
        }

        public PointF[]? CLocation(Point Point, TAlignFrom Placement, bool DropDownArrow, int ArrowSize, int Padding, int Width, int Height, Rectangle Rect, ref TAlign ArrowAlign, bool Collision = false)
        {
            CLocation(Point, Placement, DropDownArrow, Padding, Width, Height, Rect, ref ArrowAlign, Collision);
            if (Rect.Height >= Rect.Width)
            {
                int ArrowSize2 = ArrowSize * 2;
                switch (Placement)
                {
                    case TAlignFrom.TL:
                        if (ArrowAlign == TAlign.TR)
                        {
                            int x = Width - Rect.Width - Padding + Rect.Width / 2, y = Height - Padding;
                            return new PointF[] { new PointF(x - ArrowSize, y), new PointF(x + ArrowSize, y), new PointF(x, y + ArrowSize) };
                        }
                        else
                        {
                            int x = Padding + Rect.Width / 2, y = Height - Padding;
                            return new PointF[] { new PointF(x - ArrowSize, y), new PointF(x + ArrowSize, y), new PointF(x, y + ArrowSize) };
                        }
                    case TAlignFrom.TR:
                        if (ArrowAlign == TAlign.TL)
                        {
                            int x = Padding + Rect.Width / 2, y = Height - Padding;
                            return new PointF[] { new PointF(x - ArrowSize, y), new PointF(x + ArrowSize, y), new PointF(x, y + ArrowSize) };
                        }
                        else
                        {
                            int x = Width - Rect.Width - Padding + Rect.Width / 2, y = Height - Padding;
                            return new PointF[] { new PointF(x - ArrowSize, y), new PointF(x + ArrowSize, y), new PointF(x, y + ArrowSize) };
                        }
                    case TAlignFrom.BR:
                        if (ArrowAlign == TAlign.BL)
                        {
                            int x = Padding + Rect.Width / 2, y = Padding - ArrowSize;
                            return new PointF[] { new PointF(x, y), new PointF(x - ArrowSize, y + ArrowSize), new PointF(x + ArrowSize, y + ArrowSize) };
                        }
                        else
                        {
                            int x = Width - Rect.Width - Padding + Rect.Width / 2, y = Padding - ArrowSize;
                            return new PointF[] { new PointF(x, y), new PointF(x - ArrowSize, y + ArrowSize), new PointF(x + ArrowSize, y + ArrowSize) };
                        }
                    case TAlignFrom.BL:
                    default:
                        if (ArrowAlign == TAlign.BR)
                        {
                            int x = Width - Rect.Width - Padding + Rect.Width / 2, y = Padding - ArrowSize;
                            return new PointF[] { new PointF(x, y), new PointF(x - ArrowSize, y + ArrowSize), new PointF(x + ArrowSize, y + ArrowSize) };
                        }
                        else
                        {
                            int x = Padding + Rect.Width / 2, y = Padding - ArrowSize;
                            return new PointF[] { new PointF(x, y), new PointF(x - ArrowSize, y + ArrowSize), new PointF(x + ArrowSize, y + ArrowSize) };
                        }
                }
            }
            return null;
        }
    }
}