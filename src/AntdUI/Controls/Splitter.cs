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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Splitter 分隔面板
    /// </summary>
    /// <remarks>自由切分指定区域</remarks>
    [Description("Splitter 分隔面板")]
    [ToolboxItem(true)]
    public class Splitter : SplitContainer
    {
        /// <summary>
        /// 记录折叠前的分隔距离
        /// </summary>
        private int _lastDistance;

        //记录折叠控件最小大小
        //折叠忽略最小，最小只在拖动分割栏时有效
        private int _minSize;

        /// <summary>
        /// 当前鼠标状态
        /// null = 无
        /// false = 移动
        /// true = 箭头
        /// </summary>
        private bool? _MouseState;

        /// <summary>
        /// 记录折叠控件的当前大小
        /// </summary>
        private int _size;

        /// <summary>
        /// 箭头SVG
        /// </summary>
        private string[] arrowSvg = new string[4]
        {
            "UpOutlined" , "DownOutlined",
            "LeftOutlined" , "RightOutlined"
        };

        /// <summary>
        /// 鼠标是否在箭头区域
        /// </summary>
        private bool m_bIsArrowRegion;

        public Splitter()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
        }

        #region 事件

        /// <summary>
        /// SplitPanelState 属性值更改时发生
        /// </summary>
        [Description("SplitPanelState 属性值更改时发生"), Category("行为")]
        public event BoolEventHandler? SplitPanelStateChanged = null;

        #endregion 事件

        #region 属性

        private Color? _arrawBackHoverColor;

        private Color? _arrowBackColor;

        private Color? _arrowColor;

        private int _arrowSize = 30;

        private ADCollapsePanel _collapsePanel = ADCollapsePanel.Panel1;

        /// <summary>
        /// 当前折叠状态
        /// </summary>
        private bool _splitPanelState = true;

        private Color? back;

        public enum ADCollapsePanel
        {
            None = 0,
            Panel1 = 1,
            Panel2 = 2,
        }

        [Description("箭头背景色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ArrawBackColor
        {
            get => _arrowBackColor;
            set
            {
                _arrowBackColor = value;
                Invalidate();
            }
        }

        [Description("鼠标悬浮箭头背景色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ArrawBackHoverColor
        {
            get => _arrawBackHoverColor;
            set
            {
                _arrawBackHoverColor = value;
                Invalidate();
            }
        }

        [Description("箭头颜色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? ArrowColor
        {
            get => _arrowColor;
            set
            {
                _arrowColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 箭头大小
        /// </summary>
        [Description("箭头大小"), Category("外观"), DefaultValue(30)]
        public int ArrowSize
        {
            get => _arrowSize;
            set
            {
                if (_arrowSize == value) return;
                _arrowSize = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), DefaultValue(null), Category("外观")]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? BackColor
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        [Description("点击后收起的Panel"), Category("行为"), DefaultValue(ADCollapsePanel.Panel1)]
        public ADCollapsePanel CollapsePanel
        {
            get => _collapsePanel;
            set
            {
                if (_collapsePanel != value)
                {
                    Expand();
                    _collapsePanel = value;
                    Invalidate();
                }
            }
        }

        [Description("拆分器是水平的还是垂直的"), Category("行为"), DefaultValue(Orientation.Vertical)]
        public new Orientation Orientation
        {
            get => base.Orientation;
            set
            {
                if (base.Orientation == value) return;
                base.Orientation = value;
                if (!SplitPanelState)
                {
                    SplitPanelState = true;
                    SplitPanelState = false;
                }
                Invalidate();
            }
        }

        [Description("分隔栏宽度"), Category("外观"), DefaultValue(15)]
        public new int SplitterWidth
        {
            get => base.SplitterWidth;
            set
            {
                if (base.SplitterWidth == value) return;
                base.SplitterWidth = value;
                Invalidate();
            }
        }

        [Description("是否进行展开"), Category("行为"), DefaultValue(true)]
        private bool SplitPanelState
        {
            get => _splitPanelState;
            set
            {
                if (_splitPanelState == value)
                    return;

                if (value)
                    Expand();
                else
                    Collapse();

                _splitPanelState = value;
            }
        }

        /// <summary>
        /// 折叠
        /// </summary>
        public void Collapse()
        {
            if (_collapsePanel == ADCollapsePanel.None)
                return;

            _lastDistance = SplitterDistance;
            if (_collapsePanel == ADCollapsePanel.Panel1)
            {
                _minSize = Panel1MinSize;
                Panel1MinSize = 0;
                SplitterDistance = 0;
            }
            else
            {
                int width = Orientation == Orientation.Horizontal ?
                    Height : Width;
                _minSize = Panel2MinSize;
                Panel2MinSize = 0;
                SplitterDistance = width - SplitterWidth - Padding.Vertical;
            }

            _splitPanelState = false;
            Invalidate();
        }

        /// <summary>
        /// 展开
        /// </summary>
        public void Expand()
        {
            if (_collapsePanel == ADCollapsePanel.None)
                return;

            SplitterDistance = _lastDistance;

            if (_collapsePanel == ADCollapsePanel.Panel1)
                Panel1MinSize = _minSize;
            else
                Panel2MinSize = _minSize;

            _splitPanelState = true;
            Invalidate();
        }

        #endregion 属性

        #region 渲染

        /// <summary>
        /// 箭头背景区域
        /// </summary>
        private Rectangle ArrowRect
        {
            get
            {
                if (_collapsePanel == ADCollapsePanel.None)
                    return Rectangle.Empty;

                int size = ArrowSize;
                Rectangle rect = SplitterRectangle;
                if (Orientation == Orientation.Horizontal)
                {
                    rect.X = (int)((Width - size) / 2);
                    rect.Width = (int)(size);
                }
                else
                {
                    rect.Y = (int)((Height - size) / 2);
                    rect.Height = (int)(size);
                }

                return rect;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Panel1Collapsed || Panel2Collapsed) return;
            var g = e.Graphics.High();
            var rect = SplitterRectangle;

            //绘制背景
            g.Fill(back ?? Colour.Split.Get("Splitter"), rect);

            //不进行折叠
            if (_collapsePanel == ADCollapsePanel.None)
                return;

            //画箭头背景
            Point[] points = GetHandlePoints();
            if (m_bIsArrowRegion)
                g.FillPolygon(_arrawBackHoverColor ?? Colour.PrimaryBgHover.Get("Splitter"), points);
            else
                g.FillPolygon(_arrowBackColor ?? Colour.PrimaryBg.Get("Splitter"), points);

            //计算显示图标
            bool is_p1 = _collapsePanel == ADCollapsePanel.Panel1;
            int index = 0;
            if (Orientation == Orientation.Horizontal)
                index = 1;
            else
                index = 3;

            if (is_p1 == SplitPanelState)
                index--;

            //绘制箭头
            DrawArrow(g, arrowSvg[index]);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        private void DrawArrow(Canvas g, string svg)
        {
            int size = Math.Min(ArrowSize, SplitterWidth);
            Point point;

            //居中显示
            if (Orientation == Orientation.Horizontal)
                point = new Point((SplitterRectangle.Width - size) / 2, SplitterRectangle.Top);
            else
                point = new Point(SplitterRectangle.Left, (SplitterRectangle.Height - size) / 2);

            Rectangle rectangle = new Rectangle(point.X, point.Y, size, size);

            SvgExtend.GetImgExtend(g, svg, rectangle, _arrowColor ?? Colour.PrimaryBorder.Get("Splitter"));
        }

        private Point[] GetHandlePoints()
        {
            var points = new List<Point>();
            bool isPanel1 = CollapsePanel == ADCollapsePanel.Panel1;
            bool isCollapsed = isPanel1 == SplitPanelState;

            int size = 5;

            if (Orientation == Orientation.Horizontal)
            {
                int y1 = 0, y2 = 0;
                if (isCollapsed)
                {
                    y1 = ArrowRect.Bottom;
                    y2 = ArrowRect.Top;
                }
                else
                {
                    y1 = ArrowRect.Top;
                    y2 = ArrowRect.Bottom;
                }
                points.Add(new Point(ArrowRect.Left, y1));
                points.Add(new Point(ArrowRect.Right, y1));
                points.Add(new Point(ArrowRect.Right - size, y2));
                points.Add(new Point(ArrowRect.Left + size, y2));
            }
            else if (Orientation == Orientation.Vertical)
            {
                int x1 = 0, x2 = 0;
                if (isCollapsed)
                {
                    x1 = ArrowRect.Right;
                    x2 = ArrowRect.Left;
                }
                else
                {
                    x1 = ArrowRect.Left;
                    x2 = ArrowRect.Right;
                }
                points.Add(new Point(x1, ArrowRect.Top));
                points.Add(new Point(x1, ArrowRect.Bottom));
                points.Add(new Point(x2, ArrowRect.Bottom - size));
                points.Add(new Point(x2, ArrowRect.Top + size));
            }

            return points.ToArray();
        }

        #endregion 渲染

        #region 鼠标

        /// <summary>
        /// 重载鼠标按下事件
        /// </summary>
        /// <param name="e">鼠标参数</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode)
                return;

            //点位在箭头矩形内
            if (ArrowRect.Contains(e.Location))
                _MouseState = true;
            else if (!SplitPanelState)
                _MouseState = null;
            //点位在分割线内
            else if (SplitterRectangle.Contains(e.Location))
                _MouseState = false;

            if (_MouseState != true)
                base.OnMouseDown(e);
        }

        /// <summary>
        /// 重载鼠标离开事件
        /// </summary>
        /// <param name="e">鼠标参数</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (DesignMode)
                return;
            base.Cursor = Cursors.Default;
            m_bIsArrowRegion = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 重载鼠标移动事件
        /// </summary>
        /// <param name="e">鼠标参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DesignMode)
                return;

            //如果鼠标的左键没有按下，重置鼠标状态
            if (e.Button != MouseButtons.Left)
                _MouseState = null;

            //鼠标在Arrow矩形里，并且不是在拖动
            if (ArrowRect.Contains(e.Location) && _MouseState != false)
            {
                Cursor = Cursors.Hand;
                m_bIsArrowRegion = true;
                Invalidate();
                return;
            }

            if (_MouseState == true)
                return;

            m_bIsArrowRegion = false;
            Invalidate();

            //鼠标在分隔栏矩形里
            if (SplitterRectangle.Contains(e.Location))
            {
                //如果已经折叠，就不允许拖动了
                if (_collapsePanel != ADCollapsePanel.None && !SplitPanelState)
                    Cursor = Cursors.Default;
                //鼠标没有按下，设置Split光标
                else if (_MouseState == null && !IsSplitterFixed)
                    Cursor = Orientation == Orientation.Horizontal ? Cursors.HSplit : Cursors.VSplit;
                return;
            }

            //正在拖动分隔栏
            if (_MouseState == false && !IsSplitterFixed)
            {
                Cursor = Orientation == Orientation.Horizontal ? Cursors.HSplit : Cursors.VSplit;
                base.OnMouseMove(e);
                return;
            }

            base.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 重载鼠标抬起事件
        /// </summary>
        /// <param name="e">鼠标参数</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode)
                return;

            base.OnMouseUp(e);
            Invalidate();

            if (_MouseState == true && e.Button == MouseButtons.Left && ArrowRect.Contains(e.Location))
            {
                SplitPanelState = !SplitPanelState;
                SplitPanelStateChanged?.Invoke(this, new BoolEventArgs(SplitPanelState));
            }

            _MouseState = null;
        }

        #endregion 鼠标
    }
}