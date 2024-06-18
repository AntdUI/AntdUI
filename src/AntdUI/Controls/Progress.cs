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
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AntdUI
{
    /// <summary>
    /// Progress 进度条
    /// </summary>
    /// <remarks>展示操作的当前进度。</remarks>
    [Description("Progress 进度条")]
    [ToolboxItem(true)]
    [DefaultProperty("Value")]
    public class Progress : IControl
    {
        public Progress() { }

        public Progress(ContainerControl parentControl)
        {
            ContainerControl = parentControl;
        }


        #region 属性

        Color? fore;
        /// <summary>
        /// 文字颜色
        /// </summary>
        [Description("文字颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public new Color? ForeColor
        {
            get => fore;
            set
            {
                if (fore == value) fore = value;
                fore = value;
                Invalidate();
            }
        }

        #region 背景

        Color? back;
        /// <summary>
        /// 背景颜色
        /// </summary>
        [Description("背景颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
        public Color? Back
        {
            get => back;
            set
            {
                if (back == value) return;
                back = value;
                Invalidate();
            }
        }

        Color? fill;
        /// <summary>
        /// 进度条颜色
        /// </summary>
        [Description("进度条颜色"), Category("外观"), DefaultValue(null)]
        [Editor(typeof(Design.ColorEditor), typeof(UITypeEditor))]
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

        #endregion

        int radius = 6;
        /// <summary>
        /// 圆角
        /// </summary>
        [Description("圆角"), Category("外观"), DefaultValue(6)]
        public int Radius
        {
            get => radius;
            set
            {
                if (radius == value) return;
                radius = value;
                Invalidate();
            }
        }

        /// <summary>
        /// MINI模式
        /// </summary>
        [Description("MINI模式"), Category("外观"), DefaultValue(false)]
        public bool Mini { get; set; } = false;

        TShape shape = TShape.Round;
        /// <summary>
        /// 形状
        /// </summary>
        [Description("形状"), Category("外观"), DefaultValue(TShape.Round)]
        public TShape Shape
        {
            get => shape;
            set
            {
                if (shape == value) return;
                shape = value;
                Invalidate();
            }
        }

        #region 显示文本

        string? text = null;
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本"), Category("外观"), DefaultValue(null)]
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

        bool showText = false;
        /// <summary>
        /// 显示进度文本
        /// </summary>
        [Description("显示进度文本"), Category("外观"), DefaultValue(false)]
        public bool ShowText
        {
            get => showText;
            set
            {
                if (showText == value) return;
                showText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 显示进度文本小数点位数
        /// </summary>
        [Description("显示进度文本小数点位数"), Category("外观"), DefaultValue(0)]
        public int ShowTextDot { get; set; } = 0;

        #endregion

        TType state = TType.None;
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态"), Category("外观"), DefaultValue(TType.None)]
        public TType State
        {
            get => state;
            set
            {
                if (state == value) return;
                state = value;
                Invalidate();
            }
        }

        float iconratio = .7F;
        /// <summary>
        /// 图标比例
        /// </summary>
        [Description("图标比例"), Category("外观"), DefaultValue(.7F)]
        public float IconRatio
        {
            get => iconratio;
            set
            {
                if (iconratio == value) return;
                iconratio = value;
                Invalidate();
            }
        }

        #region 进度条

        float _value = 0F;
        /// <summary>
        /// 进度条
        /// </summary>
        [Description("进度条 0F-1F"), Category("数据"), DefaultValue(0F)]
        public float Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                if (value < 0) value = 0;
                else if (value > 1) value = 1;
                _value = value;
                Invalidate();
                if (showInTaskbar) ShowTaskbar();
            }
        }

        #endregion

        bool loading = false;
        float AnimationLoadingValue = 0F;
        /// <summary>
        /// 加载状态
        /// </summary>
        [Description("加载状态"), Category("外观"), DefaultValue(false)]
        public bool Loading
        {
            get => loading;
            set
            {
                if (loading == value) return;
                loading = value;
                ThreadLoading?.Dispose();
                if (showInTaskbar) ShowTaskbar(!loading);
                if (loading)
                {
                    ThreadLoading = new ITask(this, () =>
                    {
                        AnimationLoadingValue = AnimationLoadingValue.Calculate(0.01F);
                        if (AnimationLoadingValue > 1)
                        {
                            AnimationLoadingValue = 0;
                            Invalidate();
                            Thread.Sleep(1000);
                        }
                        Invalidate();
                        return loading;
                    }, 10, () =>
                    {
                        Invalidate();
                    });
                }
                else Invalidate();
            }
        }

        #region 动画

        protected override void Dispose(bool disposing)
        {
            ThreadLoading?.Dispose();
            base.Dispose(disposing);
        }
        ITask? ThreadLoading = null;

        #endregion

        #region 任务栏

        ContainerControl? ownerForm;
        public ContainerControl? ContainerControl
        {
            get => ownerForm;
            set => ownerForm = value;
        }

        public override ISite? Site
        {
            set
            {
                // Runs at design time, ensures designer initializes ContainerControl
                base.Site = value;
                if (value == null) return;
                var service = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (service == null) return;
                IComponent rootComponent = service.RootComponent;
                ContainerControl = (ContainerControl)rootComponent;
            }
        }

        bool showInTaskbar = false;
        /// <summary>
        /// 任务栏中显示进度
        /// </summary>
        [Description("任务栏中显示进度"), Category("外观"), DefaultValue(false)]
        public bool ShowInTaskbar
        {
            get => showInTaskbar;
            set
            {
                if (showInTaskbar == value) return;
                showInTaskbar = value;
                if (showInTaskbar) ShowTaskbar();
                else if (ownerForm != null) TaskbarProgressState(ownerForm, ThumbnailProgressState.NoProgress);
            }
        }
        void ShowTaskbar(bool sl = false)
        {
            if (ownerForm == null) return;
            if (state == TType.None)
            {
                if (_value == 0 && loading)
                {
                    TaskbarProgressValue(ownerForm, 0);
                    TaskbarProgressState(ownerForm, ThumbnailProgressState.Indeterminate);
                }
                else
                {
                    if (sl && old_state == ThumbnailProgressState.Indeterminate) TaskbarProgressState(ownerForm, ThumbnailProgressState.NoProgress);
                    TaskbarProgressState(ownerForm, ThumbnailProgressState.Normal);
                    TaskbarProgressValue(ownerForm, (ulong)(_value * 100));
                }
            }
            else
            {
                switch (state)
                {
                    case TType.Error:
                        TaskbarProgressState(ownerForm, ThumbnailProgressState.Error);
                        break;
                    case TType.Warn:
                        TaskbarProgressState(ownerForm, ThumbnailProgressState.Paused);
                        break;
                    default:
                        TaskbarProgressState(ownerForm, ThumbnailProgressState.Normal);
                        break;
                }
                TaskbarProgressValue(ownerForm, (ulong)(_value * 100));
            }
        }

        void TaskbarProgressState(ContainerControl hwnd, ThumbnailProgressState state)
        {
            if (old_state == state) return;
            old_state = state;
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    Windows7Taskbar.SetProgressState(hwnd.Handle, state);
                }));
                return;
            }
            Windows7Taskbar.SetProgressState(hwnd.Handle, state);
        }

        ulong old_value = 0;
        ThumbnailProgressState old_state = ThumbnailProgressState.NoProgress;
        void TaskbarProgressValue(ContainerControl hwnd, ulong value)
        {
            if (old_value == value) return;
            old_value = value;
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    Windows7Taskbar.SetProgressValue(hwnd.Handle, value);
                }));
                return;
            }
            Windows7Taskbar.SetProgressValue(hwnd.Handle, value);
        }

        #endregion

        #endregion

        #region 渲染

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect_t = ClientRectangle;
            var rect = rect_t.PaddingRect(Padding);
            var g = e.Graphics.High();
            Color _color, _back;
            switch (state)
            {
                case TType.Success:
                    _color = fill ?? Style.Db.Success;
                    break;
                case TType.Info:
                    _color = fill ?? Style.Db.Info;
                    break;
                case TType.Warn:
                    _color = fill ?? Style.Db.Warning;
                    break;
                case TType.Error:
                    _color = fill ?? Style.Db.Error;
                    break;
                case TType.None:
                default:
                    _color = fill ?? Style.Db.Primary;
                    break;
            }
            if (Mini)
            {
                _back = back ?? Color.FromArgb(40, _color);
                var font_size = g.MeasureString(text, Font);
                rect.IconRectL(font_size, out var icon_rect, out var text_rect, iconratio);
                if (icon_rect.Width == 0 || icon_rect.Height == 0) return;
                using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                {
                    if (showText) g.DrawString((_value * 100F).ToString("F" + ShowTextDot) + text, Font, brush, new RectangleF(text_rect.X + 8, text_rect.Y, text_rect.Width - 8, text_rect.Height), Helper.stringFormatLeft);
                    else g.DrawString(text, Font, brush, new RectangleF(text_rect.X + 8, text_rect.Y, text_rect.Width - 8, text_rect.Height), Helper.stringFormatLeft);
                }

                float w = radius * Config.Dpi;
                using (var brush = new Pen(_back, w))
                {
                    g.DrawEllipse(brush, icon_rect);
                }
                if (_value > 0)
                {
                    int max = (int)Math.Round(360 * _value);
                    using (var brush = new Pen(_color, w))
                    {
                        brush.StartCap = brush.EndCap = LineCap.Round;
                        g.DrawArc(brush, icon_rect, -90, max);
                    }

                    if (loading && AnimationLoadingValue > 0)
                    {
                        int a = (int)(60 * (1f - AnimationLoadingValue));

                        using (var brush = new Pen(Color.FromArgb(a, Style.Db.BgBase), w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, icon_rect, -90, (int)(max * AnimationLoadingValue));
                        }
                    }
                }
            }
            else
            {
                _back = back ?? Style.Db.FillSecondary;
                if (shape == TShape.Circle)
                {
                    float w = radius * Config.Dpi;
                    float prog_size;
                    if (rect.Width == rect.Height) prog_size = rect.Width - w;
                    else if (rect.Width > rect.Height) prog_size = rect.Height - w;
                    else prog_size = rect.Width - w;
                    var rect_prog = new RectangleF(rect.X + (rect.Width - prog_size) / 2, rect.Y + (rect.Height - prog_size) / 2, prog_size, prog_size);
                    using (var brush = new Pen(_back, w))
                    {
                        g.DrawEllipse(brush, rect_prog);
                    }
                    if (_value > 0)
                    {
                        int max = (int)Math.Round(360 * _value);
                        using (var brush = new Pen(_color, w))
                        {
                            brush.StartCap = brush.EndCap = LineCap.Round;
                            g.DrawArc(brush, rect_prog, -90, max);
                        }
                        if (loading && AnimationLoadingValue > 0)
                        {
                            int a = (int)(60 * (1f - AnimationLoadingValue));

                            using (var brush = new Pen(Color.FromArgb(a, Style.Db.BgBase), w))
                            {
                                brush.StartCap = brush.EndCap = LineCap.Round;
                                g.DrawArc(brush, rect_prog, -90, (int)(max * AnimationLoadingValue));
                            }
                        }

                        if (state == TType.None)
                        {
                            using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                            {
                                if (showText) g.DrawString((_value * 100F).ToString("F" + ShowTextDot) + text, Font, brush, rect, Helper.stringFormatCenter);
                                else g.DrawString(text, Font, brush, rect, Helper.stringFormatCenter);
                            }
                        }
                        else
                        {
                            var size = rect_prog.Width * 0.26F;
                            g.PaintIconGhosts(state, new RectangleF(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size), _color);
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            if (showText) g.DrawString((_value * 100F).ToString("F" + ShowTextDot) + text, Font, brush, rect, Helper.stringFormatCenter);
                            else g.DrawString(text, Font, brush, rect, Helper.stringFormatCenter);
                        }
                    }
                }
                else
                {
                    float _radius = radius * Config.Dpi;
                    if (shape == TShape.Round) _radius = rect.Height;
                    if (showText)
                    {
                        var size_font = g.MeasureString(100.ToString("F" + ShowTextDot), Font).Size(rect.Height * 1.6F);
                        var rect_rext = new Rectangle(rect.Right - size_font.Width, 0, size_font.Width, rect_t.Height);
                        rect.Width -= size_font.Width;
                        using (var path = rect.RoundPath(_radius))
                        {
                            using (var brush = new SolidBrush(_back))
                            {
                                g.FillPath(brush, path);
                            }
                            if (_value > 0)
                            {
                                var _w = rect.Width * _value;
                                if (_w > _radius)
                                {
                                    using (var path_prog = new RectangleF(rect.X, rect.Y, _w, rect.Height).RoundPath(_radius))
                                    {
                                        using (var brush = new SolidBrush(_color))
                                        {
                                            g.FillPath(brush, path_prog);
                                        }
                                    }
                                    if (loading && AnimationLoadingValue > 0)
                                    {
                                        int a = (int)(60 * (1f - AnimationLoadingValue));
                                        using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.BgBase)))
                                        {
                                            using (var path_prog = new RectangleF(rect.X, rect.Y, _w * AnimationLoadingValue, rect.Height).RoundPath(_radius))
                                            {
                                                g.FillPath(brush, path_prog);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    using (var bmp = new Bitmap(rect.Right, rect.Bottom))
                                    {
                                        using (var g2 = Graphics.FromImage(bmp).High())
                                        {
                                            using (var brush = new SolidBrush(_color))
                                            {
                                                g2.FillEllipse(brush, new RectangleF(rect.X, rect.Y, _w, rect.Height));
                                            }
                                            if (loading && AnimationLoadingValue > 0)
                                            {
                                                int a = (int)(60 * (1f - AnimationLoadingValue));
                                                using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.BgBase)))
                                                {
                                                    g2.FillEllipse(brush, new RectangleF(rect.X, rect.Y, _w * AnimationLoadingValue, rect.Height));
                                                }
                                            }
                                        }
                                        using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                                        {
                                            brush.TranslateTransform(rect.X, rect.Y);
                                            g.FillPath(brush, path);
                                        }
                                    }
                                }
                            }
                        }

                        if (state == TType.None)
                        {
                            using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                            {
                                g.DrawString((_value * 100F).ToString("F" + ShowTextDot) + text, Font, brush, rect_rext, Helper.stringFormatCenter);
                            }
                        }
                        else
                        {
                            var size = rect_rext.Height * 0.62F;
                            var rect_ico = new RectangleF(rect_rext.X + (rect_rext.Width - size) / 2F, rect_rext.Y + (rect_rext.Height - size) / 2F, size, size);
                            g.PaintIcons(state, rect_ico);
                        }
                    }
                    else
                    {
                        using (var path = rect.RoundPath(_radius))
                        {
                            using (var brush = new SolidBrush(_back))
                            {
                                g.FillPath(brush, path);
                            }
                            if (_value > 0)
                            {
                                var _w = rect.Width * _value;
                                if (_w > _radius)
                                {
                                    using (var path_prog = new RectangleF(rect.X, rect.Y, _w, rect.Height).RoundPath(_radius))
                                    {
                                        using (var brush = new SolidBrush(_color))
                                        {
                                            g.FillPath(brush, path_prog);
                                        }
                                    }
                                    if (loading && AnimationLoadingValue > 0)
                                    {
                                        int a = (int)(60 * (1f - AnimationLoadingValue));
                                        using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.BgBase)))
                                        {
                                            using (var path_prog = new RectangleF(rect.X, rect.Y, _w * AnimationLoadingValue, rect.Height).RoundPath(_radius))
                                            {
                                                g.FillPath(brush, path_prog);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    using (var bmp = new Bitmap(rect.Right, rect.Bottom))
                                    {
                                        using (var g2 = Graphics.FromImage(bmp).High())
                                        {
                                            using (var brush = new SolidBrush(_color))
                                            {
                                                g2.FillEllipse(brush, new RectangleF(rect.X, rect.Y, _w, rect.Height));
                                            }
                                            if (loading && AnimationLoadingValue > 0)
                                            {
                                                int a = (int)(60 * (1f - AnimationLoadingValue));
                                                using (var brush = new SolidBrush(Color.FromArgb(a, Style.Db.BgBase)))
                                                {
                                                    g2.FillEllipse(brush, new RectangleF(rect.X, rect.Y, _w * AnimationLoadingValue, rect.Height));
                                                }
                                            }
                                        }
                                        using (var brush = new TextureBrush(bmp, WrapMode.Clamp))
                                        {
                                            brush.TranslateTransform(rect.X, rect.Y);
                                            g.FillPath(brush, path);
                                        }
                                    }
                                }
                            }
                        }
                        using (var brush = new SolidBrush(fore ?? Style.Db.Text))
                        {
                            g.DrawString(text, Font, brush, rect, Helper.stringFormatCenter);
                        }
                    }
                }
            }
            this.PaintBadge(g);
            base.OnPaint(e);
        }

        #endregion
    }

    #region 任务栏

    internal static class Windows7Taskbar
    {
        static ITaskbarList3? _taskbarList;
        internal static ITaskbarList3 TaskbarList
        {
            get
            {
                if (_taskbarList == null)
                {
                    _taskbarList = (ITaskbarList3)new CTaskbarList();
                    _taskbarList.HrInit();
                }
                return _taskbarList;
            }
        }

        /// <summary>
        /// Sets the progress state of the specified window's
        /// taskbar button.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="state">The progress state.</param>
        public static void SetProgressState(IntPtr hwnd, ThumbnailProgressState state)
        {
            TaskbarList.SetProgressState(hwnd, state);
        }

        /// <summary>
        /// Sets the progress value of the specified window's
        /// taskbar button.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="current">The current value.</param>
        /// <param name="maximum">The maximum value.</param>
        public static void SetProgressValue(IntPtr hwnd, ulong current, ulong maximum)
        {
            TaskbarList.SetProgressValue(hwnd, current, maximum);
        }

        public static void SetProgressValue(IntPtr hwnd, ulong current)
        {
            TaskbarList.SetProgressValue(hwnd, current, 100);
        }
    }

    /// <summary>
    /// 表示缩略图进度条状态。
    /// </summary>
    internal enum ThumbnailProgressState
    {
        /// <summary>
        /// 没有进度。
        /// </summary>
        NoProgress = 0,
        /// <summary>
        /// 不确定的进度 (marquee)。
        /// </summary>
        Indeterminate = 0x1,
        /// <summary>
        /// 正常进度
        /// </summary>
        Normal = 0x2,
        /// <summary>
        /// 错误进度 (red).
        /// </summary>
        Error = 0x4,
        /// <summary>
        /// 暂停进度 (yellow).
        /// </summary>
        Paused = 0x8
    }

    [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport]
    internal class CTaskbarList { }

    [ComImport]
    [Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ITaskbarList3
    {
        [PreserveSig]
        void HrInit();

        [PreserveSig]
        void AddTab(IntPtr hwnd);

        [PreserveSig]
        void DeleteTab(IntPtr hwnd);

        [PreserveSig]
        void ActivateTab(IntPtr hwnd);

        [PreserveSig]
        void SetActiveAlt(IntPtr hwnd);

        // ITaskbarList2
        [PreserveSig]
        void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

        // ITaskbarList3
        void SetProgressValue(IntPtr hwnd, UInt64 ullCompleted, UInt64 ullTotal);
        void SetProgressState(IntPtr hwnd, ThumbnailProgressState tbpFlags);
    }

    #endregion
}