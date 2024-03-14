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
using System.Windows.Forms;
using static Vanara.PInvoke.User32;

namespace AntdUI
{
    /// <summary>
    /// Tabs 标签页
    /// </summary>
    /// <remarks>选项卡切换组件。</remarks>
    [Description("Tabs 标签页")]
    [ToolboxItem(true)]
    public class Tabs : TabControl
    {
        public Tabs()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        #region 属性

        Color? fill;
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色"), Category("外观"), DefaultValue(null)]
        public Color? Fill
        {
            get => fill;
            set
            {
                if (fill == value) return;
                fill = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停颜色
        /// </summary>
        [Description("悬停颜色"), Category("外观"), DefaultValue(null)]
        public Color? FillHover { get; set; }

        /// <summary>
        /// 条大小
        /// </summary>
        [Description("条大小"), Category("条"), DefaultValue(3F)]
        public float BarSize { get; set; } = 3F;

        /// <summary>
        /// 条背景大小
        /// </summary>
        [Description("条背景大小"), Category("条"), DefaultValue(1F)]
        public float BarBackSize { get; set; } = 1F;

        /// <summary>
        /// 条背景
        /// </summary>
        [Description("条背景"), Category("条"), DefaultValue(null)]
        public Color? BarBack { get; set; }

        [Description("样式"), Category("外观"), DefaultValue(TabType.Line)]
        public TabType Type { get; set; } = TabType.Line;

        Color _backColor = Color.Transparent;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Browsable(true)]
        [Description("背景颜色"), Category("外观"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor == value) return;
                _backColor = value;
                Invalidate();
            }
        }

        #region 徽标

        TabsBadgeCollection? badge;
        /// <summary>
        /// 徽标集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("徽标集合"), Category("徽标")]
        public TabsBadgeCollection Badge
        {
            get
            {
                badge ??= new TabsBadgeCollection(this);
                return badge;
            }
            set => badge = value.BindData(this);
        }

        [Description("徽标大小"), Category("徽标"), DefaultValue(9F)]
        public float BadgeSize { get; set; } = 9F;

        [Description("徽标背景颜色"), Category("徽标"), DefaultValue(null)]
        public Color? BadgeBack { get; set; }

        #endregion

        #region 隐藏头

        bool _tabMenuVisible = true;
        [Description("是否显示头"), Category("外观"), DefaultValue(true)]
        public bool TabMenuVisible
        {
            get => _tabMenuVisible;
            set
            {
                _tabMenuVisible = value;
                if (!value) ItemSize = new Size(1, 1);
                Invalidate();
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                if (_tabMenuVisible) return base.DisplayRectangle;
                else
                {
                    Rectangle rect = base.DisplayRectangle;
                    return new Rectangle(rect.Left - 4, rect.Top - 5, rect.Width + 8, rect.Height + 9);
                }
            }
        }

        #endregion

        #endregion

        #region 渲染

        int hover_i = -1;
        int Hover_i
        {
            get => hover_i;
            set
            {
                if (hover_i == value) return;
                hover_i = value;
                Invalidate();
            }
        }

        Dictionary<int, TabsBadge> badges = new Dictionary<int, TabsBadge>();
        public void ChangeBadge()
        {
            badges.Clear();
#if NET40 || NET46 || NET48
            foreach (TabsBadge it in Badge)
            {
                if (!badges.ContainsKey(it.Index)) badges.Add(it.Index, it);
            }
#else
            foreach (TabsBadge it in Badge)
            {
                badges.TryAdd(it.Index, it);
            }
#endif
        }

        #region 动画

        protected override void OnCreateControl()
        {
            SelectValue = SelectedIndex;
            base.OnCreateControl();
            if (UpDownButtonHandle != IntPtr.Zero)
            {
                if (_upDownButtonNativeWindow == null) _upDownButtonNativeWindow = new UpDownButtonNativeWindow(this);
            }
        }

        int _select = -1;
        int SelectValue
        {
            get => _select;
            set
            {
                if (_select == value) return;
                int old = _select;
                _select = value;
                if (Type == TabType.Line) SetRect(old, _select);
                else Invalidate();
            }
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            SelectValue = SelectedIndex;
            base.OnSelectedIndexChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            ThreadBar?.Dispose();
            base.Dispose(disposing);
        }
        bool AnimationBar = false;
        RectangleF AnimationBarValue;
        ITask? ThreadBar = null;

        RectangleF TabSelectRect;

        void SetRect(int old, int value)
        {
            if (value > -1)
            {
                if (old > -1)
                {
                    ThreadBar?.Dispose();
                    RectangleF OldValue, NewValue;
                    using (var g = Graphics.FromHwnd(Handle))
                    {
                        OldValue = GetRect(g, GetTabRect(old), TabPages[old]);
                        NewValue = GetRect(g, GetTabRect(value), TabPages[value]);
                    }
                    if (Config.Animation && OldValue.Y == NewValue.Y)
                    {
                        AnimationBar = true;
                        TabSelectRect = NewValue;
                        float p_val = Math.Abs(NewValue.X - AnimationBarValue.X) * 0.09F, p_w_val = Math.Abs(NewValue.Width - AnimationBarValue.Width) * 0.1F, p_val2 = (NewValue.X - AnimationBarValue.X) * 0.5F;
                        ThreadBar = new ITask(this, () =>
                        {
                            if (AnimationBarValue.Width != NewValue.Width)
                            {
                                if (NewValue.Width > OldValue.Width)
                                {
                                    AnimationBarValue.Width += p_w_val;
                                    if (AnimationBarValue.Width > NewValue.Width) AnimationBarValue.Width = NewValue.Width;
                                }
                                else
                                {
                                    AnimationBarValue.Width -= p_w_val;
                                    if (AnimationBarValue.Width < NewValue.Width) AnimationBarValue.Width = NewValue.Width;
                                }
                            }
                            if (NewValue.X > OldValue.X)
                            {
                                if (AnimationBarValue.X > p_val2)
                                    AnimationBarValue.X += p_val / 2F;
                                else AnimationBarValue.X += p_val;
                                if (AnimationBarValue.X > NewValue.X)
                                {
                                    AnimationBarValue.X = NewValue.X;
                                    Invalidate();
                                    return false;
                                }
                            }
                            else
                            {
                                AnimationBarValue.X -= p_val;
                                if (AnimationBarValue.X < NewValue.X)
                                {
                                    AnimationBarValue.X = NewValue.X;
                                    Invalidate();
                                    return false;
                                }
                            }
                            Invalidate();
                            return true;
                        }, 10, () =>
                        {
                            AnimationBarValue = NewValue;
                            AnimationBar = false;
                            Invalidate();
                        });
                        return;
                    }
                    else
                    {
                        TabSelectRect = AnimationBarValue = NewValue;
                        Invalidate();
                        return;
                    }
                }
                else
                {
                    using (var g = Graphics.FromHwnd(Handle))
                    {
                        AnimationBarValue = TabSelectRect = GetRect(g, GetTabRect(value), TabPages[value]);
                    }
                }
            }
        }

        internal RectangleF GetRect(Graphics g, Rectangle tab_rect, TabPage page)
        {
            var size = g.MeasureString(page.Text, Font);
            return new RectangleF(tab_rect.X + (tab_rect.Width - size.Width) / 2, tab_rect.Y + tab_rect.Height - BarSize, size.Width, BarSize);
        }

        #endregion

        protected override void OnMouseMove(MouseEventArgs e)
        {
            for (int i = 0; i < TabCount; i++)
            {
                var tab_rect = GetTabRect(i);
                if (tab_rect.Contains(e.Location))
                {
                    Hover_i = i;
                    return;
                }
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Hover_i = -1;
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_tabMenuVisible)
            {
                var rect = ClientRectangle;
                var g = e.Graphics.High();
                int selectedIndex = SelectValue, hover_i = Hover_i;
                if (TabCount > 0)
                {
                    Color _color = fill.HasValue ? fill.Value : Style.Db.Primary;
                    if (BarBackSize > 0)
                    {
                        var tab_rect_0 = GetTabRect(0);
                        var top = new RectangleF(Margin.Left, tab_rect_0.Y + tab_rect_0.Height - BarBackSize, rect.Width - Margin.Horizontal, BarBackSize);
                        using (var brush = new SolidBrush(BarBack.HasValue ? BarBack.Value : Style.Db.Fill))
                        {
                            g.FillRectangle(brush, top);
                        }
                    }
                    if (Type == TabType.Line)
                    {
                        if (AnimationBar)
                        {
                            using (var brush = new SolidBrush(_color))
                            {
                                g.FillRectangle(brush, AnimationBarValue);
                            }
                        }
                        for (int i = 0; i < TabCount; i++)
                        {
                            var page = TabPages[i];
                            var tab_rect = GetTabRect(i);
                            if (selectedIndex == i)//是否选中
                            {
                                if (AnimationBar)
                                {
                                    using (var brush = new SolidBrush(_color))
                                    {
                                        g.DrawString(page.Text, Font, brush, tab_rect, Helper.stringFormatCenter2);
                                    }
                                }
                                else
                                {
                                    using (var brush = new SolidBrush(_color))
                                    {
                                        g.FillRectangle(brush, TabSelectRect);
                                        g.DrawString(page.Text, Font, brush, tab_rect, Helper.stringFormatCenter2);
                                    }
                                }
                            }
                            else if (hover_i == i)
                            {
                                using (var brush = new SolidBrush(FillHover.HasValue ? FillHover.Value : Style.Db.PrimaryHover))
                                {
                                    g.DrawString(page.Text, Font, brush, tab_rect, Helper.stringFormatCenter2);
                                }
                            }
                            else
                            {
                                using (var brush = new SolidBrush(ForeColor))
                                {
                                    g.DrawString(page.Text, Font, brush, tab_rect, Helper.stringFormatCenter2);
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var brush_bg = new SolidBrush(Style.Db.FillQuaternary))
                        {
                            for (int i = 0; i < TabCount; i++)
                            {
                                var page = TabPages[i];
                                var tab_rect = GetTabRect(i);
                                using (var path = Helper.RoundPath(tab_rect, 6 * Config.Dpi, true, true, false, false))
                                {
                                    if (selectedIndex == i)//是否选中
                                    {
                                        using (var brush_bgw = new SolidBrush(Style.Db.BgContainer))
                                        {
                                            g.FillPath(brush_bgw, path);
                                            using (var pen_bg = new Pen(Style.Db.BorderSecondary, 1F * Config.Dpi))
                                            {
                                                g.DrawPath(pen_bg, path);
                                                g.FillRectangle(brush_bgw, new RectangleF(tab_rect.X, tab_rect.Bottom - pen_bg.Width, tab_rect.Width, pen_bg.Width * 2));
                                            }
                                        }
                                        using (var brush = new SolidBrush(_color))
                                        {
                                            g.DrawString(page.Text, Font, brush, tab_rect, Helper.stringFormatCenter2);
                                        }
                                    }
                                    else
                                    {
                                        g.FillPath(brush_bg, path);
                                        using (var pen_bg = new Pen(Style.Db.BorderSecondary, 1F * Config.Dpi))
                                        {
                                            g.DrawPath(pen_bg, path);
                                        }
                                        if (hover_i == i)
                                        {
                                            using (var brush = new SolidBrush(FillHover.HasValue ? FillHover.Value : Style.Db.PrimaryHover))
                                            {
                                                g.DrawString(page.Text, Font, brush, tab_rect, Helper.stringFormatCenter2);
                                            }
                                        }
                                        else
                                        {
                                            using (var brush = new SolidBrush(ForeColor))
                                            {
                                                g.DrawString(page.Text, Font, brush, tab_rect, Helper.stringFormatCenter2);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (badges.Count > 0)
                    {
                        using (var font = new Font(Font.FontFamily, BadgeSize))
                        {
                            for (int i = 0; i < TabCount; i++)
                            {
                                if (badges.TryGetValue(i, out var find)) this.PaintBadge(find, GetTabRect(i), font, g);
                            }
                        }
                    }
                }
            }
            base.OnPaint(e);
        }

        #region 左右按钮

        const string UpDownButtonClassName = "msctls_updown32";
        UpDownButtonNativeWindow? _upDownButtonNativeWindow;

        internal IntPtr UpDownButtonHandle => FindWindowEx(Handle, IntPtr.Zero, UpDownButtonClassName, null);
        public void OnPaintUpDownButton(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Style.Db.BgBase);
            var rect = e.ClipRectangle;
            if (BarBackSize > 0)
            {
                var top = new RectangleF(0, rect.Height - BarBackSize, rect.Width - Margin.Horizontal, BarBackSize);
                using (var brush = new SolidBrush(BarBack.HasValue ? BarBack.Value : Style.Db.Fill))
                {
                    g.FillRectangle(brush, top);
                }
            }
            int width_one = rect.Width / 2;
            RectangleF rect_l = new RectangleF(0, rect.Y, width_one, rect.Height), rect_r = new RectangleF(rect_l.X + width_one, rect.Y, width_one, rect.Height);
            using (var pen_arrow = new Pen(Style.Db.TextTertiary, 1.6F))
            {
                g.DrawLines(pen_arrow, TAlignMini.Left.TriangleLines(rect_l, 0.26F));
                g.DrawLines(pen_arrow, TAlignMini.Right.TriangleLines(rect_r, 0.26F));
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (UpDownButtonHandle != IntPtr.Zero)
            {
                if (_upDownButtonNativeWindow == null) _upDownButtonNativeWindow = new UpDownButtonNativeWindow(this);
            }
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            _upDownButtonNativeWindow?.Dispose();
            _upDownButtonNativeWindow = null;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (UpDownButtonHandle != IntPtr.Zero)
            {
                if (_upDownButtonNativeWindow == null) _upDownButtonNativeWindow = new UpDownButtonNativeWindow(this);
            }
        }

        private class UpDownButtonNativeWindow : NativeWindow, IDisposable
        {
            Tabs owner;

            public UpDownButtonNativeWindow(Tabs _owner)
            {
                owner = _owner;
                AssignHandle(_owner.UpDownButtonHandle);
            }

            void DrawUpDownButton(Rectangle clipRect)
            {
                var mouse = owner.PointToClient(MousePosition);
                bool mouseOver = clipRect.Contains(mouse);
                using (var g = Graphics.FromHwnd(Handle))
                {
                    owner.OnPaintUpDownButton(new PaintEventArgs(g, clipRect));
                }
            }


            Rectangle SetClipRect()
            {
                int width = (int)(52 * Config.Dpi);
                int top = 0;
                if (owner.Alignment == TabAlignment.Top) top = 0;
                else if (owner.Alignment == TabAlignment.Bottom) top = owner.Size.Height - owner.ItemSize.Height;
                return new Rectangle(owner.Size.Width - width, top, width, owner.ItemSize.Height);
            }

            protected override void WndProc(ref System.Windows.Forms.Message m)
            {
                switch ((WindowMessage)m.Msg)
                {
                    case WindowMessage.WM_PAINT:
                    case WindowMessage.WM_NCPAINT:
                        var clipRect = SetClipRect();
                        MoveWindow(Handle, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height);
                        var ps = new PAINTSTRUCT();
                        BeginPaint(m.HWnd, ref ps);
                        DrawUpDownButton(clipRect);
                        EndPaint(m.HWnd, ref ps);
                        return;
                }
                base.WndProc(ref m);
            }

            /// <summary>
            /// 析构函数
            /// </summary>
            public void Dispose()
            {
                owner = null;
                base.ReleaseHandle();
            }
        }

        #endregion

        #endregion
    }

    public class TabsBadgeCollection : iCollection<TabsBadge>
    {
        public TabsBadgeCollection(Tabs it)
        {
            BindData(it);
        }

        internal TabsBadgeCollection BindData(Tabs it)
        {
            action = render =>
            {
                if (render) it.ChangeBadge();
                it.Invalidate();
            };
            return this;
        }
    }
    public class TabsBadge
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号"), Category("外观")]
        public int Index { get; set; }


        /// <summary>
        /// 徽标计数 0是点
        /// </summary>
        [Description("徽标计数"), Category("外观")]
        public int Count { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        [Description("填充颜色"), Category("外观")]
        public Color? Fill { get; set; }

        /// <summary>
        /// 用户定义数据
        /// </summary>
        [Description("用户定义数据"), Category("数据"), DefaultValue(null)]
        public object? Tag { get; set; }
    }
}