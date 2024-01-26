// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE GPL-3.0 License.
// LICENSED UNDER THE GPL License, VERSION 3.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// WindowBar 窗口栏
    /// </summary>
    /// <remarks>窗口栏。</remarks>
    [Description("WindowBar 窗口栏")]
    [ToolboxItem(true)]
    [Designer(typeof(IControlDesigner))]
    public class WindowBar : IControl, IButtonControl, IEventListener
    {
        #region 属性

        string? text = null;
        [Description("文字"), Category("外观"), DefaultValue(null)]
        public new string? Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Invalidate();
            }
        }

        string? desc = null;
        [Description("副标题"), Category("外观"), DefaultValue(null)]
        public string? SubText
        {
            get => desc;
            set
            {
                if (desc == value) return;
                desc = value;
                Invalidate();
            }
        }

        int useLeft = 0;

        [Description("左侧使用"), Category("外观"), DefaultValue(0)]
        public int UseLeft
        {
            get => useLeft;
            set
            {
                if (useLeft == value) return;
                useLeft = value;
                Invalidate();
            }
        }

        #region 图标

        Image? icon = null;
        [Description("图标"), Category("外观"), DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                if (icon == value) return;
                icon = value;
                Invalidate();
            }
        }

        string? iconSvg = null;
        [Description("图标SVG"), Category("外观"), DefaultValue(null)]
        public string? IconSvg
        {
            get => iconSvg;
            set
            {
                if (iconSvg == value) return;
                iconSvg = value;
                Invalidate();
            }
        }

        #endregion

        #region 加载动画

        bool loading = false;
        int AnimationLoadingValue = 0;
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                ThreadLoading?.Dispose();
                if (loading)
                {
                    ThreadLoading = new ITask(this, () =>
                    {
                        AnimationLoadingValue += 6;
                        if (AnimationLoadingValue > 360) AnimationLoadingValue = 0;
                        Invalidate();
                        return loading;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            EventManager.Instance.RemoveListener(1, this);
            hove_close.Dispose();
            hove_max.Dispose();
            hove_min.Dispose();
            ThreadLoading?.Dispose();
            temp_logo?.Dispose();
            temp_min?.Dispose();
            temp_max?.Dispose();
            temp_restore?.Dispose();
            temp_close?.Dispose();
            temp_close_hover?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadLoading = null;

        #endregion

        #region 按钮

        bool maximizeBox = true;
        /// <summary>
        /// 是否显示最大化按钮
        /// </summary>
        [Description("是否显示最大化按钮"), Category("外观"), DefaultValue(true)]
        public bool MaximizeBox
        {
            get => maximizeBox;
            set
            {
                if (maximizeBox == value) return;
                maximizeBox = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        bool minimizeBox = true;
        /// <summary>
        /// 是否显示最小化按钮
        /// </summary>
        [Description("是否显示最小化按钮"), Category("外观"), DefaultValue(true)]
        public bool MinimizeBox
        {
            get => minimizeBox;
            set
            {
                if (minimizeBox == value) return;
                minimizeBox = value;
                OnSizeChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        public bool IsMax = false;

        string close_default = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M794.737778 284.444444l-56.32-56.888888-223.573334 223.573333L291.84 227.555556l-56.888889 56.888888 223.573333 223.004445-223.573333 223.573333 56.888889 56.32 223.004444-223.573333 223.573334 223.573333 56.32-56.32-223.573334-223.573333L794.737778 284.444444z\"></path></svg>",
            max_default = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M746.666667 341.333333v341.333334h-469.333334V341.333333h469.333334m68.266666-85.333333H209.066667a17.066667 17.066667 0 0 0-17.066667 17.066667v477.866666a17.066667 17.066667 0 0 0 17.066667 17.066667h605.866666a17.066667 17.066667 0 0 0 17.066667-17.066667V273.066667a17.066667 17.066667 0 0 0-17.066667-17.066667z\"></path></svg>",
           restore_default = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M853.333333 228.124444H284.444444v112.64H170.666667v455.111112h568.888889v-112.64h113.777777v-455.111112zM227.555556 738.986667v-284.444445h455.111111v284.444445H227.555556z m568.888888-112.64h-56.888888V340.764444H341.333333v-55.751111h455.111111v341.333334z\"></path></svg>",
           min_default = "<svg viewBox=\"0 0 1024 1024\"><path d=\"M207.075556 560h568.888888v113.777778h-568.888888z\"></path></svg>";

        #endregion

        /// <summary>
        /// 是否可以拖动位置
        /// </summary>
        [Description("是否可以拖动位置"), Category("行为"), DefaultValue(true)]
        public bool DragMove { get; set; } = true;

        #endregion

        public override Rectangle DisplayRectangle
        {
            get => ClientRectangle.PaddingRect(Padding, 0, 0, hasr, 0);
        }

        readonly StringFormat stringLeft = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle.PaddingRect(Padding, UseLeft, 0, 0, 0);
            var g = e.Graphics.High();

            var size = g.MeasureString(text ?? Config.NullText, Font);

            bool showLeft = false;
            int icon_size = (int)size.Height, iocn_xy = (rect.Height - icon_size) / 2;
            if (loading)
            {
                var rect_icon = new Rectangle(rect.X + iocn_xy, rect.Y + iocn_xy, icon_size, icon_size);
                using (var brush = new Pen(Color.FromArgb(170, ForeColor), 3f))
                {
                    brush.StartCap = brush.EndCap = LineCap.Round;
                    g.DrawArc(brush, rect_icon, AnimationLoadingValue, 100);
                }
                showLeft = true;
            }
            else if (iconSvg != null)
            {
                var rect_icon = new Rectangle(rect.X + iocn_xy, rect.Y + iocn_xy, icon_size, icon_size);
                if (PrintLogo(g, iconSvg, new Rectangle(rect.X + iocn_xy, rect.Y + iocn_xy, icon_size, icon_size))) showLeft = true;
            }
            if (!showLeft && icon != null)
            {
                var rect_icon = new Rectangle(rect.X + iocn_xy, rect.Y + iocn_xy, icon_size, icon_size);
                g.DrawImage(icon, rect_icon);
                showLeft = true;
            }
            if (showLeft)
            {
                int use = iocn_xy * 2 + icon_size;
                rect = new Rectangle(rect.X + use, rect.Y, rect.Width - use, rect.Height);
            }
            else if (rect.X == 0) rect = new Rectangle(rect.X + iocn_xy, rect.Y, rect.Width - iocn_xy, rect.Height);
            using (var brush = new SolidBrush(Style.Db.TextBase))
            {
                g.DrawString(text, Font, brush, rect, stringLeft);
                if (desc != null)
                {
                    using (var brushsub = new SolidBrush(Style.Db.TextSecondary))
                    {
                        g.DrawString(desc, Font, brushsub, new RectangleF(rect.X + size.Width, rect.Y, rect.Width - size.Width, rect.Height), stringLeft);
                    }
                }
            }

            #region 按钮

            int btn_size = (int)(size.Height * 1.2F), btn_x = (rect_close.Width - btn_size) / 2, btn_y = (rect_close.Height - btn_size) / 2;
            var rect_close_icon = new Rectangle(rect_close.X + btn_x, rect_close.Y + btn_y, btn_size, btn_size);
            if (hove_close.Down)
            {
                using (var brush = new SolidBrush(Style.Db.ErrorActive))
                {
                    g.FillRectangle(brush, rect_close);
                }
                PrintCloseHover(g, rect_close_icon);
            }
            else if (hove_close.Animation)
            {
                using (var brush = new SolidBrush(Color.FromArgb(hove_close.Value, Style.Db.Error)))
                {
                    g.FillRectangle(brush, rect_close);
                }
                PrintClose(g, rect_close_icon);
                using (var _bmp = SvgExtend.GetImgExtend(close_default, rect_close_icon, Color.FromArgb(hove_close.Value, Style.Db.ErrorColor)))
                {
                    if (_bmp != null) g.DrawImage(_bmp, rect_close_icon);
                }
            }
            else if (hove_close.Switch)
            {
                using (var brush = new SolidBrush(Style.Db.Error))
                {
                    g.FillRectangle(brush, rect_close);
                }
                PrintCloseHover(g, rect_close_icon);
            }
            else PrintClose(g, rect_close_icon);

            if (maximizeBox)
            {
                var rect_max_icon = new Rectangle(rect_max.X + btn_x, rect_max.Y + btn_y, btn_size, btn_size);
                if (hove_max.Animation)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(hove_max.Value, Style.Db.FillSecondary)))
                    {
                        g.FillRectangle(brush, rect_max);
                    }
                }
                else if (hove_max.Switch)
                {
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        g.FillRectangle(brush, rect_max);
                    }
                }
                if (hove_max.Down)
                {
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        g.FillRectangle(brush, rect_max);
                    }
                }
                if (IsMax) PrintRestore(g, rect_max_icon);
                else PrintMax(g, rect_max_icon);
            }
            if (minimizeBox)
            {
                var rect_min_icon = new Rectangle(rect_min.X + btn_x, rect_min.Y + btn_y, btn_size, btn_size);
                if (hove_min.Animation)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(hove_min.Value, Style.Db.FillSecondary)))
                    {
                        g.FillRectangle(brush, rect_min);
                    }
                }
                else if (hove_min.Switch)
                {
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        g.FillRectangle(brush, rect_min);
                    }
                }
                if (hove_min.Down)
                {
                    using (var brush = new SolidBrush(Style.Db.FillSecondary))
                    {
                        g.FillRectangle(brush, rect_min);
                    }
                }
                PrintMin(g, rect_min_icon);
            }

            #endregion

            base.OnPaint(e);
        }

        #region 渲染帮助

        Bitmap? temp_logo = null, temp_min = null, temp_max = null, temp_restore = null, temp_close = null, temp_close_hover = null;

        void PrintClose(Graphics g, Rectangle rect_icon)
        {
            if (temp_close == null || temp_close.Width != rect_icon.Width)
            {
                temp_close?.Dispose();
                temp_close = SvgExtend.GetImgExtend(close_default, rect_icon, Style.Db.Text);
            }
            if (temp_close != null) g.DrawImage(temp_close, rect_icon);
        }
        void PrintCloseHover(Graphics g, Rectangle rect_icon)
        {
            if (temp_close_hover == null || temp_close_hover.Width != rect_icon.Width)
            {
                temp_close_hover?.Dispose();
                temp_close_hover = SvgExtend.GetImgExtend(close_default, rect_icon, Style.Db.ErrorColor);
            }
            if (temp_close_hover != null) g.DrawImage(temp_close_hover, rect_icon);
        }
        void PrintMax(Graphics g, Rectangle rect_icon)
        {
            if (temp_max == null || temp_max.Width != rect_icon.Width)
            {
                temp_max?.Dispose();
                temp_max = SvgExtend.GetImgExtend(max_default, rect_icon, Style.Db.Text);
            }
            if (temp_max != null) g.DrawImage(temp_max, rect_icon);
        }
        void PrintRestore(Graphics g, Rectangle rect_icon)
        {
            if (temp_restore == null || temp_restore.Width != rect_icon.Width)
            {
                temp_restore?.Dispose();
                temp_restore = SvgExtend.GetImgExtend(restore_default, rect_icon, Style.Db.Text);
            }
            if (temp_restore != null) g.DrawImage(temp_restore, rect_icon);
        }
        void PrintMin(Graphics g, Rectangle rect_icon)
        {
            if (temp_min == null || temp_min.Width != rect_icon.Width)
            {
                temp_min?.Dispose();
                temp_min = SvgExtend.GetImgExtend(min_default, rect_icon, Style.Db.Text);
            }
            if (temp_min != null) g.DrawImage(temp_min, rect_icon);
        }
        bool PrintLogo(Graphics g, string svg, Rectangle rect_icon)
        {
            if (temp_logo == null || temp_logo.Width != rect_icon.Width)
            {
                temp_logo?.Dispose();
                temp_logo = SvgExtend.GetImgExtend(svg, rect_icon, Style.Db.Text);
            }
            if (temp_logo != null) { g.DrawImage(temp_logo, rect_icon); return true; }
            return false;
        }

        #endregion

        #endregion

        int hasr = 0;
        protected override void OnSizeChanged(EventArgs e)
        {
            var rect = ClientRectangle.PaddingRect(Padding);
            int btn_size = (maximizeBox || minimizeBox) ? (int)(rect.Height * 1.32F) : rect.Height;

            rect_close = new Rectangle(rect.Right - btn_size, rect.Y, btn_size, rect.Height);
            hasr = btn_size;
            int left = rect_close.Left;
            if (maximizeBox)
            {
                rect_max = new Rectangle(left - btn_size, rect.Y, btn_size, rect.Height);
                left -= btn_size;
                hasr += btn_size;
            }
            if (minimizeBox)
            {
                rect_min = new Rectangle(left - btn_size, rect.Y, btn_size, rect.Height);
                hasr += btn_size;
            }
            if (DragMove)
            {
                var form = FindPARENT(Parent);
                if (form != null) IsMax = form.WindowState == FormWindowState.Maximized;
            }
            base.OnSizeChanged(e);
        }

        #region 动画

        ITaskOpacity hove_close, hove_max, hove_min;
        public WindowBar() { hove_close = new ITaskOpacity(this); hove_max = new ITaskOpacity(this); hove_min = new ITaskOpacity(this); }

        #endregion

        #region 鼠标

        Rectangle rect_close, rect_max, rect_min;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool _close = rect_close.Contains(e.Location), _max = rect_max.Contains(e.Location), _min = rect_min.Contains(e.Location);
            if (_close != hove_close.Switch || _max != hove_max.Switch || _min != hove_min.Switch)
            {
                hove_max.MaxValue = hove_min.MaxValue = Style.Db.FillSecondary.A;
                hove_close.Switch = _close;
                hove_max.Switch = _max;
                hove_min.Switch = _min;
            }
            SetCursor(_close || _max || _min);
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hove_close.Switch = hove_max.Switch = hove_min.Switch = false;
            base.OnMouseLeave(e);
        }

        Form? FindPARENT(Control control)
        {
            if (control == null) return null;
            if (control.Parent != null) return FindPARENT(control.Parent);
            else
            {
                if (control is Form form) return form;
            }
            return null;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                hove_close.Down = rect_close.Contains(e.Location);
                hove_max.Down = rect_max.Contains(e.Location);
                hove_min.Down = rect_min.Contains(e.Location);
                if (hove_close.Down || hove_max.Down || hove_min.Down) return;
                if (DragMove)
                {
                    var form = FindPARENT(Parent);
                    if (form != null)
                    {
                        if (e.Clicks > 1)
                        {
                            if (form is BaseForm form_win) form_win.MaxRestore();
                            else
                            {
                                if (form.WindowState == FormWindowState.Maximized) form.WindowState = FormWindowState.Normal;
                                else form.WindowState = FormWindowState.Maximized;
                            }
                        }
                        else
                        {
                            if (form is Window form_win) form_win.ControlMouseDown();
                            else
                            {
                                Vanara.PInvoke.User32.ReleaseCapture();
                                Vanara.PInvoke.User32.SendMessage(form.Handle, 0x0112, 61456 | 2, IntPtr.Zero);
                            }
                        }
                    }
                }
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (hove_close.Down && rect_close.Contains(e.Location)) FindPARENT(Parent)?.Close();
            else if (hove_max.Down && rect_max.Contains(e.Location))
            {
                var form = FindPARENT(Parent);
                if (form != null)
                {
                    if (form.WindowState == FormWindowState.Maximized)
                    {
                        IsMax = false;
                        form.WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        IsMax = true;
                        form.WindowState = FormWindowState.Maximized;
                    }
                }
            }
            else if (hove_min.Down && rect_min.Contains(e.Location))
            {
                var form = FindPARENT(Parent);
                if (form != null)
                {
                    IsMax = false;
                    form.WindowState = FormWindowState.Minimized;
                }
            }
            hove_close.Down = hove_max.Down = hove_min.Down = false;
            base.OnMouseUp(e);
        }

        #region 主题变化

        protected override void CreateHandle()
        {
            base.CreateHandle();
            EventManager.Instance.AddListener(1, this);
        }
        public void HandleEvent(int eventId, IEventArgs? args)
        {
            switch (eventId)
            {
                case 1:
                    temp_logo?.Dispose();
                    temp_min?.Dispose();
                    temp_max?.Dispose();
                    temp_restore?.Dispose();
                    temp_close?.Dispose();
                    temp_logo = null;
                    temp_min = null;
                    temp_max = null;
                    temp_restore = null;
                    temp_close = null;
                    Invalidate();
                    break;
            }
        }

        #endregion

        #endregion

        #region 按钮点击

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; } = DialogResult.None;

        /// <summary>
        /// 是否默认按钮
        /// </summary>
        public void NotifyDefault(bool value)
        {

        }

        public void PerformClick()
        {
            FindPARENT(Parent)?.Close();
        }

        #endregion
    }
}