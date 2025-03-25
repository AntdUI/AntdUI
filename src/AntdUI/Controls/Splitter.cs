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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Splitter 分隔面板
    /// </summary>
    /// <remarks>自由切分指定区域。</remarks>
    [Description("Splitter 分隔面板")]
    [ToolboxItem(true)]
    public class Splitter : SplitContainer
    {
        public Splitter()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);

            SplitterMoving += Splitter_SplitterMoving;
            SplitterMoved += Splitter_SplitterMoved;
        }

        #region 参数

        /// <summary>
        /// 记录折叠前的分隔距离
        /// </summary>
        float _lastDistance;

        //记录折叠控件最小大小
        //折叠忽略最小，最小只在拖动分割栏时有效
        int _minSize;

        /// <summary>
        /// 当前鼠标状态
        /// null = 无
        /// false = 移动
        /// true = 箭头
        /// </summary>
        bool? _MouseState;

        /// <summary>
        /// 箭头SVG
        /// </summary>
        string[] arrowSvg = new string[4]
        {
            "UpOutlined" , "DownOutlined",
            "LeftOutlined" , "RightOutlined"
        };

        /// <summary>
        /// 鼠标是否在箭头区域
        /// </summary>
        bool m_bIsArrowRegion;

        #endregion

        #region 属性

        /// <summary>
        /// 滑块大小
        /// </summary>
        [Description("滑块大小"), Category("行为"), DefaultValue(20)]
        public int SplitterSize { get; set; } = 20;

        Color? splitterBack;
        /// <summary>
        /// 滑块背景
        /// </summary>
        [Description("滑块背景"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? SplitterBack
        {
            get => splitterBack;
            set
            {
                if (splitterBack == value) return;
                splitterBack = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 滑块移动背景
        /// </summary>
        [Description("滑块移动背景"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? SplitterBackMove { get; set; }

        Color? _arrowColor;
        /// <summary>
        /// 箭头颜色
        /// </summary>
        [Description("箭头颜色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ArrowColor
        {
            get => _arrowColor;
            set
            {
                if (_arrowColor == value) return;
                _arrowColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标悬浮箭头颜色
        /// </summary>
        [Description("鼠标悬浮箭头颜色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ArrawColorHover { get; set; }

        Color? _arrowBackColor;
        [Description("箭头背景色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ArrawBackColor
        {
            get => _arrowBackColor;
            set
            {
                if (_arrowBackColor == value) return;
                _arrowBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标悬浮箭头背景色
        /// </summary>
        [Description("鼠标悬浮箭头背景色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ArrawBackHover { get; set; }

        ADCollapsePanel _collapsePanel = ADCollapsePanel.None;
        /// <summary>
        /// 点击后收起的Panel
        /// </summary>
        [Description("点击后收起的Panel"), Category("行为"), DefaultValue(ADCollapsePanel.None)]
        public ADCollapsePanel CollapsePanel
        {
            get => _collapsePanel;
            set
            {
                if (_collapsePanel == value) return;
                Expand();
                _collapsePanel = value;
                Invalidate();
            }
        }

        public enum ADCollapsePanel
        {
            None = 0,
            Panel1 = 1,
            Panel2 = 2,
        }

        /// <summary>
        /// 拆分器是水平的还是垂直的
        /// </summary>
        [Description("拆分器是水平的还是垂直的"), Category("行为"), DefaultValue(Orientation.Vertical)]
        public new Orientation Orientation
        {
            get => base.Orientation;
            set
            {
                if (base.Orientation == value) return;
                base.Orientation = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 当前折叠状态
        /// </summary>
        bool _splitPanelState = true;
        [Description("是否进行展开"), Category("行为"), DefaultValue(true)]
        public bool SplitPanelState
        {
            get => _splitPanelState;
            set
            {
                if (_splitPanelState == value) return;
                if (value) Expand();
                else Collapse();
            }
        }

        /// <summary>
        /// 当前方向的大小
        /// </summary>
        private int Length => Orientation == Orientation.Horizontal ? Height : Width;

        /// <summary>
        /// 延时渲染
        /// </summary>
        [Description("延时渲染"), Category("行为"), DefaultValue(true)]
        public bool Lazy { get; set; } = true;

        #endregion

        #region 方法

        /// <summary>
        /// 折叠
        /// </summary>
        public void Collapse()
        {
            if (_collapsePanel == ADCollapsePanel.None || !SplitPanelState) return;
            _splitPanelState = false;
            _lastDistance = SplitterDistance * 1F / Length;
            if (_collapsePanel == ADCollapsePanel.Panel1)
            {
                _minSize = Panel1MinSize;
                SplitterDistance = _minSize;
            }
            else
            {
                _minSize = Panel2MinSize;
                SplitterDistance = Length - SplitterWidth - Padding.Vertical + _minSize;
            }
            Invalidate();
        }

        /// <summary>
        /// 展开
        /// </summary>
        public void Expand()
        {
            if (_collapsePanel == ADCollapsePanel.None || SplitPanelState) return;
            _splitPanelState = true;
            SplitterDistance = (int)(_lastDistance * Length);
            if (_collapsePanel == ADCollapsePanel.Panel1) Panel1MinSize = _minSize;
            else Panel2MinSize = _minSize;
            Invalidate();
        }

        #endregion

        #region 事件

        /// <summary>
        /// SplitPanelState 属性值更改时发生
        /// </summary>
        [Description("SplitPanelState 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? SplitPanelStateChanged;

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Panel1Collapsed || Panel2Collapsed) return;
            var g = e.Graphics.High();
            var rect = SplitterRectangle;
            if (_collapsePanel == ADCollapsePanel.None)
            {
                if (moving) g.Fill(SplitterBackMove ?? Style.Db.PrimaryBg, rect);
                else g.Fill(splitterBack ?? Style.Db.FillTertiary, rect);
                int size = (int)(SplitterSize * Config.Dpi);
                if (Orientation == Orientation.Horizontal) g.Fill(Style.Db.Fill, new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y, size, rect.Height));
                else g.Fill(Style.Db.Fill, new Rectangle(rect.X, rect.Y + (rect.Height - size) / 2, rect.Width, size));
            }
            else
            {
                if (moving) g.Fill(SplitterBackMove ?? Style.Db.PrimaryBg, rect);
                else g.Fill(splitterBack ?? Style.Db.FillTertiary, rect);

                //计算显示图标
                int index = 0;
                if (Orientation == Orientation.Horizontal) index = 1;
                else index = 3;
                if ((_collapsePanel == ADCollapsePanel.Panel1) == SplitPanelState) index--;

                //画箭头背景
                var points = GetHandlePoints(out var rect_arrow);

                //绘制箭头
                if (m_bIsArrowRegion)
                {
                    g.FillPolygon(ArrawBackHover ?? Colour.Primary.Get("Splitter"), points);
                    SvgExtend.GetImgExtend(g, arrowSvg[index], rect_arrow, ArrawColorHover ?? Colour.PrimaryColor.Get("Splitter"));
                }
                else
                {
                    g.FillPolygon(_arrowBackColor ?? Colour.PrimaryBg.Get("Splitter"), points);
                    SvgExtend.GetImgExtend(g, arrowSvg[index], rect_arrow, _arrowColor ?? Colour.PrimaryBorder.Get("Splitter"));
                }
            }
        }

        // <summary>
        /// 箭头背景区域
        /// </summary>
        Rectangle ArrowRect(Rectangle rect)
        {
            if (_collapsePanel == ADCollapsePanel.None) return Rectangle.Empty;
            int size = (int)(SplitterSize * Config.Dpi);
            if (Orientation == Orientation.Horizontal)
            {
                rect.X = (Width - size) / 2;
                rect.Width = size;
            }
            else
            {
                rect.Y = (Height - size) / 2;
                rect.Height = size;
            }
            return rect;
        }

        Point[] GetHandlePoints(out Rectangle rect_arrow)
        {
            bool isCollapsed = (CollapsePanel == ADCollapsePanel.Panel1) == SplitPanelState;

            int size = (int)(4 * Config.Dpi);

            rect_arrow = ArrowRect(SplitterRectangle);
            if (Orientation == Orientation.Horizontal)
            {
                int y1 = 0, y2 = 0;
                if (isCollapsed)
                {
                    y1 = rect_arrow.Bottom;
                    y2 = rect_arrow.Top;
                }
                else
                {
                    y1 = rect_arrow.Top;
                    y2 = rect_arrow.Bottom;
                }
                return new Point[] {
                    new Point(rect_arrow.Left, y1),
                    new Point(rect_arrow.Right, y1),
                    new Point(rect_arrow.Right - size, y2),
                    new Point(rect_arrow.Left + size, y2)
                };
            }
            else
            {
                int x1 = 0, x2 = 0;
                if (isCollapsed)
                {
                    x1 = rect_arrow.Right;
                    x2 = rect_arrow.Left;
                }
                else
                {
                    x1 = rect_arrow.Left;
                    x2 = rect_arrow.Right;
                }
                return new Point[] {
                    new Point(x1, rect_arrow.Top),
                    new Point(x1, rect_arrow.Bottom),
                    new Point(x2, rect_arrow.Bottom - size),
                    new Point(x2, rect_arrow.Top + size)
                };
            }
        }

        #endregion

        #region 鼠标

        bool moving = false;
        private void Splitter_SplitterMoving(object? sender, SplitterCancelEventArgs e)
        {
            if (e.Cancel) return;
            moving = true;
            Invalidate();
        }

        private void Splitter_SplitterMoved(object? sender, SplitterEventArgs e)
        {
            moving = false;
            Invalidate();
        }

        Point initialMousePoint;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Rectangle rect = SplitterRectangle, rect_arrow = ArrowRect(rect);
            if (_collapsePanel != ADCollapsePanel.None && rect_arrow.Contains(e.Location)) _MouseState = true;//点位在箭头矩形内
            else if (!SplitPanelState) _MouseState = null;
            else if (rect.Contains(e.Location))
            {
                _MouseState = false;//点位在分割线内
                initialMousePoint = e.Location;
            }
            if (_MouseState != true && SplitPanelState) base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SetCursor(CursorType.Default);
            m_bIsArrowRegion = false;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //如果鼠标的左键没有按下，重置鼠标状态
            if (e.Button != MouseButtons.Left) _MouseState = null;
            //鼠标在Arrow矩形里，并且不是在拖动
            Rectangle rect = SplitterRectangle, rect_arrow = ArrowRect(rect);
            if (_collapsePanel != ADCollapsePanel.None && rect_arrow.Contains(e.Location) && _MouseState != false)
            {
                SetCursor(CursorType.Hand);
                m_bIsArrowRegion = true;
                Invalidate();
                return;
            }

            if (_MouseState == true) return;
            m_bIsArrowRegion = false;
            Invalidate();
            //鼠标在分隔栏矩形里
            if (rect.Contains(e.Location))
            {
                //如果已经折叠，就不允许拖动了
                if (_collapsePanel != ADCollapsePanel.None && !SplitPanelState) SetCursor(CursorType.Default);
                else if (_MouseState == null && !IsSplitterFixed) SetCursor(Orientation == Orientation.Horizontal ? CursorType.HSplit : CursorType.VSplit);//鼠标没有按下，设置Split光标
                return;
            }
            //正在拖动分隔栏
            if (_MouseState == false && !IsSplitterFixed)
            {
                SetCursor(Orientation == Orientation.Horizontal ? CursorType.HSplit : CursorType.VSplit);
                if (!Lazy)
                {
                    SplitMove(e.X, e.Y);
                    initialMousePoint = e.Location;
                    return;
                }
            }
            else SetCursor(CursorType.Default);
            base.OnMouseMove(e);
        }

        private void SplitMove(int x, int y)
        {
            int size = GetSplitterDistance(x, y);
            if (SplitterDistance != size)
            {
                if (Orientation == Orientation.Vertical)
                {
                    if (size + SplitterWidth <= Width - Panel2MinSize) SplitterDistance = size;
                }
                else
                {
                    if (size + SplitterWidth <= Height - Panel2MinSize) SplitterDistance = size;
                }
            }
        }

        private int GetSplitterDistance(int x, int y)
        {
            int delta;
            if (Orientation == Orientation.Vertical) delta = x - initialMousePoint.X;
            else delta = y - initialMousePoint.Y;

            int size = 0;
            switch (Orientation)
            {
                case Orientation.Vertical:
                    size = Panel1.Width + delta;
                    break;
                case Orientation.Horizontal:
                    size = Panel1.Height + delta;
                    break;
            }
            if (Orientation == Orientation.Vertical) return Math.Max(Math.Min(size, Width - Panel2MinSize), Panel1MinSize);
            else return Math.Max(Math.Min(size, Height - Panel2MinSize), Panel1MinSize);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //if (_collapsePanel == ADCollapsePanel.None)
            //{
            //    base.OnMouseUp(e);
            //    return;
            //}
            if (Lazy) base.OnMouseUp(e);
            Invalidate();
            if (_collapsePanel != ADCollapsePanel.None && _MouseState == true && e.Button == MouseButtons.Left && ArrowRect(SplitterRectangle).Contains(e.Location))
            {
                SplitPanelState = !_splitPanelState;
                SplitPanelStateChanged?.Invoke(this, new BoolEventArgs(SplitPanelState));
            }
            _MouseState = null;
        }

        #region 鼠标

        CursorType oldcursor = CursorType.Default;
        public void SetCursor(bool val) => SetCursor(val ? CursorType.Hand : CursorType.Default);
        public void SetCursor(CursorType cursor = CursorType.Default)
        {
            if (oldcursor == cursor) return;
            oldcursor = cursor;
            bool flag = true;
            switch (cursor)
            {
                case CursorType.Hand:
                    SetCursor(Cursors.Hand);
                    break;
                case CursorType.IBeam:
                    SetCursor(Cursors.IBeam);
                    break;
                case CursorType.No:
                    SetCursor(Cursors.No);
                    break;
                case CursorType.SizeAll:
                    flag = false;
                    SetCursor(Cursors.SizeAll);
                    break;
                case CursorType.VSplit:
                    flag = false;
                    SetCursor(Cursors.VSplit);
                    break;
                case CursorType.HSplit:
                    flag = false;
                    SetCursor(Cursors.HSplit);
                    break;
                case CursorType.Default:
                default:
                    SetCursor(DefaultCursor);
                    break;
            }
            SetWindow(flag);
        }
        void SetCursor(Cursor cursor)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetCursor(cursor)));
                return;
            }
            Cursor = cursor;
        }

        bool setwindow = false;
        void SetWindow(bool flag)
        {
            if (setwindow == flag) return;
            setwindow = flag;
            var form = Parent.FindPARENT();
            if (form is BaseForm baseForm) baseForm.EnableHitTest = setwindow;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            SetWindow(true);
            SplitterMoving -= Splitter_SplitterMoving;
            SplitterMoved -= Splitter_SplitterMoved;
            base.Dispose(disposing);
        }

        #endregion
    }
}